using Codebase.Helpers;
using Codebase.Infrastructure.Interfaces;
using Codebase.Presenters;
using UnityEngine;

namespace Codebase.Infrastructure.Services.Loaders
{
    public class PrefabsLoader : IService
    {
        private static string PrefabsPath => "Prefabs/";
        private static string TablePath => $"{PrefabsPath}Table";
        private static string RandomOffsetButtonPath => $"{PrefabsPath}RandomOffsetButton";
        private static string CanvasPath => $"{PrefabsPath}Canvas";
        private static string LoadingCurtainPath => $"{PrefabsPath}LoadingCurtain";
        private static string CardPresenterPath => $"{PrefabsPath}Card";
        private static string CardsContainerPath => $"{PrefabsPath}CardsContainer";
        public CardPresenter LoadCardPresenter() => GenericResourcesLoader.Load<CardPresenter>(CardPresenterPath);

        public SortedCardsContainer LoadTablePresenter() => GenericResourcesLoader.Load<SortedCardsContainer>(TablePath);
        public SortedCardsContainer LoadCardsContainer() => GenericResourcesLoader.Load<SortedCardsContainer>(CardsContainerPath);

        public RandomOffsetButton LoadRandomOffsetButton() =>
            GenericResourcesLoader.Load<RandomOffsetButton>(RandomOffsetButtonPath);

        public Canvas LoadCanvas() => GenericResourcesLoader.Load<Canvas>(CanvasPath);
        public LoadingCurtain LoadLoadingCurtain() => GenericResourcesLoader.Load<LoadingCurtain>(LoadingCurtainPath);
    }
}