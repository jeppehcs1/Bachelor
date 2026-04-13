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

    public override int UpdateSearchPoint(TSPInstance old)
    {
        if (Problem.Fitness(SearchPoint) >= Problem.Fitness(old))
        {
            Console.WriteLine(Problem.Fitness(SearchPoint) + " :wen old: " + Problem.Fitness(old));
            SearchPoint = old;
            return 0;
        }
        Console.WriteLine(Problem.Fitness(SearchPoint) + " :wen BETTER old: " + Problem.Fitness(old));
        return 1;
    }

    public override void MutateSearchPoint(Random random)
    {
        if(random.Next(2) == 0) // 50/50 chance of 3 opt or 2 opt
        {
            SearchPoint = ((TSPProblem)Problem).MutateTSP_2opt(SearchPoint, random);
        }
        else
        {
            SearchPoint = ((TSPProblem)Problem).MutateTSP_3opt(SearchPoint, random);
        }
    }
}