﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrosshairMod
{
    static class Crosshair
    {
        private static Texture2D m_texture = new Texture2D(0, 0);
        private static GUIStyle m_style;
        private static bool m_enabled = true;

        // Toggles visibilty of the crosshair
        public static void Toggle()
        {
            m_enabled = !m_enabled;
        }

        // This must be called, or else no crosshair will be rendered
        public static void Create()
        {
            // Creates a crosshair texture
            // Assign dictionary values to variables
            int m_crosshairLength = Settings.GetValue("crosshairLength");
            int m_crosshairThickness = Settings.GetValue("crosshairThickness");
            int m_crosshairColorRed = Settings.GetValue("crosshairColorRed");
            int m_crosshairColorGreen = Settings.GetValue("crosshairColorGreen");
            int m_crosshairColorBlue = Settings.GetValue("crosshairColorBlue");
            int m_crosshairColorAlpha = Settings.GetValue("crosshairColorAlpha");

            // Construct color object from RGBA values
            Color m_crosshairColor = new Color(m_crosshairColorRed / 255f,
                                                m_crosshairColorGreen / 255f,
                                                m_crosshairColorBlue / 255f,
                                                m_crosshairColorAlpha / 255f);

            // Lazily add 1 to thickness if it's even. If it is even the crosshair can't be symmetrical
            m_crosshairThickness += (m_crosshairThickness % 2 == 0) ? 1 : 0;

            // Create Texture with approprate dimensions. The m_crosshairLength is the distance 
            // between center and end-of-line (end-points included)
            m_texture = new Texture2D((m_crosshairLength - 1) * 2 + 1, (m_crosshairLength - 1) * 2 + 1);


            // Fill Texture with some color where alpha = 0
            for (int y = 0; y < m_texture.height; y++)
                for (int x = 0; x < m_texture.width; x++)
                    m_texture.SetPixel(x, y, new Color(0, 0, 0, 0));

            // The texture will always have odd dimensions, so we can be sure that there will be a definite center (which is m_crossheirLength)
            // Draw vertical line:
            for (int y = 0; y < m_texture.height; y++)
            {
                for (int i = 0; i < m_crosshairThickness; i++)
                {
                    m_texture.SetPixel(m_crosshairLength - (int)Math.Floor((double)m_crosshairThickness / 2) + i - 1, y, m_crosshairColor);
                }
            }

            // Draw horizontal line:
            for (int x = 0; x < m_texture.height; x++)
            {
                for (int i = 0; i < m_crosshairThickness; i++)
                {
                    m_texture.SetPixel(x, m_crosshairLength - (int)Math.Floor((double)m_crosshairThickness / 2) + i - 1, m_crosshairColor);
                }
            }

            // Set texture settings and apply changes
            m_texture.wrapMode = TextureWrapMode.Repeat;
            m_texture.Apply();

            // Create GUIStyle to render the crosshair
            m_style = new GUIStyle();
            m_style.normal.background = m_texture;
        }

        public static void Render()
        {
            if(InvalidCrosshair())
            {
                Logging.LogWarning("Crosshair was either not initialized, or has an invalid size of (0, 0). Check your settings file and adjust your settings accordingly");
                return;
            }

            if (m_enabled)
                GUI.Label(new Rect(Screen.width / 2 - m_texture.width / 2, Screen.height / 2 - m_texture.height / 2, m_texture.width, m_texture.height),
                    m_texture, m_style);
        }


        private static bool InvalidCrosshair()
        {
            // Check if the texture is bigger than (0, 0) to see if it was initialized.
            return (m_texture.width == 0 && m_texture.height == 0);
        }
    }
}
