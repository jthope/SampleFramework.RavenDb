using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Omu.ValueInjecter;
using Raven.Client.Embedded;
using StructureMap;
using Raven.Client.Document;
using SampleFramework.Core.Domain;

namespace SampleFramework.Core.Infrastructure {

	public class DPContractResolver : DefaultRavenContractResolver {

		public DPContractResolver() : base(true) { }

		protected override JsonContract CreateContract(Type objectType) {

			if (ProxyUtil.IsProxiedModelType(objectType)) {
				return CreateObjectContract(objectType);
			}

			return base.CreateContract(objectType);

		}

	}

	public class ModelCreationConverter : JsonConverter {

		public override bool CanConvert(Type objectType) {
			return objectType.IsSubclassOf(typeof(Model));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {

			try {

				if (objectType.IsSubclassOf(typeof(Model)) && reader.TokenType == JsonToken.StartObject) {

					var jObject = JObject.Load(reader);
					var actual = JsonConvert.DeserializeObject(jObject.ToString(), objectType);
					var target = ObjectFactory.GetInstance(objectType);

					target = target.InjectFrom(actual);

					return target;

				}

			} catch (Exception ex) {

				throw ex;

			}

			return null;

		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {

			try {

				serializer = new JsonSerializer {
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					ContractResolver = new DPContractResolver()
				};

				if (ProxyUtil.IsProxiedModelType(value.GetType())) {

					var actualObject = Activator.CreateInstance(ProxyUtil.GetNonProxiedModelType(value.GetType()));
					actualObject.InjectFrom<CloneInjection>(value);

					serializer.Serialize(writer, actualObject);

				} else {

					serializer.Serialize(writer, value);

				}

			} catch (Exception ex) {

				// TODO - log instead of throw, probably throwing an exception trying to 
				// serialize an index, not supported anyways.
				ex.ToString();

			}

		}

	}

}
