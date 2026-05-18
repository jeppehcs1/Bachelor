using System;
using System.Collections;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class SimulatedAnnealing<T> : Algorithm<T>
{
    protected SimulatedAnnealing(IProblemType<T> problem) : base(problem)
    {
        
    }

    public override bool Iterate()
    {
        var dim = Problem.Dimension;
        var random = new Random();
        var old = CloneSearchPoint();
        MutateSearchPoint();
        return UpdateSearchPoint(old);
    }

    public abstract T CloneSearchPoint();
    public abstract bool UpdateSearchPoint(T old);
    public abstract void MutateSearchPoint();
    
}