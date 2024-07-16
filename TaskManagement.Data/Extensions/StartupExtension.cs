using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Core.Entities;
using TaskManagement.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Data.Extensions
{
	public static class StartupExtension
	{
		public static void AddIdentityWithExt(this IServiceCollection services)
		{
			services.Configure<DataProtectionTokenProviderOptions>(opt =>
			{
				opt.TokenLifespan = TimeSpan.FromHours(2);
			});

			services.AddIdentity<Employee, AppRole>(options =>
			{
				options.User.RequireUniqueEmail = true;

				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = false;
				options.Password.RequireDigit = false;

				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
				options.Lockout.MaxFailedAccessAttempts = 3;

			}).AddDefaultTokenProviders()
			.AddEntityFrameworkStores<AppDbContext>();
		}
	}
}
