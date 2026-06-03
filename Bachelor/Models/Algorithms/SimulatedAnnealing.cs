using System;
using System.Collections;
using System.Collections.Generic;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class SimulatedAnnealing<T> : Algorithm<T>
{
    protected Random _random = new Random();
    public double _temperature;
    protected double _alpha;
    protected SimulatedAnnealing(ProblemType<T> problem) : base(problem)
    {
        _alpha = 1 - 1/((double) (problem.Dimension) * 10);
        _temperature = problem.Dimension * problem.Dimension * problem.Dimension;
    }

    public override bool IterateCore()
    {
        var dim = Problem.Dimension;
        var old = CloneSearchPoint();
        MutateSearchPoint();
        return UpdateSearchPoint(old);
    }
    public override void Configure(Dictionary<string, object> config)
    {
        if (config.TryGetValue("Alpha", out var alpha)) _alpha = (double)alpha;
        if (config.TryGetValue("Temperature", out var temperature)) _temperature = (double)temperature;
    }
    public abstract T CloneSearchPoint();
    public abstract bool UpdateSearchPoint(T old);
    public abstract void MutateSearchPoint();
    
}