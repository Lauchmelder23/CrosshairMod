using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod.Input
{
    class Slider : InputObject
    {
        /// <summary>
        /// Min/Max Slider values
        /// </summary>
        public float Min = 0;
        public float Max = 0;

        /// <summary>
        /// Current slider value
        /// </summary>
        public float Value = 0;

        /// <summary>
        /// Creates a new Slider
        /// </summary>
        /// <param name="x">X-Position</param>
        /// <param name="y">Y-Position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="min">Minimum Slider value</param>
        /// <param name="max">Maximum Slider value</param>
        /// <param name="init">Initial Slider value</param>
        /// <param name="ID">ID</param>
        public Slider(float x, float y, float width, float height, float min, float max, float init, string ID)
            : base(x, y, width, height, ID)
        {
            Min = min;
            Max = max;
            Value = init;
        }

        /// <summary>
        /// Sad, pathetic default constructor
        /// </summary>
        /// <param name="ID"></param>
        public Slider(string ID)
            :base (0, 0, 0, 0, ID)
        {

        }

        /// <summary>
        /// Draws and Updates the slider
        /// </summary>
        /// <returns>Value of the Slider</returns>
        public override float Update()
        {
            Value = GUI.HorizontalSlider(new Rect(position, dimensions), Value, Min, Max);
            return Value;
        }
    }
}
