using ATM.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API.Repositories
{
    public interface IBankRepository
    {
        Task<User> ValidateUserAsync(string userName, string password);
        Task<string> GetBalanceAsync(string userId, int accountType);
        Task<TransactionResponse> DebitAsync(string userId, int accountType, int amount);
    }
}
