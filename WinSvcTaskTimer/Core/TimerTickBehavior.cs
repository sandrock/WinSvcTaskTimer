
namespace WinSvcTaskTimer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Used by <see cref="TaskTimer"/> to determine task concurrency.
    /// </summary>
    public enum TimerTickBehavior
    {
        /// <summary>
        /// Create a new task execution even if previous execution are still executing.
        /// </summary>
        Continue,

        /// <summary>
        /// Do not start execution is main task is still executing.
        /// </summary>
        WaitNextTick,

        /// <summary>
        /// Queue execution of the task after current task.
        /// </summary>
        QueueExecution,
    }
}
