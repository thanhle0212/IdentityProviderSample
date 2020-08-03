using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied(string ReturnUrl = "")
        {
            return View();
        }

        public async Task<IActionResult> Logout(string returnUrl = "")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if(returnUrl != "")
            {
                return Redirect(returnUrl);
            }
            else
            {
                return View("Logout");
            }
        }
    }
}