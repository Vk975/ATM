using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATM.API.Models;
using ATM.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ATM.API.Services
{
    public class BankService : DisposableBase, IBankService
    {
        private readonly IBankRepository _repo;
        private readonly IValidationService _validations;
        private readonly ILogger<BankService> _logger;

        public BankService(IBankRepository repo, IValidationService validationSvc, ILogger<BankService> logger)
        {
            _repo = repo;
            _validations = validationSvc;
        }

        /// <summary>
        /// Debit account with given amount
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="accountType">Account Type</param>
        /// <param name="amount">Amount</param>
        /// <returns></returns>
        public async Task<TransactionResponse> DebitAsync(string userId, string accountType, int amount)
        {
            try
            {
                return await _repo.DebitAsync(userId, _validations.GetAccountType(accountType), amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message); 
                throw;
            }
        }
        /// <summary>
        /// Validates User details - Card and Pin
        /// </summary>
        /// <param name="userName">Card Number</param>
        /// <param name="password">PIN</param>
        /// <returns></returns>
        public async Task<User> FindUserAsync(string userName, string password)
        {
            try
            {
                return await _repo.ValidateUserAsync(userName, password);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Provides account balance 
        /// </summary>
        /// <param name="userId">Customer ID</param>
        /// <param name="accountType">Account Type</param>
        /// <returns></returns>
        public async Task<string> GetBalanceAsync(string userId, string accountType)
        {
            try
            {
                return await _repo.GetBalanceAsync(userId, _validations.GetAccountType(accountType));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
