using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap.Configuration.DSL;
using Raven.Client;

namespace SampleFramework.Core.Infrastructure.StructureMap {

	public class RavenDbRegistry : Registry {

		public RavenDbRegistry(IDocumentStore sampleFrameworkDb) {

			For<IDocumentStore>().Use(() => {
				return sampleFrameworkDb;
			}).Named("SampleFrameworkDb");

			For<IDocumentSession>()
				.HybridHttpOrThreadLocalScoped()
				.Use(x => {
					return x.GetInstance<IDocumentStore>("SampleFrameworkDb").OpenSession();
				}).Named("SampleFrameworkDbSession");

		}

	}

}
