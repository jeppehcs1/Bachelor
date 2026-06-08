using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Scheduling;

namespace Bachelor.Models.Utility;

public static class AlgorithmFactory
{
    public static IAlgorithm CreateAndConfigure(Schedule schedule)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var suffix = schedule.SearchSpace switch
        {
            "Bit Strings" => "BitString",
            "Permutations" => "Permutation",
            _ => throw new ArgumentException($"Unknown search space: {schedule.SearchSpace}")
        };
    
        var algorithmType = assembly.GetTypes()
                                .FirstOrDefault(t => t.Namespace == "Bachelor.Models.Algorithms"
                                                     && t.Name == $"{schedule.AlgorithmName}{suffix}")
                            ?? throw new ArgumentException($"Unknown algorithm: {schedule.AlgorithmName}{suffix}");

        var problemType = assembly.GetTypes()
                              .FirstOrDefault(t => t.Namespace == "Bachelor.Models.Problems"
                                                   && t.Name == schedule.ProblemName)
                          ?? throw new ArgumentException($"Unknown problem: {schedule.ProblemName}");

        var problem = Activator.CreateInstance(problemType, schedule.Dimension);
    
        IAlgorithm algorithm = schedule.SearchSpace switch
        {
            "Bit Strings" => (IAlgorithm) Activator.CreateInstance(algorithmType, problem),
            "Permutations" => (IAlgorithm) Activator.CreateInstance(algorithmType, problem, schedule.TSPInstance),
            _ => throw new ArgumentException()
        };
        algorithm.Configure(schedule.AlgorithmConfig);
        algorithm.StoppingCondition = schedule.BuildStoppingCondition(algorithm);
        return algorithm;
    }
}