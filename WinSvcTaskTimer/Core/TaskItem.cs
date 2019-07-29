
namespace WinSvcTaskTimer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskItem
    {
        private Task task;
        private CancellationTokenSource cancel;
        private IRun runner;
        private Action run;
        private Action abort;

        public TaskItem(Action action)
        {
            this.cancel = new CancellationTokenSource();
            this.task = new Task(action);
        }

        public TaskItem(Task task, CancellationTokenSource cancel)
        {
            this.task = task;
            this.cancel = cancel;
        }

        public TaskItem(IRun runner, Action run, Action abort)
        {
            this.runner = runner;
            this.run = run;
            this.abort = abort;
        }

        public Task Task
        {
            get { return this.task; }
        }

        public bool IsRunning
        {
            get
            {
                if (this.task != null)
                {
                    return this.task.Status == TaskStatus.Running || this.task.Status == TaskStatus.WaitingForChildrenToComplete;
                }
                else if (this.runner != null)
                {
                    return this.runner.HasStarted && !this.runner.HasExited;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasCompleted
        {
            get
            {
                if (this.task != null)
                {
                    return this.task.IsCompleted || this.task.IsCanceled;
                }
                else if (this.runner != null)
                {
                    return this.runner.HasStarted && this.runner.HasExited;
                }
                else
                {
                    return true;
                }
            }
        }

        public Exception CreateException { get; private set; }

        public void Run()
        {
            var run = this.run;
            if (run != null)
            {
                this.run = null;
                run();
            }
        }

        public void Abort()
        {
            if (this.cancel != null)
            {
                this.cancel.Cancel(); 
            }

            if (this.abort != null)
            {
                this.abort();
            }
        }

        internal void Wait()
        {
            if (this.runner != null && this.runner.HasStarted)
            {
                while (!this.runner.HasExited)
                {
                    Thread.Sleep(500);
                }
            }
        }

        internal static TaskItem CreateError(Exception ex)
        {
            var item = new TaskItem(null);
            item.CreateException = ex;
            return item;
        }
    }
}
