using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XerShade.Website.Core.Areas.User.Data.Models;
using XerShade.Website.Core.Areas.User.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace XerShade.Website.Core.Areas.User.Controllers;

[Area("User")]
public class SessionController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
{
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly SignInManager<ApplicationUser> signInManager = signInManager;

    [HttpGet]
    public IActionResult Login() => this.View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "")
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

    [HttpGet]
    public IActionResult Register() => this.View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (this.ModelState.IsValid)
        {
            ApplicationUser user = new() { UserName = model.Email, Email = model.Email };
            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                return this.Redirect("~/");

            }
            foreach (IdentityError error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return this.View(model);
    }
}
