using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signMgr) : Controller
    {
        private UserManager<IdentityUser> userManager = userMgr;
        private SignInManager<IdentityUser> signInManager = signMgr;
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                Name = string.Empty,
                Password = string.Empty,
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await userManager.FindByNameAsync(loginModel.Name);

                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/Admin");
                    }
                }
                ModelState.AddModelError("", "Invalid name or password");
            }
            return View(loginModel);
        }

        [Authorize]
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}