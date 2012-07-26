using System;
using Castle.DynamicProxy;
using Omu.ValueInjecter;
using SampleFramework.Core.Domain;
using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace SampleFramework.Core.Infrastructure.RavenDb {

	internal class DocumentConversionListener : IDocumentConversionListener {

		public void DocumentToEntity(object entity, RavenJObject document, RavenJObject metadata) {

			var auditable = entity as AuditableModel;
			if (auditable == null)
				return;

			auditable.TakeSnapshot();

		}

		public void EntityToDocument(object entity, RavenJObject document, RavenJObject metadata) {

		}

	}

}