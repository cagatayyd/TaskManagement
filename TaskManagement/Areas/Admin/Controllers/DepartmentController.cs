using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Services.Departments;

namespace TaskManagement.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DepartmentController : Controller
	{
		private readonly IDepartmentService _departmentService;
		private readonly IMapper _mapper;

		public DepartmentController(IDepartmentService departmentService, IMapper mapper)
		{
			_departmentService = departmentService;
			_mapper = mapper;
		}

		public async Task<IActionResult> List()
		{
			var departments = await _departmentService.GetAllDepartmentsAsync();
			var departmentModels = _mapper.Map<List<DepartmentModel>>(departments);

			return View(departmentModels);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] DepartmentModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var department = new Department { Name = model.Name };
					var createdDepartment = await _departmentService.CreateDepartmentAsync(department);

					return Json(new { success = true, department = new { Id = createdDepartment.Id, Name = createdDepartment.Name } });
				}
				catch (ArgumentNullException ex)
				{
					return Json(new { success = false, message = "Invalid data: " + ex.Message });
				}
				catch (Exception ex)
				{
					// Log the exception (ex) to a logging framework or database if necessary
					return StatusCode(500, new { success = false, message = "Internal server error: " + ex.Message });
				}
			}

			return Json(new { success = false, message = "Invalid data." });
		}


		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _departmentService.DeleteDepartmentAsync(id);
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}



	}
}
