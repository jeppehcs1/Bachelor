using System;
using System.Linq;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public class TSPProblem(int dimension) : PermutationProblem(dimension)
{

    internal int EuclidianDistance((int, int) p1, (int, int) p2)
    {
        var (x1, y1) = p1;
        var (x2, y2) = p2;

        int xDist = x2 - x1;
        int yDist = y2 - y1;
        
        // Rounds to nearest integer
        return (int) Math.Round(Math.Sqrt(xDist * xDist + yDist * yDist));
    }
    
    public override int Fitness(TSPInstance c)
    {
        int fitness = 0;
        for (int i = 1; i < Dimension; i++)
        {
            (int, int) p1 = c.Graph[c.Permutation[i-1]];
            (int, int) p2 = c.Graph[c.Permutation[i]];

            fitness = fitness + EuclidianDistance(p1, p2);
        }
        fitness += EuclidianDistance(c.Graph[c.Permutation[Dimension - 1]], c.Graph[c.Permutation[0]]);
        return fitness;
    }

    public TSPInstance MutateTSP_2opt(TSPInstance instance,Random random)
    {
        
        int rand1 = random.Next(Dimension);
        int rand2 = random.Next(Dimension);
        while (rand1 == rand2)
        {
            rand2 = random.Next(Dimension);
        }
        if(rand1>rand2) (rand1, rand2) = (rand2, rand1);
        
        instance.Permutation.Reverse(rand1+1, rand2-rand1);

        return instance;
    }
    
    public TSPInstance MutateTSP_3opt(TSPInstance instance,Random random)
    {
        
        int rand1 = random.Next(dimension-1);
        int rand2 = random.Next(dimension-1);
        int rand3 = random.Next(dimension-1);
        while (rand1 == rand2 || rand3 == rand1 || rand2 == rand3)
        {
            rand2 = random.Next(dimension-1);
            rand3 = random.Next(dimension-1);
        }
        int[] indices = { rand1, rand2, rand3 };
        Array.Sort(indices);
        rand1 = indices[0];  // smallest
        rand2 = indices[1];  // middle
        rand3 = indices[2];  // largest
        
        var chunks = SplitAtIndices(instance.Permutation, indices);

        var oneRevA = chunks[0].AsEnumerable().Reverse()
            .Concat(chunks[1])
            .Concat(chunks[2])
            .ToList();
        TSPInstance permA = new TSPInstance(oneRevA, instance.Graph);
        
        var oneRevB = chunks[0]
            .Concat(chunks[1].AsEnumerable().Reverse())
            .Concat(chunks[2])
            .ToList();
        TSPInstance permB = new TSPInstance(oneRevB, instance.Graph);
        
        var oneRevC = chunks[0]
            .Concat(chunks[1])
            .Concat(chunks[2].AsEnumerable().Reverse())
            .ToList();
        TSPInstance permC = new TSPInstance(oneRevC, instance.Graph);
        
        var twoRevA = chunks[0].AsEnumerable().Reverse()
            .Concat(chunks[1].AsEnumerable().Reverse())
            .Concat(chunks[2])
            .ToList();
        TSPInstance permD = new TSPInstance(twoRevA, instance.Graph);
        
        var twoRevB = chunks[0].AsEnumerable().Reverse()
            .Concat(chunks[1])
            .Concat(chunks[2].AsEnumerable().Reverse())
            .ToList();
        TSPInstance permE = new TSPInstance(twoRevB, instance.Graph);
        
        var twoRevC = chunks[0]
            .Concat(chunks[1].AsEnumerable().Reverse())
            .Concat(chunks[2].AsEnumerable().Reverse())
            .ToList();
        TSPInstance permF = new TSPInstance(twoRevC, instance.Graph);
        
        var threeRev = chunks[0].AsEnumerable().Reverse()
            .Concat(chunks[1].AsEnumerable().Reverse())
            .Concat(chunks[2].AsEnumerable().Reverse())
            .ToList();
        TSPInstance permG = new TSPInstance(threeRev, instance.Graph);
        

        var perms = new[] { instance, permA, permB, permC, permD, permE, permF, permG };
        var best = perms
            .Select(p => new { Perm = p, Fitness = Fitness(p) })
            .MinBy(x => x.Fitness);
        
        if (best == null)
            throw new Exception("No elements");
        var result = best.Perm;

        return result;
    }
    
    public List<List<int>> SplitAtIndices(List<int> list, int[] indices)
    {
        var chunks = new List<List<int>>();

        int prev = 0;

        foreach (var idx in indices)
        {
            chunks.Add(list.GetRange(prev, idx - prev));
            prev = idx;
        }

        // last chunk
        chunks.Add(list.GetRange(prev, list.Count - prev));

        // merge last + first
        var merged = new List<int>();
        merged.AddRange(chunks[^1]); // last
        merged.AddRange(chunks[0]);  // first

        // build result
        var result = new List<List<int>> { merged };

        for (int i = 1; i < chunks.Count - 1; i++)
        {
            result.Add(chunks[i]);
        }

        return result;
    }
    
}