using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMod
{
    /*
     * The class that is responsible for loading and storing all the
     * necessary settings. There's much room for improvement.
     */
    static class Settings
    {
        // Initialize Settings dictionary
        private static Dictionary<string, int> m_settings = new Dictionary<string, int>();

        // Load settings from file
        public static void LoadSettings(string filepath)
        {
            Logging.Debug.Log("Accessing Settings at " + filepath);

            // Try to read file contents into string
            string settings = "";
            try
            {
                settings = System.IO.File.ReadAllText(filepath);
            }
            catch (Exception e)
            {
                // Log error and return invalid state
                Logging.LogError(e.Message);
                return;
            }

            // Empty settings dictionary
            m_settings.Clear();

            // Splice settings string by newline characters, to get each line into a new array member
            string[] lines = settings.Split('\n');
            foreach (string line in lines)  // Loop through all lines
            {
                if (line == "") // If line is empty, ignore it (some editors add newlines when saving, this will combat that)
                    continue;

                string[] vals = line.Split('=');    // Split each line by the = character, to get a key and a value

                m_settings.Add(vals[0], Int32.Parse(vals[1]));   // Store key and value in settings dictionary
            }

            Logging.Debug.Log("Successfully loaded settings!");
        }

        // Converts the dictionary to a sett file
        public static void SaveSettings(string filepath)
        {
            string filecontent = "";
            foreach(KeyValuePair<string, int> pair in m_settings)
            {
                filecontent += (pair.Key + "=" + pair.Value + "\n");
            }

            System.IO.File.WriteAllText(filepath, filecontent);
        }

        // Adds a setting to the settings
        public static void AddSetting(string key, int value)
        {
            if (m_settings.ContainsKey(key))
                return;

            m_settings.Add(key, value);
        }

        // Changes a settings value, and adds it if specified
        public static void SetSetting(string key, int newVal, bool addIfDoesntExist = false)
        {
            // If the setting doesn't exist, either add and set it, or print a Debug.Warning
            if(!m_settings.ContainsKey(key))
            {
                if (!addIfDoesntExist)
                {
                    Logging.LogError("Tried to change a setting with key \"" + key + "\" that doesn't exist.");
                    return;
                }
                else
                {
                    AddSetting(key, newVal);
                    Logging.Debug.LogWarning("Tried to change a setting with key \"" + key + "\" that doesn't exist. It has been added now.");
                }
            }

            m_settings[key] = newVal;
        }

        // Tries to return the value belonging to a certain key
        // If the specified key doesn't exist, it returns 0 and logs an error
        // One can also specify that the setting should be created with some initial value
        public static int GetValue(string key, bool addIfDoesntExist = false, int initialValue = 0)
        {
            int value = 0;
            bool valExists = m_settings.TryGetValue(key, out value);

            if (!valExists)
            {
                if (!addIfDoesntExist)
                {
                    Logging.LogError("Tried to access unknown setting: \"" + key + "\". Check your chSettings.sett for errors.");
                }
                else
                {
                    AddSetting(key, initialValue);
                    Logging.Debug.LogWarning("Tried to access unknown setting: \"" + key + "\". A new setting with this key was created.");
                    return initialValue;
                }
            }

            return value;
        }
    }
}
