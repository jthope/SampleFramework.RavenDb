using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SampleFramework.Core.Util {

	public static class ContextHelper {

		public static string GetCurrentLoggedInUser {

			get {

				var userName = "Unknown";

				if (HttpContext.Current.User != null &&
					!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) {

					userName = HttpContext.Current.User.Identity.Name;

				}

				return userName;

			}

		}

	}

}
