using UnityEngine;

namespace Studio.Data
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "Studio/Config/Economy Config")]
    public sealed class EconomyConfig : ScriptableObject
    {
        public string CurrencyId = "coins";
        public int StartingBalance = 0;
        public int MaxBalance = 999999;
    }
}
