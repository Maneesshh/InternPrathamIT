using Event.Models;
using Event.ViewModals;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Event.Views.Shared
{
    public class RegisteruserModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        [BindProperty]
        public Register Model { get; set; }
        public RegisteruserModel(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = Model.Email,
                    Email = Model.Email,

                };
              var result= await userManager.CreateAsync(user,Model.Password);
                if (result.Succeeded)
                {
                   await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Index");
                }
                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError("", errors.Description);
                }

            }
            return Page();
        }
    }
}
