using System.Collections.Generic;
using System.Linq;
using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.Services.Loaders;
using Codebase.Presenters;
using UnityEngine;

namespace Codebase.Infrastructure.Services.Factories
{
    public class CardPresenterFactory: IService
    {
        private readonly CardPresenter CardPresenterPrefab;
        private List<CardPresenter> _cardPresentersPool;
        private const int DefaultPoolSize = 5;

        public CardPresenterFactory(PrefabsLoader prefabsLoader)
        {
            CardPresenterPrefab = prefabsLoader.LoadCardPresenter();
            _cardPresentersPool = new List<CardPresenter>(DefaultPoolSize);
        }
        public CardPresenter GetFreeCardPresenter()
        {
            var freeCard = _cardPresentersPool.FirstOrDefault(presenter => presenter.gameObject.activeSelf == false);
            if (freeCard == null)
            {
                AdjustPool();
                freeCard = GetFreeCardPresenter();
            }

            return freeCard;
        }

        private void AdjustPool()
        {
            for (int i = 0; i < DefaultPoolSize; i++)
            {
                var cardPresenter = Object.Instantiate(CardPresenterPrefab, null);
                cardPresenter.transform.position = Vector3.zero;
                cardPresenter.gameObject.SetActive(false);
                _cardPresentersPool.Add(cardPresenter);
            }
        }
    }
}