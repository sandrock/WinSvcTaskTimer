
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class SampleRunner
    {
        private static readonly Random Random = new Random();
        private static readonly object IdLock = new object();
        private static int idCounter = 0;
        private int id = NextId();
        
        public void Run(string argument)
        {
            var time = Random.Next(5, 60);
            Trace.TraceInformation("SampleRunner " + this.id + " is running for " + time + " seconds...");
            Thread.Sleep(time * 1000);

            if (Random.NextDouble() > .9)
            {
                throw new InvalidOperationException("SampleRunner " + this.id + " ran in " + time + " seconds and threw.");
            }
            else
            {
                Trace.TraceInformation("SampleRunner " + this.id + " ran in " + time + " seconds.");
            }
        }

        private static int NextId()
        {
            lock (IdLock)
            {
                return ++idCounter;
            }
        }
    }
}
