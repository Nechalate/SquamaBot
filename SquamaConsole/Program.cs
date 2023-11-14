using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
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
