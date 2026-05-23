using System;
using System.Collections.Generic;
using Avalonia.Markup.Xaml;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;

public abstract class MinMaxAntSystem<T> : Algorithm<T>
{
    public MinMaxAntSystem(ProblemType<T> problem) : base(problem)
    {
        Neighbourhood = Permutation; // needs fix, where do we get permutation?
        defaultPheromone = 1.0 / problem.Dimension;
        tauMax = 1.0 - 1.0 / problem.Dimension;
        tauMin = 1.0 / (problem.Dimension * problem.Dimension);
    }
    public double defaultPheromone;
    public double tauMax;
    public double tauMin;
    public double alpha = 1.0; // determines impact of pheromone
    public double beta = 0.0; // determines impact of heuristic
    public double evaporationFactor = 1.0; // determines strength of pheromone update
    public Dictionary<(int, int), double> EdgePheromones; // To avoid adding every edge; if it is not there, the value is 1/dimension. PROBLEM SYMMETRIC MEANS ORDER SHOULDNT MATTER
    public override bool Iterate()
    {
        Construct();
        // Todo 
        // UpdatePheromones();
        return true;
    }
    public List<(int, int)> Graph;
    public List<int> Permutation;
    public List<int> Neighbourhood;
    public List<int> Construct() // ordered construction
    {
        List<int> result = new List<int>();
        var currentVertex = 0; // arbitrary, start vertex
        result.Add(currentVertex); 
        Neighbourhood.Remove(currentVertex);
        var random = new Random();
        for (int i = 0; i < Problem.Dimension; i++) // måske ikke korrekt num iterations??
        {
            double roll = random.NextDouble();
            double R = 0; // cumulative pheromone * heuristic
            foreach (var potentialEdgeVertex in Neighbourhood)
            {
                R += Math.Pow(EdgePheromones.GetValueOrDefault((currentVertex, potentialEdgeVertex), defaultPheromone),alpha) 
                     * Math.Pow(Heuristic(currentVertex, potentialEdgeVertex), beta);
            }
            double cum = 0;
            int edgeVertex = 0;
            foreach (var potentialEdgeVertex in Neighbourhood)
            {
                double probability = Math.Pow(EdgePheromones.GetValueOrDefault((currentVertex, potentialEdgeVertex), defaultPheromone), alpha) / R;
                cum += probability;

                if (roll <= cum)
                {
                    edgeVertex = potentialEdgeVertex;
                    break;
                }
            }
            result.Add(edgeVertex);
            Neighbourhood.Remove(edgeVertex);
            currentVertex = edgeVertex;
        }
        
        return result;
    }

    private double Heuristic(int currentVertex, int potentialEdgeVertex) // should be 1/edgeWeight but doesn't matter for beta = 0
    {
        return 1.0;
    }

    public void UpdatePheromones(List<int> tourPermutation)
    {
        double addVal =
            Math.Min((1.0 - evaporationFactor) * EdgePheromones.GetValueOrDefault((tourPermutation[^1], tourPermutation[0]), defaultPheromone),
                    tauMax);
        EdgePheromones[(tourPermutation[^1], tourPermutation[0])] = addVal;
        for (int i = 0; i < Problem.Dimension - 1; i++)
        {
            addVal = Math.Min((1.0 - evaporationFactor) * EdgePheromones.GetValueOrDefault((tourPermutation[i], tourPermutation[i + 1]), defaultPheromone),
                             tauMax);
            EdgePheromones[(tourPermutation[i], tourPermutation[i + 1])] = addVal;
            
        }

        defaultPheromone = tauMin;  // only works for evaporation = 1.0

    }
}