using System;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Scheduling;

namespace Bachelor.ViewModels;

public static class AlgorithmFactory
{
    public static IAlgorithm Create(Schedule schedule)
    {
        var algorithmName = schedule.AlgorithmName;
        var searchSpace = schedule.SearchSpace;
        var dimension = schedule.Dimension;
        IAlgorithm algorithm = (algorithmName, searchSpace) switch
        {
            ("OnePlusOne", "Bit Strings") => new OnePlusOneBitString(new OneMax(dimension)),
            ("OnePlusOne", "Permutations") => new OnePlusOnePermutation(new TSPProblem(dimension), new TSPInstance()),
            ("SimulatedAnnealing", "Bit Strings") => new SimulatedAnnealingBitString(new OneMax(dimension)),
            _ => throw new ArgumentException($"Unknown: {algorithmName}, {searchSpace}")
        };
        return algorithm;
    }
}