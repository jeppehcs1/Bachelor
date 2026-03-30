namespace Bachelor.Models.Algorithms;

using System;
using System.Collections;
using System.Collections.Generic;  
using Bachelor.Models.Problems;


public class OnePlusOnePermutation : OnePlusOne<TSPInstance>
{
    public OnePlusOnePermutation(PermutationProblem problem, TSPInstance instance) : base(problem)
    {
        Problem = problem;
        SearchPoint = instance;
    }
    
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
        if (Problem.Fitness(SearchPoint) > Problem.Fitness(old))
        {
            Console.WriteLine(Problem.Fitness(SearchPoint) + " :wen old: " + Problem.Fitness(old));
            SearchPoint = old;
        }
    }

    public override void MutateSearchPoint(Random random)
    {
        SearchPoint = ((PermutationProblem)Problem).MutateTSP(SearchPoint);
    }
}