using System;
using Bachelor.Models.Algorithms;

namespace Bachelor.Models.Utility;

public static class StoppingConditions
{
    public static Func<bool> FuncEvals(IAlgorithm algo, int max) 
        => () => algo.FuncEvals >= max;
    
    public static Func<bool> OptimumReached(IAlgorithm algo) 
        => () => algo.Optimum.HasValue && algo.BSFF == algo.Optimum.Value;
    
    public static Func<bool> Either(Func<bool> a, Func<bool> b) 
        => () => a() || b();

    public static Func<bool> ExactFitness(IAlgorithm algorithm, int exactFitness)
    {
        return () => algorithm.BSFF == exactFitness;
    }
        
    
}