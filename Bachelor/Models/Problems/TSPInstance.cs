using System;
using System.Collections.Generic;

namespace Bachelor.Models.Problems;

public struct TSPInstance
{
    public List<int> Permutation;
    public List<(int x, int y)> Graph;

    // Constructor
    public TSPInstance(List<int> permutation, List<(int x, int y)> graph)
    {
        // Create new lists to avoid sharing references → deep copy
        Permutation = new List<int>(permutation);
        Graph = new List<(int x, int y)>(graph);
    }

    // Deep copy method
    public TSPInstance DeepCopy()
    {
        // Lists are copied
        return new TSPInstance(new List<int>(Permutation), new List<(int x, int y)>(Graph));
    }

    public void Shuffle()
    {
        var rng = new Random();
        int n = Permutation.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (Permutation[k], Permutation[n]) = (Permutation[n], Permutation[k]); // swap using tuple syntax
        }
        
    }
}
