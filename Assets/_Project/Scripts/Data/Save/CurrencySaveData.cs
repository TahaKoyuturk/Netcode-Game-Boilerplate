using System;
using Studio.Core.Save;

namespace Studio.Data.Save
{
    [Serializable]
    public sealed class CurrencySaveData : SaveDataBase
    {
        public int Balance;
    }
}
