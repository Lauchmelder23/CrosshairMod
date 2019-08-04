using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod
{
    /// <summary>
    /// Handles Logging in the Crosshair mod
    /// </summary>
    public static class Logging
    {
        public const string PREFIX = "[CROSSHAIRMOD]";

        /// <summary>
        /// Logging's subclass for Debug Logging.
        /// This only logs messages in the debug build of the mod.
        /// </summary>
        public static class Debug
        {
            public static void Log(string message)
            {
#if DEBUG
                Logging.Log(message);
#endif // DEBUG
            }

            public static void LogWarning(string message)
            {
#if DEBUG
                Logging.LogWarning(message);
#endif // DEBUG
            }

            public static void LogError(string message)
            {
#if DEBUG
                Logging.LogError(PREFIX + "Error: " + message);
#endif // DEBUG
            }
        }


        public static void Log(string message)
        {
            Console.WriteLine(PREFIX + "Info: " + message);
        }

        public static void LogWarning(string message)
        {
            Console.WriteLine(PREFIX + "Warning: " + message);
        }

        
        public static void LogError(string message)
        {
            UnityEngine.Debug.Log(PREFIX + "Error: " + message);
        }
    }
}
