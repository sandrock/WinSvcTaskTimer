
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var wrapper = new ServiceWrapper();
            if (Environment.UserInteractive)
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
                Console.Write("Running service... ");
                try
                {
                    wrapper.Start();
                    Console.WriteLine("ok.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("Failed to start service.");
                    Console.WriteLine(ex.ToString());
                    Environment.ExitCode = 1;
                    return;
                }

                Console.WriteLine("Hit Q to stop.");
                Console.WriteLine();

                for (;;)
                {
                    ConsoleKeyInfo key = Console.ReadKey(false);
                    if (key.Key == ConsoleKey.Q)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Stopping service... ");
                        wrapper.Stop();
                        Console.WriteLine("Stopping service... ok.");
                        Console.WriteLine();
                        return;
                    }
                }
            }
            else
            {
                ServiceBase.Run(wrapper);
            }
        }
    }
}
