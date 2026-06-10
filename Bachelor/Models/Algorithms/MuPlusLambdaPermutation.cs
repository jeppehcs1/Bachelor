using System.Collections.Generic;
using System.Linq;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;
// author Clement
public class MuPlusLambdaPermutation : MuPlusLambda<TSPInstance>
{
    public MuPlusLambdaPermutation(TSPProblem problem, TSPInstance instance) : base(problem)
    {
        Problem = problem;
        SearchPoint = instance;
    }

    public override void InitializeCore()
    {
        Population = new List<(TSPInstance, double)>();

        for (int i = 0; i < Mu; i++)
        {
            var copy = SearchPoint.DeepCopy();
            copy.Shuffle();
            Population.Add((copy, Problem.Fitness(copy)));  // Evaluate once on creation
        }

        var best = Population.OrderBy(x => x.Fitness).First();
        SearchPoint = best.Individual;
        BSFF = (int)best.Fitness;
    }

    public override List<(TSPInstance Individual, double Fitness)> ClonePopulation()
    {
        return Population
            .Select(x => (x.Individual.DeepCopy(), x.Fitness))
            .ToList();
    }

    public override void MutateSearchPoint()
    {
        var children = new List<(TSPInstance Individual, double Fitness)>();

        for (int i = 0; i < Lambda; i++)
        {
            var parent = Population[_random.Next(Population.Count)].Individual;
            var child = parent.DeepCopy();

            if (_random.Next(2) == 0)
                child = ((TSPProblem)Problem).MutateTSP_2opt(child, _random);
            else
                child = ((TSPProblem)Problem).MutateTSP_3opt(child, _random);

            // Evaluate each child exactly once
            children.Add((child, Problem.Fitness(child)));
        }

        // Keep existing parents (fitness cached) and append evaluated children
        Population = Population
            .Select(x => (x.Individual.DeepCopy(), x.Fitness))
            .Concat(children)
            .ToList();
    }

    public override void UpdateSearchPoint(List<(TSPInstance Individual, double Fitness)> old)
    {
        // Sort by cached fitness — no Problem.Fitness calls here
        var best = Population
            .OrderBy(x => x.Fitness)  // TSP minimizes, ascending
            .Take(Mu)
            .ToList();
        

        Population = best;
        SearchPoint = best[0].Individual;
        BSFF = (int)best[0].Fitness;
       
    }
}