using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Web.Areas.Admin.Models;

namespace TaskManagement.Areas.Admin.Factories
{
    public partial class RoleModelFactory : IRoleModelFactory
    {
        #region Props

        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        #endregion

        #region Ctor
        public RoleModelFactory(IMapper mapper, UserManager<Employee> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        #endregion

        #region Methods

        public virtual Task<RoleModel> PrepareRoleModelAsync(RoleModel model, AppRole entity)
        {
            if (entity != null)
            {
                if (model == null)
                {
                    model = _mapper.Map<RoleModel>(entity);
                }
            }
            return Task.FromResult(model);
        }

        public async Task<List<RoleModel>> PrepareRoleModelListAsync(IEnumerable<AppRole> roles)
        {
            var roleModelList = _mapper.Map<List<RoleModel>>(roles);

            foreach (var roleModel in roleModelList)
            {
                var role = roles.FirstOrDefault(u => u.Id == roleModel.Id);

                if (role != null)
                {
                    roleModel.Name = role.Name;
                }
            }

            return roleModelList;
        }



        #endregion

    }
}
