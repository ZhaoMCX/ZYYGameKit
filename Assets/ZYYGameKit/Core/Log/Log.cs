
using System.Diagnostics;

namespace ZYYGameKit.Log
{
    public static class ZyLogger
    {
        [Conditional("ENABLE_DEBUG_LOG")]
        public static void Log(string info)
        {
            UnityEngine.Debug.Log(info);
        }
        public static void Warning(string info)
        {
            UnityEngine.Debug.LogWarning(info);
        }
        public static void Error(string info)
        {
            UnityEngine.Debug.LogError(info);
        }
    }
}
