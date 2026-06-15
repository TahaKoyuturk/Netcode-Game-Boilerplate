using System;

namespace Studio.Core.Save
{
    [Serializable]
    public sealed class SaveMetadata
    {
        public string Key;
        public int Version;
        public string SavedAtUtc;
    }
}
