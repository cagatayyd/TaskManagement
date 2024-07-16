using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagement.Core.Entities;
using TaskManagement.Models;
using TaskManagement.Web.Areas.Admin.Models;

namespace TaskManagement.Areas.Admin.Models;

public class UserListAndCreateModel
{
    public IEnumerable<Employee> Users { get; set; }
    public List<string> Roles { get; set; }
    public List<CorporateTask> CorporateTasks { get; set; }
    public List<UserModel> UserModel { get; set; }
    public CreateUserViewModel CreateUserViewModel { get; set; }
    public List<SelectListItem> Departments { get; set; }


}