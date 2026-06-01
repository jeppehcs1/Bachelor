using System;
using System.Collections.Generic;
using System.Linq;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;

public class MinMaxAntSystemPermutation: MinMaxAntSystem<TSPInstance>
{
    public MinMaxAntSystemPermutation(TSPProblem problem, TSPInstance instance) : base(problem)
    {
        NumAnts = problem.Dimension;
        TauMax = 1.0 - 1.0/problem.Dimension;
        TauMin = 1.0 / (Problem.Dimension *  Problem.Dimension);
        Alpha = 1.0;
        Beta = 5.0;
        Rho = 0.002;
        InitialPheromone = TauMax;
        SearchPoint = instance;
        BSFF = GetFitness();
        EdgePheromones = new double[Problem.Dimension, Problem.Dimension];
    }
    private double[,] EdgePheromones;
    private (List<int>, int) IterationBest;
    public override void UpdatePheromones()
    {
        // Evaporate all (only upper triangle since TSP is symmetric)
        for (int i = 0; i < Problem.Dimension; i++)
            for (int j = i + 1; j < Problem.Dimension; j++)
                EdgePheromones[i, j] = Math.Max(TauMin, EdgePheromones[i, j] * (1 - Rho));

        // Deposit on best tour edges
        var tour = IterationBest.Item1;
        double deposit = 1.0 / IterationBest.Item2;
    
        for (int i = 0; i < tour.Count - 1; i++)
        {
            int row = Math.Min(tour[i], tour[i + 1]);
            int col = Math.Max(tour[i], tour[i + 1]);
            EdgePheromones[row, col] = Math.Min(TauMax, EdgePheromones[row, col] + deposit);
        }
        // closing edge
        int r = Math.Min(tour[^1], tour[0]);
        int c = Math.Max(tour[^1], tour[0]);
        EdgePheromones[r, c] = Math.Min(TauMax, EdgePheromones[r, c] + deposit);
    }

    public override void InitializePheromones()
    {
        for (int i = 0; i < Problem.Dimension; i++)
            for (int j = i + 1; j < Problem.Dimension; j++)
                EdgePheromones[i, j] = InitialPheromone;
        
    }

    public override void ConstructAntSolutions() //ordered construction
    {
        int iterationBestFitness = 999999999;
        for (int i = 0; i < NumAnts; i++)
        {
            // initialize ant
            HashSet<int> antNeighbourhood = InitializeNeighbourhood();
            List<int> antPermutation = [];
            int currentVertex = _random.Next(antNeighbourhood.Count); // Random start vertex
            if (antNeighbourhood.Remove(currentVertex))
                antPermutation.Add(currentVertex);
            
            // construct tour 
            for (int j = 0; j < Problem.Dimension - 1; j++)
            {
                int component = ChooseComponent(currentVertex, antNeighbourhood);
                antNeighbourhood.Remove(component);
                antPermutation.Add(component);
                
                currentVertex = component;
            }
            int tourLength = ((TSPProblem) Problem).Fitness(new TSPInstance(antPermutation, SearchPoint.Graph));
            if (tourLength < iterationBestFitness)
            {
                IterationBest = (antPermutation, tourLength);
                iterationBestFitness = tourLength;
            }
        }

        if (iterationBestFitness < BSFF)
        {
            SearchPoint = new TSPInstance(IterationBest.Item1, SearchPoint.Graph);
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
        int chosenVertex = Problem.Dimension + 2; // fallback in case of floating point imprecision, ensure error
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

    public override double GetEdgePheromones(int currentVertex, int potentialEdgeVertex)
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
        return 1.0/((TSPProblem) Problem).GetDistance(currentVertex, potentialEdgeVertex, SearchPoint);
    }

    public override void InitializeCore()
    {
        SearchPoint.Shuffle();
        base.InitializeCore();
    }
}