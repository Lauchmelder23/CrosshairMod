using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod.Input
{
    /// <summary>
    /// Button wrapper for Unitys GUI.Button() function. I made this in order
    /// to store buttons and render them in Lists
    /// </summary>
    class Button : InputObject
    {
        /// <summary>
        /// Event Handler that contains the actions to execute when the Button was clicked
        /// </summary>
        public event EventHandler OnClick;

        public string Label = "";

        /// <summary>
        /// Create new Button
        /// </summary>
        /// <param name="x">X-Position</param>
        /// <param name="y">Y-Position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="label">Text inside the button</param>
        /// <param name="OnClickEvent">Action to execute when button is pressed</param>
        public Button(float x, float y, float width, float height, string label, params EventHandler[] OnClickEvent)
            : base(x, y, width, height)
        {
            Logging.Debug.Log("Button Constructor: " + label);

            // Assign position, dimension and label
            this.Label = label;

            // Push OnClickEvents
            foreach(EventHandler e in OnClickEvent)
                OnClick += e;
        }

        /// <summary>
        /// Sad, pathetic default constructor
        /// </summary>
        public Button()
            : base(0, 0, 0, 0)
        {
            // Empty
        }

        /// <summary>
        /// Draws button and returns button state / executes button action
        /// </summary>
        /// <returns>1f if the button is pressed, 0f else</returns>
        public override float Update()
        {
            // Get if the Button was pressed and invoke OnClick event accordingly
            bool buttonPressed = GUI.Button(new Rect(position, dimensions), Label);
            if (buttonPressed)
                OnClick?.Invoke(this, EventArgs.Empty);

            return (buttonPressed ? 1.0f : 0.0f);
        }
    }
}
