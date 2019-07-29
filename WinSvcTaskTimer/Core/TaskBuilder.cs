
namespace WinSvcTaskTimer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Creates runnable delegates.
    /// </summary>
    public class TaskBuilder
    {
        private string name;
        private string type;
        private string argument;
        private string method;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskBuilder"/> class.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="type">The object type.</param>
        /// <param name="argument">The argument.</param>
        /// <param name="method">The method to invoke.</param>
        public TaskBuilder(string name, string type, string argument, string method)
        {
            this.name = name;
            this.type = type;
            this.argument = argument;
            this.method = method;
        }

        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Creates a runnable delegate.
        /// </summary>
        /// <returns>the run method</returns>
        public Action Create()
        {
            return this.Run;
        }

        /// <summary>
        /// Verifies this instance.
        /// </summary>
        public void Verify()
        {
        }

        private void Run()
        {
            // find type
            Type type;
            try
            {
                type = Type.GetType(this.type);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("TaskBuilder " + this.name + " failed to create Type '" + this.type + "'." + Environment.NewLine + ex.ToString());
                return;
            }

            // create object
            object obj;
            try
            {
                obj = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("TaskBuilder " + this.name + " failed to create instance of type '" + this.type + "'." + Environment.NewLine + ex.ToString());
                return;
            }

            // find method
            Action<string> runAction;
            if (obj is IRun)
            {
                runAction = arg => ((IRun)obj).Run(arg);
            }
            else
            {
                MethodInfo method;
                try
                {
                    method = type.GetMethod(this.method, new Type[] { typeof(string), });
                    if (method != null)
                    {
                        runAction = arg => method.Invoke(obj, new object[] { arg, });
                    }
                    else
                    {
                        method = type.GetMethod(this.method, new Type[] { });
                        if (method != null)
                        {
                            runAction = arg => method.Invoke(obj, new object[] { });
                        }
                        else
                        {
                            Trace.WriteLine("TaskBuilder " + this.name + " found no method '" + this.method + "' on type '" + this.type + "'.");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("TaskBuilder " + this.name + " found no method '" + this.method + "' on type '" + this.type + "'." + Environment.NewLine + ex.ToString());
                    return;
                }
            }

            // run method
            try
            {
                runAction(this.argument);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("TaskBuilder " + this.name + " run delegate encountered an run error on type '" + this.type + "'." + Environment.NewLine + ex.ToString());
            }
        }
    }
}
