using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Castle.DynamicProxy;
using System.ComponentModel;

namespace SampleFramework.Core.Infrastructure {

	public class ModelInterceptor : IInterceptor {

		private PropertyChangedEventHandler _subscribers = delegate { };

		public void Intercept(IInvocation invocation) {

			if (invocation.Method.Name.StartsWith("add_") ||
				invocation.Method.Name.StartsWith("remove_")) {

				HandleSubscription(invocation);

				return;

			}

			invocation.Proceed();

			if (invocation.Method.Name.StartsWith("set_")) {
				FireNotificationChanged(invocation);
			}

		}

		private void HandleSubscription(IInvocation invocation) {

			var handler = (PropertyChangedEventHandler)invocation.Arguments[0];

			if (invocation.Method.Name.StartsWith("add_")) {
				_subscribers += handler;

			} else {
				_subscribers -= handler;

			}

		}

		private void FireNotificationChanged(IInvocation invocation) {

			var propertyName = invocation.Method.Name.Substring(4);
			_subscribers(invocation.InvocationTarget, new PropertyChangedEventArgs(propertyName));

		}

	}

}
