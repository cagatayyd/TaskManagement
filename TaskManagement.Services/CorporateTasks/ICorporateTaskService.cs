using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManagement.Core.Entities;

namespace TaskManagement.Services.CorporateTasks
{
	public partial interface ICorporateTaskService
	{
		public Task<IList<CorporateTask>> GetAllCorporateTasksAsync();
		public Task CreateCorporateTaskAsync(CorporateTask model, IList<int> employeeIds, string baseUrl);
		public Task<List<SelectListItem>> GetEmployeesForTaskCreationAsync();
		public Task DeleteCorporateTaskAsync(int id);
		public Task<bool> ApproveTaskAsync(int id);
		Task<CorporateTask> GetTaskDetailsAsync(int id);

	}
}
