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

    public void clemRun()
    {
        double totalTime = 0;
        // Write header to CSV file
        using (var writer = new StreamWriter(OutputFilePath, false))
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            writer.WriteLine("Run,Fitness,Runtime,FuncEvals");
            for (int i = 0; i < NumberRuns; i++)
            {
                Algorithm.Initialize();
                Console.WriteLine($"FuncEvals after Initialize: {Algorithm.FuncEvals}");
                Algorithm.Run();
                Console.WriteLine($"FuncEvals after Run: {Algorithm.FuncEvals}, Runtime: {Algorithm.Runtime}");
                totalTime += Algorithm.Runtime;
                // Write to file
                writer.WriteLine(
                    $"{i + 1},{Algorithm.BSFF},{Algorithm.Runtime.ToString("F3", culture)},{Algorithm.FuncEvals}");
            }

            writer.WriteLine();
            writer.WriteLine($"Total Time,{totalTime.ToString(culture)}");
            writer.WriteLine($"Average Runtime,{(totalTime / NumberRuns).ToString(culture)}");
        }

        //Console.WriteLine("Total time: " + totalTime);
        Console.WriteLine($"Results saved to: {OutputFilePath}");
        Status = Status.Completed;
    }
    public async Task RunAll()
    {
        Status = Status.Running;
        double totalTime = 0;
        for (int i = 0; i < NumberRuns; i++)
        {
            await Run();
            totalTime += Algorithm.Runtime;
        }
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