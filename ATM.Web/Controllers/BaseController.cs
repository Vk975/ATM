using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ATM.Web.Controllers
{
    public abstract class BaseController : Controller
    {  
        /// <summary>
        /// Retrieves Customer Identity from claims
        /// </summary>
        /// <returns>Customer ID</returns>
        protected string GetIdentity()
        {
            if(User != null && User.Identity != null)
            {
                var userIdentifier  = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid);
                return userIdentifier?.Value;
            }
            return null;
        }




    }
}