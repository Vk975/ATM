using ATM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Repository
{
    public interface IAccountRepositoryService
    {
        Task<UserInfo> GetUserInformation(string userName, string pwd);
        Task<string> GetBalanceAsync(string custid, string type);
        Task<WithdrawlResponse> WithdrawAmountAsync(string custId, string accountType, int withdrawlamount);
    }
}
