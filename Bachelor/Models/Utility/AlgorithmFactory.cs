using System;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Scheduling;

namespace Bachelor.Models.Utility;

public static class AlgorithmFactory
{
    public static IAlgorithm Create(Schedule schedule)
    {
        var algorithmName = schedule.AlgorithmName;
        var searchSpace = schedule.SearchSpace;
        var problemName = schedule.ProblemName;
        var dimension = schedule.Dimension;
        var instance = schedule.TSPInstance;
    
        IAlgorithm algorithm = (algorithmName, searchSpace, problemName) switch
        {
            ("OnePlusOne", "Bit Strings", "OneMax") => new OnePlusOneBitString(new OneMax(dimension)),
            ("OnePlusOne", "Bit Strings", "LeadingOnes") => new OnePlusOneBitString(new LeadingOnes(dimension)),
            ("OnePlusOne", "Permutations", _) => new OnePlusOnePermutation(new TSPProblem(dimension), instance),
            ("SimulatedAnnealing", "Bit Strings", "OneMax") => new SimulatedAnnealingBitString(new OneMax(dimension)),
            ("SimulatedAnnealing", "Bit Strings", "LeadingOnes") => new SimulatedAnnealingBitString(new LeadingOnes(dimension)),
            ("SimulatedAnnealing", "Permutations", _) => new SimulatedAnnealingPermutation(new TSPProblem(dimension), instance),
            ("MinMaxAntSystem", "Bit Strings", "OneMax") => new MinMaxAntSystemBitString(new OneMax(dimension)),
            ("MinMaxAntSystem", "Bit Strings", "LeadingOnes") => new MinMaxAntSystemBitString(new LeadingOnes(dimension)),
            ("MinMaxAntSystem", "Permutations", _) => new MinMaxAntSystemPermutation(new TSPProblem(dimension), instance),
            ("MuPlusLambda", "Bit Strings", "OneMax") => new MuPlusLambdaBitString(new OneMax(dimension)),
            ("MuPlusLambda", "Bit Strings", "LeadingOnes") => new MuPlusLambdaBitString(new LeadingOnes(dimension)),
            ("MuPlusLambda", "Permutations", _) => new MuPlusLambdaPermutation(new TSPProblem(dimension), instance),
            _ => throw new ArgumentException($"Unknown: {algorithmName}, {searchSpace}, {problemName}")
        };
        return algorithm;
    }
}