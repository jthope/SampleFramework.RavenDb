using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleFramework.Core.Repository;
using SampleFramework.Core.Domain;

namespace SampleFramework.Core.Services {

	public class AuditLogService : BaseService, IAuditLogService {

		public AuditLogService(IRavenDbRepository repository)
			: base(repository) {

			this.repository = repository;

		}

		public IQueryable<AuditLog> GetList(int take, int skip, int page, int pageSize, ref int totalRecords) {

			var auditLogs = repository
							.GetList<AuditLog>(take, skip, page, pageSize, ref totalRecords)
							.OrderByDescending(x => x.ModifiedOn)
							.AsQueryable();

			return auditLogs;

		}

	}

}
