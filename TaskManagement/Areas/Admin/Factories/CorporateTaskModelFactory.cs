using AutoMapper;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Services.CorporateTasks;

namespace TaskManagement.Areas.Admin.Factories
{
    public partial class CorporateTaskModelFactory : ICorporateTaskModelFactory
    {
        #region Props

        private readonly IMapper _mapper;
        private readonly ICorporateTaskService _corporateTaskService;

        #endregion

        #region Ctor
        public CorporateTaskModelFactory(IMapper mapper, ICorporateTaskService corporateTaskService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _corporateTaskService = corporateTaskService ?? throw new ArgumentNullException(nameof(corporateTaskService));
        }
        #endregion

        #region Methods

        public virtual async Task<CorporateTaskModel> PrepareCorporateTaskModelForDetailAsync(int id)
        {
            var task = await _corporateTaskService.GetTaskDetailsAsync(id);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            var model = new CorporateTaskModel
            {
                Id = task.Id,
                Name = task.Name,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                Detail = task.Detail,
                IsApproved = task.IsApproved,
                IsInProgress = task.IsInProgress,
                CompletionProgress = task.CompletionProgress,
                EmployeeNames = task.EmployeeTasks.Select(et => et.Employee.Name).ToList(),
                EmployeeSurnames = task.EmployeeTasks.Select(et => et.Employee.Surname).ToList(),
                EmployeeDepartments = task.EmployeeTasks.Select(et => et.Employee.Department.Name).ToList()
            };

            return model;
        }


        public virtual async Task<IEnumerable<CorporateTaskModel>> PrepareCorporateTaskModelForListAsync()
        {
            var tasks = await _corporateTaskService.GetAllCorporateTasksAsync();
            var resultModels = _mapper.Map<IEnumerable<CorporateTaskModel>>(tasks);
            return resultModels;
        }
    }

        #endregion
    }
