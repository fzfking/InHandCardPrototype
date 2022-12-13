using System.Collections.Generic;
using Codebase.Infrastructure.Interfaces;
using Codebase.Models;

namespace Codebase.Infrastructure.Services.Storages
{
    public class CardStorage : IService
    {
        private readonly List<Card> _cards;

        public CardStorage()
        {
            _cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public Card[] GetCards() => _cards.ToArray();
    }
}