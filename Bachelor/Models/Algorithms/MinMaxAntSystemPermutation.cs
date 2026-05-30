using System;
using System.Collections.Generic;
using System.Linq;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;

public class MinMaxAntSystemPermutation: MinMaxAntSystem<TSPInstance>
{
    public MinMaxAntSystemPermutation(TSPProblem problem) : base(problem)
    {
        NumAnts = problem.Dimension;
        TauMax = 1.0 - 1.0/problem.Dimension;
        TauMin = 1.0 / (Problem.Dimension *  Problem.Dimension);
        Alpha = 1.0;
        Beta = 5.0;
        InitialPheromone = TauMax;
    }
    private double[,] EdgePheromones;
    private List<int> IterationBest;
    public override void UpdatePheromones()
    {
        throw new NotImplementedException();
    }

    public override void InitializePheromones()
    {
        for (int i = 0; i < Problem.Dimension; i++)
            for (int j = i + 1; j < Problem.Dimension; j++)
                EdgePheromones[i, j] = InitialPheromone;
        return;
    }

    public override void ConstructionGraph()
    {
        
    }

    public override void ConstructAntSolutions() //ordered construction
    {
        List<int> iterationBest = [];
        int iterationBestFitness = 0;
        for (int i = 0; i < NumAnts; i++)
        {
            // initialize ant
            HashSet<int> antNeighbourhood = InitializeNeighbourhood();
            List<int> antPermutation = [];
            int currentVertex = _random.Next(antNeighbourhood.Count); // Random start vertex
            if (antNeighbourhood.Remove(currentVertex))
                antPermutation.Add(currentVertex);
            int tourLength = 0;
            // construct tour (tour length computed as it is built)
            for (int j = 0; j < Problem.Dimension - 1; j++)
            {
                int component = ChooseComponent(currentVertex, antNeighbourhood);
                antNeighbourhood.Remove(component);
                antPermutation.Add(component);
                tourLength += ((TSPProblem) Problem).GetDistance(currentVertex, component, SearchPoint);
            }

            if (tourLength < iterationBestFitness)
            {
                iterationBest = antPermutation;
                iterationBestFitness = tourLength;
            }
        }

        if (iterationBestFitness < BSFF)
        {
            SearchPoint = new TSPInstance(iterationBest, SearchPoint.Graph);
            BSFF = iterationBestFitness;
        }
    }
    
    private int ChooseComponent(int currentVertex, HashSet<int> antNeighbourhood)
    {
        double roll = _random.NextDouble(); // random between 0.0 and 1.0
        double R = 0; // sum of pheromone * heuristic for all edges in neighbourhood
        foreach (var potentialEdgeVertex in antNeighbourhood)
        {
            R += Math.Pow(GetEdgePheromones(currentVertex, potentialEdgeVertex),Alpha) 
                 * Math.Pow(Heuristic(currentVertex, potentialEdgeVertex), Beta);
        }
        double cum = 0;
        int chosenVertex = 80000000; // fallback in case of floating point imprecision, TODO
        foreach (var potentialEdgeVertex in antNeighbourhood)
        {
            double probability = Math.Pow(GetEdgePheromones(currentVertex, potentialEdgeVertex), Alpha)
                                 * Math.Pow(Heuristic(currentVertex, potentialEdgeVertex), Beta)
                                 / R;
            cum += probability;

            if (!(roll <= cum)) continue;
            chosenVertex = potentialEdgeVertex;
            break;
        }
        return chosenVertex;
    }

    private double GetEdgePheromones(int currentVertex, int potentialEdgeVertex)
    {
        int row = Math.Min(currentVertex, potentialEdgeVertex);
        int col = Math.Max(currentVertex, potentialEdgeVertex);
        return EdgePheromones[row, col];
    }

    private HashSet<int> InitializeNeighbourhood()
    {
        return new HashSet<int>(SearchPoint.Permutation);
    }

    private double Heuristic(int currentVertex, int potentialEdgeVertex)
    {
        return ((TSPProblem) Problem).GetDistance(currentVertex, potentialEdgeVertex, SearchPoint);
    }

    
}