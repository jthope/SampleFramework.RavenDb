using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleFramework.Core.Repository;

namespace SampleFramework.Core.Services {

	public interface IBaseService {

		IRavenDbRepository Repository();

		void Store(dynamic entity);
		void Delete(dynamic entity);

	}

}
