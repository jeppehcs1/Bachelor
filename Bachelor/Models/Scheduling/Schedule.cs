using System;
using System.Collections.Generic;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Utility;

namespace Bachelor.Models.Scheduling;
// author Jeppe
public class Schedule
{
    public string SearchSpace;
    public string AlgorithmName;
    public string ProblemName;
    public string FinishCondition;
    public string Visualization;
    private TSPInstance _tspInstance;
    public TSPInstance TSPInstance 
    { 
        get => _tspInstance.DeepCopy(); 
        set => _tspInstance = value; 
    }
    public int Dimension;
    public int MaxEvals;
    public int ExactFitness;
    public Dictionary<string, object> AlgorithmConfig = new Dictionary<string, object>();
    public Schedule(string searchSpace, string algorithm, string problem, string finishCondition, string visualization, 
        int dimension, int maxEvals, int exactFitness){
        SearchSpace = searchSpace;
        AlgorithmName = algorithm;
        ProblemName = problem;
        FinishCondition = finishCondition;
        Visualization = visualization;
        Dimension = dimension;
        MaxEvals = maxEvals;
        ExactFitness = exactFitness;
    }

    public Schedule(string searchSpace, string algorithm, string problem, string finishCondition, string visualization,
        TSPInstance instance, int maxEvals, int exactFitness)
    {
        SearchSpace = searchSpace;
        AlgorithmName = algorithm;
        ProblemName = problem;
        FinishCondition = finishCondition;
        Visualization = visualization;
        Dimension = instance.Graph.Count;
        TSPInstance = instance;
        MaxEvals = maxEvals;
        ExactFitness = exactFitness;
    }
    public Func<bool> BuildStoppingCondition(IAlgorithm algorithm)
    {
        return FinishCondition switch
        {
            "Function evaluations" => StoppingConditions.FuncEvals(algorithm, MaxEvals),
            "Optimum reached" => StoppingConditions.Either(
                StoppingConditions.OptimumReached(algorithm),
                StoppingConditions.FuncEvals(algorithm, int.MaxValue)),
            "Exact fitness" => 
                StoppingConditions.ExactFitness(algorithm, ExactFitness),
            _ => throw new ArgumentException($"Unknown finish condition: {FinishCondition}")
        };
    }
}
