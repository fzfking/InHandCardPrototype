using Codebase.Infrastructure.Interfaces;
using Codebase.Presenters;
using UnityEngine;

namespace Codebase.Infrastructure.Services.Loaders
{
    public class PrefabsLoader : IService
    {
        private static class Paths
        {
            private static string PrefabsPath => "Prefabs/";
            public static string TablePath => $"{PrefabsPath}Table";
            public static string RandomOffsetButtonPath => $"{PrefabsPath}RandomOffsetButton";
            public static string CanvasPath => $"{PrefabsPath}Canvas";
            public static string LoadingCurtainPath => $"{PrefabsPath}LoadingCurtain";
            public static string CardPresenterPath => $"{PrefabsPath}Card";
            public static string CardsContainerPath => $"{PrefabsPath}CardsContainer";
        }

        public CardPresenter LoadCardPresenter() => GenericResourcesLoader.Load<CardPresenter>(Paths.CardPresenterPath);

        public SortedCardsContainer LoadTablePresenter() => GenericResourcesLoader.Load<SortedCardsContainer>(Paths.TablePath);
        public SortedCardsContainer LoadCardsContainer() => GenericResourcesLoader.Load<SortedCardsContainer>(Paths.CardsContainerPath);

        public RandomOffsetButton LoadRandomOffsetButton() =>
            GenericResourcesLoader.Load<RandomOffsetButton>(Paths.RandomOffsetButtonPath);

        public Canvas LoadCanvas() => GenericResourcesLoader.Load<Canvas>(Paths.CanvasPath);
        public LoadingCurtain LoadLoadingCurtain() => GenericResourcesLoader.Load<LoadingCurtain>(Paths.LoadingCurtainPath);
    }
}