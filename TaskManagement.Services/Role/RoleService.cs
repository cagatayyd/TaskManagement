using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using TaskManagement.Data.Context;

namespace TaskManagement.Services.Role
{
	public partial class RoleService : IRoleService
	{
		private readonly UserManager<Employee> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly AppDbContext _context;

		public RoleService(UserManager<Employee> userManager, RoleManager<AppRole> roleManager, AppDbContext context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		public async Task<List<AppRole>> GetAllRolesAsync()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return roles;
		}
		public async Task<IdentityResult> CreateRoleAsync(string roleName)
		{
			var role = new AppRole { Name = roleName };
			var result = await _roleManager.CreateAsync(role);
			return result;
		}

		public async Task<AppRole> GetRoleByIdAsync(string id)
		{
			var role = await _context.Roles.FindAsync(id);
			return role;
		}
		public async Task<IdentityResult> UpdateRoleAsync(AppRole role)
		{
			return await _roleManager.UpdateAsync(role);
		}
		public async Task<IdentityResult> DeleteRoleAsync(string id)
		{
			var roleToDelete = await _roleManager.FindByIdAsync(id);
			if (roleToDelete == null)
			{
				throw new Exception("Silinebilecek rol bulunamamıştır.");
			}

			return await _roleManager.DeleteAsync(roleToDelete);
		}
		public async Task<IdentityResult> AddUserToRoleAsync(Employee user, string role)
		{
			return await _userManager.AddToRoleAsync(user, role);
		}

		public async Task<IdentityResult> RemoveUserFromRoleAsync(Employee user, string role)
		{
			return await _userManager.RemoveFromRoleAsync(user, role);
		}
		public async Task<IList<string>> GetUserRolesAsync(Employee user)
		{
			return await _userManager.GetRolesAsync(user);
		}
	}
}
