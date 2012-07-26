using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Linq;
using SampleFramework.Core.Domain;
using SampleFramework.Core.Domain.Enumerations;

namespace SampleFramework.Core.Repository {

	public class RavenDbRepository : IRavenDbRepository {

		protected IDocumentSession session;

		public RavenDbRepository(IDocumentSession session) {
			this.session = session;
		}

		public virtual T GetById<T>(string id) {

			return session.Load<T>(id);

		}

		public virtual IQueryable<T> GetList<T>() {

			var query = from x in session.Query<T>()
										 .Customize(x => x.WaitForNonStaleResults())
						select x;

			return query;

		}

		public virtual IQueryable<T> GetList<T>(int take, int skip, int page, int pageSize, ref int totalRecords) {

			var query = from x in session.Query<T>()
										 .Customize(x => x.WaitForNonStaleResults())
						select x;

			totalRecords = query.Count();

			return query.Skip(skip).Take(take);

		}

		public virtual IQueryable<T> GetListWhereIdIn<T>(string[] ids, string includes = null) where T : Model {

			var query = session.Query<T>()
							   .Where(x => x.Id.In<string>(ids));

			if (!string.IsNullOrEmpty(includes)) {

				query = query.Customize(x => x.Include(includes));

			}

			return query.AsQueryable();

		}

		public virtual void Store(dynamic entity) {

			var entityType = entity.GetType();

			if (entityType.IsSubclassOf(typeof(AuditableModel))) {

				entity = entity as AuditableModel;

				if (!entity.IsPersisted) {

					entity.TakeSnapshot();
					entity.CreatedBy = Util.ContextHelper.GetCurrentLoggedInUser;
					entity.CreatedOn = DateTime.UtcNow;

					// create audit log
					foreach (var prop in entity.GetAuditableProperties(entity.GetType())) {

						var propValue = prop.GetValue(entity);

						if (propValue != null) {
							entity.AuditLog.ChangedFields.Add(new AuditLogChangedField() { FieldName = prop.Name, NewValue = propValue.ToString() });
						} else {
							entity.AuditLog.ChangedFields.Add(new AuditLogChangedField() { FieldName = prop.Name });
						}

						entity.EntityState = EntityState.Added;
						entity.AuditLog.ModificationType = EntityState.Added.ToString();
						entity.AuditLog.ModifiedBy = entity.CreatedBy;
						entity.AuditLog.ModifiedOn = entity.CreatedOn;

					};

				}

			}

			session.Store(entity);

			if (entityType.IsSubclassOf(typeof(AuditableModel))) {

				entity = entity as AuditableModel;

				if (entity.IsDirty) {

					entity.ModifiedBy = Util.ContextHelper.GetCurrentLoggedInUser;
					entity.ModifiedOn = DateTime.UtcNow;

					entity.AuditLog.EntityId = entity.Id;
					entity.AuditLog.ModifiedBy = entity.ModifiedBy;
					entity.AuditLog.ModifiedOn = entity.ModifiedOn;
					session.Store(entity.AuditLog);

				}

			}

		}

		public virtual void Delete(dynamic entity) {

			var entityType = entity.GetType();

			if (entityType.IsSubclassOf(typeof(AuditableModel))) {

				entity = entity as AuditableModel;
				entity.IsDeleted = true;

				session.Store(entity);

				if (entity.IsDirty) {

					entity.ModifiedBy = Util.ContextHelper.GetCurrentLoggedInUser;
					entity.ModifiedOn = DateTime.UtcNow;

					entity.AuditLog.EntityId = entity.Id;
					session.Store(entity.AuditLog);

				}

			} else {

				session.Delete(entity);

			}

		}

		public virtual void Purge<T>() {

			Util.RavenDb.DeleteAllDocumentsByEntityType(session, typeof(T));

		}

		public virtual void Commit() {

			session.SaveChanges();

		}

		public virtual IDocumentSession Session() {
			return session;
		}

	}

}
