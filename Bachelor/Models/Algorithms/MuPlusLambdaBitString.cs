using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;
using System;
using System.Collections;

public class MuPlusLambdaBitString(ProblemType<BitArray> problem) : MuPlusLambda<BitArray>(problem)
{
    public override List<BitArray> CloneSearchPoint()
    {
        return Population.Select(ba => (BitArray)ba.Clone()).ToList();
    }

    public override void MutateSearchPoint()
    {
        var children = new List<BitArray>();

        for (int i = 0; i < Lambda; i++)
        {
            // Pick a random parent from the population
            var parent = Population[_random.Next(Population.Count)];
        
            // Clone and mutate the parent
            var child = (BitArray)parent.Clone();
            for (int j = 0; j < child.Count; j++)
            {
                if (_random.NextDouble() < 1.0 / child.Count)
                    child[j] = !child[j];
            }
            children.Add(child);
        }

        // Mu + Lambda: parents and children combined
        Population = Population.Select(ba => (BitArray)ba.Clone()).Concat(children).ToList();
    }

    public override bool UpdateSearchPoint(List<BitArray> old)
    {
        var best = Population
            .OrderByDescending(ba => Problem.Fitness( ba ))
            .Take(Mu)
            .ToList();

        bool improved = best.Any(ba => !old.Contains(ba));

        Population = best;
        SearchPoint = best.OrderByDescending(ba => Problem.Fitness(ba)).First();
        BSFF = Problem.Fitness(SearchPoint);
        return improved;
    }
    

    public override void InitializeCore()
    {
        var dim = Problem.Dimension; // Dimension of each bit string
        
        Population = new List<BitArray>();
        
        for (int i = 0; i < Mu; i++)
        {
            var bits = new bool[dim];
            for (int j = 0; j < dim; j++)
            {
                bits[j] = _random.Next(2) == 1;
            }
            Population.Add(new BitArray(bits));
        }

        SearchPoint = Population.OrderByDescending(ba => Problem.Fitness(ba)).First();
        BSFF = Problem.Fitness(SearchPoint);
    }
}