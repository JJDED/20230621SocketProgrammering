using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20230621SocketServer;

internal class MultiThreading
{
    int counter = 0;
    public void StartThreading()
    {
        Thread clockThread = new(new ThreadStart(ClockMethod));
        clockThread.Start();
        Thread counterThread = new(new ThreadStart(CountMethod));
        counterThread.Start();

        while (true)
        {
            Console.Write("Message: ");
            Console.ReadLine();
        }
    }
    public void ClockMethod()
    {
        while (true)
        {
            Console.SetCursorPosition(40, 1);
            Console.WriteLine(DateTime.Now);
            Thread.Sleep(1000);
        }

    }

    public void CountMethod()
    {
        while (true)
        {
            Console.SetCursorPosition(1, 5);
            Console.WriteLine(counter++);
            Thread.Sleep(500);
        }

    }

}
