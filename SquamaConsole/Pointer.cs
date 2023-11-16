using System;
using System.Runtime.InteropServices;
using System.Threading;

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

                    var color = ColorTaker.GetColorAt(new System.Drawing.Point(1052, 899));

                    if (color.ToString() == "Color [A=255, R=255, G=0, B=0]")
                    {
                        MouseClick.Click(1920, 1080);
                        Console.WriteLine("Рыбка клюет!");
                        //Console.WriteLine($"Координаты мыши: X={point.pointX}, Y={point.pointY}");
                    }
                    //Console.WriteLine(color.ToString());
                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }
    }
}
