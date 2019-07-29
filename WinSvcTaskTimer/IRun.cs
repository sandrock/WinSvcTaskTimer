
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Strongly-typed runnable object for <see cref="TaskBuilder"/>.
    /// </summary>
    public interface IRun
    {
        void Run(string argument);
    }
}
