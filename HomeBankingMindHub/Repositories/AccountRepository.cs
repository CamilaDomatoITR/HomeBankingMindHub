using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories
{
    public class AccountRepository : RepositoryBase <Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }
        //buscar cuenta por su id
        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id)
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }
        //buscar cuenta por id y cliente por email
        public Account FindByIdAndClientEmail(long id, string email)
        {
            return FindByCondition(account => account.Id == id && account.Client.Email.Equals(email))
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }
        //buscar todas las cuentas
        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Transactions)
                .ToList();
        }
        //crear y/o guardar cuenta
        public void Save(Account account)
        {
            if (account.Id == 0)
            {
                Create(account);
            }
            else
            {
                Update(account);
            }

            SaveChanges();
            RepositoryContext.ChangeTracker.Clear();
        }

        //buscar cuentas por cliente 
        public IEnumerable<Account> GetAccountsByClient(long clientId)

        {
            return FindByCondition(account => account.ClientId == clientId)
            .Include(account => account.Transactions)
            .ToList();
        }
        //buscar una cuenta por su número
        public Account FindByNumber(string number)
        {
            return FindByCondition(account => account.Number.ToUpper() == number.ToUpper())
            .Include(account => account.Transactions)
            .FirstOrDefault();
        }

    }
}
