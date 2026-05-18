using System;
using Bachelor.Models.Algorithms;

namespace Bachelor.Models.Scheduling;

public class Batch
{
    int NumberRuns { get; set; }
    IAlgorithm Algorithm { get; set; }
    public string Name { get; set; }
    public Status Status { get; set; }
    public Batch(IAlgorithm algorithm,  int numberRuns, string name)
    {
        Algorithm = algorithm;
        NumberRuns = numberRuns;
        Name = name;
        Status = Status.Awaiting;
    }

    public void Run()
    {
        Status = Status.Running;
        double totalTime = 0;
        for (int i = 0; i < NumberRuns; i++)
        {
            Algorithm.Initialize();
            Algorithm.Run();
            totalTime += Algorithm.Runtime;
            Console.WriteLine("Run " + (i+1) + " fitness: "  + Algorithm.BSFF + " time: " + Algorithm.Runtime + " FuncEvals: " + Algorithm.FuncEvals);
        }
        Console.WriteLine("Total time: " + totalTime);
        Status = Status.Completed;
    }
}

public enum Status
{
    Awaiting,
    Running,
    Completed,
    Failed
}