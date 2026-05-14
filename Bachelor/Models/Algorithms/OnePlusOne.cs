using System;
using System.Collections;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class OnePlusOne<T> : Algorithm<T>
{
    private readonly Random _random = new Random();
    protected OnePlusOne(ProblemType<T> problem) : base(problem)
    {
        
    }

    

    public override int Iterate()
    {
        var dim = Problem.Dimension;
        var old = CloneSearchPoint();
        MutateSearchPoint(_random);
        return UpdateSearchPoint(old);
    }

    public abstract T CloneSearchPoint();
    public abstract int UpdateSearchPoint(T old);
    public abstract void MutateSearchPoint(Random random);



}                      