using System;
using System.IO;
using System.Threading.Tasks;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Utility;

namespace Bachelor.Models.Scheduling;

public class Batch
{
    int NumberRuns { get; set; }
    public IAlgorithm Algorithm { get; set; }
    public AlgorithmRunner Runner { get; } = new();
    public string Name { get; set; }
    public Status Status { get; set; }
    string OutputFilePath { get; set; }
    public Batch(IAlgorithm algorithm,  int numberRuns, string name, string outputFilePath = null)
    {
        Algorithm = algorithm;
        NumberRuns = numberRuns;
        Name = name;
        Status = Status.Awaiting;
        
        // Generate file path if not provided
        if (string.IsNullOrEmpty(outputFilePath))
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            OutputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                $"batch_results_{Name}_{timestamp}.csv");
        }
        else
        {
            OutputFilePath = outputFilePath;
        }
    }
    
    public async Task RunAll()
    {
        Status = Status.Running;
        var logger = new CsvLogger(OutputFilePath);

        for (int i = 0; i < NumberRuns; i++)
        {
            await Run();
            var snapshot = Runner.TakeSnapshot(Algorithm);
            logger.LogRun(snapshot);
        }
        logger.WriteSummary();

        Console.WriteLine($"Results saved to: {OutputFilePath}");
        Status = Status.Completed;
    }
    public async Task Run() => await Runner.Run(Algorithm);
    public void Pause() => Runner.Pause();
    public void Resume() => Runner.Resume();
    public void Restart()
    {
        Runner.Stop();
        Runner.Restart();
    }
}

public enum Status
{
    Awaiting,
    Running,
    Completed,
    Failed
}