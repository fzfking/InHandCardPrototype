using Codebase.DataTransferObjects;
using Codebase.Infrastructure.Interfaces;
using Codebase.Models;

namespace Codebase.Infrastructure.Services.Factories
{
    public class CardFactory : IService
    {
        private readonly CardIconFactory _cardIconFactory;

        public CardFactory(CardIconFactory cardIconFactory)
        {
            _cardIconFactory = cardIconFactory;
        }

        public Card Create(CardSetup setup)
        {
            return new Card(setup, _cardIconFactory.GetRandomIcon());
        }
    }
}