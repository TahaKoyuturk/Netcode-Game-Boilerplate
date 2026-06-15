using System;

namespace Studio.Systems.Settings
{
    public interface ISetting
    {
        string Key { get; }
        Type ValueType { get; }
        object GetValue();
        void SetValue(object value);
        void ResetToDefault();
    }

    public sealed class Setting<T> : ISetting
    {
        public string Key { get; }
        public Type ValueType => typeof(T);
        public T Value { get; private set; }
        public T DefaultValue { get; }

        public Setting(string key, T defaultValue)
        {
            Key = key;
            DefaultValue = defaultValue;
            Value = defaultValue;
        }

        public object GetValue() => Value;

        public void SetValue(object value)
        {
            Value = (T)value;
        }

        public void ResetToDefault()
        {
            Value = DefaultValue;
        }
    }
}
