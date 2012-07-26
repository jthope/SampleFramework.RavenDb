using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleFramework.Core.Domain;

namespace SampleFramework.Core.Services {

	public interface IAuditLogService : IBaseService {

		IQueryable<AuditLog> GetList(int take, int skip, int page, int pageSize, ref int totalRecords);

	}

}
