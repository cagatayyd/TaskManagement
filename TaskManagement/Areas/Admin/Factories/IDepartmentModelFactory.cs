using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagement.Core.Entities;

namespace TaskManagement.Areas.Admin.Factories
{
    public partial interface IDepartmentModelFactory
    {
            List<SelectListItem> CreateSelectListItems(List<Department> departments);
    }
}
