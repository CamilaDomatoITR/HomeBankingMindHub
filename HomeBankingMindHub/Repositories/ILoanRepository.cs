using HomeBankingMindHub.Models.DTOS;

namespace HomeBankingMindHub.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<LoanDTO> GetAll();
        LoanDTO FindById(long id);
    }
}
