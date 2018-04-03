using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public int AccountType { get; set; }
        public int Balance { get; set; }
        public List<Card> Cards { get; set; }
        public virtual User Customer { get; set; }
    }
}
