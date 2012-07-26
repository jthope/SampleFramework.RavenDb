using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using SampleFramework.Core.Domain;
using System.ComponentModel;

namespace SampleFramework.Core.Infrastructure {

	public static class ProxyUtil {

		private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

		public static object CreateServiceProxyClass(Type pluginType, object target) {

			var interceptors = new ServiceInterceptor();

			return ProxyGenerator.CreateInterfaceProxyWithTarget(
				pluginType,
				target,
				ProxyGenerationOptions.Default,
				interceptors);

		}

		public static object CreateModelProxyClass(Type pluginType, object target) {

			var interceptors = new ModelInterceptor();
			var interfaces = new Type[] { typeof(INotifyPropertyChanged) };

			if (target != null) {

				return ProxyGenerator.CreateClassProxyWithTarget(
					pluginType,
					interfaces,
					target,
					ProxyGenerationOptions.Default,
					interceptors);

			} else {

				return ProxyGenerator.CreateClassProxy(
						pluginType,
						interfaces,
						ProxyGenerationOptions.Default,
						interceptors);

			}

		}

		public static bool IsProxiedModelType(Type modelType) {

			var isProxiedType = false;

			if (typeof(IProxyTargetAccessor).IsAssignableFrom(modelType) &&
				modelType.IsSubclassOf(typeof(Model))) {

				isProxiedType = true;

			}

			return isProxiedType;

		}

		public static Type GetNonProxiedModelType(Type modelType) {

			if (IsProxiedModelType(modelType)) {

				return modelType.BaseType;

			} else {

				return modelType;

			}

		}

	}

}
