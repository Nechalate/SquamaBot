using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Documents;

namespace SquamaConsole
{
    internal class MainThread
    {
        public struct POINT
        {
            public int pointX;
            public int pointY;
        }

        static string windowTitle = Program.windowTitle;

        static object lockObject = new object();
        static bool isPaused = false;

        static int errorsCounter = 0;

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

        public static void MainProgramThread()
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

                    var fishHookingColor = ColorGetter.GetColorAtInWindow(windowTitle, point1);
                    var repeatFishingColor = ColorGetter.GetColorAtInWindow(windowTitle, point2);
                    var captchaColor = ColorGetter.GetColorAtInWindow(windowTitle, point3);

                    //InventorySpaceControl();

                    if (fishHookingColor.ToString() == "Color [A=255, R=255, G=0, B=0]")
                    {
                        LkmEmulation.FishHooking(windowTitle, 50, 900);
                    }
                    if (repeatFishingColor.ToString() == "Color [A=255, R=148, G=248, B=7]")
                    {
                        CastingFishingRod();
                    }
                    if (captchaColor.ToString() == "Color [A=255, R=51, G=219, B=42]")
                    {
                        CatchTheCaptcha();
                    }

                    Thread.Sleep(70);
                }
            }
            catch (ThreadAbortException)
            {
                Console.Write("Error");
            }
        }

        private static void CastingFishingRod() // Connected method
        {
            Thread.Sleep(300);

            ButtonEmulation.PressTheButton(windowTitle, "I"); // Open Inventory

            Thread.Sleep(500);

            InventorySpaceControl();

            Thread.Sleep(5000); // Delay to avoid errors
        }

        private static void CatchTheCaptcha() // Connected method
        {
            Console.Beep(); // Sound signal to catch

            Bitmap captchaImage = Screens.CaptureScreenshotArea(861, 456, 197, 48); // Area of screenshot
            Bitmap[] bit = Screens.ScreenShotCutter(captchaImage); // Screenshot slices

            for (int i = 0; i < 7; i++)
            {
                string captchaText = Screens.RecognizeScreen(bit[i]); // Job the tesseract | Метод будет изменен на прилет скриншота капчи в лс вконтакте
                Console.WriteLine($"Распознанный текст капчи: {captchaText}"); // Print results
            }

            Thread.Sleep(3000); // Delay to avoid errors
        }

        public static string InventorySpaceControl() // Checker the space inventory
        {
            Bitmap inventorySpaceImage = Screens.CaptureScreenshotArea(1628, 182, 45, 30); // Area of inventory space 41 25 //45
            string inventoryText = Screens.RecognizeScreen(ScreenshotsEffects.ColorReplace(inventorySpaceImage)); // Tesseract work

            if (inventoryText.Contains(" "))
            {
                
                inventoryText = inventoryText.Replace(" ", "");
            }

            try
            {
                if (Convert.ToInt32(inventoryText) >= 950 && Convert.ToInt32(inventoryText[0]) < 9) // TEST
                {
                    Console.Beep();
                    Console.WriteLine("Инвентарь заполнен.");
                    Console.WriteLine(Convert.ToInt32(inventoryText));
                    Screens.SaveBitmaps(inventorySpaceImage);
                    TogglePause();
                }
                else
                {
                    Screens.SaveBitmaps(inventorySpaceImage);
                    errorsCounter = 0;

                    Thread.Sleep(550);
                    LkmEmulation.FishHooking(windowTitle, 1492, 287); // Click the rod

                    Thread.Sleep(350);
                    LkmEmulation.FishHooking(windowTitle, 1492, 329); // Click the start fishing
                    Console.WriteLine(Convert.ToInt32(inventoryText));
                }
            }
            catch (System.FormatException)
            {
                errorsCounter++;
                Console.WriteLine($"ERROR: {errorsCounter}");

                if (errorsCounter < 10)
                {
                    InventorySpaceControl();
                }
                else
                {
                    Console.WriteLine("Непредвиденная ошибка чтения. Начините или остановите рыбалку" +
                        " самостоятельно.");
                    errorsCounter = 0;
                    Console.Beep();
                    Screens.SaveBitmaps(ScreenshotsEffects.ColorReplace(inventorySpaceImage));
                    TogglePause();
                }
            }

            return inventoryText;
        }
    }
}
