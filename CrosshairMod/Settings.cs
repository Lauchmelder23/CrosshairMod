using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosshairMod
{
    static class Settings
    {
        // Initialize Settings dictionary
        private static Dictionary<string, int> m_settings = new Dictionary<string, int>();

        public static void LoadSettings(string filepath)
        {
            Logging.Log("Accessing Settings at " + filepath);

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

            Logging.Log("Successfully loaded settings!");
        }

        public static void writeSettings(string filepath)
        {
            //TODO: Implement saving
        }

        // Tries to return the value belonging to a certain key
        // If the specified key doesn't exist, it returns 0 and logs an error
        public static int GetValue(string key)
        {
            int value = 0;
            bool valExists = m_settings.TryGetValue(key, out value);

            if (!valExists)
                Logging.LogError("Tried to access unknown setting: \"" + key + "\". Check your chSettings.sett for errors.");

            return value;
        }
    }
}
