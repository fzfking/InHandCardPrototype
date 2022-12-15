using System;
using System.Collections.Generic;
using Codebase.DataTransferObjects;
using Codebase.Helpers;
using Codebase.Models.Interfaces;
using UnityEngine;

namespace Codebase.Models
{
    public class Card: IReadonlyCard
    {
        public event Action<StatType, int> AnyFieldChanged;
        public event Action<Card> Disposed;
        public string Name => _name;
        public string Description => _description;

        private readonly CardSetup _setup;
        public Sprite Icon { get; }
        private Dictionary<StatType, int> _stats;

        private readonly string _name;
        private readonly string _description;

        public Card(CardSetup setup, Sprite icon)
        {
            _name = setup.Name;
            _description = setup.Description;
            _stats = new Dictionary<StatType, int>(setup.Stats);
            _setup = setup;
            Icon = icon;
        }

        public int GetStat(StatType statType) => _stats[statType];

        public void OffsetStat(StatType statType, int offset)
        {
            var previousValue = _stats[statType];
            _stats[statType] += offset;
            if (_stats[statType] < 0)
            {
                _stats[statType] = 0;
            }

            if (previousValue == _stats[statType])
            {
                return;
            }
            AnyFieldChanged?.Invoke(statType, previousValue);
        }

        public Card CreateCopy()
        {
            return new Card(_setup, Icon);
        }

        public void Dispose()
        {
            Disposed?.Invoke(this);
            AnyFieldChanged = null;
            Disposed = null;
        }
    }
}