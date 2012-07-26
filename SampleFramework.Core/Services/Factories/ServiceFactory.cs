using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using SampleFramework.Core.Infrastructure;

namespace SampleFramework.Core.Services {

	public class ServiceFactory {

		public static ServiceFactory Instance {
			get { return Singleton<ServiceFactory>.Instance; }
		}

		public virtual T Get<T>() where T : class {
			return ObjectFactory.GetInstance<T>();
		}

	}

}
