using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SampleFramework.Core.Domain;
using SampleFramework.Core.Util;
using SampleFramework.Core.Services;

namespace SampleFramework.Web {

	public partial class Default : System.Web.UI.Page {

		protected void Page_Load(object sender, EventArgs e) {

			if (!IsPostBack) {

				BindPeopleGrid();
				BindAuditTrailGrid();

			}

		}

		public void BindPeopleGrid() {

			var totalPeeps = 0;

			uxPeopleGrid.DataSource = DocumentHelper.GetRepository
													.GetList<Person>(uxPeopleGrid.PageSize,
														uxPeopleGrid.CurrentPageIndex * uxPeopleGrid.PageSize,
														uxPeopleGrid.CurrentPageIndex,
														uxPeopleGrid.PageSize,
														ref totalPeeps)
													.OrderBy(x => x.ModifiedOn)
													.ToList();

			uxPeopleGrid.VirtualItemCount = totalPeeps;
			uxPeopleGrid.DataBind();

		}

		public void BindAuditTrailGrid() {

			var totalAudits = 0;

			uxAuditTrailGrid.DataSource =
				ServiceFactory.Instance.Get<IAuditLogService>()
									   .GetList(uxAuditTrailGrid.PageSize,
										   uxAuditTrailGrid.CurrentPageIndex * uxAuditTrailGrid.PageSize,
										   uxAuditTrailGrid.CurrentPageIndex,
										   uxAuditTrailGrid.PageSize,
										   ref totalAudits)
									   .OrderByDescending(x => x.ModifiedOn)
									   .ToList();

			uxAuditTrailGrid.VirtualItemCount = totalAudits;
			uxAuditTrailGrid.DataBind();

		}

		protected void uxPeopleGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {

			uxPeopleGrid.CurrentPageIndex = e.NewPageIndex;
			BindPeopleGrid();

		}

		protected void uxAuditTrailGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {

			uxAuditTrailGrid.CurrentPageIndex = e.NewPageIndex;
			BindAuditTrailGrid();

		}

		protected void uxAddPeople_Click(object sender, EventArgs e) {

			var repository = DocumentHelper.GetRepository;
			repository.Purge<Person>();
			repository.Purge<AuditLog>();

			for (var i = 0; i < 10; i++) {

				var person = ModelFactory.Instance.Get<Person>();
				person.FirstName = "JT " + i.ToString();
				person.LastName = "Hope " + i.ToString();

				repository.Store(person);

			}

			repository.Commit();

			BindPeopleGrid();
			BindAuditTrailGrid();

		}

		protected void uxEditRecord_Click(object sender, EventArgs e) {

			var random = new Random();
			var randomNumber = random.Next(1000);

			// lets edit a record to show the changes in the audit trail
			var repository = DocumentHelper.GetRepository;

			var person = repository.GetById<Person>("people/1");
			person.FirstName = "JT EDIT " + randomNumber.ToString();
			person.LastName = "Hope EDIT";

			repository.Store(person);
			repository.Commit();

			BindPeopleGrid();
			BindAuditTrailGrid();

		}

	}

}