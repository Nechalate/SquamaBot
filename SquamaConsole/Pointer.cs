using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SquamaConsole
{
    internal class Pointer
    {
        public struct POINT
        {
            public int pointX;
            public int pointY;
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDc, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

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

                    var color = GetColorAt(new System.Drawing.Point(1052, 899));

                    if (color.ToString() == "Color [A=255, R=255, G=0, B=0]")
                    {
                        MouseClick.Click(1920, 1080);
                        Console.WriteLine("Рыбка клюет!");
                        //Console.WriteLine($"Координаты мыши: X={point.pointX}, Y={point.pointY}");
                    }
                    //Console.WriteLine(color.ToString());

                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }
        public static System.Drawing.Color GetColorAt(System.Drawing.Point location)
        {
            var screenPixel = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var gdest = Graphics.FromImage(screenPixel))
            {
                using (var gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDc = gsrc.GetHdc();
                    IntPtr hDc = gdest.GetHdc();
                    BitBlt(hDc, 0, 0, 1, 1, hSrcDc, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }
    }
}
