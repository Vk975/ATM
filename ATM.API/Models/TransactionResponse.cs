using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API.Models
{
    public class TransactionResponse
    {
        public bool Success { get; set; }
        public bool InsufficientBalance { get; set; }
        public bool InvalidAccount { get; set; }
        public int Balance { get; set; }  
    }
}
