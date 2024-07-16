using TaskManagement.Models;

namespace TaskManagement.Factories
{
	public partial interface ICorporateTaskModelFactory
	{
		public Task<CorporateTaskModel> CreateCorporateTaskModelAsync(string userId, string userName);
		public Task<List<CorporateTaskModel>> GetChartData(string userId, string userName);

	}
}
