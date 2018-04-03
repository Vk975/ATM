using ATM.API.Models;
using ATM.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATM.API.Tests
{
    [TestClass]
    public class BankRepositoryTests
    {
        Context _context;
        public BankRepositoryTests()
        {
            _context = new TestDataContext().GetDataContext();

        }

        [TestMethod]
        public async Task User_Valid()
        {

            BankRepository svc = new BankRepository(_context);

            var user = await svc.ValidateUserAsync("1234567890123456", "1234");

            Assert.AreEqual(user.UserId, "C00001");
            Assert.AreEqual(user.Name, "Customer 1");
        }

        [TestMethod]
        public async Task User_Invalid()
        {

            BankRepository svc = new BankRepository(_context);

            var user = await svc.ValidateUserAsync("234567890123456", "1234");

            Assert.AreEqual(user, null);
        }

        [TestMethod]
        public async Task Balance_valid()
        {
            Repositories.BankRepository svc = new Repositories.BankRepository(_context);
            var balance = await svc.GetBalanceAsync("C00001", 1);
            Assert.AreEqual(balance, "5000");
        }

        [TestMethod]
        public async Task Balance_Invalid()
        {
            Repositories.BankRepository svc = new Repositories.BankRepository(_context);
            var balance = await svc.GetBalanceAsync("C000011", 1);
            Assert.AreEqual(balance, null);
        }

        [TestMethod]
        public async Task Debit_Account_Invalid()
        {
            Repositories.BankRepository svc = new Repositories.BankRepository(_context);
            var response = await svc.DebitAsync("C000011", 1, 400); 
            Assert.AreEqual(response.InvalidAccount, true);
            Assert.AreEqual(response.Success, false);
            Assert.AreEqual(response.InsufficientBalance, false);
        }

        [TestMethod]
        public async Task Debit_Account_InsufficientBalance()
        {
            Repositories.BankRepository svc = new Repositories.BankRepository(_context);
            var response = await svc.DebitAsync("C00001", 1, 5001);
            Assert.AreEqual(response.InvalidAccount, false);
            Assert.AreEqual(response.Success, false);
            Assert.AreEqual(response.InsufficientBalance, true);
        }

        [TestMethod]
        public async Task Debit_Account_Success() 
        {
            using (var tempContext = new TestDataContext(Guid.NewGuid().ToString()).GetDataContext())
            {
                Repositories.BankRepository svc = new Repositories.BankRepository(tempContext);

                Random rnd = new Random();
                int random = rnd.Next(1, 21);
                int amount = 5 * random;
                int balance = 5000 - amount;


                var response = await svc.DebitAsync("C00001", 1, amount);

                Assert.AreEqual(response.InvalidAccount, false);
                Assert.AreEqual(response.Success, true);
                Assert.AreEqual(response.InsufficientBalance, false);
                Assert.AreEqual(response.Balance, balance);

            }

        }





    }
}
