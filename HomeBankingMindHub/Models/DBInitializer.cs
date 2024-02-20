using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com",
                        FirstName="Victor", LastName="Coronado",
                        Password="123456"},
                    new Client { Email = "cdomato@gmail.com",
                        FirstName="Camila", LastName="Domato",
                        Password="112233"},
                    new Client { Email = "dbenitez@gmail.com",
                        FirstName="Damian", LastName="Benitez",
                        Password="7891011"}
                };

                context.Clients.AddRange(clients);

                //guardamos
                context.SaveChanges();

            }

            if (!context.Account.Any())
            {
                var accountDami = context.Clients.FirstOrDefault(c => c.Email == "dbenitez@gmail.com");
                if (accountDami != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountDami.Id, CreationDate = DateTime.Now, Number = "VIN003", Balance = 50 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                }
                var accountCamila = context.Clients.FirstOrDefault(c => c.Email == "cdomato@gmail.com");
                if (accountCamila != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountCamila.Id, CreationDate = DateTime.Now.AddDays(-1), Number = "VIN002", Balance = 1000 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                }
                var accountVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (accountVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountVictor.Id, CreationDate = DateTime.Now.AddDays(5), Number = "VIN001", Balance = 0 }

                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }

                }

                context.SaveChanges();
            }

            if (!context.Transaction.Any())

            {

                var account1 = context.Account.FirstOrDefault(c => c.Number == "VIN001");

                if (account1 != null)

                {

                    var transactions = new Transaction[]

                    {

                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia recibida", Type = TransactionType.CREDIT },

                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT },

                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT },

                    };

                    foreach (Transaction transaction in transactions)

                    {

                        context.Transaction.Add(transaction);

                    }

                }   

                var account2 = context.Account.FirstOrDefault(c => c.Number == "VIN002");

                if (account2 != null)

                    {

                        var transactions = new Transaction[]

                        {

                        new Transaction { AccountId= account2.Id, Amount = 500, Date= DateTime.Now.AddHours(-2), Description = "Transferencia recibida", Type = TransactionType.CREDIT },

                        new Transaction { AccountId= account2.Id, Amount = -20, Date= DateTime.Now.AddHours(-1), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT },

                        new Transaction { AccountId= account2.Id, Amount = -400, Date= DateTime.Now.AddHours(7), Description = "Compra en tienda compra gamer", Type = TransactionType.DEBIT },

                        };

                        foreach (Transaction transaction in transactions)

                    {

                            context.Transaction.Add(transaction);

                        }
                    }

                var account3 = context.Account.FirstOrDefault(c => c.Number == "VIN003");

                if (account3 != null)

                {

                    var transactions = new Transaction[]

                        {

                        new Transaction { AccountId= account2.Id, Amount = 50, Date= DateTime.Now.AddHours(2), Description = "Transferencia recibida", Type = TransactionType.CREDIT },

                        new Transaction { AccountId= account2.Id, Amount = -400, Date= DateTime.Now.AddHours(-1), Description = "Compra en tienda compumundo", Type = TransactionType.DEBIT },

                        new Transaction { AccountId= account2.Id, Amount = -5000, Date= DateTime.Now.AddHours(7), Description = "Compra en tienda florida", Type = TransactionType.DEBIT },

                        };

                        foreach (Transaction transaction in transactions)

                    {

                        context.Transaction.Add(transaction);

                    }
                }

                context.SaveChanges();

            }

            

        }
       

    }
}
