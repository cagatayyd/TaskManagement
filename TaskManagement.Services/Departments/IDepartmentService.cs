using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;

namespace TaskManagement.Services.Departments
{
	public partial interface IDepartmentService
	{
		public Task<List<Department>> GetAllDepartmentsAsync();
		public Task<Department> CreateDepartmentAsync(Department department);
		public Task DeleteDepartmentAsync(int id);
		public Task<Department> GetDepartmentByIdAsync(int id);

	}
}
