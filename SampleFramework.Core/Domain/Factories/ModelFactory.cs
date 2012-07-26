using SampleFramework.Core.Common;
using SampleFramework.Core.Infrastructure;
using StructureMap;

namespace SampleFramework.Core.Domain {

	public class ModelFactory {

		public static ModelFactory Instance {
			get { return Singleton<ModelFactory>.Instance; }
		}

		public virtual T Get<T>() where T : Model {
			return ObjectFactory.GetInstance<T>();
		}

	}

}
