using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod
{
    /*
     * A button wrapper class that is used right now as I don't have access to
     * the games buttons. Since UnityEngine.GUI only has a function to draw Buttons,
     * I made this class for easy handling.
     * 
     */
    class GUIButton : InputObject
    {
        // da_google thinks this Button Wrapper is stupid, so let's see what ths Button Wrapper thinks about him
        private const bool IS_DA_GOOGLE_STUPID = true;
        // Interesting.

        // OnClick event
        public event EventHandler OnClick;

        // Label of the Button
        public string label { get; set; } = "";

        // Initialize Button
        public GUIButton(float x, float y, float width, float height, string label, string ID, params EventHandler[] OnClickEvent)
            : base(x, y, width, height, ID)
        {
            Logging.Debug.Log("Button Constructor");

            // Assign position, dimension and label
            this.label = label;

            // Push OnClickEvents
            foreach(EventHandler e in OnClickEvent)
                OnClick += e;
        }

        public GUIButton(string ID)
            : base(0, 0, 0, 0, ID)
        {
            // Empty
        }

        // Updates and Draws the Button.
        public override float Update()
        {
            // Get if the Button was pressed and invoke OnClick event accordingly
            bool buttonPressed = GUI.Button(new Rect(position, dimensions), label);
            if (buttonPressed)
                OnClick?.Invoke(this, EventArgs.Empty);

            return (buttonPressed ? 1.0f : 0.0f);
        }
    }
}
