using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Models
{
    /// <summary>
    /// Request Object for withdrawl transaction
    /// </summary>
    public class WithdrawlRequest : BaseView
    {
        public string Balance { get; set; }
        public int Amount { get; set; }
        public string AccountType { get; set; }

        public bool InvalidAccount { get; set; }
    }

    /// <summary>
    /// Response object from Withdrawl transaction
    /// </summary>
    public class WithdrawlResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int Balance { get; set; }
    }


}
