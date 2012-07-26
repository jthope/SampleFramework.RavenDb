using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleFramework.Core.Repository;

namespace SampleFramework.Core.Services {

	public class BaseService : IBaseService {

		protected IRavenDbRepository repository;

		public BaseService(IRavenDbRepository repository) {
			this.repository = repository;
		}

		public IRavenDbRepository Repository() {
			return repository;
		}

		public virtual void Store(dynamic entity) {
			repository.Store(entity);
		}

		public virtual void Delete(dynamic entity) {
			repository.Delete(entity);
		}

	}

}
