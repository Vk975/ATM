using ATM.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API.Repositories
{
    public class BankRepository : DisposableBase, IBankRepository
    {
        private  readonly Context _dbContext;

        public BankRepository(Context dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Debit Operation
        /// </summary>
        /// <param name="userId">Customer ID</param>
        /// <param name="accountType">Account Type</param>
        /// <param name="amount">debit Amount</param>
        /// <returns>Balance</returns>
        public async Task<TransactionResponse> DebitAsync(string userId, int accountType, int amount)
        {

            TransactionResponse response = new TransactionResponse();

            var account = await _dbContext.Accounts.Include("Customer").
        Where(a => a.AccountType == accountType && a.Customer.UserId.ToLower() == userId.ToLower()).FirstOrDefaultAsync();

            if (account == null)
            {
                response.InvalidAccount = true;
            }
            else
            {
                if (account.Balance < amount)
                    response.InsufficientBalance = true;
                else
                {
                    account.Balance -= amount;
                    int dbUpdateResult = await _dbContext.SaveChangesAsync();

                    if (dbUpdateResult > 0)
                    {
                        response.Success = true;
                        response.Balance = account.Balance;
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// returns balance for selected account
        /// </summary>
        /// <param name="userId">Customer ID</param>
        /// <param name="accountType">Account Type</param>
        /// <returns>Balance</returns>
        public async Task<string> GetBalanceAsync(string userId, int accountType)
        {
            var account = await _dbContext.Accounts.Include("Customer").
        Where(a => a.AccountType == accountType && a.Customer.UserId.ToLower() == userId.ToLower()).FirstOrDefaultAsync();

            return account?.Balance.ToString();
        }

        /// <summary>
        /// Validate login details 
        /// </summary>
        /// <param name="userName">Card Number</param>
        /// <param name="password">Pin</param>
        /// <returns>User Details</returns>
        public async Task<User> ValidateUserAsync(string userName, string password)
        {
            //Get card, Account and User deatils if the card number is valid
            var card = await _dbContext.Cards.Include("Account").Include("Account.Customer").Where(c => c.CardNumber.ToLower().Trim() == userName.ToLower().Trim()).FirstOrDefaultAsync();

            //Credential validation
            if (card != null && card.Pin.Equals(password, StringComparison.OrdinalIgnoreCase))
            {
                if (card.Account != null && card.Account.Customer != null)
                {
                    return card.Account.Customer;
                }
            }

            //return null if card is not found or Pin doesn't match
            return null;
        }


    }
}
