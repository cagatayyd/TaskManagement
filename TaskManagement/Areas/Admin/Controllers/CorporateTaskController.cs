using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Areas.Admin.Factories;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Services.CorporateTasks;
using TaskManagement.Services.Email;

namespace TaskManagement.Areas.Admin.Controllers
{
	[Area("Admin")]

	public class CorporateTaskController : Controller
	{
		#region Fields
		private readonly IEmailService _emailService;
		private readonly ICorporateTaskService _corporateTaskService;
		private readonly IMapper _mapper;
		private readonly ICorporateTaskModelFactory _corporateTaskModelFactory;
		private readonly UserManager<Employee> _userManager;

		#endregion

		#region Ctor
		public CorporateTaskController(IEmailService emailService, ICorporateTaskService corporateTaskService, IMapper mapper, ICorporateTaskModelFactory corporateTaskModelFactory, UserManager<Employee> userManager)
		{
			_corporateTaskService = corporateTaskService;
			_emailService = emailService;
			_mapper = mapper;
			_corporateTaskModelFactory = corporateTaskModelFactory;
			_userManager = userManager;
		}
		#endregion

		#region Methods
		[HttpGet]
		[Authorize(Roles = "Manager,Admin")]
		public async Task<IActionResult> List()
		{
			var models = await _corporateTaskModelFactory.PrepareCorporateTaskModelForListAsync();
			return View(models);
		}
		[HttpGet]
		[Authorize(Roles = "Manager,Admin")]
		public async Task<IActionResult> Create()
		{
			var employees = await _corporateTaskService.GetEmployeesForTaskCreationAsync();
			ViewData["Employees"] = employees;

			return View();
		}



		[HttpPost]
		[Authorize(Roles = "Manager,Admin")]
		public async Task<IActionResult> Create(CorporateTaskModel model)
		{
			if (model == null)
			{
				return BadRequest("Model cannot be null");
			}

			var corporateTask = _mapper.Map<CorporateTask>(model);

			string baseUrl = Url.Action("TaskDetails", "EmployeeTask", null, Request.Scheme);
			await _corporateTaskService.CreateCorporateTaskAsync(corporateTask, model.EmployeeTasks, baseUrl);

			return RedirectToAction("List");
		}



		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _corporateTaskService.DeleteCorporateTaskAsync(id);
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}


		[HttpPost]
		[Authorize(Roles = "Manager,Admin")]
		public async Task<IActionResult> ApproveTask(int id)
		{
			try
			{
				var result = await _corporateTaskService.ApproveTaskAsync(id);
				return Json(new { success = result });
			}
			catch (ArgumentException ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
			catch (KeyNotFoundException ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
			catch (Exception ex)
			{
				// Log the exception (ex) to a logging framework or database if necessary
				return StatusCode(500, new { success = false, message = "Internal server error: " + ex.Message });
			}
		}
		[HttpGet]
		[Authorize(Roles = "Employee,Manager,Admin")]
		public async Task<IActionResult> Details(int id)
		{
			try
			{
				var taskViewModel = await _corporateTaskModelFactory.PrepareCorporateTaskModelForDetailAsync(id);
				return View(taskViewModel);
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
			catch (Exception ex)
			{
				// Log the exception (ex) to a logging framework or database if necessary
				return StatusCode(500, new { success = false, message = "Internal server error: " + ex.Message });
			}
		}







		#endregion

	}
}
