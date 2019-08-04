/* 
 * Source code of a mod that adds a crosshair into
 * the game Blackwake.
 * 
 * @author Lauchmelder
 * @version v0.3.1
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace CrosshairMod
{ 
    /// <summary>
    /// Class that gets loaded by the ModLoader. Calls every component of the Mod
    /// </summary>
    public class CrosshairMod : MonoBehaviour
    {
        /// <summary>
        /// Default Hotkey Definitions
        /// </summary>
        private char MENU_OPEN_KEY = 'H';
        private char CH_TOGGLE_KEY = 'J';

        void Start()
        {

            // Update the settings
            Settings.LoadSettings(".\\Blackwake_Data\\Managed\\Mods\\Assets\\chSettings.sett");
            // Create Crosshair
            Crosshair.Create();
            // Create Panel
            Interface.Init();

            // Load Hotkeys
            MENU_OPEN_KEY = (char)Settings.GetValue("hotkeyCrosshairToggle", true, MENU_OPEN_KEY);
            CH_TOGGLE_KEY = (char)Settings.GetValue("hotkeyGUIToggle", true, CH_TOGGLE_KEY);
        }

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

        void OnApplicationQuit()
        {
            // Save settings
            Settings.SaveSettings(".\\Blackwake_Data\\Managed\\Mods\\Assets\\chSettings.sett");
            Logging.Debug.Log("Saved Settings");
        }
    }
}
