using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Web.Models
{
    public class BaseView
    {
        public List<string> ErrorMesage { get; set; }
        public List<string> SuccessMessage { get; set; }

        public BaseView()
        {
            ErrorMesage = new List<string>();
            SuccessMessage = new List<string>();
        }
    }
}
