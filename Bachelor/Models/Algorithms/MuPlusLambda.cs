using System.Collections;
using System.Collections.Generic;

namespace Bachelor.Models.Algorithms;
using System;
using Bachelor.Models.Problems;

public abstract class MuPlusLambda<T> : Algorithm<T>
{
    protected readonly Random _random = new Random();
    public int Mu;
    public int Lambda;
    public List<T> Population;
    
    protected MuPlusLambda(ProblemType<T> problem) : base(problem)
    {
        Mu = 5;
        Lambda = 20;
    }
    public override void Configure(Dictionary<string, object> config)
    {
        if (config.TryGetValue("Mu", out var mu)) Mu = (int)mu;
        if (config.TryGetValue("Lambda", out var lambda)) Lambda = (int)lambda;
    }
    public override bool Iterate()
    {
        var dim = Problem.Dimension;
        var old = CloneSearchPoint();
        MutateSearchPoint();
        return UpdateSearchPoint(old);
    }
    
    public abstract List<T> CloneSearchPoint();
    public abstract bool UpdateSearchPoint(List<T> old);
    public abstract void MutateSearchPoint();
    
}