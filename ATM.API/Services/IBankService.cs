using ATM.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API.Services
{
    public interface IBankService
    {
        Task<User> FindUserAsync(string userId, string password);
        Task<string> GetBalanceAsync(string userId, string accountType);
        Task<TransactionResponse> DebitAsync(string userId, string accountType, int amount);
    }
}
