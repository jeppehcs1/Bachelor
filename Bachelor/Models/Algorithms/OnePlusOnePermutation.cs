namespace Bachelor.Models.Algorithms;

using Bachelor.Models.Problems;
using System.Collections.Generic;
using System.Collections;  
using System;


public class OnePlusOnePermutation(PermutationProblem problem) : OnePlusOne<TSPInstance>(problem)
{
    public override int GetFitness()
    {
        return Problem.Fitness(SearchPoint);
    }
    

    public override void Initialize()
    {
        SearchPoint.Shuffle();
    }

    public override TSPInstance CloneSearchPoint()
    {
        return SearchPoint.DeepCopy();
    }

    public override void UpdateSearchPoint(TSPInstance old)
    {
        if (Problem.Fitness(SearchPoint) < Problem.Fitness(old))
        {
            SearchPoint = old;
        }
    }

    public override void MutateSearchPoint(Random random)
    {
        SearchPoint = problem.MutateTSP(SearchPoint);
    }
}