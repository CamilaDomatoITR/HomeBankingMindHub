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

        }
    }
}
