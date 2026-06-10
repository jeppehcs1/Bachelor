using System.Collections.Generic;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Utility;
// author Jeppe
public static class NearestNeighbour
{
    public static TSPInstance Solve(TSPInstance instance, TSPProblem problem)
    {
        int n = instance.Graph.Count;
        var visited = new bool[n];
        var permutation = new List<int>(n);

        int current = 0;
        visited[current] = true;
        permutation.Add(current);

        for (int i = 1; i < n; i++)
        {
            int nearest = -1;
            int nearestDist = int.MaxValue;

            for (int j = 0; j < n; j++)
            {
                if (visited[j]) continue;
                int dist = problem.GetEuclidianDistance(current, j, instance);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = j;
                }
            }

            visited[nearest] = true;
            permutation.Add(nearest);
            current = nearest;
        }

        return new TSPInstance(permutation, instance.Graph);
    }
}
