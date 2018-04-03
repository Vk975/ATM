using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Pin { get; set; }
        public Account Account { get; set; }
    }
}
