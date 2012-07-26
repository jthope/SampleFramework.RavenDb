using System;
using Castle.DynamicProxy;
using StructureMap;
using StructureMap.Interceptors;

namespace SampleFramework.Core.Infrastructure {

	internal class ServiceInterceptor : IInterceptor {

		public void Intercept(IInvocation invocation) {

			// nothing to see here yet....
			invocation.Proceed();

		}

	}

}
