using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TechEngine.Engine
{
    public static class InputHandler
    {
        public static bool KeyQ { get; set; }
        public static bool KeyW { get; set; }
        public static bool KeyE { get; set; }
        public static bool KeyA { get; set; }
        public static bool KeyS { get; set; }
        public static bool KeyD { get; set; }
        public static bool KeyZ { get; set; }
        public static bool KeyX { get; set; }

        public static bool KeyNumPad4 { get; set; }
        public static bool KeyNumPad6 { get; set; }
        public static bool KeyNumPad8 { get; set; }
        public static bool KeyNumPad5 { get; set; }
        public static bool KeyNumPad7 { get; set; }
        public static bool KeyNumPad9 { get; set; }

        public static void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                    KeyQ = true;
                    break;
                case Keys.W:
                    KeyW = true;
                    break;
                case Keys.E:
                    KeyE = true;
                    break;
                case Keys.A:
                    KeyA = true;
                    break;
                case Keys.S:
                    KeyS = true;
                    break;
                case Keys.D:
                    KeyD = true;
                    break;
                case Keys.Z:
                    KeyZ = true;
                    break;
                case Keys.X:
                    KeyX = true;
                    break;
                case Keys.NumPad4:
                    KeyNumPad4 = true;
                    break;
                case Keys.NumPad6:
                    KeyNumPad6 = true;
                    break;
                case Keys.NumPad8:
                    KeyNumPad8 = true;
                    break;
                case Keys.NumPad5:
                    KeyNumPad5 = true;
                    break;
                case Keys.NumPad7:
                    KeyNumPad7 = true;
                    break;
                case Keys.NumPad9:
                    KeyNumPad9 = true;
                    break;
            }
        }

        public static void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                    KeyQ = false;
                    break;
                case Keys.W:
                    KeyW = false;
                    break;
                case Keys.E:
                    KeyE = false;
                    break;
                case Keys.A:
                    KeyA = false;
                    break;
                case Keys.S:
                    KeyS = false;
                    break;
                case Keys.D:
                    KeyD = false;
                    break;
                case Keys.Z:
                    KeyZ = false;
                    break;
                case Keys.X:
                    KeyX = false;
                    break;
                case Keys.NumPad4:
                    KeyNumPad4 = false;
                    break;
                case Keys.NumPad6:
                    KeyNumPad6 = false;
                    break;
                case Keys.NumPad8:
                    KeyNumPad8 = false;
                    break;
                case Keys.NumPad5:
                    KeyNumPad5 = false;
                    break;
                case Keys.NumPad7:
                    KeyNumPad7 = false;
                    break;
                case Keys.NumPad9:
                    KeyNumPad9 = false;
                    break;
            }
        }
    }
}
