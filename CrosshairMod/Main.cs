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
        // Initialize State checker. If this is false, the crosshair won't be drawn
        // This is a temporary fix to stop this mod from spamming errors in the log
        private bool m_validState = true;
        private bool m_renderCrosshair = true;
       
        // Reads settings file and sets all variables
        

        


        // This will be executed first
        void Start()
        {
            // Update the settings
            Settings.LoadSettings(".\\Blackwake_Data\\Managed\\Mods\\chSettings.sett");
            // Create Crosshair
            Crosshair.Create();

            // Add function to Button
            crosshairButton.OnClick += (object sender, EventArgs e) => { Crosshair.Toggle(); };
        }

        private GUIButton crosshairButton = new GUIButton(200, 10, 100, 20, "Toggle Crosshair");

        void OnGUI()
        {
            crosshairButton.Update();

            //Render Crosshair
            Crosshair.Render();
        }
    }
}
