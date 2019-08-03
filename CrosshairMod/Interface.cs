using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
        private static List<InputObject> m_inputs = new List<InputObject>();

        // Values of the RGBA Sliders
        private static int rSliderValue, gSliderValue, bSliderValue, aSliderValue;

        // Texture and Styles for the GUI background
        private static Texture2D m_background = new Texture2D(1, 1);
        private static GUIStyle m_style = new GUIStyle();
   
        // Position ind dimension of the GUI background
        private static Vector2 m_position;
        private static Vector2 m_dimension;


        // Creates a new button object and adds it to the List
        private static void AddButton(float x, float y, float width, float height, string label, string ID, params EventHandler[] onClickEvent)
        {
            GUIButton buttonObj = new GUIButton(x, y, width, height, label, ID, onClickEvent);
            m_inputs.Add(buttonObj);
        }

        // Creates a new slider object and adds it to the List
        // Returns the index of the button
        private static void AddSlider(float x, float y, float width, float height, float min, float max, float init, string ID)
        {
            GUISlider sliderObj = new GUISlider(x, y, width, height, min, max, init, ID);
            m_inputs.Add(sliderObj);
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

            // Create Crosshair Visibilty Button
            AddButton(m_position.x + 20, m_position.y + 20, 200, 30,
                (Crosshair.Enabled() ? "Hide Crosshair" : "Show Crosshair"), "Toggle", (object sender, EventArgs e) => { Crosshair.Toggle(); },
                (object sender, EventArgs e) => { GUIButton btn = (GUIButton)sender; btn.label = (Crosshair.Enabled() ? "Hide Crosshair" : "Show Crosshair"); });

            // Create Crosshair Size +/- Buttons
            AddButton(m_position.x + 20, m_position.y + 60, 30, 30, 
                "-", "sizedown", (object sender, EventArgs e) => { Crosshair.ChangeSize(-1); });
            AddButton(m_position.x + 190, m_position.y + 60, 30, 30, 
                "+", "sizeup", (object sender, EventArgs e) => { Crosshair.ChangeSize(+1); });

            // Create Crosshair Thickness +/- Buttons
            AddButton(m_position.x + 20, m_position.y + 100, 30, 30, 
                "-", "thickdown", (object sender, EventArgs e) => { Crosshair.ChangeThickness(-1); });
            AddButton(m_position.x + 190, m_position.y + 100, 30, 30, 
                "+", "thickup", (object sender, EventArgs e) => { Crosshair.ChangeThickness(+1); });

            rSliderValue = Settings.GetValue("crosshairColorRed");
            gSliderValue = Settings.GetValue("crosshairColorGreen");
            bSliderValue = Settings.GetValue("crosshairColorBlue");
            aSliderValue = Settings.GetValue("crosshairColorAlpha");

            // Create RGBA Sliders
            AddSlider(m_position.x + m_dimension.x / 2 + 60, m_position.y + 20, 200, 30, 0, 255, rSliderValue, "red");
            AddSlider(m_position.x + m_dimension.x / 2 + 60, m_position.y + 60, 200, 30, 0, 255, gSliderValue, "green");
            AddSlider(m_position.x + m_dimension.x / 2 + 60, m_position.y + 100, 200, 30, 0, 255, bSliderValue, "blue");
            AddSlider(m_position.x + m_dimension.x / 2 + 60, m_position.y + 140, 200, 30, 0, 255, aSliderValue, "alpha");
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
                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 70, 200, 30), "G: " + gSliderValue);
                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 110, 200, 30), "B: " + bSliderValue);
                GUI.Label(new Rect(m_position.x + m_dimension.x / 2 + 20, m_position.y + 150, 200, 30), "A: " + aSliderValue);

                // Set crosshair Colour after getting slider values
                IEnumerable<GUISlider> it = m_inputs.OfType<GUISlider>();
                rSliderValue = (int)it.First(slider => slider.ID == "red").Value;
                gSliderValue = (int)it.First(slider => slider.ID == "green").Value;
                bSliderValue = (int)it.First(slider => slider.ID == "blue").Value;
                aSliderValue = (int)it.First(slider => slider.ID == "alpha").Value;

                Crosshair.SetColor(rSliderValue, gSliderValue, bSliderValue, aSliderValue);

                // Update Buttons
                HandleButtons();
            }
        }

        // Calls the Update function on all Buttons to check if they were pressed, and execute their Action
        private static void HandleButtons()
        {
            foreach(InputObject obj in m_inputs)
            {
                obj.Update();
            }
        }
    }
}
