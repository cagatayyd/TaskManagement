using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Entities
{
	public partial class Employee : IdentityUser<int>
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string? Picture { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public ICollection<EmployeeTask> EmployeeTasks { get; set; } = new List<EmployeeTask>();
		public Department Department { get; set; }
		public int DepartmentId { get; set; }
	}
}
