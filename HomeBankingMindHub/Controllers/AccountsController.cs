using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)

        {
            _accountRepository = accountRepository;
        }


        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();
                var AccountsDTO = new List<AccountDTO>();
                foreach (Account account in accounts)

                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO
                        {
                            Id = tr.Id,
                            Type = tr.Type.ToString(),
                            Amount = tr.Amount,
                            Description = tr.Description,
                            Date = tr.Date

                        }).ToList()
                    };

                    AccountsDTO.Add(newAccountDTO);
                }
                return Ok(AccountsDTO);
            }
            catch (Exception ex)

            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{id}")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult Get(long id)

        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                var account = _accountRepository.FindByIdAndClientEmail(id , email);

                if (account == null)

                {
                    return Forbid();
                }

                var accountDTO = new AccountDTO

                {

                    Id = account.Id,

                    Number = account.Number,

                    CreationDate = account.CreationDate,

                    Balance = account.Balance,

                    Transactions = account.Transactions.Select(tr => new TransactionDTO

                    {

                        Id = tr.Id,

                        Type = tr.Type.ToString(),

                        Amount = tr.Amount,

                        Description = tr.Description,

                        Date = tr.Date

                    }).ToList()

                };

                return Ok(accountDTO);
            }

            catch (Exception ex)

            {
                return StatusCode(500, ex.Message);
            }


        }
    }
}
