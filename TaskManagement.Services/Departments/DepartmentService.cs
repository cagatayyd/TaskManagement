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

namespace TaskManagement.Services.Departments
{
	public partial class DepartmentService : IDepartmentService
	{
		#region Fields
		private readonly AppDbContext _context;
		private readonly UserManager<Employee> _userManager;
		private readonly IEmailService _emailService;
		private readonly IMapper _mapper;
		#endregion

		#region Ctor
		public DepartmentService(AppDbContext appDbContext, UserManager<Employee> userManager, IEmailService emailService, IMapper mapper)
		{
			_context = appDbContext;
			_userManager = userManager;
			_emailService = emailService;
			_mapper = mapper;
		}
		#endregion

		#region Methods
		public async Task<List<Department>> GetAllDepartmentsAsync()
		{
			return await _context.Departments.ToListAsync();
		}
		public async Task<Department> CreateDepartmentAsync(Department department)
		{
			if (department == null)
			{
				throw new ArgumentNullException(nameof(department));
			}

			_context.Departments.Add(department);
			await _context.SaveChangesAsync();

			return department;
		}
		public async Task DeleteDepartmentAsync(int id)
		{
			var department = await _context.Departments.FindAsync(id);
			if (department == null)
			{
				throw new Exception("Görev bulunamamıştır.");
			}
			department.IsDeleted = true;
			await _context.SaveChangesAsync();
		}

		public async Task<Department> GetDepartmentByIdAsync(int id)
		{
			var department = await _context.Departments.FindAsync(id);
			if (department == null)
			{
				throw new ArgumentNullException(nameof(department));
			}
			return department;
		}
		#endregion
	}
}
