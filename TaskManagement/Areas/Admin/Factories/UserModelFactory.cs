using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Areas.Admin.Models;
using TaskManagement.Core.Entities;
using TaskManagement.Services.CorporateTasks;
using TaskManagement.Services.User;
using TaskManagement.Web.Areas.Admin.Models;

namespace TaskManagement.Areas.Admin.Factories
{
    public partial class UserModelFactory : IUserModelFactory
    {
        #region Props

        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<Employee> _userManager;

        #endregion

        #region Ctor
        public UserModelFactory(IMapper mapper, IUserService userService, UserManager<Employee> userManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userManager = userManager;

        }
        #endregion

        #region Methods

        public virtual Task<UserModel> PrepareUserModelAsync(UserModel model,Employee entity)
        {
            if (entity != null)
            {
                if (model == null)
                {
                    model = _mapper.Map<UserModel>(entity);
                }
            }
            return Task.FromResult(model);
        }

        public async Task<List<UserModel>> PrepareUserModelListAsync(IEnumerable<Employee> users)
        {
            var userModelList = _mapper.Map<List<UserModel>>(users);

            foreach (var userModel in userModelList)
            {
                var user = users.First(u => u.Id.ToString() == userModel.Id);
                var roles = await _userManager.GetRolesAsync(user);
                userModel.Roles = roles.ToList();
            }

            return userModelList;
        }

        #endregion
    }
}
