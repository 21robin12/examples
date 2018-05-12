using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpConcurrency
{
    // https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming
    [TestClass]
    public class C_TPL_Tasks
    {
        [TestMethod]
        public void TPL_ImplicitTasks()
        {
            var total = 0;

            // Parallel.Invoke is the easiest way to run a number of parallel operations. The supplied actions are scheduled across multiple threads, 
            // and code execution only continues when all operations have completed (hence why the Thread.Sleep is not an issue here)
            Parallel.Invoke(
                () => {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                    Interlocked.Add(ref total, 1);
                },
                () => Interlocked.Add(ref total, 10),
                () => Interlocked.Add(ref total, 100),
                () => Interlocked.Add(ref total, 1000),
                () => Interlocked.Add(ref total, 10000));

            Assert.AreEqual(total, 11111);
        }

        [TestMethod]
        public void TPL_ExplicitTasks()
        {
            string result = string.Empty;

            // TASKS represent asynchronous operations. They exist at a higher level of abstraction than threads, and offer a couple of major benefits:
            //  - More efficient & scalable use of system resources
            // - More programmatic control than with a thread
            var taskA = new Task(() => result += "AAA");

            // Tasks need to be started after they are defined, and waited on to ensure that the task has completed.
            taskA.Start();
            taskA.Wait();

            // Task.Run defines and starts a task in one operation.
            var taskB = Task.Run(() => result += "BBB");
            taskB.Wait();

            // Task.Factory.StartNew does the same as Task.Run, but allows us to pass additional options. This is equivalent to the Task.Run statement above:
            var taskC =  Task.Factory.StartNew(() => result += "CCC", CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            taskC.Wait();

            Assert.AreEqual("AAABBBCCC", result);
        }
    }
}
