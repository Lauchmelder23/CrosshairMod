using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrosshairMod
{
    static class Interface
    {
        private static bool m_visible = false;

        private static Dictionary<string, GUIButton> m_buttons = new Dictionary<string, GUIButton>();
        private static int rSliderValue, gSliderValue, bSliderValue, aSliderValue;

        private static Texture2D m_background = new Texture2D(1, 1);
        private static GUIStyle m_style = new GUIStyle();
   

        private static Vector2 m_position;
        private static Vector2 m_dimension;

        // Initializes all Buttons, gives them their function etc
        public static void Init()
        {
            m_dimension = new Vector2(Screen.width / 4, Screen.height / 4);
            m_position = new Vector2((Screen.width - m_dimension.x) / 2, (Screen.height - m_dimension.y) / 2);

            m_background.SetPixel(0, 0, new Color(0.4f, 0.4f, 0.4f, 0.4f));
            m_background.wrapMode = TextureWrapMode.Repeat;
            m_background.Apply();

            m_style.normal.background = m_background;

            m_buttons.Add("visibility", new GUIButton((uint)m_position.x + 20, (uint)m_position.y + 20, 200, 30, "Toggle Crosshair"));
            m_buttons["visibility"].OnClick += (object sender, EventArgs e) => { Crosshair.Toggle(); };

            m_buttons.Add("size-", new GUIButton((uint)m_position.x + 20, (uint)m_position.y + 60, 30, 30, "-"));
            m_buttons.Add("size+", new GUIButton((uint)m_position.x + 190, (uint)m_position.y + 60, 30, 30, "+"));
            m_buttons["size-"].OnClick += (object sender, EventArgs e) => { Crosshair.ChangeSize(-1); };
            m_buttons["size+"].OnClick += (object sender, EventArgs e) => { Crosshair.ChangeSize(+1); };

            m_buttons.Add("thick-", new GUIButton((uint)m_position.x + 20, (uint)m_position.y + 100, 30, 30, "-"));
            m_buttons.Add("thick+", new GUIButton((uint)m_position.x + 190, (uint)m_position.y + 100, 30, 30, "+"));
            m_buttons["thick-"].OnClick += (object sender, EventArgs e) => { Crosshair.ChangeThickness(-1); };
            m_buttons["thick+"].OnClick += (object sender, EventArgs e) => { Crosshair.ChangeThickness(+1); };

            rSliderValue = Settings.GetValue("crosshairColorRed");
            gSliderValue = Settings.GetValue("crosshairColorGreen");
            bSliderValue = Settings.GetValue("crosshairColorBlue");
            aSliderValue = Settings.GetValue("crosshairColorAlpha");
        }

        // Displays / Hides the menu
        public static void Toggle()
        {
            m_visible = !m_visible;
        }

        // Renders the Panel, but also handles Updating the buttons
        public static void Render()
        {
            if(m_visible)
            {
                GUI.Label(new Rect(m_position, m_dimension), m_background, m_style);

                GUI.Label(new Rect(m_position.x + 60, m_position.y + 70, 120, 30), "Length: " + Settings.GetValue("crosshairLength"));
                GUI.Label(new Rect(m_position.x + 60, m_position.y + 110, 120, 30), "Thickness: " + Settings.GetValue("crosshairThickness"));

                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 30, 200, 30), "R: " + rSliderValue);
                rSliderValue = (int)GUI.HorizontalSlider(new Rect(m_position.x + m_dimension.x / 2 + 60, m_position.y + 20, 200, 30), (int)rSliderValue, 0f, 255f);

                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 70, 200, 30), "G: " + gSliderValue);
                gSliderValue = (int)GUI.HorizontalSlider(new Rect(m_position.x + m_dimension.x / 2 + 60, m_position.y + 60, 200, 30), (int)gSliderValue, 0f, 255f);

                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 110, 200, 30), "B: " + bSliderValue);
                bSliderValue = (int)GUI.HorizontalSlider(new Rect(m_position.x + m_dimension.x / 2 + 60, m_position.y + 100, 200, 30), (int)bSliderValue, 0f, 255f);

                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 150, 200, 30), "A: " + aSliderValue);
                aSliderValue = (int)GUI.HorizontalSlider(new Rect(m_position.x + m_dimension.x / 2 + 60, m_position.y + 140, 200, 30), (int)aSliderValue, 0f, 255f);

                Crosshair.SetColor(rSliderValue, gSliderValue, bSliderValue, aSliderValue);

                // Update Buttons
                HandleButtons();
            }
        }

        private static void HandleButtons()
        {
            foreach(KeyValuePair<string, GUIButton> pair in m_buttons)
            {
                pair.Value.Update();
            }
        }
    }
}
