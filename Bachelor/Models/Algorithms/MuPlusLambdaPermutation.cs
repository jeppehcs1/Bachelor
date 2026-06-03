using System.Collections.Generic;
using System.Linq;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public class MuPlusLambdaPermutation : MuPlusLambda<TSPInstance>
{
    public MuPlusLambdaPermutation(PermutationProblem problem, TSPInstance instance) : base(problem)
    {
        Problem = problem;
        SearchPoint = instance;
    }
    

    public override void InitializeCore()
    {
        Population = new List<TSPInstance>();
        
        for (int i = 0; i < Mu; i++)
        {
            var copy = SearchPoint.DeepCopy();
            copy.Shuffle();
            Population.Add(copy);
        }
    
        SearchPoint = Population.OrderBy(i => Problem.Fitness(i)).First();
        BSFF = Problem.Fitness(SearchPoint);
    }

    public override List<TSPInstance> CloneSearchPoint()
    {
        return Population.Select(instance => instance.DeepCopy()).ToList();
    }
    
    public override void MutateSearchPoint()
    {
        var children = new List<TSPInstance>();

        for (int i = 0; i < Lambda; i++)
        {
            var parent = Population[_random.Next(Population.Count)];
            var child = parent.DeepCopy();
        
            if (_random.Next(2) == 0)
                child = ((TSPProblem)Problem).MutateTSP_2opt(child, _random);
            else
                child = ((TSPProblem)Problem).MutateTSP_3opt(child, _random);
        
            children.Add(child);
        }

        Population = Population.Select(i => i.DeepCopy()).Concat(children).ToList();
    }

    public override bool UpdateSearchPoint(List<TSPInstance> old)
    {
        var best = Population
            .OrderBy(instance => Problem.Fitness(instance))  // TSP minimizes, so ascending
            .Take(Mu)
            .ToList();

        bool improved = Problem.Fitness(best.First()) < Problem.Fitness(old.OrderBy(i => Problem.Fitness(i)).First());

        Population = best;
        SearchPoint = best.First();
        BSFF = Problem.Fitness(SearchPoint);
        return improved;
    }
    
    
}