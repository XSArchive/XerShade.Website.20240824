using XerShade.Website.Core.Areas.Account.Data.Models;
using XerShade.Website.Core.Areas.Account.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Asreyion.Core.Areas.Account.Controllers;

[Area("Account")]
public class ManagementController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
{
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly SignInManager<ApplicationUser> signInManager = signInManager;

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
