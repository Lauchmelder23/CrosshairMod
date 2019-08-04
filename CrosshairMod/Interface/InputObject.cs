﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod
{
    /// <summary>
    /// Base class for all InputObjects. Defines basic attributes that
    /// every input object needs
    /// </summary>
    abstract class InputObject
    {
        public Vector2 position, dimensions;
        
        // TODO: Remove once dictionary was implemented
        public readonly string ID;

        /// <summary>
        /// Create new InputObject
        /// </summary>
        /// <param name="x">X-Position</param>
        /// <param name="y">Y-Position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="ID">ID</param>
        public InputObject(float x, float y, float width, float height, string ID)
        {
            this.position = new Vector2(x, y);
            this.dimensions = new Vector2(width, height);
            this.ID = ID;
        }

        /// <summary>
        /// The Update method every Input Object needs.
        /// This is needed to store all kinds of input objects in one List for example
        /// </summary>
        /// <returns>Value of the Object</returns>
        public abstract float Update();
    }
}
