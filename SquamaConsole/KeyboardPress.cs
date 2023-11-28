﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SquamaConsole
{
    internal class KeyboardPress
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName,
        string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void RodPut(string windowTitle)
        {
            IntPtr rageHandle = FindWindow(null, windowTitle);
            SetForegroundWindow(rageHandle);
            SendKeys.SendWait("1");
        }
    }
}
