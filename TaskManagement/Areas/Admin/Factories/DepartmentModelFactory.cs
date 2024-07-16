using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagement.Core.Entities;

namespace TaskManagement.Areas.Admin.Factories
{
    public partial class DepartmentModelFactory : IDepartmentModelFactory
    {
        public List<SelectListItem> CreateSelectListItems(List<Department> departments)
        {
            return departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();
        }
    }
}
