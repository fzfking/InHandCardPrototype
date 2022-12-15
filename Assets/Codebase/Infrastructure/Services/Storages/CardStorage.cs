using System.Collections.Generic;
using Codebase.Infrastructure.Interfaces;
using Codebase.Models;
using UnityEngine;

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

        public Card[] GetRandomCardDeck(int size) => GenerateRandomDeck(size);

        private Card[] GenerateRandomDeck(int size)
        {
            List<Card> cards = new List<Card>(size);
            for (int i = 0; i < size; i++)
            {
                cards.Add(_cards[Random.Range(0, _cards.Count)].CreateCopy());
            }

            return cards.ToArray();
        }
    }
}