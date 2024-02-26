﻿using HomeBankingMindHub.Models.Enums;
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
                        Password="112233" },
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

            if (!context.Loans.Any())
            {
                //crearemos 3 prestamos Hipotecario, Personal y Automotriz
                var loans = new Loan[]
                {
                    new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                };

                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }

                context.SaveChanges();

                //ahora agregaremos los clientloan (Prestamos del cliente)
                //usaremos al único cliente que tenemos y le agregaremos un préstamo de cada item
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    //ahora usaremos los 3 tipos de prestamos
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }

                }
                var client2 = context.Clients.FirstOrDefault(c => c.Email == "cdomato@gmail.com");
                if (client2 != null)
                {
                    // 3 tipos de prestamos
                    var loanUno = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loanUno != null)
                    {
                        var clientLoanUno = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client2.Id,
                            LoanId = loanUno.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoanUno);
                    }


                    var loanDos = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loanDos != null)
                    {
                        var clientLoanDos = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client2.Id,
                            LoanId = loanDos.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoanDos);
                    }

                    var loanTres = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loanTres != null)
                    {
                        var clientLoanTres = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client2.Id,
                            LoanId = loanTres.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoanTres);
                    }

                    //guardamos todos los prestamos
                    context.SaveChanges();

                }
            }
            if (!context.Card.Any())
            {
                //buscamos al unico cliente
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    //le agregamos 2 tarjetas de crédito una GOLD y una TITANIUM, de tipo DEBITO Y CREDITO RESPECTIVAMENTE
                    var cards = new Card[]
                    {
                        new Card {
                            ClientId= client1.Id,
                            CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.GOLD.ToString(),
                            Number = "3325-6745-7876-4445",
                            Cvv = 990,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(4),
                        },
                        new Card {
                            ClientId= client1.Id,
                            CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.CREDIT.ToString(),
                            Color = CardColor.TITANIUM.ToString(),
                            Number = "2234-6745-552-7888",
                            Cvv = 750,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(5),
                        },
                    };

                    foreach (Card card in cards)
                    {
                        context.Card.Add(card);
                    }
                    context.SaveChanges();
                }
            }
        }

    }
       

}

