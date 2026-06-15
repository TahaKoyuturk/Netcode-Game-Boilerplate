using System;
using Studio.Core.Save;

namespace Studio.Data.Save
{
    [Serializable]
    public sealed class SettingsSaveData : SaveDataBase
    {
        public int QualityLevel = 2;
        public bool Fullscreen = true;
        public bool VSync = true;
        public float MasterVolume = 1f;
        public float MusicVolume = 0.8f;
        public float SfxVolume = 1f;
        public int ResolutionWidth;
        public int ResolutionHeight;
    }
}
