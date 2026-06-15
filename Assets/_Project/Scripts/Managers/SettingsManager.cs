using Studio.Core.Services;
using Studio.Systems.Settings;

namespace Studio.Managers
{
    public sealed class SettingsManager : IManager
    {
        private readonly SettingsSystem _settingsSystem;

        public SettingsManager(SettingsSystem settingsSystem)
        {
            _settingsSystem = settingsSystem;
        }

        public SettingsSystem System => _settingsSystem;

        public void Initialize()
        {
            _settingsSystem.ApplyAll();
        }

        public void Shutdown()
        {
            _settingsSystem.Save();
        }

        public void SetQuality(int level)
        {
            _settingsSystem.QualityLevel.SetValue(level);
            _settingsSystem.ApplyAll();
            _settingsSystem.Save();
        }

        public void SetMasterVolume(float volume)
        {
            _settingsSystem.MasterVolume.SetValue(volume);
            _settingsSystem.Save();
        }
    }
}
