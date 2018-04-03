using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API.Services
{
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Maps Account Type to internal account value 
        /// </summary>
        /// <param name="accountType">Account Type</param>
        /// <returns></returns>
        public int GetAccountType(string accountType)
        {
            accountType = accountType.ToLower().Trim();
            if (accountType == "savings")
                return 1;
            if (accountType == "cheque")
                return 2;
            else
                return 0;
        }

        /// <summary>
        /// Validates allowed accounts 
        /// </summary>
        /// <param name="accountType">Account Type</param>
        /// <returns></returns>
        public bool ValidateAccountType(string accountType)
        {
            if (accountType == null)
                return false;
            accountType = accountType.ToLower().Trim();

            return (accountType == "savings" || accountType == "cheque") ?
                true : false;
        }

        /// <summary>
        /// Validate Business rule for allowed withdrawl amount
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <returns></returns>
        public bool ValidateAmount(int amount)
        {
            return (amount != 0 && amount % 5 == 0) ? true : false;
        }

    }
}
