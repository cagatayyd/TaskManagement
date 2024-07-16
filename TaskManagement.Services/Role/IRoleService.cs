using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;

namespace TaskManagement.Services.Role
{
	public partial interface IRoleService
	{
		public Task<List<AppRole>> GetAllRolesAsync();
		public Task<IdentityResult> CreateRoleAsync(string roleName);
		public Task<AppRole> GetRoleByIdAsync(string id);
		public Task<IdentityResult> DeleteRoleAsync(string id);
		public Task<IdentityResult> UpdateRoleAsync(AppRole role);
		public Task<IdentityResult> AddUserToRoleAsync(Employee user, string role);
		public Task<IdentityResult> RemoveUserFromRoleAsync(Employee user, string role);
		Task<IList<string>> GetUserRolesAsync(Employee user);
	}
}
