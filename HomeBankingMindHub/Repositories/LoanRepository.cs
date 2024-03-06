using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Repositories
{
    public class LoanRepository : ILoanRepository
    {
      
        private List<LoanDTO> loans = new List<LoanDTO>();

        public LoanRepository()
        {
            // Inicializar la lista de préstamos
            loans = new List<LoanDTO>
            {
                //agrego los 3 préstamos
                new LoanDTO { Id = 1, Type = "Hipotecario", MaxAmount = 500000, Payments = new List<int> { 12, 24, 36, 48, 60 } },
                new LoanDTO { Id = 2, Type = "Personal", MaxAmount = 100000, Payments = new List<int> { 6, 12, 24 } },
                new LoanDTO { Id = 3, Type = "Automotriz", MaxAmount = 300000, Payments = new List<int> { 6, 12, 24, 36 } }
            };
        }

        public IEnumerable<LoanDTO> GetAll()
        {
            return loans;
        }

        public LoanDTO FindById(long id)
        {
            return loans.Find(loan => loan.Id == id);
        }
    }
}
