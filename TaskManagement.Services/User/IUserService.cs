using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;

namespace TaskManagement.Services.User
{
	public partial interface IUserService
	{
		Task<IEnumerable<Employee>> GetAllUsersAsync();
		Task<IdentityResult> CreateUserAsync(Employee user, string password);
		Task<Employee> GetUserByIdAsync(string id);
		Task DeleteUser(string id);
		Task<Employee> GetUserByUserNameAsync(string userName);

	}
}
