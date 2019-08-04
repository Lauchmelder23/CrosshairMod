using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CrosshairMod
{
    /// <summary>
    /// Loads, contains and handles settings
    /// </summary>
    static class Settings
    {
        private static Dictionary<string, int> m_settings = new Dictionary<string, int>();

        /// <summary>
        /// Load settings from file
        /// </summary>
        /// <param name="filepath"></param>
        public static void LoadSettings(string filepath)
        {
            Logging.Debug.Log("Accessing Settings at " + filepath);

            // Try to read file contents into string
            // If no file exists, create one
            string settings = "";
            if(!System.IO.File.Exists(filepath))
            {
                // No settings file found, create one
                Logging.Debug.LogWarning("Settings file not found, creating one...");
                try
                {
                    System.IO.File.Create(filepath);
                }
                // If that fails then shit just hit the fan. React accordingly
                catch(Exception e)
                {
                    Logging.LogError("Something went wrong while creating a settings file... :(");
                    Logging.LogError(e.Message);
                    Logging.LogError(e.StackTrace);

                    return;
                }
            }
            
            // Read file to string
            try
            {
                settings = System.IO.File.ReadAllText(filepath);
            }
            // Something incredibly weird just happened
            catch (Exception e)
            {
                Logging.LogError("Something went wrong while reading a settings file... :(");
                Logging.LogError(e.Message);
                Logging.LogError(e.StackTrace);

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

            Logging.Log("Settings loaded.");
        }

        /// <summary>
        /// Converts dictionary to a string to be stored in the settings file
        /// </summary>
        /// <param name="filepath">Save location</param>
        public static void SaveSettings(string filepath)
        {
            string filecontent = "";
            foreach(KeyValuePair<string, int> pair in m_settings)
            {
                filecontent += (pair.Key + "=" + pair.Value + "\n");
            }

            System.IO.File.WriteAllText(filepath, filecontent);
        }

        /// <summary>
        /// Create new setting and add it to the dictionary
        /// </summary>
        /// <param name="key">Setting name</param>
        /// <param name="value">Setting value</param>
        public static void AddSetting(string key, int value)
        {
            if (m_settings.ContainsKey(key))
                return;

            m_settings.Add(key, value);
        }

        /// <summary>
        /// Change a settings value
        /// </summary>
        /// <param name="key">Setting name</param>
        /// <param name="newVal">New setting value</param>
        /// <param name="addIfDoesntExist">If the setting doesn't exist, create it</param>
        public static void SetSetting(string key, int newVal, bool addIfDoesntExist = false)
        {
            // If the setting doesn't exist, either add and set it, or print a Debug.Warning
            if(!m_settings.ContainsKey(key))
            {
                if (!addIfDoesntExist)
                {
                    Logging.Debug.LogError("Tried to change a setting with key \"" + key + "\" that doesn't exist.");
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

        /// <summary>
        /// Returns the value of a setting
        /// </summary>
        /// <param name="key">Setting name</param>
        /// <param name="addIfDoesntExist">Add the setting if it doesn't exist</param>
        /// <param name="initialValue">Initial value of the setting in case it doesn't exist</param>
        /// <returns></returns>
        public static int GetValue(string key, bool addIfDoesntExist = false, int initialValue = 0)
        {
            int value = 0;
            bool valExists = m_settings.TryGetValue(key, out value);

            if (!valExists)
            {
                if (!addIfDoesntExist)
                {
                    Logging.Debug.LogError("Tried to access unknown setting: \"" + key + "\". Check your chSettings.sett for errors.");
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
