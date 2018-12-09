using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_7_A
{
    class Program
    {
        static void Main(string[] args)
        {
            var steps = DataProvider.Input_7().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                 .Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(x => new { Before = x.Substring(5, 1), After = x.Substring(36, 1) })
                 .ToList();
            var tree = new StepTree();
            foreach(var step in steps)
            {
                tree.Add(step.Before, step.After);
            }

            var taskList = tree.Traverse().ToList();
            var path = taskList.Select(x=> x.StepValue).ToList();
            var pathString = string.Join("", path);

            Console.WriteLine(pathString);

            Console.WriteLine(tree.CompletionTime(5, 60));

            Console.ReadKey();
        }

        //Step G must be finished before step M can begin.
    }

    public class Step
    {
        private readonly List<Step> beforeSteps;
        private readonly List<Step> afterSteps;

        public Step(string stepValue, IEnumerable<Step> beforeSteps = null, IEnumerable<Step> afterSteps = null)
        {
            this.StepValue = stepValue == null ? string.Empty : stepValue.Trim().ToUpper();
            this.beforeSteps = beforeSteps == null ? new List<Step>() : beforeSteps.ToList();
            this.afterSteps = afterSteps == null ? new List<Step>() : afterSteps.ToList();
        }

        public string StepValue { get; }

        public int Duration
        {
            get {
                return StepValue.ToCharArray()[0] - 'A' + 1;
            }
    }

        public IEnumerable<Step> BeforeSteps => beforeSteps;

        public IEnumerable<Step> AfterSteps => afterSteps;

        public void AddAfter(Step afterStep)
        {
            this.afterSteps.Add(afterStep);
        }

        public void AddBefore(Step beforeStep)
        {
            this.beforeSteps.Add(beforeStep);
        }
    }

    public class Assignment
    {
        private readonly Step step;
        private readonly int worker;
        private readonly int startSecond;
        private readonly int endSecond;

        public Assignment(Step step, int worker, int startSecond, int endSecond)
        {
            this.step = step;
            this.worker = worker;
            this.startSecond = startSecond;
            this.endSecond = endSecond;
        }

        public Step Step => step;

        public int Worker => worker;

        public int StartSecond => startSecond;

        public int EndSecond => endSecond;
    }

    public class StepTree
    {
        private readonly Dictionary<string, Step> directory = new Dictionary<string, Step>();

        public void Add(string stepBeforeValue, string stepAfterValue)
        {
            if(!directory.ContainsKey(stepBeforeValue))
            {
                directory.Add(stepBeforeValue, new Step(stepBeforeValue));
            }

            if (!directory.ContainsKey(stepAfterValue))
            {
                directory.Add(stepAfterValue, new Step(stepAfterValue));
            }

            var stepBefore = directory[stepBeforeValue];
            var stepAfter = directory[stepAfterValue];

            stepBefore.AddAfter(stepAfter);
            stepAfter.AddBefore(stepBefore);
        }

        public IEnumerable<Step> Traverse()
        {
            var returned = new HashSet<Step>();
            while (directory.Values.Any(x => !returned.Contains(x)))
            {
                var nextStep = directory.Values
                    .Where(x => !returned.Contains(x) && !x.BeforeSteps.Any(y => !returned.Contains(y)))
                    .OrderBy(x => x.StepValue)
                    .FirstOrDefault();

                returned.Add(nextStep);
                yield return nextStep;
            }
        }

        public int CompletionTime(int numWorkers, int delay = 0)
        {
            var returned = new List<Step>();
            var assignments = new List<Assignment>();
            var workers = Enumerable.Range(1, numWorkers).ToList();
            var second = 0;
            while (directory.Values.Any(x => !returned.Contains(x)))
            {
                var nextSteps = directory.Values
                   .Where(x => !returned.Contains(x) && !x.BeforeSteps.Any(y => !returned.Contains(y)))
                   .Where(x=> !assignments.Select(y=> y.Step).Contains(x))
                   .OrderBy(x => x.StepValue)
                   .ToList();

                var nextWorkers = workers.Except(assignments.Where(y => second >= y.StartSecond && second <= y.EndSecond).Select(z=> z.Worker)).ToList();
                var newAssignments = nextWorkers.Zip(nextSteps, (a, b) => new Assignment(b, a, second, second + b.Duration + delay - 1)).ToList();

                foreach(var assignment in newAssignments)
                {
                    assignments.Add(assignment);
                    Console.WriteLine($"{assignment.Step.StepValue}-{assignment.StartSecond}-{assignment.EndSecond}-{assignment.Worker}-{second}");
                }

                foreach(var finished in assignments.Where(x => x.EndSecond <= second).Select(x=> x.Step).Except(returned))
                {
                    returned.Add(finished);
                }

                second++;
            }

            return second;
        }
    }
}
