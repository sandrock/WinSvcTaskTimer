
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ShellRunner : IRun
    {
        private ShellStart start;
        private Process process;

        public bool HasStarted { get; set; }

        public bool HasExited
        {
            get
            {
                return !this.HasStarted || this.process.HasExited;
            }
        }

        public void Run(string argument)
        {
            this.start = ShellStart.Create(argument);
            var workDir = start.Directory ?? Environment.CurrentDirectory;
            /*
            var logDir = Path.Combine(workDir, "Logs");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            var logFile = Path.Combine(logDir, "ShellRunner-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ".log");
            var startDate = DateTime.UtcNow;
            */
            var startInfo = new ProcessStartInfo(start.FileName, start.Arguments)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = workDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            this.process = Process.Start(startInfo);
            this.HasStarted = true;
            Trace.WriteLine("ShellRunner: process " + this.process.Id + " has started");
            var outputReader = process.StandardOutput;
            var errorReader = process.StandardError;

            this.process.Exited += Process_Exited;

            ////process.WaitForExit();

            /*
            using(var logStream = File.Open(logFile, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var logWriter = new StreamWriter(logStream))
            {
                logWriter.WriteLine("Start date:              " + startDate.ToString("u"));
                logWriter.WriteLine("End date:                " + DateTime.UtcNow.ToString("u"));
                logWriter.WriteLine("ShellStart.Directory:    " + start.Directory);
                logWriter.WriteLine("ShellStart.FileName:     " + start.FileName);
                logWriter.WriteLine("ShellStart.Arguments:    " + start.Arguments);
                logWriter.WriteLine("ShellStart.ExpectedCode: " + start.ExpectedCode);
                logWriter.WriteLine("Exit code:               " + process.ExitCode);
                logWriter.WriteLine();
                logWriter.WriteLine("=====================");
                logWriter.WriteLine("Standard Output");
                logWriter.WriteLine("=====================");
                logWriter.WriteLine();
                logWriter.WriteLine(outputReader.ReadToEnd());
                logWriter.WriteLine();
                logWriter.WriteLine("=====================");
                logWriter.WriteLine("Standard Error");
                logWriter.WriteLine("=====================");
                logWriter.WriteLine();
                logWriter.WriteLine(errorReader.ReadToEnd());
            }
            */
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Trace.WriteLine("ShellRunner: process " + this.process.Id + " has exited");
            if (process.ExitCode != this.start.ExpectedCode)
            {
                Trace.WriteLine("ShellRunner: process " + this.process.Id + " has exited with exit code " + process.ExitCode + " (expected " + this.start.ExpectedCode + ")");
            }
        }

        public void Abort()
        {
            if (this.process != null)
            {
                this.process.Kill();
            }
        }

        public class ShellStart
        {
            private static Regex codeRegex = new Regex("^CODE:(\\d+) ");
            private static Regex dirRegex = new Regex("^DIR:(\"([^\"]+)\"|([^\" ]+)) ");

            public int ExpectedCode { get; set; }

            public string FileName { get; set; }

            public string Arguments { get; set; }

            public string Directory { get; set; }

            public static ShellStart Create(string argument)
            {
                var start = new ShellStart();

                var codeMatch = codeRegex.Match(argument);
                if (codeMatch.Success)
                {
                    start.ExpectedCode = int.Parse(codeMatch.Groups[1].Captures[0].Value);
                    argument = argument.Substring(codeMatch.Groups[0].Captures[0].Value.Length);
                }

                var dirMatch = dirRegex.Match(argument);
                if (dirMatch.Success)
                {
                    start.Directory = dirMatch.Groups[1].Captures[0].Value;
                    argument = argument.Substring(dirMatch.Groups[0].Captures[0].Value.Length);
                }

                string fileName, arguments;
                if (argument.Length > 3 && argument[0] == '"')
                {
                    var quoteEnd = argument.IndexOf('"', 2);
                    if (quoteEnd > 1)
                    {
                        start.FileName = argument.Substring(1, quoteEnd - 1);
                    }
                    else
                    {
                        start.FileName = argument;
                    }

                    var argStart = argument.IndexOf('"', 2) + 2;
                    if (quoteEnd > 1 && argument.Length > argStart)
                    {
                        start.Arguments = argument.Substring(argStart);
                    }
                }
                else
                {
                    var quoteEnd = argument.IndexOf(' ', 2);
                    if (quoteEnd > 1)
                    {
                        start.FileName = argument.Substring(0, quoteEnd);
                    }
                    else
                    {
                        start.FileName = argument;
                    }

                    var argStart = argument.IndexOf(' ', 2);
                    if (argStart > 1 && argument.Length > argStart)
                    {
                        start.Arguments = argument.Substring(argStart + 1);
                    }
                }

                return start;
            }
        }
    }
}
