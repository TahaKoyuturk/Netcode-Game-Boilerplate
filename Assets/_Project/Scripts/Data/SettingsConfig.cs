using UnityEngine;

namespace Studio.Data
{
    [CreateAssetMenu(fileName = "SettingsConfig", menuName = "Studio/Config/Settings Config")]
    public sealed class SettingsConfig : ScriptableObject
    {
        public int DefaultQualityLevel = 2;
        public bool DefaultFullscreen = true;
        public bool DefaultVSync = true;
        [Range(0f, 1f)] public float DefaultMasterVolume = 1f;
        [Range(0f, 1f)] public float DefaultMusicVolume = 0.8f;
        [Range(0f, 1f)] public float DefaultSfxVolume = 1f;
    }
}
