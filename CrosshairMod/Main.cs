/* 
 * Source code of a mode that adds a crosshair into
 * the game Blackwake.
 * 
 * @author Lauchmelder
 * @version v0.2
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod
{
    public class Main : MonoBehaviour
    {
        // Logs a message made by this mod
        // [CROSSHAIRMOD]Message
        private void Log(string message)
        {
            System.Console.WriteLine("[CROSSHAIRMOD]" + message);
        }

        // Initialize Settings dictionary
        private Dictionary<string, int> m_settings = new Dictionary<string, int>();

        // Initialize Crosshair texture
        private Texture2D crosshair = new Texture2D(-1, -1);

        // Initialize State checker. If this is false, the crosshair won't be drawn
        // This is a temporary fix to stop this mod from spamming errors in the log
        private bool m_validState = true;
       
        // Reads settings file and sets all variables
        private bool readSettings(string filepath, ref Dictionary<string, int> sett_dict)
        {
            Log("Accessing Settings at " + filepath);

            // Try to read file contents into string
            string settings = "";
            try
            {
                settings = System.IO.File.ReadAllText(filepath);
            } catch(Exception e)
            {
                // Log error and return invalid state
                Log(e.Message);
                return false;
            }

            // Empty settings dictionary
            sett_dict.Clear();

            // Splice settings string by newline characters, to get each line into a new array member
            string[] lines = settings.Split('\n');
            foreach (string line in lines)  // Loop through all lines
            {
                if (line == "") // If line is empty, ignore it (some editors add newlines when saving, this will combat that)
                    continue;

                string[] vals = line.Split('=');    // Split each line by the = character, to get a key and a value

                sett_dict.Add(vals[0], Int32.Parse(vals[1]));   // Store key and value in settings dictionary
            }

            Log("Successfully loaded settings!");

            return true;    // Return valid state
        }

        // Creates a crosshair texture
        private bool createCrossTexture(ref Texture2D texture)
        {
            // Assign dictionary values to variables
            int m_crosshairLength = 0, m_crosshairThickness = 0, m_crosshairColorRed = 0, m_crosshairColorGreen = 0, m_crosshairColorBlue = 0, m_crosshairColorAlpha = 0;
            // Checks wether the key is in the dictionary, and assigns it
            // If the function finds that a necessary setting is missing, it will
            // return an invalid state
            if (!m_settings.TryGetValue("crosshairLength", out m_crosshairLength))
            {
                Log("Missing Setting: crosshairLength");
                return false;
            }

            if (!m_settings.TryGetValue("crosshairThickness", out m_crosshairThickness))
            {
                Log("Missing Setting: crosshairThickness");
                return false;
            }

            if (!m_settings.TryGetValue("crosshairColorRed", out m_crosshairColorRed))
            {
                Log("Missing Setting: crosshairColorRed");
                return false;
            }

            if (!m_settings.TryGetValue("crosshairColorGreen", out m_crosshairColorGreen))
            {
                Log("Missing Setting: crosshairColorGreen");
                return false;
            }

            if (!m_settings.TryGetValue("crosshairColorBlue", out m_crosshairColorBlue))
            {
                Log("Missing Setting: crosshairColorBlue");
                return false;
            }

            if (!m_settings.TryGetValue("crosshairColorAlpha", out m_crosshairColorAlpha))
            {
                Log("Missing Setting: crosshairColorAlpha");
                return false;
            }

            // Construct color object from RGBA values
            Color m_crosshairColor = new Color(m_crosshairColorRed / 255f,
                                                m_crosshairColorGreen / 255f,
                                                m_crosshairColorBlue / 255f,
                                                m_crosshairColorAlpha / 255f);

            // Lazily add 1 to thickness if it's even. If it is even the crosshair can't be symmetrical
            m_crosshairThickness += (m_crosshairThickness % 2 == 0) ? 1 : 0;

            // Create Texture with approprate dimensions. The m_crosshairLength is the distance 
            // between center and end-of-line (end-points included)
            texture = new Texture2D((m_crosshairLength - 1) * 2 + 1, (m_crosshairLength - 1) * 2 + 1);


            // Fill Texture with some color where alpha = 0
            for (int y = 0; y < texture.height; y++)
                for (int x = 0; x < texture.width; x++)
                    texture.SetPixel(x, y, new Color(0, 0, 0, 0));

            // The texture will always have odd dimensions, so we can be sure that there will be a definite center (which is m_crossheirLength)
            // Draw vertical line:
            for (int y = 0; y < texture.height; y++)
            {
                for (int i = 0; i < m_crosshairThickness; i++)
                {
                    texture.SetPixel(m_crosshairLength - (int)Math.Floor((double)m_crosshairThickness / 2) + i - 1, y, m_crosshairColor);
                }
            }

            // Draw horizontal line:
            for (int x = 0; x < texture.height; x++)
            {
                for (int i = 0; i < m_crosshairThickness; i++)
                {
                    texture.SetPixel(x, m_crosshairLength - (int)Math.Floor((double)m_crosshairThickness / 2) + i - 1, m_crosshairColor);
                }
            }

            // Set texture settings and apply changes
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.Apply();

            return true;    // Return valid state
        }


        // This will be executed first
        void Start()
        {
            // Read settings, if this fails, the state will be invalid
            m_validState = readSettings(".\\Blackwake_Data\\Managed\\Mods\\chSettings.sett", ref m_settings);
            foreach (KeyValuePair<string, int> setting in m_settings)
                Log(setting.Key + ": " + setting.Value);
        }
        void OnGUI()
        {
            // If the mod is in an invalid state, the OnGUI function won't execute
            if (m_validState)
            {
                // If the state is valid, construct crosshair texture, and update state
                // TODO: Remove createCrossTexture, this doesn't need to be constructed each time
                m_validState = createCrossTexture(ref crosshair);

                // Create GUIStyle to render the crosshair
                GUIStyle style = new GUIStyle();
                style.normal.background = crosshair;

                // Render the crosshair
                GUI.Label(new Rect(Screen.width / 2 - crosshair.width / 2, Screen.height / 2 - crosshair.height / 2, crosshair.width, crosshair.height),
                    crosshair, style);
            }
        }
    }
}
