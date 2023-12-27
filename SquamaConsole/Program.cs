using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace SquamaConsole
{
    internal class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static public string windowTitle = "RАGЕ Мultiрlaуer";

        static void Main(string[] args)
        {
            Thread mouseTrackingThread = new Thread(MainThread.MainProgramThread);

            Console.Write("Поиск окна.");

            while (true)
            {
                IntPtr windowHandle = FindWindow(null, windowTitle);

                if (windowHandle != IntPtr.Zero)
                {
                    Console.WriteLine("Окно найдено.");
                    break;
                }
                else
                {
                    windowTitle += " ";
                    Console.Write(".");
                }
            }

            mouseTrackingThread.Start();

            Console.WriteLine("Нажмите клавишу F5 для приостановки/возобновления выполнения программы.\nНажмите клавишу F6 для очистки консоли");
            
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.F5)
                    {
                        MainThread.TogglePause();
                    }
                    if (keyInfo.Key == ConsoleKey.F6)
                    {
                        Console.Clear();
                    }
                }
                else
                {
                    Thread.Sleep(100); 
                }
            }
        }
    }
}
