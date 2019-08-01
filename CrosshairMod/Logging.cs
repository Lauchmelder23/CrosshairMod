using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrosshairMod
{
    // CrosshairMod's Logging class
    // Simply writes messages to the Unity Debug output
    // However, I prefer this over Debug.Log() since it doesn't include a stacktrace (Except for Errors)
    public static class Logging
    {
        public const string PREFIX = "[CROSSHAIRMOD]";

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
            Debug.Log(PREFIX + "Error: " + message);
        }
    }
}
