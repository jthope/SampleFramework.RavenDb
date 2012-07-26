using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleFramework.Core.Infrastructure;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace SampleFramework.Core.Domain {

	[Serializable]
	public abstract class AuditableModel : Model {

		[Audit(false)]
		public virtual string CreatedBy { get; set; }

		[Audit(false)]
		public virtual DateTime CreatedOn { get; set; }

		[Audit(false)]
		public virtual string ModifiedBy { get; set; }

		[Audit(false)]
		public virtual DateTime ModifiedOn { get; set; }
		public virtual bool IsDeleted { get; set; }

		[JsonIgnore]
		[Audit(false)]
		internal AuditLog _auditLog = null;

		[JsonIgnore]
		[Audit(false)]
		internal bool SnapshotTaken = false;

		public AuditableModel() {

			IsDeleted = false;

			((INotifyPropertyChanged)this).PropertyChanged += (sender, e) => Model_PropertyChanged(sender, e);

		}

		[JsonIgnore]
		[Audit(false)]
		internal AuditLog AuditLog {
			get { return _auditLog; }
		}

		internal List<PropertyInfo> GetAuditableProperties(Type type) {

			return (from x in new List<PropertyInfo>(type.GetProperties())
					let attribute = Attribute.GetCustomAttribute(x, typeof(AuditAttribute)) as AuditAttribute
					where
						(attribute == null || attribute.IsFieldAuditable) &&
						x.Name != "Id"
					select x).ToList();

		}

		internal void TakeSnapshot() {

			if (_auditLog == null) {

				_auditLog = new AuditLog();
				_auditLog.Entity = Util.Inflector.Pluralize(ProxyUtil.GetNonProxiedModelType(GetType()).Name);
				_auditLog.EntityId = this.Id;

			}

			if (IsPersisted && !SnapshotTaken && _auditLog.ChangedFields.Count == 0) {

				_auditLog.EntityId = this.Id;

				GetAuditableProperties(GetType()).ForEach(prop => {

					var propValue = prop.GetValue(this, null);

					if (propValue != null) {
						_auditLog.OriginalFields.Add(prop.Name, propValue.ToString());
					} else {
						_auditLog.OriginalFields.Add(prop.Name, null);
					}

				});

				SnapshotTaken = true;

			}

		}

		private AuditLogChangedField FindPropertyInChangesList(string propertyName) {

			var auditLogChangedField = _auditLog.ChangedFields.Find(a => a.FieldName == propertyName);

			return auditLogChangedField;

		}

		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e) {

			if (!SnapshotTaken)
				return;

			var propertyChanged = false;

			var prop = (from x in GetAuditableProperties(sender.GetType())
						where x.Name == e.PropertyName
						select x).SingleOrDefault();

			if (prop != null) {

				string originalPropValue = null;
				var propValueObj = prop.GetValue(this, null);
				string propValue = null;

				if (propValueObj != null) {
					propValue = propValueObj.ToString();
				}

				if (_auditLog.OriginalFields.ContainsKey(prop.Name)) {

					var originalProp = _auditLog.OriginalFields[prop.Name];

					if (originalProp != null) {
						originalPropValue = originalProp.ToString();
					}

				}

				if (originalPropValue != propValue) {

					var auditLogChangedField = FindPropertyInChangesList(prop.Name);

					if (auditLogChangedField != null) {

						auditLogChangedField.FieldName = prop.Name;

						if (IsPersisted)
							auditLogChangedField.OldValue = originalPropValue;

						auditLogChangedField.NewValue = propValue;

						propertyChanged = true;

					} else {

						_auditLog.ChangedFields.Add(new AuditLogChangedField { FieldName = prop.Name, OldValue = originalPropValue, NewValue = propValue });

						propertyChanged = true;

					}

				}

			}

			if (propertyChanged) {

				this.EntityState = Enumerations.EntityState.Modified;

				_auditLog.ModificationType = this.EntityState.ToString();

			}

		}

	}

}
