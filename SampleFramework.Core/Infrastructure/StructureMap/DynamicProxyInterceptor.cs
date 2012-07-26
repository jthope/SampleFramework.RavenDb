using System;
using Castle.DynamicProxy;
using StructureMap;
using StructureMap.Interceptors;

namespace SampleFramework.Core.Infrastructure.StructureMap {

	internal class DynamicProxyInterceptor : InstanceInterceptor {

		private readonly Type _pluginType;
		private IInterceptor[] _interceptors;
		private bool _createInterfaceProxy = true;

		public DynamicProxyInterceptor(Type pluginType, params IInterceptor[] interceptors) {

			_pluginType = pluginType;
			_interceptors = interceptors;

			if (!pluginType.IsInterface) {
				_createInterfaceProxy = false;
			}

		}

		#region InstanceInterceptor Members

		public object Process(object target, IContext context) {

			if (_createInterfaceProxy) {

				return ProxyUtil.CreateServiceProxyClass(_pluginType, target);

			} else {

				return ProxyUtil.CreateModelProxyClass(_pluginType, target);

			}

		}

		#endregion

	}

}
