using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SquamaConsole
{
    internal class Pointer
    {
        public struct POINT
        {
            public int pointX;
            public int pointY;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        public static void MouseTrackingThread()
        {
            try
            {
                while (true)
                {
                    POINT point;
                    GetCursorPos(out point);

                    Console.WriteLine($"Координаты мыши: X={point.pointX}, Y={point.pointY}");

                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }
    }
}
