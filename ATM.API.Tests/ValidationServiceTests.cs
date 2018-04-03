using ATM.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.API.Tests
{
    [TestClass]
    public class ValidationServiceTests
    {
        IValidationService _validations;
        public ValidationServiceTests() 
        {
            _validations = new ValidationService();
        }


        [TestMethod]
        public void Account_Savings_Valid()
        {
            var response = _validations.GetAccountType("savings");
            Assert.AreEqual(response, 1);
        }

        [TestMethod]
        public void Account_Savings_Invalid()
        {
            var response = _validations.GetAccountType("Savings1");
            Assert.AreEqual(response, 0);
        }

        [TestMethod]
        public void Account_Validation_null()
        {
            var response = _validations.ValidateAccountType(null);
            Assert.AreEqual(response, false);
        }

        [TestMethod]
        public void Account_Validation_Valid()
        {
            var response = _validations.ValidateAccountType("Savings");
            Assert.AreEqual(response, true);
        }

        [TestMethod]
        public void Account_Validation_Invalid()
        {
            var response = _validations.ValidateAccountType("Save");
            Assert.AreEqual(response, false);
        }

        [TestMethod]
        public void Account_Cheque()
        {
            var response = _validations.GetAccountType("cheque");
            Assert.AreEqual(response, 2);
        }

        [TestMethod]
        public void Validate_Amount_Valid() 
        {
            var response = _validations.ValidateAmount(20);
            Assert.AreEqual(response,true);
        }

        [TestMethod]
        public void Validate_Amount_Invalid_0()
        {
            var response = _validations.ValidateAmount(0);
            Assert.AreEqual(response, false);
        }


        [TestMethod]
        public void Validate_Amount_Invalid_nonDivisible()
        {
            var response = _validations.ValidateAmount(11);
            Assert.AreEqual(response, false);
        }
    }
}
