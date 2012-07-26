namespace SampleFramework.Core.Infrastructure {

	public class Singleton<T> where T : class, new() {

		private static object _sync = new object();
		private static volatile T _instance = null;

		static Singleton() { }

		public static T Instance {

			get {

				if (_instance == null) {

					lock (_sync) {

						if (_instance == null) {
							_instance = new T();
						}

					}

				}

				return _instance;

			}

		}

	}

}
