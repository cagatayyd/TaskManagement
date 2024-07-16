using TaskManagement.Core.Entities;
using TaskManagement.Web.Areas.Admin.Models;

namespace TaskManagement.Areas.Admin.Factories
{
    public partial interface IUserModelFactory
    {
        public Task<List<UserModel>> PrepareUserModelListAsync(IEnumerable<Employee> users);
        public Task<UserModel> PrepareUserModelAsync(UserModel model, Employee entity);


    }
}
