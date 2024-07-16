using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Areas.Admin.Factories;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Services.Departments;
using TaskManagement.Services.User;
using TaskManagement.Web.Areas.Admin.Models;

namespace TaskManagement.Areas.Admin.Controllers
{
	[Area("Admin")]

	public class UserController : Controller
	{
		#region Props

		private readonly UserManager<Employee> _userManager;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly IDepartmentService _departmentService;
		private readonly IDepartmentModelFactory _departmentModelFactory;
		private readonly IUserModelFactory _userModelFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;

		#endregion

		#region Ctor 

		public UserController(UserManager<Employee> userManager,IHttpContextAccessor httpContextAccessor, IUserService userService, IMapper mapper, IDepartmentService departmentService, IDepartmentModelFactory departmentModelFactory, IUserModelFactory userModelFactory)
		{
			_userManager = userManager;
			_userService = userService;
			_mapper = mapper;
			_departmentService = departmentService;
			_departmentModelFactory = departmentModelFactory;
			_userModelFactory = userModelFactory;
			_httpContextAccessor = httpContextAccessor;
		}

		#endregion

		#region Methods
		[HttpGet]
		public async Task<IActionResult> List()
		{
			// Fetching all users
			var users = await _userService.GetAllUsersAsync();
			var userModelList = await _userModelFactory.PrepareUserModelListAsync(users);

			// Fetching the current user
			var currentUser = await _userManager.GetUserAsync(User);

			ViewBag.NameSurname = currentUser.Name + " " + currentUser.Surname;

			// Fetching all departments
			var departments = await _departmentService.GetAllDepartmentsAsync();
			var selectListItems = _departmentModelFactory.CreateSelectListItems(departments);

			var viewModel = new UserListAndCreateModel()
			{
				UserModel = userModelList ?? new List<UserModel>(), // Ensure UserModel is not null
				CreateUserViewModel = new CreateUserViewModel(),
				Departments = selectListItems ?? new List<SelectListItem>(), // Ensure Departments is not null
			};

			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var departments = await _departmentService.GetAllDepartmentsAsync();
			var selectListItems = _departmentModelFactory.CreateSelectListItems(departments);

			var model = new SignUpViewModel
			{
				Departments = selectListItems
			};

			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateUserViewModel request)
		{
			var departments = await _departmentService.GetAllDepartmentsAsync();
			request.Departments = _departmentModelFactory.CreateSelectListItems(departments);

			var department = await _departmentService.GetDepartmentByIdAsync(request.DepartmentId);

			var user = _mapper.Map<Employee>(request);
			user.UserName = user.Email;
			var identityResult = await _userService.CreateUserAsync(user, request.Password);

			if (identityResult.Succeeded)
			{
				TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarı ile gerçekleşmiştir";
				return RedirectToAction(nameof(Create));
			}

			request.Errors = identityResult.Errors.Select(x => x.Description).ToList();
			return View(request);
		}


		public async Task<IActionResult> DeleteUser(string id)
		{
			try
			{
				var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);
				if (user != null)
				{
					user.IsDeleted = true;
					await _userManager.UpdateAsync(user);
					return Json(new { success = true });
				}
				else
				{
					return Json(new { success = false, message = "Kullanıcı bulunamadı." });
				}
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}
        [HttpPost]
        public async Task<IActionResult> CreateAjax(CreateUserViewModel request)
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            request.Departments = _departmentModelFactory.CreateSelectListItems(departments);

            var department = await _departmentService.GetDepartmentByIdAsync(request.DepartmentId);

            var user = _mapper.Map<Employee>(request);
            user.UserName = user.Email;
            var identityResult = await _userService.CreateUserAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                return Json(new { success = true, message = "User created successfully." });
            }

            return Json(new { success = false, message = string.Join(", ", identityResult.Errors.Select(e => e.Description)) });
        }
        #endregion
    }
}
