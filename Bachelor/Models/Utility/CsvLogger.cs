using System;
using System.Globalization;
using System.IO;

namespace Bachelor.Models.Utility;
// author Clement
public class CsvLogger
{
    private readonly string _filePath;
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;
    private int _runCount;
    private double _totalTime;

    public CsvLogger(string filePath)
    {
        _filePath = filePath;
        using var writer = new StreamWriter(filePath, false);
        writer.WriteLine("Run,Fitness,Runtime,FuncEvals,Iterations");
    }

    public void LogRun(AlgorithmSnapshot snapshot)
    {
        _runCount++;
        _totalTime += snapshot.Runtime;
        using var writer = new StreamWriter(_filePath, append: true);
        writer.WriteLine($"{_runCount},{snapshot.BSFF},{snapshot.Runtime.ToString("F3", _culture)},{snapshot.FuncEvals},{snapshot.Iterations}");
    }

    public void WriteSummary()
    {
        using var writer = new StreamWriter(_filePath, append: true);
        writer.WriteLine();
        writer.WriteLine($"Total Time,{_totalTime.ToString("F3",_culture)}");
        writer.WriteLine($"Average Runtime,{(_totalTime / _runCount).ToString("F3",_culture)}");
    }
}