using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using TaskManagement.Web.ViewModels;
using UserViewModel = TaskManagement.Web.ViewModels.UserViewModel;
using TaskManagement.Core.Entities;

namespace TaskManagement.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<Employee> signInManager, UserManager<Employee> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userViewModel = new UserViewModel
            {
                Email = currentUser!.Email,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture,
            };
            return View(userViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
			return RedirectToAction("SignIn", "Home");
		}
		public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser!, request.OldPassword);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }
            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser!, request.OldPassword, request.NewPassword);
            if (!resultChangePassword.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifre değiştirilirken sorun oluştu";
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.NewPassword, true, false);

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";
            return View();
        }
        public async Task<IActionResult> UserEdit()
        {
            var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;

            var userEditViewModel = new UserEditViewModel()
            {
                Phone = currentUser.PhoneNumber!
            };
            return View(userEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);


            currentUser.PhoneNumber = request.Phone;



            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }
            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                TempData["UpdateErrorMessage"] = "Üye güncellenirken sorun oluştu";
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            TempData["SuccessMessage"] = "Üye bilgileriniz başarıyla değiştirilmiştir";

            var userEditViewModel = new UserEditViewModel()
            {
                Phone = currentUser.PhoneNumber!,
            };

            return View(userEditViewModel);
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;

            message = "Bu sayfayı görmek için yetkiniz yoktur. Yetki almak için yöneticiniz ile görüşün.";

            ViewBag.message = message;

            return View();
        }
    }
}
