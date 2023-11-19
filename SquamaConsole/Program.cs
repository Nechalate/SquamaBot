using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
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

            Console.WriteLine("Нажмите клавишу F5 для приостановки/возобновления выполнения программы.");
            
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.F5)
                    {
                        Pointer.TogglePause();
                    }
                }
                else
                {
                    Thread.Sleep(100); // Пауза, чтобы не загружать процессор
                }
            }
        }
    }
}
