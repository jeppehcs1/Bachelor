using Bachelor.Models.Algorithms;

namespace Bachelor.Models.Scheduling;

public class Batch<T>
{
    int NumberRuns { get; set; }
    Algorithm<T> Algorithm { get; set; }
    public string Name { get; set; }
}