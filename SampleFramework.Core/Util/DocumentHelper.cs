using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using Raven.Client;
using SampleFramework.Core.Repository;

namespace SampleFramework.Core.Util {

	public static class DocumentHelper {

		public static IDocumentStore DocumentStore {
			get {
				return ObjectFactory.GetNamedInstance<IDocumentStore>("SampleFrameworkDb");
			}
		}

		public static IDocumentSession OpenDocumentSession {
			get {
				return ObjectFactory.GetNamedInstance<IDocumentSession>("SampleFrameworkDbSession");
			}
		}

		public static IRavenDbRepository GetRepository {
			get {
				return ObjectFactory.GetInstance<IRavenDbRepository>();
			}
		}

	}

}
