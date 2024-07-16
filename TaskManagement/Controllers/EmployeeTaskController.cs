// EmployeeTaskController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Data.Context;
using TaskManagement.Factories;
using TaskManagement.Services.EmployeeTask;

namespace TaskManagement.Controllers
{
    public class EmployeeTaskController : Controller
    {
        private readonly IEmployeeTaskService _employeeTaskService;
        private readonly IMapper _mapper;
        private readonly ICorporateTaskModelFactory _corporateTaskModelFactory;

        public EmployeeTaskController(IEmployeeTaskService employeeTaskService, IMapper mapper, ICorporateTaskModelFactory corporateTaskModelFactory)
        {
            _employeeTaskService = employeeTaskService;
            _mapper = mapper;
            _corporateTaskModelFactory = corporateTaskModelFactory;


		}

		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userName = User.Identity!.Name!;

			var viewModel = await _corporateTaskModelFactory.CreateCorporateTaskModelAsync(userId, userName);

			// Fetching all corporate tasks and counts
			var corporateTasks = await _employeeTaskService.GetTasksForUserAsync(userId);
			ViewBag.TaskCount = corporateTasks.Where(c => c.IsFinish == false).Count();
			ViewBag.FinishedTaskCount = corporateTasks.Where(c => c.IsFinish == true).Count();

			return View(viewModel);
		}

		[HttpPost]
        public async Task<IActionResult> BeginTask(int id)
        {
            try
            {
                if (id == 0)
                {
                    return Json(new { success = false, message = "Invalid task ID." });
                }

                var success = await _employeeTaskService.BeginTaskAsync(id);
                if (!success)
                {
                    return Json(new { success = false, message = "Task not found." });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) to a logging framework or database if necessary
                return StatusCode(500, new { success = false, message = "Internal server error: " + ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Manager,Admin")]
        public async Task<IActionResult> TaskDetails(int id)
        {
            var task = await _employeeTaskService.GetTaskDetailsAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            var taskViewModel = _mapper.Map<TaskModel>(task);


            return View(taskViewModel);
        }
        [HttpPost]
		public IActionResult UpdateCompletionProgress(int id, string value)
		{
			if (_employeeTaskService.UpdateCompletionProgress(id, value, out string errorMessage))
			{
				return Ok();
			}
			return BadRequest(errorMessage);
		}
		[HttpGet]
		public async Task<JsonResult> GetChartData()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userName = User.Identity!.Name!;

			var viewModel = await _corporateTaskModelFactory.GetChartData(userId, userName);
			var chartData = viewModel.Select(c => new
			{
				c.Id,
				c.Name,
				c.StartDate,
				c.EndDate,
				c.Detail,
				c.IsApproved,
				c.IsInProgress,
				c.CompletionProgress,
				c.EmployeeNames,
				c.EmployeeSurnames,
			}).OrderBy(c => c.Name).Where(c =>c.IsApproved == true && c.CompletionProgress != 100).ToList();

			return Json(chartData);
		}


	}
}