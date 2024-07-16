using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;
using TaskManagement.Data.Context;
using TaskManagement.Services.Email;

namespace TaskManagement.Services.User
{
	public partial class UserService : IUserService
	{
		#region Fields
		private readonly AppDbContext _context;
		private readonly UserManager<Employee> _userManager;
		private readonly IEmailService _emailService;
		private readonly IMapper _mapper;
		#endregion

		#region Ctor
		public UserService(AppDbContext appDbContext, UserManager<Employee> userManager, IEmailService emailService, IMapper mapper)
		{
			_context = appDbContext;
			_userManager = userManager;
			_emailService = emailService;
			_mapper = mapper;
		}
		#endregion

		#region Methods

		public async Task<IEnumerable<Employee>> GetAllUsersAsync()
		{
			var userList = await _userManager.Users
											 .Include(u => u.Department)
											 .ToListAsync();
			return userList;
		}
		public async Task<IdentityResult> CreateUserAsync(Employee user, string password)
		{
			var identityResult = await _userManager.CreateAsync(user, password);
			if (identityResult.Succeeded)
			{
				var createdUser = await _userManager.FindByEmailAsync(user.Email);
				if (createdUser != null)
				{
					createdUser.EmailConfirmed = true;
					var roleResult = await _userManager.AddToRoleAsync(createdUser, "Employee");
					if (!roleResult.Succeeded)
					{
						return roleResult;
					}
				}
			}
			return identityResult;
		}
		public async Task<Employee> GetUserByIdAsync(string id)
		{
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);
			return user;
		}
		public async Task DeleteUser(string id)
		{
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);
			user.IsDeleted = true;
		}
		public async Task<Employee> GetUserByUserNameAsync(string userName)
		{
			var currentUser = await _userManager.FindByNameAsync(userName);

			var employee = new Employee
			{
				Name = currentUser.Name,
				Surname = currentUser.Surname,
			};

			return employee;
		}
	}

	#endregion
}
