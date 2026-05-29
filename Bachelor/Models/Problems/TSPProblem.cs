using System;
using System.Linq;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public class TSPProblem : PermutationProblem
{
    public TSPProblem(int dimension) : base(dimension)
    {
        DistanceMatrix = new int[dimension,dimension];
    }
    public int[,] DistanceMatrix;
    public int GetDistance(int i, int j, TSPInstance instance)
    {
        int row = Math.Min(i, j);
        int col = Math.Max(i, j);
    
        if (DistanceMatrix[row, col] == 0)
            DistanceMatrix[row, col] = EuclidianDistance(instance.Graph[row], instance.Graph[col]);
    
        return DistanceMatrix[row, col];
    }
    internal int EuclidianDistance((int, int) p1, (int, int) p2)
    {
        
        var (x1, y1) = p1;
        var (x2, y2) = p2;

        int xDist = x2 - x1;
        int yDist = y2 - y1;
        
        // Rounds to nearest integer
        return (int) Math.Round(Math.Sqrt(xDist * xDist + yDist * yDist));
    }
    
    protected override int FitnessCore(TSPInstance c)
    {
        int fitness = 0;
        for (int i = 1; i < Dimension; i++)
            fitness += GetDistance(c.Permutation[i-1], c.Permutation[i], c);
    
        fitness += GetDistance(c.Permutation[Dimension - 1], c.Permutation[0], c);
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
        
        int rand1 = random.Next(Dimension-1);
        int rand2 = random.Next(Dimension-1);
        int rand3 = random.Next(Dimension-1);
        while (rand1 == rand2 || rand3 == rand1 || rand2 == rand3)
        {
            rand2 = random.Next(Dimension-1);
            rand3 = random.Next(Dimension-1);
        }
        int[] indices = { rand1, rand2, rand3 };
        Array.Sort(indices);
        rand1 = indices[0];  // smallest
        rand2 = indices[1];  // middle
        rand3 = indices[2];  // largest
        
        
        
        var chunks = SplitAtIndices(instance.Permutation, indices);
        // chunk[0] first element Perm[indices[2]] last element Perm[indices[0]-1] (wraps around)
        // chunk[1] first element Perm[indices[0]] last element Perm[indices[1]-1]
        // chunk[1] first element Perm[indices[1]] last element Perm[indices[2]-1]
        var permOriginalIntermediateFitness = IntermediateFitness((chunks[0][^1],chunks[1][0]),
                                                                        (chunks[1][^1],chunks[2][0]),
                                                                        (chunks[2][^1],chunks[0][0]), instance);
        var oneRevA = chunks[0].AsEnumerable().Reverse()
            .Concat(chunks[1])
            .Concat(chunks[2])
            .ToList();
        TSPInstance permA = new TSPInstance(oneRevA, instance.Graph);
        // calculate fitness of new edges
        var permAIntermediateFitness = IntermediateFitness((chunks[0][0],chunks[1][0]),
                                                                (chunks[1][^1],chunks[2][0]),
                                                                (chunks[2][^1],chunks[0][^1]), instance);
        var oneRevB = chunks[0]
            .Concat(chunks[1].AsEnumerable().Reverse())
            .Concat(chunks[2])
            .ToList();
        TSPInstance permB = new TSPInstance(oneRevB, instance.Graph);
        var permBIntermediateFitness = IntermediateFitness((chunks[0][^1],chunks[1][^1]),
                                                                (chunks[1][0],chunks[2][0]),
                                                                (chunks[2][^1],chunks[0][0]), instance);
        var oneRevC = chunks[0]
            .Concat(chunks[1])
            .Concat(chunks[2].AsEnumerable().Reverse())
            .ToList();
        TSPInstance permC = new TSPInstance(oneRevC, instance.Graph);
        var permCIntermediateFitness = IntermediateFitness((chunks[0][^1],chunks[1][0]),
                                                                (chunks[1][^1],chunks[2][^1]),
                                                                (chunks[2][0],chunks[0][0]), instance);
        var twoRevA = chunks[0].AsEnumerable().Reverse()
            .Concat(chunks[1].AsEnumerable().Reverse())
            .Concat(chunks[2])
            .ToList();
        TSPInstance permD = new TSPInstance(twoRevA, instance.Graph);
        var permDIntermediateFitness = IntermediateFitness((chunks[0][0],chunks[1][^1]),
                                                                (chunks[1][0],chunks[2][0]),
                                                                (chunks[2][^1],chunks[0][^1]), instance);
        var twoRevB = chunks[0].AsEnumerable().Reverse()
            .Concat(chunks[1])
            .Concat(chunks[2].AsEnumerable().Reverse())
            .ToList();
        TSPInstance permE = new TSPInstance(twoRevB, instance.Graph);
        var permEIntermediateFitness = IntermediateFitness((chunks[0][0],chunks[1][0]),
                                                                (chunks[1][0],chunks[2][^1]),
                                                                (chunks[2][0],chunks[0][^1]), instance);
        var twoRevC = chunks[0]
            .Concat(chunks[1].AsEnumerable().Reverse())
            .Concat(chunks[2].AsEnumerable().Reverse())
            .ToList();
        TSPInstance permF = new TSPInstance(twoRevC, instance.Graph);
        var permFIntermediateFitness = IntermediateFitness((chunks[0][^1],chunks[1][^1]),
                                                                (chunks[1][0],chunks[2][^1]),
                                                                (chunks[2][0],chunks[0][0]), instance);
        var threeRev = chunks[0].AsEnumerable().Reverse()
            .Concat(chunks[1].AsEnumerable().Reverse())
            .Concat(chunks[2].AsEnumerable().Reverse())
            .ToList();
        TSPInstance permG = new TSPInstance(threeRev, instance.Graph);
        var permGIntermediateFitness = IntermediateFitness((chunks[0][0],chunks[1][^1]),
                                                                (chunks[1][0],chunks[2][^1]),
                                                                (chunks[2][0],chunks[0][^1]), instance);

        var perms = new[] { (instance, permOriginalIntermediateFitness), 
                            (permA, permAIntermediateFitness), 
                            (permB, permBIntermediateFitness),
                            (permC, permCIntermediateFitness),
                            (permD, permDIntermediateFitness),
                            (permE, permEIntermediateFitness),
                            (permF, permFIntermediateFitness),
                            (permG, permGIntermediateFitness) };
        var best = perms
            .MinBy(p => p.Item2);
        
        
        var result = best.Item1;

        return result;
    }
   
    public int IntermediateFitness((int, int) indexPair1, (int, int) indexPair2, (int, int) indexPair3, TSPInstance instance)
    {
        int i1 = CheckBoundary(indexPair1.Item1), j1 = CheckBoundary(indexPair1.Item2);
        int i2 = CheckBoundary(indexPair2.Item1), j2 = CheckBoundary(indexPair2.Item2);
        int i3 = CheckBoundary(indexPair3.Item1), j3 = CheckBoundary(indexPair3.Item2);

        return GetDistance(i1, j1, instance)
               + GetDistance(i2, j2, instance)
               + GetDistance(i3, j3, instance);
    }

    public int CheckBoundary(int index)
    {
        if (index < 0) 
        { return Dimension - 1;
        }
        return index == Dimension ? 0 : index;
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