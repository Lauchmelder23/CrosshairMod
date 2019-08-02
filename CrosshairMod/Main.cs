/* 
 * Source code of a mode that adds a crosshair into
 * the game Blackwake.
 * 
 * @author Lauchmelder
 * @version v0.3
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CrosshairMod
{ 

    /*
     * This is the Main class that is responsible for
     * handling initializing and updating the components
     * of the crosshair mod. 
     */
    public class Main : MonoBehaviour
    {
        // Define Hotkeys for Menu and Crosshair Toggle
        private char MENU_OPEN_KEY = 'H';
        private char CH_TOGGLE_KEY = 'J';

        // This will be executed first
        void Start()
        {

            // Update the settings
            Settings.LoadSettings(".\\Blackwake_Data\\Managed\\Mods\\chSettings.sett");
            // Create Crosshair
            Crosshair.Create();
            // Create Panel
            Interface.Init();

            // Load Hotkeys
            MENU_OPEN_KEY = (char)Settings.GetValue("hotkeyCrosshairToggle", true, MENU_OPEN_KEY);
            CH_TOGGLE_KEY = (char)Settings.GetValue("hotkeyGUIToggle", true, CH_TOGGLE_KEY);
        }

        // This gets called on every GUI Update (Can be multiple tiems per Frame)
        void OnGUI()
        {
            // Check for Key presses
            if (Event.current.Equals(Event.KeyboardEvent(MENU_OPEN_KEY.ToString())))
            {
                // Toggle Crosshair GUI
                Interface.Toggle();
            }

            if (Event.current.Equals(Event.KeyboardEvent(CH_TOGGLE_KEY.ToString())))
            {
                // Toggle Crosshair
                Crosshair.Toggle();
            }

            //Render GUI
            Interface.Render();
            //Render Crosshair
            Crosshair.Render();
        }

        // Will be called when the application is closed
        void OnApplicationQuit()
        {
            // Save settings
            Settings.SaveSettings(".\\Blackwake_Data\\Managed\\Mods\\chSettings.sett");
            Logging.Debug.Log("Saved Settings");
        }
    }
}
