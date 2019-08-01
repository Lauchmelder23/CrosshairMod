using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrosshairMod
{
    // Button Wrapper class, utilizing GUI.Button()
    public class Button
    {
        public Vector2 position = new Vector2(0, 0);
        public Vector2 dimensions = new Vector2(0, 0);
        public string label = "";
        public Action OnClick = new Action(() => { });

        public Button(uint x, uint y, uint width, uint height, string label, Action e)
        {
            this.position = new Vector2(x, y);
            this.dimensions = new Vector2(width, height);
            this.label = label;
            this.OnClick = e;
        }

        public Button() { /*Empty*/ }

        public void Update()
        {
            bool buttonClicked = GUI.Button(new Rect(position, dimensions), label);
            if (buttonClicked)
                OnClick();
        }
    }
}
