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
    
    
    public override void InitializeCore()
    {
        _temperature = _initialTemperature;
        SearchPoint.Shuffle();
        BSFF = Problem.Fitness(SearchPoint);
        CurrentFitness = BSFF;
    }

    public override TSPInstance CloneSearchPoint()
    {
        return SearchPoint.DeepCopy();
    }

    public override bool UpdateSearchPoint(TSPInstance old)
    {
        _temperature *= _alpha;
        int newFitness = GetFitness();
        int oldFitness = CurrentFitness;
        if (newFitness < oldFitness)
        {
            BSFF = Math.Min(BSFF, newFitness);
            return true;
        }
        var delta = oldFitness - newFitness;
        var prob = Math.Exp(delta/_temperature);
        
        if (_random.NextDouble() < prob)
        {
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