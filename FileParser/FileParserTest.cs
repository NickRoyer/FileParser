using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileParser
{
    public class FileParserTest : ITest
    {
        private static readonly string FILE_PATH = "largedata.csv";

        private List<List<float>> LinqSerialTest()
        {
            float floatTester = 0;
            List<List<float>> result = ReadFile()
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => new { Line = l, Fields = SplitFileLine(l) })
                .Select(x => x.Fields
                              .Where(f => float.TryParse(f, out floatTester))
                              .Select(f => floatTester).ToList())
                .ToList();

            return result;
        }

        private List<List<float>> LinqParallelTest()
        {
            float floatTester = 0;
            List<List<float>> result = ReadFile().AsParallel()
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => new { Line = l, Fields = SplitFileLine(l) })
                .Select(x => x.Fields
                              .Where(f => float.TryParse(f, out floatTester))
                              .Select(f => floatTester).ToList())
                .ToList();

            return result;
        }

        private IEnumerable<string> ReadFile()
        {
            //Read all the lines AFTER the header
            return File.ReadLines(FILE_PATH).Skip(1);
        }

        private string[] SplitFileLine(string s)
        {
            char[] Separators = { ',' };
            return s.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private List<List<float>> SerialRowLockTest()
        {
            var numbers = new List<List<float>>();
            /*System.Threading.Tasks.*/
            Parallel.ForEach(ReadFile(), line =>
            {
                lock (numbers)
                {
                    numbers.Add(ProcessLine(line));
                }
            });
            return numbers;
        }

        /*
          public static ParallelLoopResult ForEach<TSource, TLocal>(
                IEnumerable<TSource> source,
                Func<TLocal> localInit,
                Func<TSource, ParallelLoopState, TLocal, TLocal> taskBody,
                Action<TLocal> localFinally        
            )
        */
        private List<List<float>> ParallelMergeLockTest()
        {
            var numbers = new List<List<float>>();
            /*System.Threading.Tasks.*/
            Parallel.ForEach<string, List<List<float>>>(ReadFile(), //Source collection 
                    () => new List<List<float>>(), // method to initialize the local thread storage
                    (line, loop, localNumbers) => // Each iteration executes the following
                    {
                        localNumbers.Add(ProcessLine(line));
                        return localNumbers;
                    },

                (finalResult) =>
                {
                    lock (numbers)
                    {
                        numbers.AddRange(finalResult);
                    }
                }

            );
            return numbers;
        }

        private List<float> ProcessLine(string line)
        {
            var list = new List<float>();

            foreach (var s in SplitFileLine(line))
            {
                if (float.TryParse(s, out float i))
                {
                    list.Add(i);
                }
            }
            return list;
        }

        private enum TestType { Linq, LinqParallel, ParallelRowLock, ParallelMergeLock };

        private void ProcessTest(TestType t)
        {
            List<List<float>> results = null;
            Console.WriteLine(t.ToString());

            Stopwatch sw = new Stopwatch();
            sw.Start();

            switch (t)
            {
                case TestType.Linq:
                    results = LinqSerialTest();
                    break;
                case TestType.LinqParallel:
                    results = LinqParallelTest();
                    break;
                case TestType.ParallelRowLock:
                    results = SerialRowLockTest();
                    break;
                case TestType.ParallelMergeLock:
                    results = ParallelMergeLockTest();
                    break;
                default: throw new Exception("TestType not implemented for type: " + t.ToString());
            }
            sw.Stop();

            if (results != null)
            {
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = sw.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                Console.WriteLine(elapsedTime);

                // now get your totals
                int numberOfRows = results.Count();
                Console.WriteLine("Number of Rows: " + numberOfRows.ToString());

                int numberOfAllFloats = results.Sum(fa => fa.Count);
                Console.WriteLine("Number of all floats: " + numberOfAllFloats.ToString());
            }
            else
                Console.WriteLine("Test Failed to process!");

            Console.WriteLine();
        }

        public void RunTest()
        {
            ProcessTest(TestType.Linq);
            ProcessTest(TestType.LinqParallel);
            ProcessTest(TestType.ParallelRowLock);
            ProcessTest(TestType.ParallelMergeLock);
        }
    }
}
