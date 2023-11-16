using System;
using System.Threading;

namespace SquamaConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread mouseTrackingThread = new Thread(Pointer.MouseTrackingThread);
            mouseTrackingThread.Start();

            Console.WriteLine("Нажмите любую клавишу для выхода.");
            Console.ReadKey();

            mouseTrackingThread.Abort();
        }
    }
}
