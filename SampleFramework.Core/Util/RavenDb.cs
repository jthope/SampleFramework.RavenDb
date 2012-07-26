using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Abstractions.Data;
using Raven.Client;

namespace SampleFramework.Core.Util {

	internal static class RavenDb {

		public static void DeleteAllDocumentsByEntityType(IDocumentSession session, Type entityType) {

			try {

				// delete collection
				session.Advanced
					.DatabaseCommands
					.DeleteByIndex("Raven/DocumentsByEntityName",
						new IndexQuery {
							Query = "Tag:" + Core.Util.Inflector.Pluralize(entityType.Name)
						},
						allowStale: true);

				// delete hilo to reset id keys
				session.Advanced
					.DatabaseCommands
					.Delete("Raven/Hilo/" + Core.Util.Inflector.Pluralize(entityType.Name).ToLower(), null);

			} catch (Exception ex) {

				// index takes a while to register in ravendb on first launch
				// rather than just throwing the exception away, check for an index first, but this is just a demo and 
				// not intended to be in a production environment.
				ex.ToString();

			}

		}

	}

}
