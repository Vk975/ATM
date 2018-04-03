using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API.Services
{
    public interface IValidationService
    {
        int GetAccountType(string accountType);
        bool ValidateAccountType(string accountType);
        bool ValidateAmount(int amount);
    }
}
