using System;
using System.Collections;
using System.Collections.Generic;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class OnePlusOne<T> : Algorithm<T>
{
    protected readonly Random _random = new Random();
    protected OnePlusOne(ProblemType<T> problem) : base(problem)
    {
        
    }

    public override void Configure(Dictionary<string, object> config)
    {

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