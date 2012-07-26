using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using SampleFramework.Core.Repository;
using SampleFramework.Core.Conventions;

namespace SampleFramework.Core.Infrastructure.StructureMap {

	public class RepositoryRegistry : Registry {

		public RepositoryRegistry() {

			For<IRavenDbRepository>()
				.Use(new ConfiguredInstance(typeof(RavenDbRepository)) {
					Interceptor = new DynamicProxyInterceptor(typeof(IRavenDbRepository), new ServiceInterceptor())
				});

			Scan(s => {
				s.AssemblyContainingType(typeof(IRavenDbRepository));
				s.Convention<ServicesAreSingletonsAndProxies>();
				s.Convention<ModelsAreProxies>();
			});

		}

	}

}
