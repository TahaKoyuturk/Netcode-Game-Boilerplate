using Studio.Core.Save;
using Studio.Core.Services;
using Studio.Data;
using Studio.Data.Save;

namespace Studio.Systems.Settings
{
    public sealed class SettingsSystem : IService
    {
        public const string SaveKey = "settings";

        public readonly SettingsRegistry Registry = new();

        private readonly ISaveService _saveService;
        private readonly SettingsConfig _config;

        public Setting<int> QualityLevel { get; private set; }
        public Setting<bool> Fullscreen { get; private set; }
        public Setting<bool> VSync { get; private set; }
        public Setting<float> MasterVolume { get; private set; }
        public Setting<float> MusicVolume { get; private set; }
        public Setting<float> SfxVolume { get; private set; }

        public SettingsSystem(ISaveService saveService, SettingsConfig config)
        {
            _saveService = saveService;
            _config = config;
            BuildSettings();
            Load();
        }

        private void BuildSettings()
        {
            QualityLevel = new Setting<int>("quality", _config.DefaultQualityLevel);
            Fullscreen = new Setting<bool>("fullscreen", _config.DefaultFullscreen);
            VSync = new Setting<bool>("vsync", _config.DefaultVSync);
            MasterVolume = new Setting<float>("masterVolume", _config.DefaultMasterVolume);
            MusicVolume = new Setting<float>("musicVolume", _config.DefaultMusicVolume);
            SfxVolume = new Setting<float>("sfxVolume", _config.DefaultSfxVolume);

            Registry.Register(QualityLevel);
            Registry.Register(Fullscreen);
            Registry.Register(VSync);
            Registry.Register(MasterVolume);
            Registry.Register(MusicVolume);
            Registry.Register(SfxVolume);
        }

        public void ApplyAll()
        {
            SettingsApplier.ApplyQuality(QualityLevel.Value);
            SettingsApplier.ApplyFullscreen(Fullscreen.Value);
            SettingsApplier.ApplyVSync(VSync.Value);
        }

        public void Save()
        {
            var data = new SettingsSaveData
            {
                QualityLevel = QualityLevel.Value,
                Fullscreen = Fullscreen.Value,
                VSync = VSync.Value,
                MasterVolume = MasterVolume.Value,
                MusicVolume = MusicVolume.Value,
                SfxVolume = SfxVolume.Value,
                ResolutionWidth = UnityEngine.Screen.width,
                ResolutionHeight = UnityEngine.Screen.height
            };

            _saveService.Save(SaveKey, data);
        }

        public void Load()
        {
            if (!_saveService.TryLoad(SaveKey, out SettingsSaveData data))
            {
                ApplyAll();
                return;
            }

            QualityLevel.SetValue(data.QualityLevel);
            Fullscreen.SetValue(data.Fullscreen);
            VSync.SetValue(data.VSync);
            MasterVolume.SetValue(data.MasterVolume);
            MusicVolume.SetValue(data.MusicVolume);
            SfxVolume.SetValue(data.SfxVolume);
            SettingsApplier.ApplyResolution(data.ResolutionWidth, data.ResolutionHeight, data.Fullscreen);
            ApplyAll();
        }
    }
}
