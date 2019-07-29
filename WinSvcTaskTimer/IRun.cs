
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Strongly-typed runnable object for <see cref="TaskBuilder"/>.
    /// </summary>
    public interface IRun
    {
        bool HasStarted { get; }

        bool HasExited { get; }

        void Run(string argument);

        void Abort();
    }
}
