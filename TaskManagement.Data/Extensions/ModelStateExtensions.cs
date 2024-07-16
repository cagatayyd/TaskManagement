using Microsoft.AspNetCore.Identity;
using System.Web.Mvc;

namespace TaskManagement.Data.Extensions
{
	public static class ModelStateExtensions
	{
		public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errors)
		{
			errors.ForEach(x =>
			{
				modelState.AddModelError(string.Empty, x);
			});
		}

		public static void AddModelErrorList(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
		{
			errors.ToList().ForEach(x =>
			{
				modelState.AddModelError(string.Empty, x.Description);
			});
		}
	}
}
