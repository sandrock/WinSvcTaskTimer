
namespace WinSvcTaskTimer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Controls task execution.
    /// </summary>
    public class TaskTimer : IDisposable
    {
        private readonly List<TaskItem> tasks = new List<TaskItem>();

        /// <summary>
        /// Indicates the object was disposed.
        /// </summary>
        private bool disposed;

        private Timer timer;

        private string name;
        private TimeSpan delay;
        private TimeSpan interval;
        private TimerTickBehavior tickBehavior;
        private TaskBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTimer"/> class.
        /// </summary>
        /// <param name="name">The timer name.</param>
        /// <param name="delay">The delay before the first execution.</param>
        /// <param name="interval">The interval between each execution.</param>
        /// <param name="tickBehavior">The tick behavior.</param>
        /// <param name="builder">The delegate builder.</param>
        public TaskTimer(string name, TimeSpan delay, TimeSpan interval, TimerTickBehavior tickBehavior, TaskBuilder builder)
        {
            this.name = name;
            this.delay = delay;
            this.interval = interval;
            this.tickBehavior = tickBehavior;
            this.builder = builder;
        }

        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            if (this.timer == null)
            {
                this.timer = new Timer(this.OnTick, this, this.delay, this.interval);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases managed and - optionally - unmanaged resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // stop internal timer
                    if (this.timer != null)
                    {
                        var timer = this.timer;
                        this.timer = null;
                        timer.Dispose();
                    }

                    // wait for tasks completion
                    var tasks = this.tasks.ToArray();
                    this.tasks.Clear();

                    // kill tasks
                    foreach (var task in tasks)
                    {
                        if (task.IsRunning)
                        {
                            Trace.WriteLine("TaskTimer " + this.name + " is waiting for task " + task + " before stopping");
                            task.Abort();
                        }
                    }

                    // wait tasks
                    foreach (var task in tasks)
                    {
                        task.Wait();
                    }
                }

                this.disposed = true;
            }
        }

        private void OnTick(object state)
        {
            TaskItem task;
            if (this.tickBehavior == TimerTickBehavior.Continue)
            {
                task = this.CreateTask();
                this.tasks.Add(task);
                task.Task.Start();
            }
            else if (this.tickBehavior == TimerTickBehavior.QueueExecution)
            {
                task = this.CreateTask();
                this.tasks.Add(task);
            }
            else if (this.tickBehavior == TimerTickBehavior.WaitNextTick)
            {
                if (this.tasks.All(t => t.HasCompleted))
                {
                    task = this.CreateTask();
                    this.tasks.Add(task);
                    task.Run();
                }
                else
                {
                    Trace.WriteLine("TaskTimer " + this.name + " not ticking at " + DateTime.Now.ToString("u") + " because previous task is still executing");
                }
            }
            else
            {
            }
        }

        private TaskItem CreateTask()
        {
            var item = this.builder.Create();
            return item;
        }

        /// <summary>
        /// Starts the next pending task.
        /// </summary>
        /// <returns>the task selected to run (may be null)</returns>
        private TaskItem StartNextTask()
        {
            foreach (var task in this.tasks)
            {
                if (task.Task.Status == TaskStatus.Created)
                {
                    task.Task.Start();
                    return task;
                }
            }

            return null;
        }
    }
}
