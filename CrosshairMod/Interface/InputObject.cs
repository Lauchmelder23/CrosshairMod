using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;

namespace CrosshairMod
{
    /*
     * Base of all Input Objects.
     * 
     * Any Input Object that wants to be displayed in the Interface must
     * inherit from this class.
     */
    abstract class InputObject
    {
        // position and dimension of the object
        public Vector2 position, dimensions;

        // ID of the Object
        public readonly string ID;

        // constructor to set position and size
        public InputObject(float x, float y, float width, float height, string ID)
        {
            this.position = new Vector2(x, y);
            this.dimensions = new Vector2(width, height);
            this.ID = ID;
        }

        // the update method (that works as renderer) must be overriden by each object
        public abstract float Update();
    }
}
