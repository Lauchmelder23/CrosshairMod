using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod
{
    class GUISlider : InputObject
    {
        // Min/Max values for the slider
        public float Min { get; set; } = 0;
        public float Max { get; set; } = 0;

        // Current slider value
        public float Value { get; set; } = 0;

        public GUISlider(float x, float y, float width, float height, float min, float max, float init, string ID)
            : base(x, y, width, height, ID)
        {
            Min = min;
            Max = max;
            Value = init;
        }

        public GUISlider(string ID)
            :base (0, 0, 0, 0, ID)
        {

        }

        public override float Update()
        {
            Value = GUI.HorizontalSlider(new Rect(position, dimensions), Value, Min, Max);
            return Value;
        }
    }
}
