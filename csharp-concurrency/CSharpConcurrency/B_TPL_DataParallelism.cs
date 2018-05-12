using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpConcurrency
{
    // https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/data-parallelism-task-parallel-library
    [TestClass]
    public class B_TPL_DataParallelism
    {
        [TestMethod]
        public void TPL_ForLoop()
        {
            // DATA PARALLELISM: the same operation is performed concurrently on elements in a collection. In data parallel operations, the source collection is PARTITIONED so that different threads can operate on different partitions concurrently. The TPL supports ddata parallelism through the Parallel class.

            var totalSequential = Enumerable.Range(0, 100).Select(x => x * x).Sum();

            var totalParallel = 0;
            Parallel.For(0, 100, i => Interlocked.Add(ref totalParallel, i * i));

            Assert.AreEqual(totalSequential, totalParallel);
        }

        [TestMethod]
        public void TPL_ForEachLoop()
        {
            // Parallel.For and Parallel.ForEach are parallel equivalents of for and foreach loops.

            var totalSequential = Enumerable.Range(0, 100).Select(x => x * x).Sum();

            var totalParallel = 0;
            Parallel.ForEach(Enumerable.Range(0, 100), i => Interlocked.Add(ref totalParallel, i * i));

            Assert.AreEqual(totalSequential, totalParallel);
        }

        [TestMethod]
        public void TPL_BreakingParallelLoop()
        {
            var total = 0;
            var lockObject = new Object();

            Parallel.For(0, 1000, (i, loopState) =>
            {
                lock (lockObject)
                {
                    total++;

                    if (total >= 50)
                    {
                        // We can break out of a parallel loop using the ParallelLoopState object 
                        loopState.Stop();
                    }
                }
            });

            Assert.AreEqual(50, total);
        }

        [TestMethod]
        public void TPL_ThreadLocalVariables()
        {
            var total = 0;
            Parallel.For(0, 1000, () => 0, (i, loopState, subtotal) =>
            {
                // Called once per item, i.e. 1000 times
                subtotal++;
                return subtotal;
            },
                (subTotal) =>
                {
                    // Called once per partition/thread, around 7 or 8 times (depends on how the scheduler partitions our loop up). This gives us a performance increase since we don't need to lock total on every iteration; only at the end of each partition
                    Interlocked.Add(ref total, subTotal);
                }
            );

            Assert.AreEqual(1000, total);
        }

        [TestMethod]
        public void TPL_ThreadLocalVariablesPerformance()
        {
            const int limit = 10000000;

            (int result, int elapsedMs) timeOperation(Func<int> operation)
            {
                var sw = new Stopwatch();
                sw.Start();
                var result = operation();
                sw.Stop();
                return (result, (int)sw.ElapsedMilliseconds);
            };

            var (perPartitionTotal, perPartitionElapsedMs) = timeOperation(() =>
            {
                var total = 0;
                Parallel.For(0, limit, () => 0, (i, loopState, subtotal) =>
                {
                    subtotal++;
                    return subtotal;
                },
                    (subTotal) => Interlocked.Add(ref total, subTotal)
                );

                return total;
            });

            var (everyIterationTotal, everyIterationElapsedMs) = timeOperation(() =>
            {
                var total = 0;
                Parallel.For(0, limit, i => Interlocked.Add(ref total, 1));
                return total;
            });

            Assert.AreEqual(limit, perPartitionTotal);
            Assert.AreEqual(limit, everyIterationTotal);

            // Asserts that using thread local variables is at least twice as fast as incrementing our counter on every iteration. Interestingly, this 
            // assert only passes when running this test by itself - it fails when running all tests in the project! Presumably this is because tests are 
            // run in parallel, using threads / CPU resources and altering how the scheduler divides up the total work for this tetst into partitions / threads.

            // Assert.IsTrue(perPartitionElapsedMs < everyIterationElapsedMs / 2);
        }
    }
}
