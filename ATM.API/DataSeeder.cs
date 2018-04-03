using ATM.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.API
{
    static class DataSeeder
    {

        public static void SeedData(Context ctx)
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
                        AccountNumber = "12345678-S",
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
                    },
                    new Models.Account()
                    {
                        Id = 3,
                        AccountNumber = "12345678-C",
                        AccountType = 2,
                        Balance = 10000,
                        Cards = new List<Models.Card>()
                        {
                            new Models.Card()
                            {
                                Id = 5,
                                CardNumber = "5678901234567890",
                                Pin = "5678"
                            },

                            new Models.Card()
                            {
                                Id = 6,
                                CardNumber = "6789012345678901",
                                Pin = "6789"
                            },
                        }
                    },

                }
            });

            ctx.Customers.Add(new Models.User()
            {
                Id = 2,
                Name = "Customer 2",
                UserId = "C00002",
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
                                CardNumber = "3456789012345678",
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

            ctx.SaveChanges();
        }
    }
}
