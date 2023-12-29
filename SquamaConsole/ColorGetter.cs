using System.Drawing;
using System.Runtime.InteropServices;
using System;

internal class ColorGetter
{
    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    public static extern int BitBlt(IntPtr hDc, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    public static System.Drawing.Color GetColorAtInWindow(string windowTitle, System.Drawing.Point location)
    {
        IntPtr hwnd = FindWindow(null, windowTitle);

        POINT clientPoint = new POINT { x = location.X, y = location.Y };
        ScreenToClient(hwnd, ref clientPoint);

        var screenPixel = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        IntPtr hDc = GetDC(hwnd);

        using (var gdest = Graphics.FromImage(screenPixel))
        {
            IntPtr hDcDest = gdest.GetHdc();
            BitBlt(hDcDest, 0, 0, 1, 1, hDc, clientPoint.x, clientPoint.y, (int)CopyPixelOperation.SourceCopy);
            gdest.ReleaseHdc();
        }

        ReleaseDC(hwnd, hDc);
        return screenPixel.GetPixel(0, 0);
    }
}