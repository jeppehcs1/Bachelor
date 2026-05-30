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
    
    
    

    public override void InitializeCore()
    {
        SearchPoint.Shuffle();
    }

    public override TSPInstance CloneSearchPoint()
    {
        return SearchPoint.DeepCopy();
    }

    public override bool UpdateSearchPoint(TSPInstance old)
    {
        if (Problem.Fitness(SearchPoint) >= Problem.Fitness(old))
        {
            //Console.WriteLine(Problem.Fitness(SearchPoint) + " :wen old: " + Problem.Fitness(old));
            SearchPoint = old;
            return false;
        }
        //Console.WriteLine(Problem.Fitness(SearchPoint) + " :wen BETTER old: " + Problem.Fitness(old));
        return true;
    }

    public override void MutateSearchPoint()
    {
        if(_random.Next(2) == 0) // 50/50 chance of 3 opt or 2 opt
        {
            SearchPoint = ((TSPProblem)Problem).MutateTSP_2opt(SearchPoint, _random);
        }
        else
        {
            SearchPoint = ((TSPProblem)Problem).MutateTSP_3opt(SearchPoint, _random);
        }
    }
}