using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrosshairMod
{
    // Button Wrapper class, utilizing GUI.Button()
    class GUIButton
    {

        public Vector2 position = new Vector2(0, 0);
        public Vector2 dimensions = new Vector2(0, 0);
        public string label = "";
        public event EventHandler OnClick;

        public GUIButton(uint x, uint y, uint width, uint height, string label)
        {
            Logging.Log("Button Constructor");

            this.position = new Vector2(x, y);
            this.dimensions = new Vector2(width, height);
            this.label = label;
        }

        public GUIButton()
        {
            // Empty
        }

        public void Update()
        {
            bool buttonPressed = GUI.Button(new Rect(position, dimensions), label);
            if (buttonPressed)
                OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
