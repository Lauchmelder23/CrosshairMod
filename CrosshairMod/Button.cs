using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrosshairMod
{
    /*
     * A button wrapper class that is used right now as I don't have access to
     * the games buttons. Since UnityEngine.GUI only has a function to draw Buttons,
     * I made this class for easy handling.
     * 
     */
    class GUIButton
    {
        // da_google thinks this Button Wrapper is stupid, so let's see what ths Button Wrapper thinks about him
        private const bool IS_DA_GOOGLE_STUPID = true;
        // Interesting.

        // Position / Dimension of the Button.
        public Vector2 position = new Vector2(0, 0);
        public Vector2 dimensions = new Vector2(0, 0);

        // Visibilty variables
        private bool m_visible = true;
        private bool m_active = true;

        // Label of the Button
        public string label { get; set; } = "";

        // OnClick event
        public event EventHandler OnClick;

        // Initialize Button
        public GUIButton(uint x, uint y, uint width, uint height, string label)
        {
            Logging.Debug.Log("Button Constructor");

            // Assign position, dimension and label
            this.position = new Vector2(x, y);
            this.dimensions = new Vector2(width, height);
            this.label = label;
        }

        public GUIButton()
        {
            // Empty
        }


        // Changes visibilty of the Button
        public void Toggle()
        {
            m_visible = !m_visible;
        }

        // Changes Usabilty of the button
        public void Activate()
        {
            m_active = !m_active;
        }

        // Updates and Draws the Button.
        public void Update()
        {
            if (m_visible)
            {
                // Get if the Button was pressed and invoke OnClick event accordingly
                bool buttonPressed = GUI.Button(new Rect(position, dimensions), label);
                if (buttonPressed && m_active)
                    OnClick?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
