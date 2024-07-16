using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;

namespace TaskManagement.Areas.Admin.Factories
{
    public partial interface ICorporateTaskModelFactory
    {
        public Task<IEnumerable<CorporateTaskModel>> PrepareCorporateTaskModelForListAsync();
        public Task<CorporateTaskModel> PrepareCorporateTaskModelForDetailAsync(int id);

    }
}
