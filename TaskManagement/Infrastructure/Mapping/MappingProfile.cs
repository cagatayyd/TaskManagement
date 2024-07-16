using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Web.Areas.Admin.Models;
using TaskManagement.Web.ViewModels;

namespace TaskManagement.Infrastructure.Mapping
{
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Employee, UserModel>()
                    .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                    .ForMember(dest => dest.Roles, opt => opt.Ignore());
                CreateMap<CorporateTask, CorporateTaskModel>()
                    .ForMember(dest => dest.EmployeeNames, opt => opt.MapFrom(src => src.EmployeeTasks.Select(et => et.Employee.Name).ToList()))
                    .ForMember(dest => dest.EmployeeSurnames, opt => opt.MapFrom(src => src.EmployeeTasks.Select(et => et.Employee.Surname).ToList()))
                    .ForMember(dest => dest.EmployeeDepartments, opt => opt.MapFrom(src => src.EmployeeTasks.Select(et => et.Employee.Department.Name).ToList()));
                CreateMap<SignUpViewModel, Employee>();
                CreateMap<CreateUserViewModel, Employee>();
                CreateMap<IdentityRole, RoleModel>();
                CreateMap<IdentityRole, RoleModel>()
                    .ForMember(dest => dest.Exist, opt => opt.Ignore());
                CreateMap<Department, DepartmentModel>();
                CreateMap<AppRole, RoleModel>();
                CreateMap<CorporateTask, CorporateTaskModel>()
                    .ForMember(dest => dest.EmployeeTasks, opt => opt.MapFrom(src => src.EmployeeTasks.Select(et => et.EmployeeId).ToList()))
                    .ReverseMap()
                    .ForMember(dest => dest.EmployeeTasks, opt => opt.Ignore());

                // Mapping for EmployeeTask to int (EmployeeId)
                CreateMap<EmployeeTask, int>().ConvertUsing(et => et.EmployeeId);
			CreateMap<Employee, UserViewModel>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname));
			CreateMap<Employee, UserModel>();
		}
	}
}
