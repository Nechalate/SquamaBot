namespace SquamaConsole
{
    internal class MouseClick
    {
        [System.Runtime.InteropServices.DllImport("user32.dll",
        CharSet = System.Runtime.InteropServices.CharSet.Auto,
        CallingConvention =
        System.Runtime.InteropServices.CallingConvention.StdCall)]

        public static extern void mouse_event(uint dwFlags,
        int dx,
        int dy,
        int dwData,
        int dwExtraInfo);

        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;

        private const int MOUSEEVENTF_LEFTUP = 0x0004;

        private const int MOUSEEVENTF_MOVE = 0x0001;

        public static void Click(int X, int Y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, 15000, 50000, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, 50000, 15000, 0, 0);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
    }
}
