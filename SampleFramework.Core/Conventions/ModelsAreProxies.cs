using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleFramework.Core.Domain;
using SampleFramework.Core.Infrastructure;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;
using SampleFramework.Core.Infrastructure.StructureMap;

namespace SampleFramework.Core.Conventions {

	internal class ModelsAreProxies : IRegistrationConvention {

		public void Process(Type type, Registry registry) {

			if (type.IsInterface) { return; }
			if (type.IsAbstract) { return; }
			if (!type.IsSubclassOf(typeof(Model))) { return; }

			registry.For(type)
				.Use(
					new ConfiguredInstance(type) {
						Interceptor = new DynamicProxyInterceptor(type, new ModelInterceptor())
					});

		}

	}

}
