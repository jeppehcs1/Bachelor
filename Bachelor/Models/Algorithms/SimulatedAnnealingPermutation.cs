using System;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;

public class SimulatedAnnealingPermutation : SimulatedAnnealing<TSPInstance>
{
    
    public SimulatedAnnealingPermutation(PermutationProblem problem, TSPInstance instance) : base(problem)
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
        _temperature *= _alpha;
        
        Console.WriteLine("Old: " + Problem.Fitness(old) +  "\nNew: " + Problem.Fitness(SearchPoint) + 
        "\ntemperature: " + _temperature);
        if (Problem.Fitness(SearchPoint) <= Problem.Fitness(old))
        {
            return 1;
        }
        
        var delta = Problem.Fitness(old) - Problem.Fitness(SearchPoint);
        var prob = Math.Exp(delta/_temperature);
        Console.WriteLine("delta: " + delta + "   prob: " + prob);
        
        if (_random.NextDouble() < prob)
        {
            Console.WriteLine("Random: " + _random.NextDouble() + "  prob:   " + prob);
            return 1;
        }
        
        SearchPoint = old;
        return 0;
        
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