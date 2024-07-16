using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Entities;

namespace TaskManagement.Data.Context
{
	public class AppDbContext : IdentityDbContext<Employee, AppRole, int>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<EmployeeTask> EmployeeTasks { get; set; }
		public DbSet<CorporateTask> CorporateTasks { get; set; }
		public DbSet<Department> Departments { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Global query filter for soft delete
			base.OnModelCreating(builder);

			// Global query filter for Employee
			builder.Entity<Employee>().HasQueryFilter(m => !m.IsDeleted);

			// Global query filter for CorporateTask
			builder.Entity<CorporateTask>().HasQueryFilter(m => !m.IsDeleted);

			// Global query filter for EmployeeTask
			builder.Entity<EmployeeTask>().HasQueryFilter(m => !m.IsDeleted);

			// Composite primary key definition for EmployeeTask
			builder.Entity<EmployeeTask>().HasKey(et => new { et.EmployeeId, et.TaskId });

			// Define relationship between EmployeeTask and Employee
			builder.Entity<EmployeeTask>()
				.HasOne(et => et.Employee)
				.WithMany(e => e.EmployeeTasks)
				.HasForeignKey(et => et.EmployeeId);

			// Define relationship between EmployeeTask and CorporateTask
			builder.Entity<EmployeeTask>()
				.HasOne(et => et.CorporateTasks)
				.WithMany(ct => ct.EmployeeTasks)
				.HasForeignKey(et => et.TaskId);
		}
	}
}
