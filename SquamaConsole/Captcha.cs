using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace SquamaConsole
{
    internal class Captcha
    {
        public static string RecognizeCaptcha(Bitmap captchaImage)
        {
            using (var engine = new TesseractEngine(@"\\traineddata", "eng", EngineMode.Default))
            {
                engine.SetVariable("tessedit_char_whitelist", "0123456789");

                using (var image = PixConverter.ToPix(captchaImage))
                {
                    //image.Binarize(150);
                    //image.ContrastStretch();
                    //image.RemoveSaltAndPepperNoise();
                    //image.Smooth(3);
                    //Bitmap captchaImage = Captcha.CaptureCaptchaArea(861, 456, 197, 48);
                    //string captchaText = Captcha.RecognizeCaptcha(captchaImage);
                    //Console.WriteLine($"Распознанный текст капчи: {captchaText}");

                    using (var page = engine.Process(image))
                    {
                        return page.GetText().Trim();
                    }
                }
            }
        }

        public static Bitmap CaptureCaptchaArea(int x, int y, int width, int height)
        {
            Bitmap screenshot = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(x, y, 0, 0, screenshot.Size);
            }
            return screenshot;
        }
    }
}
