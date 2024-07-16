using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManagement.Core.Entities;
using TaskManagement.Data.Context;
using TaskManagement.Services.Email;

namespace TaskManagement.Services.CorporateTasks
{
	public partial class CorporateTaskService : ICorporateTaskService
	{
		#region Fields
		private readonly AppDbContext _context;
		private readonly UserManager<Employee> _userManager;
		private readonly IEmailService _emailService;
		private readonly IMapper _mapper;
		#endregion

		#region Ctor
		public CorporateTaskService(AppDbContext appDbContext, UserManager<Employee> userManager, IEmailService emailService, IMapper mapper)
		{
			_context = appDbContext;
			_userManager = userManager;
			_emailService = emailService;
			_mapper = mapper;
		}
		#endregion

		#region Methods
		public virtual async Task<IList<CorporateTask>> GetAllCorporateTasksAsync()
		{
			var tasks = await _context.CorporateTasks
				   .Include(t => t.EmployeeTasks)
				   .ThenInclude(et => et.Employee)
				   .ToListAsync();
			return tasks;
		}
		public virtual async Task<List<SelectListItem>> GetEmployeesForTaskCreationAsync()
		{
			string[] rolesToInclude = { "Employee", "Manager" };

			var allUsers = _userManager.Users
				.Include(u => u.Department)
				.Where(u => u.Department.Name != "Yönetici")
				.ToList();

			var usersInRoles = new List<Employee>();
			foreach (var user in allUsers)
			{
				foreach (var role in rolesToInclude)
				{
					if (await _userManager.IsInRoleAsync(user, role))
					{
						usersInRoles.Add(user);
						break;
					}
				}
			}

			var employeeSelectListItems = usersInRoles.Select(user =>
			{
				var departmentName = user.Department != null ? user.Department.Name : "Belirtilmemiş";

				return new SelectListItem
				{
					Value = user.Id.ToString(),
					Text = $"{user.Name} {user.Surname} - {departmentName}"
				};
			}).ToList();

			return employeeSelectListItems;
		}

		public async Task CreateCorporateTaskAsync(CorporateTask model, IList<int> employeeIds, string baseUrl)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}

			if (employeeIds == null || !employeeIds.Any())
			{
				throw new ArgumentException("En az bir çalışan ID'si gereklidir.", nameof(employeeIds));
			}

			try
			{
				// CorporateTask önce eklenir
				_context.CorporateTasks.Add(model);
				await _context.SaveChangesAsync(); // model.Id dolacak

				// EmployeeTask'ler eklenir
				foreach (var employeeId in employeeIds)
				{
					var employeeTask = new Core.Entities.EmployeeTask
					{
						EmployeeId = employeeId,
						TaskId = model.Id // CorporateTask'ın Id'si atanır
					};
					_context.EmployeeTasks.Add(employeeTask);
				}

				await _context.SaveChangesAsync();

				// Email gönderimini arka planda yönetmek için uygun bir yapı kullanılabilir.

				foreach (var employeeId in employeeIds)
				{
					var employee = await _context.Employees.FindAsync(employeeId);
					if (employee != null)
					{
						var hasUser = await _userManager.FindByEmailAsync(employee.Email);
						if (hasUser == null)
						{
							// Kullanıcı bulunamadı hatası daha uygun bir şekilde loglanabilir veya işleme alınabilir.
							// throw new Exception("Kullanıcı bulunamamıştır.");
							continue; // veya işleme devam edilebilir.
						}

						var taskDetailLink = $"{baseUrl}/{model.Id}";
						await _emailService.SendTaskAssignedEmail(hasUser.Name, hasUser.Surname, taskDetailLink, hasUser.Email);
					}
				}
			}
			catch (DbUpdateException ex)
			{
				// Veritabanı işlem hatası yönetimi
				throw new ApplicationException("Görev oluşturma işlemi sırasında bir hata oluştu. Detaylar: " + ex.Message, ex);
			}
		}



		public async Task DeleteCorporateTaskAsync(int id)
		{
			var task = await _context.CorporateTasks.FindAsync(id);
			if (task == null)
			{
				throw new Exception("Görev bulunamamıştır.");
			}
			task.IsDeleted = true;
			await _context.SaveChangesAsync();
		}
		public async Task<bool> ApproveTaskAsync(int id)
		{
			if (id == 0)
			{
				throw new ArgumentException("Invalid task ID.", nameof(id));
			}

			var task = await _context.CorporateTasks.FindAsync(id);
			if (task == null)
			{
				throw new KeyNotFoundException("Task not found.");
			}

			task.IsApproved = true;
			await _context.SaveChangesAsync();

			return true;
		}
		public async Task<CorporateTask> GetTaskDetailsAsync(int id)
		{
			var task = await _context.CorporateTasks
				.Include(t => t.EmployeeTasks)
					.ThenInclude(et => et.Employee)
						.ThenInclude(e => e.Department)
				.FirstOrDefaultAsync(t => t.Id == id);

			if (task == null)
			{
				throw new KeyNotFoundException("Task not found.");
			}

			return task;
		}

	}
	#endregion
}