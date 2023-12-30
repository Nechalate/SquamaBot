using System.Drawing;
using System.IO;
using Tesseract;
using System.Linq;
using Emgu.CV.Reg;
using System;

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
    }
}
