using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TaskManagement.Core.Entities;
using TaskManagement.Web.Areas.Admin.Models;

namespace TaskManagement.Areas.Admin.Models;

public class AdminModel
{
	public IEnumerable<Employee> Users { get; set; }
	public List<string> Roles { get; set; }
	public IList<CorporateTask> CorporateTasks { get; set; }
	public List<UserModel> UserModel { get; set; }
	public List<SelectListItem> Departments { get; set; }

}
