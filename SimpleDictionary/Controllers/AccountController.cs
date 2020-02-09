using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleDictionary.Data;
using SimpleDictionary.Models.DataModels;
using SimpleDictionary.Models.ViewModels;
using SimpleDictionary.Services;

namespace SimpleDictionary.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMailSender mailSender;
        private readonly DictionaryContext context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMailSender mailSender, DictionaryContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailSender = mailSender;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    RegistrationDate = DateTime.Now,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Feed");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            User user = await userManager.FindByEmailAsync(model.Email);

            if (ModelState.IsValid && user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.Remember, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Feed");
                } else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }

            }
            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Feed");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            User user = await userManager.FindByEmailAsync(model.Email);

            if(user != null && await userManager.IsEmailConfirmedAsync(user))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, protocol: HttpContext.Request.Scheme); 
                await mailSender.SendEmailAsync(model.Email, "Password reset", $"To reset your password please follow the <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            } else
            {
                ModelState.AddModelError(string.Empty, "Either user with such an email doesn't exist or email hasn't been confirmed");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token = null)
        {
            return token == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("ResetPasswordConfirmation");
            }

            User user = await userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                return View("ResetPasswordConfirmation");
            }

            IdentityResult result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Feed");
            } else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);

        }

        [Authorize]
        public async Task<IActionResult> Profile(string user)
        {
            User appUser = await context.Users.Include(d => d.Definitions).Include(l => l.LikedDefinitions).FirstOrDefaultAsync(x => x.UserName == user);

            if(appUser == null)
            {
                return RedirectToAction("Index", "Feed");
            }
            //Doesn't let other users to access your profile
            else if(!User.Identity.Name.Equals(appUser.UserName))
            {
                return RedirectToAction("UserDefinitions", "Definition", new { userId = appUser.Id });
            }
            else
            {
                return View("Profile", appUser);
            }
        }
    }
}