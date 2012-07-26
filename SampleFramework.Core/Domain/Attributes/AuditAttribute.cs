using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Core.Domain {

	public class AuditAttribute : Attribute {

		#region Members

		private bool _isFieldAuditable = true;

		#endregion

		#region Constructors

		public AuditAttribute() {
			_isFieldAuditable = true;
		}

		public AuditAttribute(bool isFieldAuditable) {
			_isFieldAuditable = isFieldAuditable;
		}

		#endregion

		#region Public Properties

		public bool IsFieldAuditable {
			get { return _isFieldAuditable; }
		}

		#endregion

	}

}
