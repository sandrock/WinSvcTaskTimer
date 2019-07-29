
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Logs tracing event to the <see cref="Console"/> with coloration.
    /// </summary>
    public class ConsoleTraceListener : TraceListener
    {
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            base.TraceEvent(eventCache, source, eventType, id, message);

            var foreg = Console.ForegroundColor;
            var backg = Console.BackgroundColor;

            switch (eventType)
            {
                case TraceEventType.Critical:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;

                case TraceEventType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.White;
                    break;

                case TraceEventType.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.White;
                    break;

                case TraceEventType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;

                case TraceEventType.Information:
                case TraceEventType.Resume:
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Transfer:
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }

            Console.Write(eventType.ToString().Substring(0, 4).ToUpperInvariant() + " ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine(message);

            Console.ForegroundColor = foreg;
            Console.BackgroundColor = backg;
        }

        public override void Write(string message)
        {
            Console.Write(message);
        }

        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
