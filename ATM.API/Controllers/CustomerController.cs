using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ATM.API.Models;
using ATM.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ATM.API.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {

        private readonly IBankService _bankService;
        private readonly ILogger _logger;
        private readonly IValidationService _validations;

        public CustomerController(IBankService service, IValidationService validations, ILogger<CustomerController> logger)
        {
            _bankService = service;
            _validations = validations;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves account balance
        /// </summary>
        /// <param name="userId">Customer ID</param>
        /// <param name="accountType">Account Type</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}/{accountType}")]
        public async Task<IActionResult> GetBalanceAsync(string userId, string accountType)
        {

            try
            {
                //Input validations
                if (string.IsNullOrEmpty(userId) ||
                    string.IsNullOrEmpty(accountType))
                    //400
                    return BadRequest();

                if (!_validations.ValidateAccountType(accountType))
                    //400
                    return BadRequest();

                string balance = await _bankService.GetBalanceAsync(userId, accountType);
                if (!string.IsNullOrEmpty(balance))
                    //200
                    return Ok(balance);
                else
                    //404
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //500
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Server Error.");
            }
        }

        /// <summary>
        /// Login Service
        /// </summary>
        /// <param name="data">cardnumber and PIN</param>
        /// <returns>Customer Information</returns>
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody]UserInfo data)
        {
            try
            {
                string userName = data.UserId, password = data.Pwd;

                if (string.IsNullOrEmpty(userName) ||
                    string.IsNullOrEmpty(password) ||
                    userName.Trim().Length != 16 ||
                    password.Trim().Length != 4)
                    //400
                    return BadRequest();

                var user = await _bankService.FindUserAsync(userName, password);
                if (user != null)
                    //200
                    return Ok(new { UserId = user.UserId, UserDisplayName = user.Name });
                else
                    //404
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //500
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError,"Server Error.");
            }
        }

        /// <summary>
        /// Debits account
        /// </summary>
        /// <param name="userId">Customer ID</param>
        /// <param name="accountType">Account Type</param>
        /// <param name="amount">Withdrawl Amount</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{userId}/{accountType}/{amount}")]
        public async Task<IActionResult> DebitAsync(string userId, string accountType, int amount) 
        {

            try
            {
                //Input validations

                if (string.IsNullOrEmpty(userId) ||
                    string.IsNullOrEmpty(accountType))
                    //400
                    return BadRequest();

                if (!_validations.ValidateAccountType(accountType))
                    //400
                    return BadRequest();

                if (!_validations.ValidateAmount(amount))
                    //400
                    return BadRequest(); 


                var response = await _bankService.DebitAsync(userId, accountType, amount); 
                if (response.Success)
                    //200 
                    return Ok(response.Balance);
                else if(response.InvalidAccount)
                    //404
                    return NotFound();
                else
                    //insufficient Funds 
                    return StatusCode(403, "Insufficient funds.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //500
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "Server Error.");
            }
        }




    }
}