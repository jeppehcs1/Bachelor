using System;
using System.Collections;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class OnePlusOne<T> : Algorithm<T>
{
    protected OnePlusOne(ProblemType<T> problem) : base(problem)
    {
        
    }

    

    public override void Iterate()
    {
        var dim = Problem.dimension;
        var random = new Random();
        var old = CloneSearchPoint();//searchPointString.Clone() as BitArray;
        MutateSearchPoint(random);
        UpdateSearchPoint(old);
        //Console.WriteLine(BitArrayToString(searchPointString));
    }

    public abstract T CloneSearchPoint();
    public abstract void UpdateSearchPoint(T old);
    public abstract void MutateSearchPoint(Random random);



}                      