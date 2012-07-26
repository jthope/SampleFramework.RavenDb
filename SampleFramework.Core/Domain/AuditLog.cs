using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SampleFramework.Core.Domain {

	public class AuditLogChangedField {

		[Audit(false)]
		public virtual string FieldName { get; set; }

		[Audit(false)]
		public virtual string OldValue { get; set; }

		[Audit(false)]
		public virtual string NewValue { get; set; }

	}

	public class AuditLog : Model {

		[Audit(false)]
		public string Entity { get; set; }

		[Audit(false)]
		public string ModificationType { get; set; }

		[Audit(false)]
		public string EntityId { get; set; }

		[Audit(false)]
		public string ModifiedBy { get; set; }

		[Audit(false)]
		public DateTime ModifiedOn { get; set; }

		[Audit(false)]
		public List<AuditLogChangedField> ChangedFields { get; set; }

		[JsonIgnore]
		[Audit(false)]
		public Dictionary<string, string> OriginalFields;

		public AuditLog() {

			ChangedFields = new List<AuditLogChangedField>();
			OriginalFields = new Dictionary<string, string>();

		}

		public static AuditLog CreateGeneralAudit(string modification, string modifiedBy) {

			var auditLog = new AuditLog();
			auditLog.ModifiedOn = DateTime.UtcNow;
			auditLog.ModificationType = modification;
			auditLog.ModifiedBy = modifiedBy;

			return auditLog;

		}

	}

}
