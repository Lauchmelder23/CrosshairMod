﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrosshairMod
{

    /* A class that handles the Crosshair GUI.
     * 
     * Contains all Buttons, Sliders etc. that are able to modify the crosshair.
     */

    // TODO: Create GUILayout.Window to make a less crappy version of the settings window
    static class Interface
    {
        // Saves wether the interface is visible or not
        private static bool m_visible = false;

        // Stores all Buttons used in the interface.
        private static List<GUIButton> m_buttons = new List<GUIButton>();

        // Values of the RGBA Sliders
        private static int rSliderValue, gSliderValue, bSliderValue, aSliderValue;

        // Texture and Styles for the GUI background
        private static Texture2D m_background = new Texture2D(1, 1);
        private static GUIStyle m_style = new GUIStyle();
   
        // Position ind dimension of the GUI background
        private static Vector2 m_position;
        private static Vector2 m_dimension;


        // Creates a new button object and adds it to the ButtonList
        // Returns the index of the button
        private static int AddButton(uint x, uint y, uint width, uint height, string label, params EventHandler[] onClickEvent)
        {
            GUIButton buttonObj = new GUIButton(x, y, width, height, label);
            foreach(EventHandler e in onClickEvent)
            {
                buttonObj.OnClick += e;
            }

            m_buttons.Add(buttonObj);
            return m_buttons.Count - 1;
        }

        // Initializes all Buttons, gives them their function etc
        public static void Init()
        {
            // Set dimension to 0.25 of the screen width/height
            m_dimension = new Vector2(Screen.width / 4, Screen.height / 4);
            // Center the interface
            m_position = new Vector2((Screen.width - m_dimension.x) / 2, (Screen.height - m_dimension.y) / 2);

            // Create Texture that is dark gray and slightly see-through
            m_background.SetPixel(0, 0, new Color(0.4f, 0.4f, 0.4f, 0.4f));
            m_background.wrapMode = TextureWrapMode.Repeat;
            m_background.Apply();

            // Apply Texture to Style
            m_style.normal.background = m_background;

            AddButton(3, 3, 3, 3, "", (object sender, EventArgs e) => { });

            // Create Crosshair Visibilty Button
            int index = AddButton((uint)m_position.x + 20, (uint)m_position.y + 20, 200, 30, 
                "Hide Crosshair", (object sender, EventArgs e) => { Crosshair.Toggle(); });
            m_buttons[index].OnClick += (object sender, EventArgs args) => { m_buttons[index].label = (Crosshair.Enabled()) ? "Show Crosshair" : "Hide Crosshair"; };

            // Create Crosshair Size +/- Buttons
            AddButton((uint)m_position.x + 20, (uint)m_position.y + 60, 30, 30, 
                "-", (object sender, EventArgs e) => { Crosshair.ChangeSize(-1); });
            AddButton((uint)m_position.x + 190, (uint)m_position.y + 60, 30, 30, 
                "+", (object sender, EventArgs e) => { Crosshair.ChangeSize(+1); });

            // Create Crosshair Thickness +/- Buttons
            AddButton((uint)m_position.x + 20, (uint)m_position.y + 100, 30, 30, 
                "-", (object sender, EventArgs e) => { Crosshair.ChangeThickness(-1); });
            AddButton((uint)m_position.x + 190, (uint)m_position.y + 100, 30, 30, 
                "+", (object sender, EventArgs e) => { Crosshair.ChangeThickness(+1); });

            // Assign setting values to sliders
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
                // Draw the background
                GUI.Label(new Rect(m_position, m_dimension), m_background, m_style);

                // Draw the Length and Thickness Labels
                GUI.Label(new Rect(m_position.x + 60, m_position.y + 70, 120, 30), "Length: " + Settings.GetValue("crosshairLength"));
                GUI.Label(new Rect(m_position.x + 60, m_position.y + 110, 120, 30), "Thickness: " + Settings.GetValue("crosshairThickness"));

                // Draw the RGBA Labels and Sliders
                // TODO: Find better way to handle Sliders. Maybe make some InputInterface class that handles Buttons/Sliders etc
                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 30, 200, 30), "R: " + rSliderValue);
                rSliderValue = (int)GUI.HorizontalSlider(new Rect(m_position.x + m_dimension.x / 2 + 60, m_position.y + 20, 200, 30), (int)rSliderValue, 0f, 255f);

                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 70, 200, 30), "G: " + gSliderValue);
                gSliderValue = (int)GUI.HorizontalSlider(new Rect(m_position.x + m_dimension.x / 2 + 60, m_position.y + 60, 200, 30), (int)gSliderValue, 0f, 255f);

                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 110, 200, 30), "B: " + bSliderValue);
                bSliderValue = (int)GUI.HorizontalSlider(new Rect(m_position.x + m_dimension.x / 2 + 60, m_position.y + 100, 200, 30), (int)bSliderValue, 0f, 255f);

                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 150, 200, 30), "A: " + aSliderValue);
                aSliderValue = (int)GUI.HorizontalSlider(new Rect(m_position.x + m_dimension.x / 2 + 60, m_position.y + 140, 200, 30), (int)aSliderValue, 0f, 255f);

                // Set crosshair Colour after getting slider values
                Crosshair.SetColor(rSliderValue, gSliderValue, bSliderValue, aSliderValue);

                // Update Buttons
                HandleButtons();
            }
        }

        // Calls the Update function on all Buttons to check if they were pressed, and execute their Action
        private static void HandleButtons()
        {
            foreach(GUIButton button in m_buttons)
            {
                button.Update();
            }
        }
    }
}
