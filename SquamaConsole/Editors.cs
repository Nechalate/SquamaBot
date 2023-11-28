using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquamaConsole
{
    internal class Editors
    {
        public static Bitmap ApplyContrast(Bitmap image, float contrast)
        {
            Bitmap adjustedImage = new Bitmap(image.Width, image.Height);

            float factor = (100.0f + contrast) / 100.0f;
            factor *= factor;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color originalColor = image.GetPixel(x, y);
                    float r = originalColor.R / 255.0f;
                    float g = originalColor.G / 255.0f;
                    float b = originalColor.B / 255.0f;

                    r = (((r - 0.5f) * factor) + 0.5f) * 255.0f;
                    g = (((g - 0.5f) * factor) + 0.5f) * 255.0f;
                    b = (((b - 0.5f) * factor) + 0.5f) * 255.0f;

                    r = Math.Max(0, Math.Min(255, r));
                    g = Math.Max(0, Math.Min(255, g));
                    b = Math.Max(0, Math.Min(255, b));

                    Color adjustedColor = Color.FromArgb((int)r, (int)g, (int)b);
                    adjustedImage.SetPixel(x, y, adjustedColor);
                }
            }

            return adjustedImage;
        }

        public static Bitmap ConvertToBlackAndWhite(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color originalColor = image.GetPixel(x, y);
                    int avg = (originalColor.R + originalColor.G + originalColor.B) / 3;
                    Color newColor = Color.FromArgb(avg, avg, avg);
                    result.SetPixel(x, y, newColor);
                }
            }

            return result;
        }
    }
}
