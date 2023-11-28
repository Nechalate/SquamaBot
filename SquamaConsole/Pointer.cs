using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;

namespace SquamaConsole
{
    internal class Pointer
    {
        public struct POINT
        {
            public int pointX;
            public int pointY;
        }

        static string windowTitle = "RАGЕ Multiplаyer ";

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

                    var point1 = new System.Drawing.Point(1052, 899);
                    var point2 = new System.Drawing.Point(661, 1019);
                    var point3 = new System.Drawing.Point(912, 605);

                    var color = ColorTaker.GetColorAtInWindow(windowTitle, point1);
                    var colorGrab = ColorTaker.GetColorAtInWindow(windowTitle, point2);
                    var colorCaptcha = ColorTaker.GetColorAtInWindow(windowTitle, point3);
                    /*
                    Console.WriteLine($"Color1: {color}");
                    Console.WriteLine($"Color2: {colorGrab}");
                    Console.WriteLine($"Color3: {colorCaptcha}");
                    */
                    if (color.ToString() == "Color [A=255, R=255, G=0, B=0]")
                    {
                        MouseClick.ClickInsideWindow(windowTitle, 50, 900);
                    }
                    if (colorGrab.ToString() == "Color [A=255, R=148, G=248, B=7]")
                    {
                        //Thread.Sleep(500);
                        //KeyboardPress.RodPut(windowTitle);
                        //Thread.Sleep(5000);
                    }
                    if (colorCaptcha.ToString() == "Color [A=255, R=51, G=219, B=42]")
                    {
                        Console.Beep();
                        Bitmap captchaImage = Captcha.CaptureCaptchaArea(861, 456, 197, 48);
                        Bitmap[] bit = Captcha.ScreenShotCutter(captchaImage);

                        for (int i = 0; i < 7; i++)
                        {
                            string captchaText = Captcha.RecognizeCaptcha(bit[i]);
                            Console.WriteLine($"Распознанный текст капчи: {captchaText}");
                        }

                        //string captchaText = Captcha.RecognizeCaptcha(captchaImage);
                        //Console.WriteLine($"Распознанный текст капчи: {captchaText}");
                        Thread.Sleep(5000);
                    }

                    Thread.Sleep(3000);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }
    }
}
