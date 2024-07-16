using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Core.Entities;
using TaskManagement.Factories;
using TaskManagement.Services.Email;
using TaskManagement.ViewModels;

namespace TaskManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ICorporateTaskModelFactory _corporateTaskModelFactory;

        public HomeController(ILogger<HomeController> logger, ICorporateTaskModelFactory corporateTaskModelFactory, UserManager<Employee> userManager, SignInManager<Employee> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _corporateTaskModelFactory = corporateTaskModelFactory;

		}

		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userName = User.Identity!.Name!;
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.NameSurname = currentUser.Name + " " + currentUser.Surname;

			var viewModel = await _corporateTaskModelFactory.CreateCorporateTaskModelAsync(userId, userName);

			return View(viewModel);
		}

		public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            returnUrl ??= Url.Action("Index", "Home");

            _logger.LogInformation("Attempting to find user by email: {Email}", model.Email);

            var hasUser = await _userManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre hatalı.");
                return View(model);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password,false, lockoutOnFailure: true);

            if (signInResult.Succeeded)
            {
                bool isFirstLogin = !hasUser.LastLoginDate.HasValue;

                hasUser.LastLoginDate = DateTime.UtcNow;
                await _userManager.UpdateAsync(hasUser);

                if (isFirstLogin)
                {
                    _logger.LogInformation("User {Email} is logging in for the first time.", model.Email);

                    // Şifre yenileme bağlantısını oluştur
                    string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
                    var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

                    // Şifre yenileme bağlantısını e-posta ile gönder
                    await _emailService.SendResetPasswordEmail(passwordResetLink!, hasUser.Email!);

                    // Kullanıcıyı hesaptan çıkar
                    await _signInManager.SignOutAsync();

                    return RedirectToAction("SignIn", "Home");
                }

                return LocalRedirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                TempData["ErrorMessage"] = "Çok fazla deneme yaptınız. 3 dakika boyunca giriş yapamazsınız.";
                return View(model);
            }

            TempData["ErrorMessage"] = $"Email veya şifre hatalı. Başarısız giriş sayısı: {await _userManager.GetAccessFailedCountAsync(hasUser)}";

            return View(model);
        }



        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine ait kullanıcı bulunamadı.");
                return View(request);
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordResetLink!, hasUser.Email!);

            TempData["SuccessMessage"] = "Şifre yenileme linki e-posta adresinize gönderilmiştir";

            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamamıştır.");
                return View();
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarı ile yenilenmiştir.";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Şifre yenilenirken hata oluştu.");
            }

            return View();
        }
	}
}
