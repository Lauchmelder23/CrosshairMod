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
     */
    class GUIButton
    {
        // Position / Dimension of the Button.
        public Vector2 position = new Vector2(0, 0);
        public Vector2 dimensions = new Vector2(0, 0);

        // Label of the Button
        public string label = "";

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

        // Updates and Draws the Button.
        // TODO: Seperate PressChecking and Rendering in order to have Disabled Buttons
        public void Update()
        {
            // Get if the Button was pressed and invoke OnClick event accordingly
            bool buttonPressed = GUI.Button(new Rect(position, dimensions), label);
            if (buttonPressed)
                OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
