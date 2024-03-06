using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Migrations;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanRepository _loanRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IClientLoanRepository _clientLoanRepository;
    private readonly ITransactionRepository _transactionRepository;

    public LoansController(ILoanRepository loanRepository, IClientRepository clientRepository, IAccountRepository accountRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
    {
        _loanRepository = loanRepository;
        _clientRepository = clientRepository;
        _accountRepository = accountRepository;
        _clientLoanRepository = clientLoanRepository;
        _transactionRepository = transactionRepository;
    }
    [HttpGet]
   
    public IActionResult GetLoans()
    {
        var loans = _loanRepository.GetAll(); // todos los préstamos del repositorio
        return Ok(loans); // Devolver los préstamos como un JSON
    }

    [HttpPost]
    [Authorize(Policy = "ClientOnly")]
    public IActionResult ApplyLoan(LoanApplicationDTO loanAppDto)
    {
        try
        {
            // Obtener información del cliente autenticado
            

            string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
            if (email == string.Empty)
            {
                return Forbid();
            }
            // Buscar el cliente por su correo electrónico
            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                return Forbid();
            }
            // Verifica si el préstamo existe
            var loan = _loanRepository.FindById(loanAppDto.LoanId);
            if (loan == null){
                return Forbid(); 
            }

            // Verifica que el monto no sea cero y no sobrepase el máximo autorizado
            if (loanAppDto.Amount <= 0 || loanAppDto.Amount > loan.MaxAmount)
                return BadRequest("Monto de préstamo no válido");

            // Verifica que los pagos no estén vacíos
            if (string.IsNullOrWhiteSpace(loanAppDto.Payments))
                return BadRequest("Los pagos no pueden estar vacíos");

            //Verificar si la cuenta de destino existe
            var account = _accountRepository.FindByNumber(loanAppDto.ToAccountNumber);
            if (account == null)
                return Forbid();

            // Calcula el monto del préstamo aumentado en un 20%
            double loanAmount = loanAppDto.Amount * 1.20;

            // Guarda el préstamo del cliente
            var clientLoan = new ClientLoan
            {
                ClientId = account.ClientId,
                LoanId = loanAppDto.LoanId,
                Amount = loanAmount,
                Payments = loanAppDto.Payments,
            };
            _clientLoanRepository.Save(clientLoan);

            // Guarda la transacción
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Type = TransactionType.CREDIT,
                Amount = loanAppDto.Amount,
                Description = $"{loan.Name} loan approved",
                Date = DateTime.Now,
            };
            _transactionRepository.Save(transaction);

            // Actualiza el balance de la cuenta de destino
            account.Balance += loanAppDto.Amount;
            _accountRepository.Save(account);

            return Ok("Préstamo aplicado exitosamente");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
      
    }
}