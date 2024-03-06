using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Models.Enums;

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

    [HttpPost]
    [Authorize(Policy = "ClientOnly")]
    public IActionResult ApplyLoan(LoanApplicationDTO loanAppDto)
    {
        // Verifica si el préstamo existe
        var loan = _loanRepository.FindById(loanAppDto.LoanId);
        if (loan == null)
            return Forbid();

        // Verifica que el monto no sea cero y no sobrepase el máximo autorizado
        if (loanAppDto.Amount <= 0 || loanAppDto.Amount > loan.MaxAmount)
            return BadRequest("Monto de préstamo no válido");

        // Verifica que los pagos no estén vacíos
        if (string.IsNullOrWhiteSpace(loanAppDto.Payments))
            return BadRequest("Los pagos no pueden estar vacíos");

        // Verifica si la cuenta de destino existe y pertenece al cliente autenticado
        var account = _accountRepository.FindByNumber(loanAppDto.ToAccountNumber);
        if (account == null || !long.TryParse(User.Identity.Name, out long clientId) || account.ClientId != clientId)
            return Forbid();

        // Calcula el monto del préstamo aumentado en un 20%
        double loanAmount = loanAppDto.Amount * 1.20;

        // Guarda el préstamo del cliente
        var clientLoan = new ClientLoan
        {
            ClientId = account.ClientId,
            LoanId = loanAppDto.LoanId,
            Amount = loanAmount
        };
        _clientLoanRepository.Save(clientLoan);

        // Guarda la transacción
        var transaction = new Transaction
        {
            AccountId = account.Id,
            Type = TransactionType.CREDIT,
            Amount = loanAmount,
            Description = $"{loan.Name} loan approved"
        };
        _transactionRepository.Save(transaction);

        // Actualiza el balance de la cuenta
        account.Balance += loanAppDto.Amount;
        _accountRepository.Update(account);

        return Ok("Préstamo aplicado exitosamente");
    }
}