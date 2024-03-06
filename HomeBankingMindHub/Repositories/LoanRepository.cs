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
                new LoanDTO { Id = 1, Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                new LoanDTO { Id = 2, Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                new LoanDTO { Id = 3, Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" }
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
