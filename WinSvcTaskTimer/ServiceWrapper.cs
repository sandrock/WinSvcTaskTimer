
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.ServiceProcess;
    using System.Threading;
    using System.Threading.Tasks;
    using WinSvcTaskTimer.Configuration;
    using WinSvcTaskTimer.Core;

    public class ServiceWrapper : ServiceBase
    {
        private Dictionary<string, TaskBuilder> tasks;
        private Dictionary<string, TaskTimer> timers;

        public ServiceWrapper()
        {
            var conf = LocalServiceInstallerConfiguration.CreateFromConfiguration();

            this.ServiceName = conf.ServiceName;
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = true;
        }

        public void Start()
        {
            this.DoStart();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        protected override void OnStart(string[] args)
        {
            Trace.WriteLine("ServiceWrapper: OnStart at " + DateTime.UtcNow.ToString());
            Trace.WriteLine("ServiceWrapper: Environment.CurrentDirectory: " + Environment.CurrentDirectory);
            Trace.WriteLine("ServiceWrapper: Environment.CommandLine:      " + Environment.CommandLine);
            Trace.WriteLine("ServiceWrapper: args:                         " + string.Join(" ", args));
            Trace.WriteLine("ServiceWrapper: will use:                     " + Path.GetDirectoryName(Environment.CommandLine.Trim('"')));
            Trace.Flush();

            base.OnStart(args);
            this.DoStart();
        }

        protected override void OnStop()
        {
            Trace.WriteLine("ServiceWrapper: OnStop at " + DateTime.UtcNow.ToString());
            Trace.Flush();

            this.DoStop();
            base.OnStop();

            Trace.WriteLine("ServiceWrapper: OnStop ended at " + DateTime.UtcNow.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DoStop();
            }

            base.Dispose(disposing);
        }

        private void DoStart()
        {
            var workDir = Path.GetDirectoryName(Environment.CommandLine.Trim('"'));
            if (!Directory.Exists(workDir))
            {
                throw new InvalidOperationException("Work directory cannot be defined to '" + workDir + "': directory does not exist");
            }

            Environment.CurrentDirectory = workDir;
            Trace.WriteLine("ServiceWrapper: Work directory set to '" + workDir + "'");

            var config = ConfigurationManager.GetSection("TaskHost") as TaskHostSection;
            if (config == null)
            {
                throw new InvalidOperationException("Not configured. Make sure the <TaskHost> section is present and declared.");
            }

            this.tasks = new Dictionary<string, TaskBuilder>();
            foreach (TaskHostSection.TasksElementCollection.TaskElement task in config.Tasks)
            {
                var builder = new TaskBuilder(task.Name, task.Type, task.Argument, task.Method);
                this.tasks.Add(task.Name, builder);
                Trace.WriteLine("ServiceWrapper: Task '" + builder.Name + "' initialized");
            }

            this.timers = new Dictionary<string, TaskTimer>();
            foreach (TaskHostSection.TimersElementCollection.TimerElement element in config.Timers)
            {
                if (!element.Enabled)
                {
                    continue;
                }

                var builder = this.tasks[element.TaskName];
                var timer = new TaskTimer(element.Name, element.Delay, element.Interval, element.TickBehavior, builder);
                this.timers.Add(element.Name, timer);
                Trace.WriteLine("ServiceWrapper: Timer '" + timer.Name + "' initialized");
            }

            Trace.WriteLine("ServiceWrapper: Starting all timers.");
            foreach (var timer in this.timers)
            {
                timer.Value.Run();
            }
        }

        private void DoStop()
        {
            if (this.timers != null)
            {
                Trace.WriteLine("ServiceWrapper: Stopping all timers.");
                var watch = new Stopwatch();
                watch.Start();
                var timers = this.timers;
                this.timers = null;
                var tasks = new List<Task>();
                foreach (var timer in timers)
                {
                    var theTimer = timer;
                    var task = new Task(() =>
                    {
                        theTimer.Value.Dispose();
                    });
                    tasks.Add(task);
                    task.Start();
                }

                Task.WaitAll(tasks.ToArray());
                Trace.WriteLine("ServiceWrapper: Stopped all timers in " + watch.Elapsed.ToString() + ".");
            }
        }
    }
}
