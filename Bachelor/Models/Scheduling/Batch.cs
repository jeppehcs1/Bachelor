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
        for (int i = 0; i < NumberRuns; i++)
        {
            Algorithm.Initialize();
            Algorithm.Run();
            Console.WriteLine("Run " + (i+1) + " fitness: "  + Algorithm.GetFitness());
        }
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