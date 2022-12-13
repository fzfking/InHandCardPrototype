using System;
using System.Collections.Generic;
using Codebase.Helpers;

namespace Codebase.DataTransferObjects
{
    [Serializable]
    public class CardSetup
    {
        public string Name;
        public string Description;
        public Dictionary<StatType, int> Stats;
    }
}