/* 
 * Source code of a mode that adds a crosshair into
 * the game Blackwake.
 * 
 * @author Lauchmelder
 * @version v0.2
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CrosshairMod
{ 
    public class Main : MonoBehaviour
    {
        private const string MENU_OPEN_KEY = "H";
        private const string CH_TOGGLE_KEY = "J";

        // This will be executed first
        void Start()
        {
            // Update the settings
            Settings.LoadSettings(".\\Blackwake_Data\\Managed\\Mods\\chSettings.sett");
            // Create Crosshair
            Crosshair.Create();
            // Create Panel
            Interface.Init();
        }

        void OnGUI()
        {
            // Check for Key press
            if(Event.current.Equals(Event.KeyboardEvent(MENU_OPEN_KEY)))
            {
                Interface.Toggle();
            }

            if (Event.current.Equals(Event.KeyboardEvent(CH_TOGGLE_KEY)))
            {
                Crosshair.Toggle();
            }

            //Render GUI
            Interface.Render();
            //Render Crosshair
            Crosshair.Render();
        }

        void OnApplicationQuit()
        {
            // Save settings
            Settings.SaveSettings(".\\Blackwake_Data\\Managed\\Mods\\chSettings.sett");
            Logging.Log("Saved Settings");
        }
    }
}
