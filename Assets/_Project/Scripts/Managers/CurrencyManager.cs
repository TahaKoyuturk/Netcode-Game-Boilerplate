using Studio.Core.Events;
using Studio.Core.Save;
using Studio.Core.Services;
using Studio.Data;
using Studio.Data.Save;

namespace Studio.Managers
{
    public readonly struct CurrencyChangedEvent : IEvent
    {
        public readonly int Balance;

        public CurrencyChangedEvent(int balance)
        {
            Balance = balance;
        }
    }

    public sealed class CurrencyManager : IManager
    {
        public const string SaveKey = "currency";

        private readonly ISaveService _saveService;
        private readonly EconomyConfig _config;
        private int _balance;

        public int Balance => _balance;

        public CurrencyManager(ISaveService saveService, EconomyConfig config)
        {
            _saveService = saveService;
            _config = config;
        }

        public void Initialize()
        {
            if (_saveService.TryLoad(SaveKey, out CurrencySaveData data))
            {
                _balance = data.Balance;
            }
            else
            {
                _balance = _config.StartingBalance;
                Save();
            }
        }

        public void Shutdown()
        {
            Save();
        }

        public bool CanAfford(int amount) => _balance >= amount;

        public bool TrySpend(int amount)
        {
            if (!CanAfford(amount))
            {
                return false;
            }

            _balance -= amount;
            PublishAndSave();
            return true;
        }

        public void Earn(int amount)
        {
            _balance = UnityEngine.Mathf.Min(_balance + amount, _config.MaxBalance);
            PublishAndSave();
        }

        public void SetBalance(int amount)
        {
            _balance = UnityEngine.Mathf.Clamp(amount, 0, _config.MaxBalance);
            PublishAndSave();
        }

        private void PublishAndSave()
        {
            EventBus.Publish(new CurrencyChangedEvent(_balance));
            Save();
        }

        private void Save()
        {
            _saveService.Save(SaveKey, new CurrencySaveData { Balance = _balance });
        }
    }
}
