using System.Drawing;
using System.IO;
using Tesseract;
using System.Linq;
using Emgu.CV.Reg;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;
using AutoIt;
using System.Windows.Automation;
using System.Windows.Forms;

namespace SquamaConsole
{
    internal class Screens
    {
        static string directory = @"D:\PetProjects\Squama\inventory_errors"; // Directory to save screenshots

        public static string RecognizeScreen(Bitmap captchaImage)
        {
            using (var engine = new TesseractEngine(@"D:\PetProjects\Squama\traineddata", "eng", EngineMode.Default))
            {
                engine.SetVariable("tessedit_char_whitelist", "0123456789");

                using (var image = PixConverter.ToPix(captchaImage))
                {
                    using (var page = engine.Process(image))
                    {
                        return page.GetText().Trim();
                    }
                }
            }
        }

        public static void ScreenshotFull(IntPtr windowTitle)
        {
            SetForegroundWindow(windowTitle);
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                bitmap.Save("test.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        static IntPtr FindWindow(string title)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (var process in System.Diagnostics.Process.GetProcesses())
            {
                if (process.MainWindowTitle == title)
                {
                    hWnd = process.MainWindowHandle;
                    break;
                }
            }
            return hWnd;
        }

        static Bitmap CaptureWindow(IntPtr hWnd, RECT rect)
        {
            Bitmap screenshot = new Bitmap(rect.Right - rect.Left, rect.Bottom - rect.Top);
            Graphics gfx = Graphics.FromImage(screenshot);
            IntPtr hdcBitmap = gfx.GetHdc();

            PrintWindow(hWnd, hdcBitmap, 0);

            gfx.ReleaseHdc(hdcBitmap);
            gfx.Dispose();

            return screenshot;
        }

        public static Bitmap[] ScreenShotCutter(Bitmap screenshot)
        {
            int[] sizes = { 0, 38, 63, 85, 106, 131, 156, screenshot.Width };
            int height = screenshot.Height;

            Bitmap[] screenshotsCutted = new Bitmap[7];

            for (int i = 0; i < 7; i++)
            {
                screenshotsCutted[i] = screenshot.Clone(new Rectangle(sizes[i], 0, sizes[i + 1] - sizes[i], height), screenshot.PixelFormat);
            }
            
            return screenshotsCutted;
        }

        public static void ScreensScan()
        {
            string[] files = Directory.GetFiles(directory);

            foreach (string file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }
        }

        public static void SaveBitmaps(Bitmap bitmaps)
        {
            string[] files = Directory.GetFiles(directory);

            string lastFileDigitStr = Path.GetFileNameWithoutExtension(files.Last());
            int lastFileDigitInt = Convert.ToInt32(lastFileDigitStr) + 1;

            string fileName = Path.Combine(directory, lastFileDigitInt.ToString() + ".png");
            bitmaps.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }
        
        public static Bitmap CaptureScreenshotArea(int x, int y, int width, int height)
        {
            Bitmap screenshot = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(x, y, 0, 0, screenshot.Size);
            }

            screenshot = ScreenshotsEffects.ConvertToBlackAndWhite(screenshot);
            //screenshot = Editors.ApplyContrast(screenshot, 50f);

            return screenshot;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr handle, ref Rectangle rect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}