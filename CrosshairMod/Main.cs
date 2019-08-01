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
        private void Log(string message)
        {
            System.Console.WriteLine("[CROSSHAIRMOD]" + message);
        }

        private Dictionary<string, int> m_settings = new Dictionary<string, int>();
        private Texture2D crosshair = new Texture2D(-1, -1);
        private bool m_validState = true;
       
        // Reads settings file and sets all variables
        private bool readSettings(string filepath, ref Dictionary<string, int> sett_dict)
        {
            Log("Accessing Settings at " + filepath);

            string settings = "";
            try
            {
                settings = System.IO.File.ReadAllText(filepath);
            } catch(Exception e)
            {
                Log(e.Message);
                return false;
            }

            sett_dict = new Dictionary<string, int>();

            string[] lines = settings.Split('\n');
            foreach (string line in lines)
            {
                if (line == "")
                    continue;

                string[] vals = line.Split('=');

                sett_dict.Add(vals[0], Int32.Parse(vals[1]));
            }

            return true;
        }

        // Creates a crosshair texture
        private bool createCrossTexture(ref Texture2D texture)
        {
            int m_crosshairLength = 0, m_crosshairThickness = 0, m_crosshairColorRed = 0, m_crosshairColorGreen = 0, m_crosshairColorBlue = 0, m_crosshairColorAlpha = 0;
            if(!m_settings.TryGetValue("crosshairLength", out m_crosshairLength))
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

            Color m_crosshairColor = new Color(m_crosshairColorRed / 255f,
                                                m_crosshairColorGreen / 255f,
                                                m_crosshairColorBlue / 255f,
                                                m_crosshairColorAlpha / 255f);

            // Lazy
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

            texture.wrapMode = TextureWrapMode.Repeat;
            texture.Apply();

            return true;
        }


        void Start()
        {
            m_validState = readSettings(".\\Blackwake_Data\\Managed\\Mods\\chSettings.sett", ref m_settings);
            foreach (KeyValuePair<string, int> setting in m_settings)
                Log(setting.Key + ": " + setting.Value);
        }
        void OnGUI()
        {
            if (m_validState)
            {
                m_validState = createCrossTexture(ref crosshair);

                GUIStyle style = new GUIStyle();
                style.normal.background = crosshair;

                GUI.Label(new Rect(Screen.width / 2 - crosshair.width / 2, Screen.height / 2 - crosshair.height / 2, crosshair.width, crosshair.height),
                    crosshair, style);
            }
        }
    }
}
