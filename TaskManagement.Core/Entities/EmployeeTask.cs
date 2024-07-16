using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Entities
{
	public partial class EmployeeTask : BaseEntity
	{
		public int EmployeeId;
		public Employee Employee { get; set; }

		public int TaskId { get; set; }
		public CorporateTask CorporateTasks { get; set; }
		public bool IsDeleted { get; set; }

	}
}
