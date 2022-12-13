using System.Collections.Generic;
using System.Linq;
using Codebase.Infrastructure.Services.Loaders;
using Codebase.Infrastructure.Services.Saver;
using UnityEngine;

namespace Codebase.EditorTools
{
    [CreateAssetMenu(fileName = "Card setups editor", menuName = "EditorTools/Create card setups", order = 0)]
    public class CardSetupsEditor : ScriptableObject
    {
        [SerializeField] private List<CardSetupForEditor> CardSetups;

        public void Load()
        {
            var cardSetupsLoader = new CardSetupsLoader();
            CardSetups = cardSetupsLoader.GetSetups().Select(CardSetupForEditor.FromCardSetup).ToList();
        }

        public void Save()
        {
            var cardSetupsSaver = new CardSetupsSaver();
            cardSetupsSaver.SaveSetups(CardSetups.Select(setup => setup.ToCardSetup()).ToArray());
        }
    }
}