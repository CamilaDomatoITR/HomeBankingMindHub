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

          
        }
       
    }
}
