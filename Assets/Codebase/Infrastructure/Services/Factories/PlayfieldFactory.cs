using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.Services.Loaders;
using Codebase.Presenters;
using UnityEngine;

namespace Codebase.Infrastructure.Services.Factories
{
    public class PlayfieldFactory : IService
    {
        public Canvas Canvas => _canvas ??= Object.Instantiate(_prefabsLoader.LoadCanvas());

        public LoadingCurtain LoadingCurtain => _loadingCurtain ??= Object.Instantiate(
            _prefabsLoader.LoadLoadingCurtain(),
            Canvas.transform);

        private readonly PrefabsLoader _prefabsLoader;
        private Canvas _canvas;
        private LoadingCurtain _loadingCurtain;

        public PlayfieldFactory(PrefabsLoader prefabsLoader)
        {
            _prefabsLoader = prefabsLoader;
        }

        public SortedCardsContainer CreateTable(CardPresenterFactory factory) => Object.Instantiate(_prefabsLoader.LoadTablePresenter()).Init(factory);
        public SortedCardsContainer CreateCardsContainer() => Object.Instantiate(_prefabsLoader.LoadCardsContainer());

        public RandomOffsetButton CreateButton() => Object.Instantiate(_prefabsLoader.LoadRandomOffsetButton(),
            Canvas.transform);
    }
}