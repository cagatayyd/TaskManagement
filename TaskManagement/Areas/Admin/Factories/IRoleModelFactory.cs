using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;

namespace TaskManagement.Areas.Admin.Factories
{
    public partial interface IRoleModelFactory
    {
        public Task<RoleModel> PrepareRoleModelAsync(RoleModel model, AppRole entity);
        public Task<List<RoleModel>> PrepareRoleModelListAsync(IEnumerable<AppRole> roles);

    }
}
