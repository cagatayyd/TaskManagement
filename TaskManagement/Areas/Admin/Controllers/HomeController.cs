using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Areas.Admin.Factories;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Data;
using TaskManagement.Data.Context;
using TaskManagement.Services.CorporateTasks;
using TaskManagement.Services.Departments;
using TaskManagement.Services.User;
using TaskManagement.Web.Areas.Admin.Models;

namespace TaskManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        #region Props
        
        private readonly AppDbContext _context;
        private readonly UserManager<Employee> _userManager;
        private readonly IUserService _userService;
        private readonly IUserModelFactory _userModelFactory;
        private readonly IDepartmentService _departmentService;
        private readonly IDepartmentModelFactory _departmentModelFactory;
        private readonly ICorporateTaskService _corporateTaskService;
        #endregion

        #region Ctor
        public HomeController(AppDbContext context, UserManager<Employee> userManager,
            IUserService userService,
            IUserModelFactory userModelFactory,
            IDepartmentService departmentService,
            IDepartmentModelFactory departmentModelFactory,ICorporateTaskService corporateTaskService)
        {
            _context = context;
            _userManager = userManager;
            _userService = userService;
            _userModelFactory = userModelFactory;
            _departmentService = departmentService;
            _departmentModelFactory = departmentModelFactory;
            _corporateTaskService = corporateTaskService;
        }
        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Fetching all users
            var users = await _userService.GetAllUsersAsync();
            var userModelList = await _userModelFactory.PrepareUserModelListAsync(users);

            // Fetching current user's roles
            var currentUser = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            // Fetching all corporate tasks
            var corporateTasks = await _corporateTaskService.GetAllCorporateTasksAsync();
            ViewBag.TaskCount = corporateTasks.Where(c=>c.IsFinish == false).Count();
            ViewBag.FinishedTaskCount = corporateTasks.Where(c=>c.IsFinish == true).Count();
            // Fetching all departments
            var departments = await _departmentService.GetAllDepartmentsAsync();
            var selectListItems = _departmentModelFactory.CreateSelectListItems(departments);
            ViewBag.NameSurname = currentUser.Name + " " + currentUser.Surname;
            var taskEmployeeMappings = new Dictionary<int, List<string>>();
            var employeeTasks = _context.EmployeeTasks.ToList();
            foreach (var task in corporateTasks)
            {
                var employeeIds = employeeTasks.Where(et => et.TaskId == task.Id).Select(et => et.EmployeeId).ToList();
                var employeeNames = users.Where(u => employeeIds.Contains(u.Id)).Select(u => $"{u.Name} {u.Surname}").ToList();
                taskEmployeeMappings[task.Id] = employeeNames;
            }

            ViewBag.TaskEmployeeMappings = taskEmployeeMappings;
            // Prepare view model
            var viewModel = new AdminModel()
            {
                Users = users,
                Roles = userRoles.ToList(),
                CorporateTasks = corporateTasks,
                UserModel = userModelList,
                Departments = selectListItems
            };

            // Pass the model to the view
            return View(viewModel);
        }

        #endregion

	}
}
