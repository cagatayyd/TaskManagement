using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using TaskManagement.Core.Entities;
using TaskManagement.Data.Context;
using TaskManagement.Services.User;
using TaskManagement.Services.Departments;
using TaskManagement.Services.Email;
using TaskManagement.Services.EmployeeTask;
using TaskManagement.Services.CorporateTasks;
using TaskManagement.Areas.Admin.Factories;
using TaskManagement.Infrastructure.Mapping;
using TaskManagement.Services.Role;
using System.Configuration;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("SqlCon"),
		b => b.MigrationsAssembly("TaskManagement.Data")
	)
);

builder.Services.AddIdentity<Employee, AppRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();



builder.Services.ConfigureApplicationCookie(opt =>
{
	var cookieBuilder = new CookieBuilder
	{
		Name = "TaskManagement"
	};
	opt.LoginPath = new PathString("/Home/Signin");
	opt.LogoutPath = new PathString("/Member/Logout");
	opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
	opt.Cookie = cookieBuilder;
	opt.ExpireTimeSpan = TimeSpan.FromDays(60);
	opt.SlidingExpiration = true;
});

#region DI
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeTaskService, EmployeeTaskService>();
builder.Services.AddScoped<ICorporateTaskService, CorporateTaskService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICorporateTaskModelFactory, CorporateTaskModelFactory>();
builder.Services.AddScoped<IDepartmentModelFactory, DepartmentModelFactory>();
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.Configure<TaskManagement.Services.Email.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserModelFactory, UserModelFactory>();
builder.Services.AddScoped<TaskManagement.Factories.ICorporateTaskModelFactory, TaskManagement.Factories.CorporateTaskModelFactory>();

#endregion


builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doğrulama middleware'i ekleyin
app.UseAuthorization();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=SignIn}/{id?}");


app.Run();
