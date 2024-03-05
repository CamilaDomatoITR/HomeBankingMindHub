using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase

    {
        private IAccountRepository _accountRepository;

        private IClientRepository _clientRepository;

        private ICardRepository _cardRepository;
        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository , ICardRepository cardRepository)

        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get()

        {
            try

            {
                var clients = _clientRepository.GetAllClients();

                var clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients)

                {
                    var newClientDTO = new ClientDTO

                    {
                        Id = client.Id,

                        Email = client.Email,

                        FirstName = client.FirstName,

                        LastName = client.LastName,

                        Accounts = client.Accounts.Select(ac => new AccountDTO

                        {
                            Id = ac.Id,

                            Balance = ac.Balance,

                            CreationDate = ac.CreationDate,

                            Number = ac.Number

                        }).ToList(),
                        Loans = client.ClientLoan.Select(cl => new ClientLoanDTO
                        {
                            Id = cl.Id,
                            LoanId = cl.LoanId,
                            Name = cl.Loan.Name,
                            Amount = cl.Amount,
                            Payments = int.Parse(cl.Payments)
                        }).ToList(),
                        Cards = client.Card.Select(c => new CardDTO
                        {
                            Id = c.Id,
                            CardHolder = c.CardHolder,
                            Color = c.Color,
                            Cvv = c.Cvv,
                            FromDate = c.FromDate,
                            Number = c.Number,
                            ThruDate = c.ThruDate,
                            Type = c.Type
                        }).ToList()

                    };

                    clientsDTO.Add(newClientDTO);
                }

                return Ok(clientsDTO);
            }

            catch (Exception ex)

            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]

        public IActionResult Get(long id)

        {
            try

            {
                var client = _clientRepository.FindById(id);
                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO

                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number

                    }).ToList(),

                    Loans = client.ClientLoan.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Card.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList()
                };

                return Ok(clientDTO);

            }

            catch (Exception ex)

            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Loans = client.ClientLoan.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Card.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrentClientAccounts()
        {
            try
            {
                string email = User.FindFirst("Client")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                var clientAccounts = _accountRepository.GetAccountsByClient(client.Id);

                return Ok(clientAccounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrentClientCards()
        {
            try
            {
                string email = User.FindFirst("Client")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                var clientCards = _cardRepository.GetCardsByClient((int)client.Id);

                return Ok(clientCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //crea user
        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                //validamos datos antes
                if (String.IsNullOrEmpty(client.Email)) {
                    return StatusCode(403, "Email incorrecto");
                }
                if (String.IsNullOrEmpty(client.Password)) {
                    return StatusCode(403, "contraseña incorrecta");
                }
                if (String.IsNullOrEmpty(client.FirstName))
                {
                    return StatusCode(403, "El campo nombre es incorrecto");
                }
                if (String.IsNullOrEmpty(client.LastName))
                {
                    return StatusCode(403, "El campo apellido es incorrecto");
                }

                //buscamos si ya existe el usuario
                //Client user = _clientRepository.FindByEmail(client.Email);

                if (_clientRepository.ExistsByEmail(client.Email))
                {
                    return StatusCode(403, "El Email está en uso");
                }
                // Crear un nuevo cliente

                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };
                //guardar el nuevo cliente
                _clientRepository.Save(newClient);
                return Created("", newClient);


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //implementando creacion de cuentas

        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateAccount()
        {
            try
            {
                // Obtener información del cliente autenticado
                string email = User.FindFirst("Client")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return Forbid();
                }

                // Buscar el cliente por su correo electrónico
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                // Verificar si el cliente ya tiene 3 cuentas
                var clientAccounts = _accountRepository.GetAccountsByClient(client.Id);
                if (clientAccounts.Count() >= 3)
                {
                    return StatusCode(403, "El cliente ya tiene 3 cuentas registradas");
                }

                // Generar número de cuenta aleatorio utilizando RandomGenerator
                string accountNumber = RandomGenerator.GenerateAccountNumber();


                // Crear una nueva cuenta con saldo inicial 0
                Account newAccount = new Account
                {
                    ClientId = client.Id,
                    CreationDate = DateTime.Now,
                    Number = accountNumber,
                    Balance = 0
                };

                // Guardar la nueva cuenta
                _accountRepository.Save(newAccount);

                // Retornar respuesta de éxito
                return StatusCode(201, "Cuenta creada satisfactoriamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //implementando creacion de tarjetas
        [HttpPost("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult CreateCard([FromBody] CardCreationDTO cardCreationDTO)
        {
            try
            {
                // Obtener información del cliente autenticado
                string email = User.FindFirst("Client")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return Forbid();
                }

                // Buscar el cliente por su correo electrónico
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }

                // Verificar si el cliente ya tiene 6 tarjetas (3 de débito y 3 de crédito)
                var debitCardsCount = client.Card.Count(c => c.Type == "Debit");

                var creditCardsCount = client.Card.Count(c => c.Type == "Credit");

                if (debitCardsCount >= 3 && cardCreationDTO.Type == "Debit")
                {
                    return StatusCode(403, "El cliente ya tiene 3 tarjetas de débito registradas");
                }
                else if (creditCardsCount >= 3 && cardCreationDTO.Type == "Credit")
                {
                    return StatusCode(403, "El cliente ya tiene 3 tarjetas de crédito registradas");
                }

                // Verificar si ya existe una tarjeta con el mismo tipo y color
                bool isColorAlreadyUsed = client.Card.Any(c => c.Type == cardCreationDTO.Type && c.Color == cardCreationDTO.Color);
                if (isColorAlreadyUsed)
                {
                    return StatusCode(403, $"Ya existe una tarjeta de tipo {cardCreationDTO.Type} con el color {cardCreationDTO.Color}");
                }

                // Generar número de tarjeta aleatorio
                string cardNumber = RandomGenerator.GenerateCardNumber();

                // Otros datos de la tarjeta
                int cvv = RandomGenerator.GenerateRandomCvv(); 
                DateTime fromDate = DateTime.Now;
                DateTime thruDate = fromDate.AddYears(5); // La fecha de vto es 5 años después de la creación

                // Crea una nueva tarjeta
                Card newCard = new Card
                {
                    ClientId = client.Id,
                    CardHolder = client.FirstName + " " + client.LastName,
                    Number = cardNumber,
                    Cvv = cvv,
                    FromDate = fromDate,
                    ThruDate = thruDate,
                    Type = cardCreationDTO.Type,
                    Color = cardCreationDTO.Color
                };

                // Guarda la nueva tarjeta
                _cardRepository.SaveCard(newCard);

                // Retornar respuesta de éxito
                return StatusCode(201, "Tarjeta creada satisfactoriamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}
