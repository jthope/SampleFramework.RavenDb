using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Linq;
using SampleFramework.Core.Domain;

namespace SampleFramework.Core.Repository {

	public interface IRavenDbRepository {

		T GetById<T>(string id);

		IQueryable<T> GetList<T>();
		IQueryable<T> GetList<T>(int take, int skip, int page, int pageSize, ref int totalRecords);
		IQueryable<T> GetListWhereIdIn<T>(string[] ids, string includes = null) where T : Model;

		void Store(dynamic entity);
		void Delete(dynamic entity);

		void Purge<T>();
		void Commit();

		IDocumentSession Session();

	}

}
