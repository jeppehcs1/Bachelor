using System.Collections.Generic;
using Bachelor.Models.Algorithms;

namespace Bachelor.Models.Scheduling;

public class Schedule<T>
{
    
    List<Batch<T>> Batches { get; set; }
}