using XerShade.Website.Core.Areas.Account.Data.Models;
using XerShade.Website.Core.Areas.Account.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace XerShade.Website.Core.Areas.Account.Controllers;

[Area("Account")]
public class SessionController(SignInManager<ApplicationUser> signInManager) : Controller
{
    private readonly SignInManager<ApplicationUser> signInManager = signInManager;

    [HttpGet]
    public IActionResult Login() => this.View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (this.ModelState.IsValid)
        {
            SignInResult result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return this.Url.IsLocalUrl(returnUrl) ? this.Redirect(returnUrl) : (IActionResult)this.Redirect("~/");
            }
            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return this.View(model);
        }
        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await this.signInManager.SignOutAsync();
        return this.Redirect("~/");
    }
}
