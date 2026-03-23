namespace Bachelor.Models.Algorithms;

using Bachelor.Models.Problems;
using System.Collections.Generic;
using System.Collections;  
using System;


public class OnePlusOnePermutation(ProblemType<TSPInstance> problem) : OnePlusOne<TSPInstance>(problem)
{
    public override int GetFitness()
    {
        return Problem.Fitness(SearchPoint);
    }
    

    public override void Initialize()
    {
        throw new NotImplementedException();
    }

    public override TSPInstance CloneSearchPoint()
    {
        return SearchPoint.DeepCopy();
    }

    public override void UpdateSearchPoint(TSPInstance problemtype)
    {
        throw new NotImplementedException();
    }

    public override void MutateSearchPoint(Random random)
    {
        throw new NotImplementedException();
    }
}