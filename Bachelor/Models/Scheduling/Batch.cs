using System;
using System.IO;
using System.Threading;
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
            var _random = new Random();
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string logDir = Path.Combine(GetProjectRoot(), "Assets", "LogFiles");
            Directory.CreateDirectory(logDir); // creates it if it doesn't exist
            OutputFilePath = Path.Combine(logDir, $"batch_{Name}_{_random.NextInt64(1000)}_{timestamp}.csv");
            
        }
        else
        {
            OutputFilePath = outputFilePath;
        }
    }
    private static string GetProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && dir.Name != "Bachelor")
            dir = dir.Parent;
        return dir?.FullName ?? AppContext.BaseDirectory;
    }
    
    public async Task RunAll(CancellationToken ct = default)
    {
        Status = Status.Running;
        var logger = new CsvLogger(OutputFilePath);

        for (int i = 0; i < NumberRuns; i++)
        {
            if (ct.IsCancellationRequested) break;
            Runner.Restart();
            await Run(ct);
            var snapshot = Runner.TakeSnapshot(Algorithm);
            logger.LogRun(snapshot);
        }
        logger.WriteSummary();
        Console.WriteLine($"Results saved to: {OutputFilePath}");
        Status = Status.Completed;
    }
    public async Task Run(CancellationToken ct) => await Runner.Run(Algorithm, ct);
    public void Pause() => Runner.Pause();
    public void Play() => Runner.Play();
    
}

public enum Status
{
    Awaiting,
    Running,
    Completed,
    Failed
}