using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

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
                    var colorGrab = ColorTaker.GetColorAt(new System.Drawing.Point(661, 1019));
                    
                    if (color.ToString() == "Color [A=255, R=255, G=0, B=0]")
                    {
                        MouseClick.Click(1920, 1080);
                        //Console.WriteLine("Рыбка клюет!");
                        //Console.WriteLine($"Координаты мыши: X={point.pointX}, Y={point.pointY}");
                    }
                    if (colorGrab.ToString() == "Color [A=255, R=148, G=248, B=7]")
                    {
                        Console.WriteLine("I see");
                        Thread.Sleep(500);
                        KeyboardPress.RodPut();
                        Thread.Sleep(5000);
                    }
                        //Console.WriteLine(colorGrab.ToString());

                        //Console.WriteLine($"Координаты мыши: X={point.pointX}, Y={point.pointY}");
                        Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }
    }
}
