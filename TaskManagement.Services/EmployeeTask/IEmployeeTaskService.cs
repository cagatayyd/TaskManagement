using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;

namespace TaskManagement.Services.EmployeeTask
{
	public partial interface IEmployeeTaskService
	{
		public Task<List<CorporateTask>> GetTasksForUserAsync(string userId);
		public Task<bool> BeginTaskAsync(int taskId);
		public Task<CorporateTask> GetTaskDetailsAsync(int id);
		public bool UpdateCompletionProgress(int id, string value, out string errorMessage);


	}
}
