using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using TaskManagement.Core.Entities;

namespace TaskManagement.Web.TagHelpers
{
    public class UserRoleNamesTagHelper:TagHelper
    {
        public string UserId { get; set; } = null!;

        private readonly UserManager<Employee> _userManager;

        public UserRoleNamesTagHelper(UserManager<Employee> userManager)
        {
            _userManager = userManager;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            var userRoles = await _userManager.GetRolesAsync(user!);

            var stringBuilder = new StringBuilder();

            userRoles.ToList().ForEach(x =>
            {
                stringBuilder.Append(@$"<span class='badge bg-secondary mx-1'>{x.ToLower()}</span>");
            });
            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
