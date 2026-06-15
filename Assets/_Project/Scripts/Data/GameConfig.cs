using NaughtyAttributes;
using UnityEngine;

namespace Studio.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Studio/Config/Game Config")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("Scenes")]
        public string BootstrapScene = "Bootstrap";
        public string MainMenuScene = "MainMenu";
        public string GameplayScene = "Gameplay";

        [Header("Debug")]
        public bool EnableDebugTools = true;

        [Header("References")]
        [Expandable]
        public AudioConfig AudioConfig;
        [Expandable]
        public NetworkConfig NetworkConfig;
        [Expandable]
        public EconomyConfig EconomyConfig;
        [Expandable]
        public SettingsConfig SettingsConfig;
    }
}
