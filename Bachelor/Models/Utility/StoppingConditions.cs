using System;
using Bachelor.Models.Algorithms;

namespace Bachelor.Models.Utility;

public static class StoppingConditions
{
    public static Func<bool> FuncEvals(IAlgorithm algo, int max) 
        => () => algo.FuncEvals >= max;
    
    public static Func<bool> OptimumReached(IAlgorithm algo, int optimum) 
        => () => algo.BSFF == optimum;
    
    public static Func<bool> Either(Func<bool> a, Func<bool> b) 
        => () => a() || b();
}