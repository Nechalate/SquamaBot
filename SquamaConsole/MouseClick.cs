using System;
using System.Runtime.InteropServices;

internal class MouseClick
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_LBUTTONUP = 0x0202;

    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

    public static void FishHooking(string windowTitle, int relativeX, int relativeY)
    {
        IntPtr windowHandle = FindWindow(null, windowTitle);

        RECT windowRect;
        GetWindowRect(windowHandle, out windowRect);

        int absoluteX = windowRect.Left + relativeX;
        int absoluteY = windowRect.Top + relativeY;

        SendMessage(windowHandle, WM_LBUTTONDOWN, 0, (absoluteY << 16) | absoluteX);
        SendMessage(windowHandle, WM_LBUTTONUP, 0, (absoluteY << 16) | absoluteX);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}