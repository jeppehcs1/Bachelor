using System.Collections.Generic;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Utility;

namespace Bachelor.Models.Scheduling;

public class Schedule
{
    public string SearchSpace;
    public string AlgorithmName;
    public string ProblemName;
    public string FinishCondition;
    public string Visualization;
    public TSPInstance TSPInstance;
    public int Dimension;
    public Schedule(string searchSpace, string algorithm, string problem, string finishCondition, string visualization, int dimension){
        SearchSpace = searchSpace;
        AlgorithmName = algorithm;
        ProblemName = problem;
        FinishCondition = finishCondition;
        Visualization = visualization;
        Dimension = dimension;
    }

    public Schedule(string searchSpace, string algorithm, string problem, string finishCondition, string visualization,
        TSPInstance instance)
    {
        SearchSpace = searchSpace;
        AlgorithmName = algorithm;
        ProblemName = problem;
        FinishCondition = finishCondition;
        Visualization = visualization;
        Dimension = instance.Graph.Count;
        TSPInstance = instance;
    }
    
}
