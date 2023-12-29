using System;
using System.Collections.Generic;
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

        static public string windowTitle = "";

        static void Main(string[] args)
        {
            Thread mouseTrackingThread = new Thread(MainThread.MainProgramThread);

            windowTitle = RageFinder();

            IntPtr windowHandle = FindWindow(null, windowTitle);

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

        private static string RageFinder()
        {
            Console.WriteLine("Поиск окна.");

            List<string> openWindowNames = WindowsList.GetOpenWindowNames(); // List all opens windows

            foreach (string windowName in openWindowNames)
            {
                if (windowName.Contains("RАGЕ"))
                {
                    Console.WriteLine("Окно найдено.");

                    return windowName;
                }
            }
            return "";
        }
    }
}
