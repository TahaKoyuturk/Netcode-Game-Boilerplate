using UnityEngine;

namespace Studio.Data
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Studio/Config/Audio Config")]
    public sealed class AudioConfig : ScriptableObject
    {
        public AudioClip DefaultMusic;
        public AudioClip ButtonClick;
        public int SfxPoolSize = 8;
        [Range(0f, 1f)] public float DefaultMasterVolume = 1f;
        [Range(0f, 1f)] public float DefaultMusicVolume = 0.8f;
        [Range(0f, 1f)] public float DefaultSfxVolume = 1f;
    }
}
