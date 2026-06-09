using System;
using System.Collections;
using System.Collections.Generic;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class OnePlusOne<T> : Algorithm<T>
{
    
    protected OnePlusOne(ProblemType<T> problem) : base(problem)
    {
        
    }

    public override void Configure(Dictionary<string, object> config)
    {

    }

    public override void IterateCore()
    {
        var dim = Problem.Dimension;
        var old = CloneSearchPoint();
        MutateSearchPoint();
        UpdateSearchPoint(old);
    }

    public abstract T CloneSearchPoint();
    public abstract void UpdateSearchPoint(T old);
    public abstract void MutateSearchPoint();



}                      