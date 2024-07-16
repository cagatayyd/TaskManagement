using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using TaskManagement.Data.Context;

namespace TaskManagement.Services.EmployeeTask
{
	public partial class EmployeeTaskService : IEmployeeTaskService
	{
		private readonly AppDbContext _context;

		public EmployeeTaskService(AppDbContext context)
		{
			_context = context;
		}
		public async Task<List<CorporateTask>> GetTasksForUserAsync(string userId)
		{
			return await _context.CorporateTasks
				.Include(t => t.EmployeeTasks)
				.ThenInclude(et => et.Employee)
				.Where(t => t.EmployeeTasks.Any(et => et.EmployeeId.ToString() == userId))
				.ToListAsync();
		}

		public async Task<bool> BeginTaskAsync(int taskId)
		{
			var task = await _context.CorporateTasks.FindAsync(taskId);
			if (task == null)
			{
				return false;
			}

			task.IsInProgress = true;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<CorporateTask> GetTaskDetailsAsync(int id)
		{
			var tasks = await _context.CorporateTasks
				.Include(t => t.EmployeeTasks)
				.ThenInclude(et => et.Employee)
				.FirstOrDefaultAsync(t => t.Id == id);
			return tasks;
		}
		public bool UpdateCompletionProgress(int id, string value, out string errorMessage)
		{
			errorMessage = null;
			var task = _context.CorporateTasks.Find(id);

			if (task == null)
			{
				errorMessage = "Car not found.";
				return false;
			}

			if (!int.TryParse(value, out int completionProgress))
			{
				errorMessage = "Invalid value for CompletionProgress.";
				return false;
			}

			task.CompletionProgress = completionProgress;
			if (completionProgress == 100)
			{
				task.IsFinish = true;
			}
			_context.SaveChanges();

			return true;
		}
	}
}
