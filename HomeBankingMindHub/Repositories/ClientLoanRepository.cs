using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public class ClientLoanRepository : IClientLoanRepository
    {
        private readonly HomeBankingContext _context;

        public ClientLoanRepository(HomeBankingContext context)
        {
            _context = context;
        }

        public void Save(ClientLoan clientLoan)
        {
            // Agrega el préstamo del cliente al contexto
            _context.ClientLoans.Add(clientLoan);

            // Guarda los cambios en la base de datos
            _context.SaveChanges();
        }
    }
}
