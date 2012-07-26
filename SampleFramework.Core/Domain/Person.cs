using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Core.Domain {

	public class Person : AuditableModel {

		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }

	}

}
