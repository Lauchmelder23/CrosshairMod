using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod
{
    // CrosshairMod's Logging class
    // Simply writes messages to the Unity Debug output
    // However, I prefer this over Debug.Log() since it doesn't include a stacktrace (Except for Errors)
    public static class Logging
    {
        // The Prefix that gets put in front of every Log
        public const string PREFIX = "[CROSSHAIRMOD]";

        // A kind of sub-class that is used to Log messages that will only appear
        // when the User installs a Debug build of the Mod. Release versions will
        // not log Debug messages this way.
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


        // Logs information
        public static void Log(string message)
        {
            Console.WriteLine(PREFIX + "Info: " + message);
        }

        // Logs warnings
        public static void LogWarning(string message)
        {
            Console.WriteLine(PREFIX + "Warning: " + message);
        }

        // Logs errors
        public static void LogError(string message)
        {
            UnityEngine.Debug.Log(PREFIX + "Error: " + message);
        }
    }
}
