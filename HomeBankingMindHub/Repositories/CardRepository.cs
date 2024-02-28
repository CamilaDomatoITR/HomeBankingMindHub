using System.Collections.Generic;
using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly HomeBankingContext _context;

        public CardRepository(HomeBankingContext context)
        {
            _context = context;
        }

        public void SaveCard(Card card)
        {
            _context.Card.Add(card);
            _context.SaveChanges();
        }

        public IEnumerable<Card> GetAllCards()
        {
            return _context.Card.ToList();
        }

        public Card GetCardById(int id)
        {
            return _context.Card.FirstOrDefault(c => c.Id == id);
        }

       
    }
}