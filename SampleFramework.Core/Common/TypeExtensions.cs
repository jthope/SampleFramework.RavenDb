using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SampleFramework.Core.Common {

	public static class TypeExtensions {

		public static IEnumerable<Type> GetBaseTypes(this Type type) {

			var baseTypes = new List<Type> { type.BaseType };

			if (type.BaseType != typeof(object))
				baseTypes.AddRange(GetBaseTypes(type.BaseType));

			return baseTypes;

		}

		public static ConstructorInfo GetDefaultCtor(this Type type) {
			return type.GetConstructor(new Type[0]);
		}

	}

}
