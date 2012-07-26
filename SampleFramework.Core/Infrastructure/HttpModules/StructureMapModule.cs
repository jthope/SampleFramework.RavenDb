using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using System.Web;

namespace SampleFramework.Core.Infrastructure.HttpModules {

	public class StructureMapModule : IHttpModule {

		public void Init(HttpApplication context) {

			context.EndRequest += new EventHandler(EndRequest);

		}

		protected void EndRequest(object sender, EventArgs e) {

			ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();

		}

		public void Dispose() {

		}

	}

}
