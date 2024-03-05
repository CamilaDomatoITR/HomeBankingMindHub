using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult TransferFunds([FromBody] TransferDTO transferDTO)
        {
            try
            {
                // Verificar que los parámetros no estén vacíos
                if (string.IsNullOrEmpty(transferDTO.FromAccountNumber) ||
                    string.IsNullOrEmpty(transferDTO.ToAccountNumber) ||
                    transferDTO.Amount <= 0 ||
                    string.IsNullOrEmpty(transferDTO.Description))
                {
                    return BadRequest("Los parámetros de la transferencia son inválidos.");
                }

                // Verificar que los números de cuenta no sean iguales
                if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
                {
                    return BadRequest("Los números de cuenta no pueden ser iguales.");
                }

                // Buscar las cuentas de origen y destino
                var fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);
                var toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);

                // Verificar que existan las cuentas
                if (fromAccount == null || toAccount == null)
                {
                    return NotFound("Una o ambas cuentas especificadas no existen.");
                }

                // Verificar que la cuenta de origen tenga el monto disponible
                if (fromAccount.Balance < transferDTO.Amount)
                {
                    return BadRequest("La cuenta de origen no tiene suficientes fondos para realizar la transferencia.");
                }

                // Crear las transacciones DEBIT y CREDIT
                var debitTransaction = new Transaction
                {
                    AccountId = fromAccount.Id,
                    Amount = -transferDTO.Amount,
                    Description = $"{transferDTO.Description} (DEBIT a {transferDTO.ToAccountNumber})",
                    Type = TransactionType.DEBIT,
                    Date = DateTime.Now
                };

                var creditTransaction = new Transaction
                {
                    AccountId = toAccount.Id,
                    Amount = transferDTO.Amount,
                    Description = $"{transferDTO.Description} (CREDIT de {transferDTO.FromAccountNumber})",
                    Type = TransactionType.CREDIT,
                    Date = DateTime.Now
                };

                // Guardar las transacciones
                _transactionRepository.Save(debitTransaction);
                _transactionRepository.Save(creditTransaction);

                // Actualizar los saldos de las cuentas
                fromAccount.Balance -= transferDTO.Amount;
                toAccount.Balance += transferDTO.Amount;
                _accountRepository.Save(fromAccount);
                _accountRepository.Save(toAccount);

                return Ok("Transferencia completada satisfactoriamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la transferencia: {ex.Message}");
            }
        }
    }
}
