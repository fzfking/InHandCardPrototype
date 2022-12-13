using System;
using System.Collections.Generic;
using System.Linq;
using Codebase.DataTransferObjects;
using UnityEngine;

namespace Codebase.EditorTools
{
    [Serializable]
    public class CardSetupForEditor
    {
        public string Name;
        [Multiline]
        public string Description;
        public List<StatPair> Stats;

        public static CardSetupForEditor FromCardSetup(CardSetup cardSetup) =>
            new CardSetupForEditor
            {
                Name = cardSetup.Name,
                Description = cardSetup.Description,
                Stats = cardSetup.Stats.
                    Select(stat => new StatPair() { StatType = stat.Key, Value = stat.Value })
                    .ToList()
            };

        public CardSetup ToCardSetup() =>
            new CardSetup
            {
                Name = Name,
                Description = Description,
                Stats = Stats.ToDictionary(stat => stat.StatType, stat => stat.Value)
            };
    }
}