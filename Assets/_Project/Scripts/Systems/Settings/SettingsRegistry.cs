using System.Collections.Generic;

namespace Studio.Systems.Settings
{
    public sealed class SettingsRegistry
    {
        private readonly Dictionary<string, ISetting> _settings = new();

        public void Register(ISetting setting)
        {
            _settings[setting.Key] = setting;
        }

        public bool TryGet(string key, out ISetting setting)
        {
            return _settings.TryGetValue(key, out setting);
        }

        public IEnumerable<ISetting> GetAll()
        {
            return _settings.Values;
        }

        public void ResetAll()
        {
            foreach (var setting in _settings.Values)
            {
                setting.ResetToDefault();
            }
        }
    }
}
