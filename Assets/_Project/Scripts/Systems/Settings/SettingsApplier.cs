using UnityEngine;

namespace Studio.Systems.Settings
{
    public static class SettingsApplier
    {
        public static void ApplyQuality(int qualityLevel)
        {
            QualitySettings.SetQualityLevel(qualityLevel, true);
        }

        public static void ApplyFullscreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
        }

        public static void ApplyResolution(int width, int height, bool fullscreen)
        {
            if (width > 0 && height > 0)
            {
                Screen.SetResolution(width, height, fullscreen);
            }
        }

        public static void ApplyVSync(bool enabled)
        {
            QualitySettings.vSyncCount = enabled ? 1 : 0;
        }
    }
}
