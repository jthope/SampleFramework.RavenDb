using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SampleFramework.Core.Common;

namespace SampleFramework.Core.Infrastructure.HttpModules {

	public class RavenDbModule : IHttpModule {

		private static volatile bool isStarted = false;
		private static object moduleStart = new Object();

		public void Init(HttpApplication context) {

			if (!isStarted) {

				lock (moduleStart) {

					if (!isStarted) {

						try {

							Bootstrapper.Startup();

						} catch (Exception ex) {

							throw ex;

						}

						isStarted = true;

					}

				}

			}

		}

		public void Dispose() {

		}

	}

}
