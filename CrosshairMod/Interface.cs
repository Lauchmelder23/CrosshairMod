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


            // Create Crosshair Visibilty Button
            AddButton(20, 20, 200, 30,
                (Crosshair.Enabled() ? "Hide Crosshair" : "Show Crosshair"), "Toggle", (object sender, EventArgs e) => { Crosshair.Toggle(); },
                (object sender, EventArgs e) => { GUIButton btn = (GUIButton)sender; btn.label = (Crosshair.Enabled() ? "Hide Crosshair" : "Show Crosshair"); });

            // Create Crosshair Size +/- Buttons
            AddButton(20, 60, 30, 30, 
                "-", "sizedown", (object sender, EventArgs e) => { Crosshair.ChangeSize(-1); });
            AddButton(190, 60, 30, 30, 
                "+", "sizeup", (object sender, EventArgs e) => { Crosshair.ChangeSize(+1); });

            // Create Crosshair Thickness +/- Buttons
            AddButton(20, 100, 30, 30, 
                "-", "thickdown", (object sender, EventArgs e) => { Crosshair.ChangeThickness(-1); });
            AddButton(190, 100, 30, 30, 
                "+", "thickup", (object sender, EventArgs e) => { Crosshair.ChangeThickness(+1); });

            rSliderValue = Settings.GetValue("crosshairColorRed");
            gSliderValue = Settings.GetValue("crosshairColorGreen");
            bSliderValue = Settings.GetValue("crosshairColorBlue");
            aSliderValue = Settings.GetValue("crosshairColorAlpha");

            // Create RGBA Sliders
            AddSlider(m_dimension.x / 2 + 60, 30, 200, 10, 0, 255, rSliderValue, "red");
            AddSlider(m_dimension.x / 2 + 60, 70, 200, 30, 0, 255, gSliderValue, "green");
            AddSlider(m_dimension.x / 2 + 60, 110, 200, 30, 0, 255, bSliderValue, "blue");
            AddSlider(m_dimension.x / 2 + 60, 150, 200, 30, 0, 255, aSliderValue, "alpha");
        }

        // Displays / Hides the menu
        public static void Toggle()
        {
            m_visible = !m_visible;
        }

        // Renders the window
        public static void Render()
        {
            if (m_visible)
                GUI.Window(420, new Rect(m_position, m_dimension), RenderFunc, "Crosshair Settings");
        }

        // Renders the Panel, but also handles Updating the buttons
        private static void RenderFunc(int windowID)
        {
           
            // Draw the Length and Thickness Labels
            GUI.Label(new Rect(60, 70, 120, 30), "Length: " + Settings.GetValue("crosshairLength"));
            GUI.Label(new Rect(60, 110, 120, 30), "Thickness: " + Settings.GetValue("crosshairThickness"));

            // Draw the RGBA Labels and Sliders
            GUI.Label(new Rect(m_dimension.x / 2 + 20, 30, 200, 30), "R: " + rSliderValue);
            GUI.Label(new Rect(m_dimension.x / 2 + 20, 70, 200, 30), "G: " + gSliderValue);
            GUI.Label(new Rect(m_dimension.x / 2 + 20, 110, 200, 30), "B: " + bSliderValue);
            GUI.Label(new Rect(m_dimension.x / 2 + 20, 150, 200, 30), "A: " + aSliderValue);

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
