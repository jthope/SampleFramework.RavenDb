using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleFramework.Core.Infrastructure;
using Raven.Client;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;
using SampleFramework.Core.Infrastructure.StructureMap;

namespace SampleFramework.Core.Conventions {

	internal class ServicesAreSingletonsAndProxies : IRegistrationConvention {

		public void Process(Type type, Registry registry) {

			if (!type.IsClass || !IsService(type) || !Constructor.HasConstructors(type)) {
				return;
			}

			Type pluginType = FindPluginType(type);

			if (pluginType == null) {
				return;
			}

			registry.For(pluginType)
				.Use(
					new ConfiguredInstance(type) {
						Interceptor = new DynamicProxyInterceptor(pluginType, new ServiceInterceptor())
					});

		}

		private static bool IsService(Type type) {
			return type.Name.EndsWith("Service");
		}

		private static Type FindPluginType(Type concreteType) {

			string interfaceName = "I" + concreteType.Name;

			return concreteType.GetInterfaces()
				.Where(t => string.Equals(t.Name, interfaceName, StringComparison.Ordinal))
				.FirstOrDefault();

		}

	}

}
