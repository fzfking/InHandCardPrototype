using System;
using System.Collections.Generic;
using Codebase.DataTransferObjects;
using Codebase.Helpers;
using UnityEngine;

namespace Codebase.Models
{
    public class Card
    {
        public event Action<StatType, int> AnyFieldChanged;
        public string Name => _name;
        public string Description => _description;

        private readonly Sprite _icon;
        private Dictionary<StatType, int> _stats;

        private readonly string _name;
        private readonly string _description;

        public Card(CardSetup setup, Sprite icon)
        {
            _name = setup.Name;
            _description = setup.Description;
            _stats = setup.Stats;
            _icon = icon;
        }

        public int GetStat(StatType statType) => _stats[statType];

        public void OffsetStat(StatType statType, int offset)
        {
            var previousValue = _stats[statType];
            _stats[statType] += offset;
            AnyFieldChanged?.Invoke(statType, previousValue);
        }
    }
}