using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SampleFramework.Core.Infrastructure.StructureMap;
using Raven.Database;
using Raven.Client;
using Raven.Json.Linq;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Config;
using StructureMap;
using SampleFramework.Core.Infrastructure.RavenDb;
using SampleFramework.Core.Infrastructure;

namespace SampleFramework.Core {

	public static class Bootstrapper {

		public static void Startup() {
		
			var sampleFrameworkDb = new EmbeddableDocumentStore {
				DataDirectory = "Data",
				UseEmbeddedHttpServer = true
			};
			
			sampleFrameworkDb.RegisterListener(new DocumentConversionListener());
			sampleFrameworkDb.Conventions.FindClrTypeName = FindClrTypeName;
			sampleFrameworkDb.Conventions.FindTypeTagName = FindTypeTagName;
			sampleFrameworkDb.Conventions.JsonContractResolver = new DPContractResolver();
			sampleFrameworkDb.Conventions.ShouldCacheRequest = url => false;
			sampleFrameworkDb.Conventions.CustomizeJsonSerializer = serializer => {
				serializer.ContractResolver = new DPContractResolver();
				serializer.Converters.Add(new ModelCreationConverter());
			};

			sampleFrameworkDb.Initialize();
			sampleFrameworkDb.DatabaseCommands.DisableAllCaching();

			ObjectFactory.Initialize(x => {

				x.AddRegistry(new RavenDbRegistry(sampleFrameworkDb));
				x.AddRegistry(new RepositoryRegistry());

			});

		}

		private static string FindTypeTagName(Type entityType) {

			var typeTageName = string.Empty;
			var nonProxiedType = Infrastructure.ProxyUtil.GetNonProxiedModelType(entityType);

			typeTageName = Util.Inflector.Pluralize(nonProxiedType.Name);

			return typeTageName;

		}

		private static string FindClrTypeName(Type entityType) {

			var clrTypeName = string.Empty;
			var nonProxiedType = Infrastructure.ProxyUtil.GetNonProxiedModelType(entityType);

			clrTypeName = Raven.Client.Document.ReflectionUtil.GetFullNameWithoutVersionInformation(nonProxiedType);

			return clrTypeName;

		}

	}

}
