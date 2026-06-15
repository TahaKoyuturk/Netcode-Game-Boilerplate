using System;
using Studio.Core.Save;

namespace Studio.Data.Save
{
    [Serializable]
    public sealed class InputBindingsSaveData : SaveDataBase
    {
        public string BindingsJson;
    }
}
