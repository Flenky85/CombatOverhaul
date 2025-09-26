using System;
using UnityEngine;

namespace CombatOverhaul.Utils
{
    internal static class Log
    {
        private const string Prefix = "[CombatOverhaul] ";

        public static void Info(string message)
        {
            Debug.Log(Prefix + message);
        }

        public static void Warning(string message)
        {
            Debug.LogWarning(Prefix + message);
        }

        public static void Error(string message)
        {
            Debug.LogError(Prefix + message);
        }

        public static void Error(string message, Exception ex)
        {
            if (ex == null)
            {
                Debug.LogError(Prefix + message);
                return;
            }

            Debug.LogError(Prefix + message + " | Exception: " + ex.GetType().Name + " - " + ex.Message + "\n" + ex.StackTrace);
        }

#if DEBUG
        public static void DebugLog(string message)
        {
            Debug.Log(Prefix + "[DEBUG] " + message);
        }
#endif
    }
}
