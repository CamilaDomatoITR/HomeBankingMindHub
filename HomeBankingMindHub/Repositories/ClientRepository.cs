using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }
        //buscar cliente por id
        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id)
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoan)
                .ThenInclude(cl => cl.Loan)
                .Include(client => client.Card)
                .FirstOrDefault();
        }
        //buscar todos los clientes
        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                .Include(client => client.Accounts)
                .Include(client => client.ClientLoan)
                .ThenInclude(cl => cl.Loan)
                .Include(client => client.Card)
                .ToList();
        }
        //buscar clientes por email
        public Client FindByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToUpper() == email.ToUpper())
            .Include(client => client.Accounts)
            .Include(client => client.ClientLoan)
                .ThenInclude(cl => cl.Loan)
            .Include(client => client.Card)
            .FirstOrDefault();
        }
        //crear y/o guardar cliente
        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }
        //consultar si existe el user por email
        public bool ExistsByEmail(string email)
        {
            return FindByCondition(client => client.Email == email).Any();
        }
    }
}
