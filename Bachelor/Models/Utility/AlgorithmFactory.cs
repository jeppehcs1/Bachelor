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
        var dimension = schedule.Dimension;
        var instance = schedule.TSPInstance;
        IAlgorithm algorithm = (algorithmName, searchSpace) switch
        {
            ("OnePlusOne", "Bit Strings") => new OnePlusOneBitString(new OneMax(dimension)),
            ("OnePlusOne", "Permutations") => new OnePlusOnePermutation(new TSPProblem(dimension), instance),
            ("SimulatedAnnealing", "Bit Strings") => new SimulatedAnnealingBitString(new OneMax(dimension)),
            ("MinMaxAntSystem", "Bit Strings") => new MinMaxAntSystemBitString(new OneMax(dimension)),
            ("MinMaxAntSystem", "Permutations") => new MinMaxAntSystemPermutation(new TSPProblem(dimension), instance),
            _ => throw new ArgumentException($"Unknown: {algorithmName}, {searchSpace}")
        };
        return algorithm;
    }
}