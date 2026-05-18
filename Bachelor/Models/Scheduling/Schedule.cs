using System.Collections.Generic;
using Bachelor.Models.Algorithms;

namespace Bachelor.Models.Scheduling;

public class Schedule
{
    public string SearchSpace;
    public string AlgorithmName;
    public string ProblemName;
    public string FinishCondition;
    public string Visualization;
    public int Dimension;
    
    List<Batch> Batches { get; set; }
    public Schedule(string searchSpace, string algorithm, string problem, string finishCondition, string visualization, int dimension){
        SearchSpace = searchSpace;
        AlgorithmName = algorithm;
        ProblemName = problem;
        FinishCondition = finishCondition;
        Visualization = visualization;
        Dimension = dimension;
    }
    
}
