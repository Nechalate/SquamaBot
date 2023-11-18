using System;
using System.Threading;
using System.Windows.Forms;

namespace SquamaConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Thread mouseTrackingThread = new Thread(Pointer.MouseTrackingThread);

            mouseTrackingThread.Start();

            Console.WriteLine("Нажмите клавишу для приостановки/возобновления выполнения программы.");

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Pointer.TogglePause();
                }
            }
        }
    }
}
