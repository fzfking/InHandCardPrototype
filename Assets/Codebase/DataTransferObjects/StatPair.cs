using System;
using Codebase.Helpers;

namespace Codebase.DataTransferObjects
{
    [Serializable]
    public class StatPair
    {
        public StatType StatType;
        public int Value;
    }
}