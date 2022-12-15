using System;
using Codebase.Helpers;
using UnityEngine;

namespace Codebase.Models.Interfaces
{
    public interface IReadonlyCard
    {
        event Action<StatType, int> AnyFieldChanged;
        event Action<Card> Disposed;
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        int GetStat(StatType statType);
    }
}