using ATM.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.API.Tests
{
    public class TestDataContext : IDisposable
    {
        Context _context;

        public TestDataContext(string name = "ATMTestDb")
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(databaseName: name).Options;
            _context = new Context(options);

            AddTestData(_context);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;

            }
        }

        public Context GetDataContext()
        {
            return _context;
        }

        private void AddTestData(Models.Context ctx)
        {
            ctx.Customers.Add(new Models.User()
            {
                Id = 1,
                Name = "Customer 1",
                UserId = "C00001",
                Accounts = new List<Models.Account>()
                {
                    new Models.Account()
                    {
                        Id = 1,
                        AccountNumber = "12345678",
                        AccountType = 1,
                        Balance = 5000,
                        Cards = new List<Models.Card>()
                        {
                            new Models.Card()
                            {
                                Id = 1,
                                CardNumber = "1234567890123456",
                                Pin = "1234"
                            },

                            new Models.Card()
                            {
                                Id = 2,
                                CardNumber = "2345678901234567",
                                Pin = "2345"
                            },
                        }
                    }
                }
            });

            ctx.Customers.Add(new Models.User()
            {
                Id = 2,
                Name = "Customer 2",
                UserId = "C00001",
                Accounts = new List<Models.Account>()
                {
                    new Models.Account()
                    {
                        Id = 2,
                        AccountNumber = "23456789",
                        AccountType = 1,
                        Balance = 5000,
                        Cards = new List<Models.Card>()
                        {
                            new Models.Card()
                            {
                                Id = 3,
                                CardNumber = "3456789012345689",
                                Pin = "3456"
                            },

                            new Models.Card()
                            {
                                Id = 4,
                                CardNumber = "4567890123456789",
                                Pin = "4567"
                            },
                        }
                    }
                }
            });

            ctx.SaveChangesAsync();
        }
    }
}
