using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;

namespace SampleFramework.Core.Domain {

	[Serializable]
	public abstract class Model : INotifyPropertyChanged {

		#region Properties

		public virtual string Id { get; set; }

		[JsonIgnore]
		[Audit(false)]
		public Enumerations.EntityState EntityState { get; set; }

		[JsonIgnore]
		[Audit(false)]
		public virtual bool IsPersisted {
			get {
				return !String.IsNullOrEmpty(Id);
			}
		}

		[JsonIgnore]
		[Audit(false)]
		public bool IsDirty {
			get { return this.EntityState != Enumerations.EntityState.Unchanged; }
		}

		#endregion

		public Model() {

			this.EntityState = Enumerations.EntityState.Unchanged;

		}

		public event PropertyChangedEventHandler PropertyChanged;

	}

}
