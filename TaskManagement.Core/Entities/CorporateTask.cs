using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Entities
{
	public partial class CorporateTask : BaseEntity
	{
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Detail { get; set; }
		public bool IsApproved { get; set; }
		public bool IsInProgress { get; set; }
		public int CompletionProgress { get; set; }
		public bool IsFinish { get; set; }
		public ICollection<EmployeeTask> EmployeeTasks { get; set; } = new List<EmployeeTask>();
	}
}
