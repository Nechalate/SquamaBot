using System.Drawing;
using System.IO;
using Tesseract;
using OpenCvSharp;
using Emgu.CV.Reg;

namespace SquamaConsole
{
    internal class Captcha
    {
        static string directory = @"C:\Users\relay\OneDrive\Рабочий стол\scrensos";

        public static string RecognizeCaptcha(Bitmap captchaImage)
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
        
        public static void SaveBitmaps(Bitmap[] bitmaps)
        {
            for (int i = 0; i < bitmaps.Length; i++)
            {
                string fileName = Path.Combine(directory, $"part_{i}.png");
                bitmaps[i].Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        
        public static Bitmap CaptureCaptchaArea(int x, int y, int width, int height)
        {
            Bitmap screenshot = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(x, y, 0, 0, screenshot.Size);
            }

            screenshot = Editors.ConvertToBlackAndWhite(screenshot);
            screenshot = Editors.ApplyContrast(screenshot, 50f);

            //string fileName = Path.Combine(directory, $"part.png");
            //screenshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

            return screenshot;
        }
    }
}
