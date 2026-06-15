using Cysharp.Text;
using UnityEngine;

namespace Studio.Utilities
{
    public static class ZStringUtility
    {
        public static string FormatPercent(float value01)
        {
            using var sb = ZString.CreateStringBuilder();
            sb.Append(Mathf.RoundToInt(Mathf.Clamp01(value01) * 100f));
            sb.Append('%');
            return sb.ToString();
        }

        public static string FormatLoadingStatus(float progress)
        {
            using var sb = ZString.CreateStringBuilder();
            sb.Append("Loading... ");
            sb.Append(FormatPercent(progress));
            return sb.ToString();
        }
    }
}
