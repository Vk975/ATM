using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ATM.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using ATM.Web.Repository;
using Microsoft.Extensions.Logging;

namespace ATM.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Constructors

        private readonly IAccountRepositoryService _service;
        private readonly ILogger _logger;


        public AccountController(IAccountRepositoryService service, ILogger<AccountController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #endregion

        #region Log in and log out

        /// <summary>
        /// Login Action Page
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginInfo());
        }

        /// <summary>
        /// Login Action Postback
        /// </summary>
        /// <param name="info">Login Information</param>
        [HttpPost]
        public async Task<IActionResult> Login(LoginInfo info)
        {
            if(ModelState.IsValid)
            {
                //Get User information based on Login information
                var response = await _service.GetUserInformation(info.LoginId, info.Password);

                //validate User information
                if (response != null)
                {
                    //Add claims for the User
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, response.UserDisplayName),
                    new Claim(ClaimTypes.Sid, response.UserId)
                };

                    var userIdentity = new ClaimsIdentity(claims, "login");

                    //Log in User
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);

                    //Redirect to our transactions after logging in. 
                    return RedirectToAction("Transactions", "Home");
                }
                else
                {
                    info.ErrorMesage.Add("Invalid Login Details. Please try again.");
                }
            }

            return View(info);
        } 


        /// <summary>
        /// Log out avtion
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //Singining out user and redirecting to Home
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion
    }
}