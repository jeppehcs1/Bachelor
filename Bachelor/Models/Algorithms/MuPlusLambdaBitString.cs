using System.Collections.Generic;
using System.Linq;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;
using System;
using System.Collections;
// author Clement
public class MuPlusLambdaBitString(ProblemType<BitArray> problem) : MuPlusLambda<BitArray>(problem)
{
    public override List<(BitArray Individual, double Fitness)> ClonePopulation()
    {
        return Population
            .Select(x => ((BitArray)x.Individual.Clone(), x.Fitness))
            .ToList();
    }

    public override void MutateSearchPoint()
    {
        var children = new List<(BitArray Individual, double Fitness)>();

        for (int i = 0; i < Lambda; i++)
        {
            var parent = Population[_random.Next(Population.Count)].Individual;
            var child = (BitArray)parent.Clone();

            for (int j = 0; j < child.Count; j++)
            {
                if (_random.NextDouble() < 1.0 / child.Count)
                    child[j] = !child[j];
            }

            // Evaluate each child exactly once
            children.Add((child, Problem.Fitness(child)));
        }

        // Mu + Lambda: keep existing parents (fitness cached) and append evaluated children
        Population = Population
            .Select(x => ((BitArray)x.Individual.Clone(), x.Fitness))
            .Concat(children)
            .ToList();
    }

    public override void UpdateSearchPoint(List<(BitArray Individual, double Fitness)> old)
    {
        // Sort by cached fitness — no Problem.Fitness calls here
        var best = Population
            .OrderByDescending(x => x.Fitness)
            .Take(Mu)
            .ToList();

        

        Population = best;
        SearchPoint = best[0].Individual;
        BSFF = (int)best[0].Fitness;
    }

    public override void InitializeCore()
    {
        var dim = Problem.Dimension;
        Population = new List<(BitArray, double)>();

        for (int i = 0; i < Mu; i++)
        {
            var bits = new bool[dim];
            for (int j = 0; j < dim; j++)
                bits[j] = _random.Next(2) == 1;

            var ba = new BitArray(bits);
            Population.Add((ba, Problem.Fitness(ba)));  // Evaluate once on creation
        }

        var best = Population.OrderByDescending(x => x.Fitness).First();
        SearchPoint = best.Individual;
        BSFF = (int)best.Fitness;
    }
}