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

        static object lockObject = new object();
        static bool isPaused = false;

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        static public void TogglePause()
        {
            lock (lockObject)
            {
                isPaused = !isPaused;

                if (!isPaused)
                {
                    Monitor.Pulse(lockObject);
                }
            }
            Console.WriteLine(isPaused ? "Программа приостановлена." : "Программа возобновлена.");
        }

        public static void MouseTrackingThread()
        {
            try
            {
                while (true)
                {
                    lock (lockObject)
                    {
                        while (isPaused)
                        {
                            Monitor.Wait(lockObject);
                        }
                    }

                    POINT point;
                    GetCursorPos(out point);

                    var color = ColorTaker.GetColorAt(new System.Drawing.Point(1052, 899));
                    var colorGrab = ColorTaker.GetColorAt(new System.Drawing.Point(661, 1019));
                    var colorCaptcha = ColorTaker.GetColorAt(new System.Drawing.Point(912, 605));

                    if (color.ToString() == "Color [A=255, R=255, G=0, B=0]")
                    {
                        MouseClick.Click(1920, 1080);
                    }
                    if (colorGrab.ToString() == "Color [A=255, R=148, G=248, B=7]")
                    {
                        Thread.Sleep(500);
                        KeyboardPress.RodPut();
                        Thread.Sleep(5000);
                    }
                    if (colorCaptcha.ToString() == "Color [A=255, R=51, G=219, B=42]")
                    {
                        Console.Beep();
                        Thread.Sleep(10000);
                    }

                    Thread.Sleep(50);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }
    }
}
