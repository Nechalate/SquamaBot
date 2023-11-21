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
        /*
         string captchaText = RecognizeCaptcha(captchaImage);

         // Вывод распознанного текста
         Console.WriteLine($"Распознанный текст капчи: {captchaText}");
         */
        static string RecognizeCaptcha(Bitmap captchaImage)
        {
            // Распознавание текста с использованием Tesseract
            using (var engine = new TesseractEngine(@"путь_к_папке_с_языковыми_данными", "eng", EngineMode.Default))
            {
                using (var image = PixConverter.ToPix(captchaImage))
                {
                    using (var page = engine.Process(image))
                    {
                        return page.GetText().Trim();
                    }
                }
            }
        }

        static Bitmap CaptureCaptchaArea(int x, int y, int width, int height) // Bitmap captchaImage = CaptureCaptchaArea(x, y, width, height);
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
