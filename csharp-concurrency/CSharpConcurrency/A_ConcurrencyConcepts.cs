using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpConcurrency
{
    [TestClass]
    public class A_ConcurrencyConcepts
    {
        [TestMethod]
        public void ConcurrencyConcepts_NoLocking_RaceCondition()
        {
            var total = 0;
            var iterations = 1000;
            
            Parallel.ForEach(Enumerable.Range(0, iterations), i => total++);

            // RACE CONDITION: Thread A reads the value of total as 57 and increments it to 58. Before writing it back, Thread B also reads 57 and increments to 58. Both threads write 58, so one of the increment operations is lost.
            Assert.AreNotEqual(iterations, total);
        }

        [TestMethod]
        public void ConcurrencyConcepts_LockKeyword_CorrectValue()
        {
            var total = 0;
            var iterations = 1000;

            // We can prevent a race condition by using a LOCK OBJECT. This marks a statement block as a CRITICAL SECTION, ensuring that one thread does not enter the block while another thread is already executing it. Using a lock object, we can ensure that our code is THREAD-SAFE.
            var lockObject = new Object();
            Parallel.ForEach(Enumerable.Range(0, iterations), i =>
            {
                // (the lock keyword is essentially syntax sugar over Monitor.Enter and Monitor.Exit)
                lock (lockObject)
                {
                    total++;
                }
            });

            Assert.AreEqual(iterations, total);
        }

        [TestMethod]
        public void ConcurrencyConcepts_Interlocking_CorrectValue()
        {
            var total = 0;
            var iterations = 1000;

            // The INTERLOCKED class provides atomic operations for thread safety.
            Parallel.ForEach(Enumerable.Range(0, iterations), i => Interlocked.Add(ref total, 1));

            Assert.AreEqual(iterations, total);
        }

        [TestMethod]
        public void ConcurrencyConcepts_IncorrectLocking_Deadlock()
        {
            var task = Task.Run(() =>
            {
                var total = 0;
                var iterations = 1000;

                var lockObject1 = new Object();
                var lockObject2 = new Object();

                Parallel.ForEach(Enumerable.Range(0, iterations), i =>
                {
                    if (i % 2 == 0)
                    {
                        lock (lockObject1)
                        {
                            // Thread A locks lockObject1, and before it can lock lockObject2...
                            lock (lockObject2)
                            {
                                total++;
                            }
                        }
                    }
                    else
                    {
                        lock (lockObject2)
                        {
                            // ... Thread B locks lockObject2. Now Thread A is waiting for lockObject2 to be released, and Thread B is waiting for lockObject1 to be released - so we have a DEADLOCK (i.e. waiting indefinitely)
                            lock (lockObject1)
                            {
                                total++;
                            }
                        }
                    }
                });

                return total;
            });

            if (task.Wait(TimeSpan.FromMilliseconds(1000)))
            {
                Assert.Fail("No deadlock was encountered - likely due to chance. Re-run the test to attempt to deadlock again.");
            }
            else
            {
                // It look more than 1000ms for the task to complete (in other tests this takes around 20ms), which is due to a (deliberate!) deadlock
            }
        }
    }
}
