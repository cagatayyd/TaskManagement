using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using TaskManagement.Services.CorporateTasks;
using TaskManagement.Services.EmployeeTask;
using TaskManagement.Services.User;
using TaskManagement.Web.ViewModels;

namespace TaskManagement.Factories
{
	public partial class CorporateTaskModelFactory : ICorporateTaskModelFactory
	{
		#region Props 

		private readonly IUserService _userService;
		private readonly ICorporateTaskService _corporateTaskService;
		private readonly IMapper _mapper;
		private readonly IEmployeeTaskService _employeeTaskService;

		#endregion

		#region Ctor

		public CorporateTaskModelFactory(IUserService userService, ICorporateTaskService corporateTaskService, IMapper mapper, IEmployeeTaskService employeeTaskService)
		{
			_userService = userService;
			_corporateTaskService = corporateTaskService;
			_mapper = mapper;
			_employeeTaskService = employeeTaskService;
		}

		#endregion

		#region Methods

		public async Task<CorporateTaskModel> CreateCorporateTaskModelAsync(string userId, string userName)
		{
			var employee = await _userService.GetUserByUserNameAsync(userName);
			var tasks = await _employeeTaskService.GetTasksForUserAsync(userId);

			var employeeViewModel = _mapper.Map<UserViewModel>(employee);

			var corporateTaskModel = new CorporateTaskModel
			{
				Name = employeeViewModel.Name,
				EmployeeSurnames = new List<string> { employeeViewModel.Surname },
				Tasks = tasks,
			};

			return corporateTaskModel;
		}
		public async Task<List<CorporateTaskModel>> GetChartData(string userId, string userName)
		{
			var tasks = await _employeeTaskService.GetTasksForUserAsync(userId);

			var viewModel = tasks.Select(t => new CorporateTaskModel
			{
				Id = t.Id,
				Name = t.Name,
				StartDate = t.StartDate,
				EndDate = t.EndDate,
				Detail = t.Detail,
				IsApproved = t.IsApproved,
				IsInProgress = t.IsInProgress,
				CompletionProgress = t.CompletionProgress,
				EmployeeNames = t.EmployeeTasks.Select(et => et.Employee.Name).ToList(),
				EmployeeSurnames = t.EmployeeTasks.Select(et => et.Employee.Surname).ToList(),
				EmployeeTasks = t.EmployeeTasks.Select(et => et.Id).ToList(),
			}).ToList();

			return viewModel;
		}
	}

		#endregion
	}
