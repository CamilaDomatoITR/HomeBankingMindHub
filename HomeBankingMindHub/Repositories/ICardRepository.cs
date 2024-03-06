
using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public interface ICardRepository
    {
        void SaveCard(Card card);
        IEnumerable<Card> GetAllCards();
        Card GetCardById(int id);
        IEnumerable<Card> GetCardsByClient(int clientId);


    }
}