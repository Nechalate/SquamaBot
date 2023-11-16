using System;
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
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
        string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void RodPut()
        {
            IntPtr rageHandle = FindWindow("RAGE Multiplayer Launcher", "RAGE Multiplayer");
            SetForegroundWindow(rageHandle);
            SendKeys.SendWait("1");
        }
    }
}
