using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ATM.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ATM.Web.Repository;
using Microsoft.Extensions.Logging;

namespace ATM.Web.Controllers
{
    public class HomeController : BaseController
    {

        #region Constructor
        private readonly IAccountRepositoryService _service;
        private readonly ILogger _logger;


        public HomeController(IAccountRepositoryService service, ILogger<HomeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #endregion

        /// <summary>
        /// Home Page - Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                if (!string.IsNullOrEmpty(GetIdentity()))
                    return RedirectToAction("Transactions");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            return View();

        }


        #region Bank Transactions

        /// <summary>
        /// Shows configured accounts in application
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Transactions()
        {
            return View();
        }

        /// <summary>
        /// Provides balance for selected account
        /// </summary>
        /// <param name="accType">Acount Type</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Balance(string accType)
        {
            //validate if account type is specified
            if (string.IsNullOrEmpty(accType))
            {
                _logger.LogWarning("Invalid account name");
                return Redirect("/");
            }

            string custId = GetIdentity();

            //validate if customer ID is available
            if (string.IsNullOrEmpty(custId))
            {
                _logger.LogWarning("Invalid Customer Id");
                return Redirect("/");
            }
            else
            {
                try
                {
                    var response = await _service.GetBalanceAsync(custId, accType);
                    ViewBag.Balance = response;
                    ViewBag.AccountType = accType;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }

            return View();
        }

        /// <summary>
        /// Withdraw page
        /// </summary>
        /// <param name="accType">Account Type</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Withdraw(string accType)
        {
            //validate if account type is specified
            if (string.IsNullOrEmpty(accType))
            {
                _logger.LogWarning("Invalid account name");
                return Redirect("/");
            }

            //getting Customer Id from claim
            string custId = GetIdentity();

            //validate if customer ID is available
            if (string.IsNullOrEmpty(custId))
            {
                _logger.LogWarning("Invalid Customer ID");
                return Redirect("/");
            }
            else
            {
                //Get available balance
                try
                {
                    var response = await _service.GetBalanceAsync(custId, accType);
                    var viewResponse = new WithdrawlRequest() { AccountType = accType, Amount = 0, Balance = response };

                    if (string.IsNullOrEmpty(response))
                    {
                        viewResponse.InvalidAccount = true;
                        viewResponse.ErrorMesage.Add("Invalid account details or Insufficient balance. Please try again");
                    }

                    //initialize View
                    return View(viewResponse);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }

        }

        /// <summary>
        /// Withdraw response Page
        /// </summary>
        /// <param name="command">Withdrawl Option</param>
        /// <param name="data">Withdrawl request data</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Withdraw(string command, WithdrawlRequest data)
        {
            int withdrawlamount = 0;
            bool invalid = false;


            //Custom Amount
            if (command == "amount")
            {
                withdrawlamount = data.Amount;

                //Validation if amount is in multiples of $5 and not zero
                if (withdrawlamount == 0 || withdrawlamount % 5 != 0)
                {
                    data.ErrorMesage.Add("Amount should be greater than zero and in multiples of $5");
                }

            }
            else
            {
                //invalid amount input
                if (!int.TryParse(command, out withdrawlamount))
                    invalid = true;
            }


            if (invalid)
                data.ErrorMesage.Add("Invalid Selection");

            //If no validation errors are raised, proceed with the call.
            if (data.ErrorMesage.Count == 0)
            {
                try
                {
                    string custId = GetIdentity();
                    var response = await _service.WithdrawAmountAsync(custId, data.AccountType, withdrawlamount);

                    if (response.Success)
                    {
                        data.SuccessMessage.Add("Transaction Completed. Updated balance: $" + response.Balance);
                        data.Balance = response.Balance.ToString();
                    }
                    else
                    {
                        //Process error message from service
                        data.ErrorMesage.Add(response.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }

            return View(data);
        }

        #endregion

        //Default error page
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
