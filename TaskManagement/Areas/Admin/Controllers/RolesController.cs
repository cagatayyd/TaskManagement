using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskManagement.Web.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Services.User;
using TaskManagement.Services.Role;
using TaskManagement.Areas.Admin.Factories;
using TaskManagement.Areas.Admin.Models;

namespace TaskManagement.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	public class RolesController : Controller
	{
		#region Fields
		private readonly IRoleService _roleService;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly IRoleModelFactory _roleModelFactory;
		#endregion

		#region Ctor
		public RolesController(IRoleService roleService, RoleManager<AppRole> roleManager, IMapper mapper, IUserService userService, IRoleModelFactory roleModelFactory)
		{
			_roleService = roleService;
			_roleManager = roleManager;
			_mapper = mapper;
			_userService = userService;
			_roleModelFactory = roleModelFactory;
		}
		#endregion

		#region Methods
		public async Task<IActionResult> List()
		{
			var roles = await _roleService.GetAllRolesAsync();
			var roleModels = await _roleModelFactory.PrepareRoleModelListAsync(roles); // Await the async method here
			return View(roleModels);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] RoleModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _roleService.CreateRoleAsync(model.Name);

				if (result.Succeeded)
				{
					return Json(new { success = true, role = new { Id = model.Id, Name = model.Name } });
				}
				else
				{
					return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
				}
			}
			return Json(new { success = false, message = "Geçersiz veri." });
		}
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var roleToUpdate = await _roleService.GetRoleByIdAsync(id);

			if (roleToUpdate == null)
			{
				throw new Exception("Güncellenecek rol bulunamamıştır.");
			}

			var model = _mapper.Map<RoleModel>(roleToUpdate);


			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(RoleModel request)
		{
			var roleToUpdate = await _roleService.GetRoleByIdAsync(request.Id.ToString());

			if (roleToUpdate == null)
			{
				throw new Exception("Güncellenecek rol bulunamamıştır.");
			}

			roleToUpdate.Name = request.Name;

			var updateResult = await _roleService.UpdateRoleAsync(roleToUpdate);

			if (!updateResult.Succeeded)
			{
				throw new Exception("Rol güncelleme işlemi başarısız: " + string.Join(", ", updateResult.Errors.Select(e => e.Description)));
			}

			ViewData["SuccessMessage"] = "Rol bilgisi güncellenmiştir";

			return View();
		}

		public async Task<IActionResult> Delete(string id)
		{
			var deleteResult = await _roleService.DeleteRoleAsync(id);

			TempData["SuccessMessage"] = "Rol silinmiştir.";

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> AssignRoleToUser(string id)
		{
			var currentUser = await _userService.GetUserByIdAsync(id);
			if (currentUser == null)
			{
				return NotFound();
			}

			ViewBag.userId = id;

			try
			{
				var roles = await _roleService.GetAllRolesAsync();
				var userRoles = await _roleService.GetUserRolesAsync(currentUser);

				var roleModelList = _mapper.Map<List<RoleModel>>(roles);

				foreach (var role in roleModelList)
				{
					role.Exist = userRoles.Contains(role.Name);
				}

				return View(roleModelList);
			}
			catch (Exception ex)
			{
				// Handle exception or log it
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPost]
		public async Task<IActionResult> AssignRoleToUser(string userId, List<RoleModel> requestList)
		{
			var userToAssignRoles = await _userService.GetUserByIdAsync(userId);
			if (userToAssignRoles == null)
			{
				return NotFound();
			}

			foreach (var role in requestList)
			{
				if (role.Exist)
				{
					await _roleService.AddUserToRoleAsync(userToAssignRoles, role.Name);
				}
				else
				{
					await _roleService.RemoveUserFromRoleAsync(userToAssignRoles, role.Name);
				}
			}

			return RedirectToAction(nameof(RolesController.List), "List");
		}


		#endregion
	}
}
