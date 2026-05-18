using System;
using System.Collections;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class SimulatedAnnealing<T> : Algorithm<T>
{
    protected Random _random = new Random();
    protected double _temperature;
    protected double _alpha;
    protected SimulatedAnnealing(IProblemType<T> problem) : base(problem)
    {
        _alpha = 1 - 1/((double) (problem.Dimension) * 10);
        _temperature = problem.Dimension * problem.Dimension * problem.Dimension;
    }

    public override bool Iterate()
    {
        var dim = Problem.Dimension;
        var old = CloneSearchPoint();
        MutateSearchPoint();
        return UpdateSearchPoint(old);
    }

    public abstract T CloneSearchPoint();
    public abstract bool UpdateSearchPoint(T old);
    public abstract void MutateSearchPoint();
    
}