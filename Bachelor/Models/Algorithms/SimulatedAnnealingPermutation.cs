using System;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;

public class SimulatedAnnealingPermutation : SimulatedAnnealing<TSPInstance>
{
    
    public SimulatedAnnealingPermutation(GraphProblem problem, TSPInstance instance) : base(problem)
    {
        Problem = problem;
        SearchPoint = instance;
    }
    
    
    public override void Initialize()
    {
        SearchPoint.Shuffle();
    }

    public override TSPInstance CloneSearchPoint()
    {
        return SearchPoint.DeepCopy();
    }

    public override bool UpdateSearchPoint(TSPInstance old)
    {
        _temperature *= _alpha;
        
        Console.WriteLine("Old: " + Problem.Fitness(old) +  "\nNew: " + Problem.Fitness(SearchPoint) + 
        "\ntemperature: " + _temperature);
        if (Problem.Fitness(SearchPoint) <= Problem.Fitness(old))
        {
            return true;
        }
        
        var delta = Problem.Fitness(old) - Problem.Fitness(SearchPoint);
        var prob = Math.Exp(delta/_temperature);
        Console.WriteLine("delta: " + delta + "   prob: " + prob);
        
        if (_random.NextDouble() < prob)
        {
            Console.WriteLine("Random: " + _random.NextDouble() + "  prob:   " + prob);
            return true;
        }
        
        SearchPoint = old;
        return false;
        
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