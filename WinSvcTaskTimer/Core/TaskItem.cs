
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
        private readonly IRun runner;
        private Action run;
        private readonly Action abort;
        
        private TaskItem()
        {
        }

        public TaskItem(IRun runner, Action run, Action abort)
        {
            this.runner = runner;
            this.run = run;
            this.abort = abort;
        }
        
        public bool IsRunning
        {
            get
            {
                if (this.runner != null)
                {
                    return this.runner.HasStarted && !this.runner.HasExited;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasStarted
        {
            get
            {
                if (this.runner != null)
                {
                    return this.runner.HasStarted;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool HasCompleted
        {
            get
            {
                if (this.runner != null)
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
            var item = new TaskItem();
            item.CreateException = ex;
            return item;
        }
    }
}
