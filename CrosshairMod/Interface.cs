using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod
{
    /// <summary>
    /// Handles and contains every Object needed for the interface
    /// </summary>
    static class Interface
    {
        private static bool m_visible = false;

        /// <summary>
        /// A list of all Objects (That includes buttons, sliders etc) inside the GUI Window
        /// This makes it easy to add more components if it is needed.
        /// </summary>
        private static Dictionary<string, Input.InputObject> m_inputs = new Dictionary<string, Input.InputObject>();

        /// <summary>
        /// RGBA Slider values
        /// </summary>
        private static int rSliderValue, gSliderValue, bSliderValue, aSliderValue;
   
        private static Vector2 m_position;
        private static Vector2 m_dimension;


        /// <summary>
        /// Creates a new Button and adds it to the content list
        /// </summary>
        /// <param name="x">X-Position</param>
        /// <param name="y">Y-position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="label">Text to be displayed on the button</param>
        /// <param name="ID">Key of the Button</param>
        /// <param name="onClickEvent">Action to be executed when the button is pressed</param>
        private static void AddButton(float x, float y, float width, float height, string label, string ID, params EventHandler[] onClickEvent)
        {
            Input.Button buttonObj = new Input.Button(x, y, width, height, label, onClickEvent);
            m_inputs.Add(ID, buttonObj);
        }

        /// <summary>
        /// Creates a new Slider and adds ot to the content list
        /// </summary>
        /// <param name="x">X-Position</param>
        /// <param name="y">Y-Position/param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="min">Minimum value of the slider</param>
        /// <param name="max">Maximum value of the slider</param>
        /// <param name="init">Starting value of the slider</param>
        /// <param name="ID">Key of the slider</param>
        private static void AddSlider(float x, float y, float width, float height, float min, float max, float init, string ID)
        {
            Input.Slider sliderObj = new Input.Slider(x, y, width, height, min, max, init);
            m_inputs.Add(ID, sliderObj);
        }

        /// <summary>
        /// Creates all needed input objects and initializes window
        /// </summary>
        public static void Init()
        {
            // Set dimension to 0.25 of the screen width/height
            m_dimension = new Vector2(Screen.width / 4, Screen.height / 4);
            // Center the interface
            m_position = new Vector2((Screen.width - m_dimension.x) / 2, (Screen.height - m_dimension.y) / 2);


            // Create Crosshair Visibilty Button
            AddButton(20, 20, 200, 30,
                (Crosshair.Enabled() ? "Hide Crosshair" : "Show Crosshair"), "Toggle", (object sender, EventArgs e) => { Crosshair.Toggle(); },
                (object sender, EventArgs e) => { Input.Button btn = (Input.Button)sender; btn.Label = (Crosshair.Enabled() ? "Hide Crosshair" : "Show Crosshair"); });

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

        public static void Toggle()
        {
            m_visible = !m_visible;
        }

        public static void Render()
        {
            if (m_visible)
                GUI.Window(420, new Rect(m_position, m_dimension), RenderFunc, "Crosshair Settings");
        }

        /// <summary>
        /// Where actual rendering happens. Draws all objects and updated them
        /// </summary>
        private static void RenderFunc(int windowID)
        {
            // Make Window draggable
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            
            // Draw the Length and Thickness Labels
            GUI.Label(new Rect(60, 70, 120, 30), "Length: " + Settings.GetValue("crosshairLength"));
            GUI.Label(new Rect(60, 110, 120, 30), "Thickness: " + Settings.GetValue("crosshairThickness"));

            // Draw the RGBA Labels and Sliders
            GUI.Label(new Rect(m_dimension.x / 2 + 20, 30, 200, 30), "R: " + rSliderValue);
            GUI.Label(new Rect(m_dimension.x / 2 + 20, 70, 200, 30), "G: " + gSliderValue);
            GUI.Label(new Rect(m_dimension.x / 2 + 20, 110, 200, 30), "B: " + bSliderValue);
            GUI.Label(new Rect(m_dimension.x / 2 + 20, 150, 200, 30), "A: " + aSliderValue);

            // Set crosshair Colour after getting slider values
            IEnumerable<Input.Slider> it = m_inputs.OfType<Input.Slider>();
            rSliderValue = (int)((Input.Slider)m_inputs["red"]).Value;
            gSliderValue = (int)((Input.Slider)m_inputs["green"]).Value;
            bSliderValue = (int)((Input.Slider)m_inputs["blue"]).Value;
            aSliderValue = (int)((Input.Slider)m_inputs["alpha"]).Value;

            Crosshair.SetColor(rSliderValue, gSliderValue, bSliderValue, aSliderValue);

            // Update Buttons
            HandleInputObjects();
            
        }

        /// <summary>
        /// Calls the Update function of each input object
        /// </summary>
        private static void HandleInputObjects()
        {
            foreach(KeyValuePair<string, Input.InputObject> obj in m_inputs)
            {
                obj.Value.Update();
            }
        }
    }
}
