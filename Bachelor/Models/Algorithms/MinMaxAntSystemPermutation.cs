using System;
using System.Collections.Generic;
using Bachelor.Models.Problems;
using Bachelor.Models.Utility;

namespace Bachelor.Models.Algorithms;
// author Jeppe
public class MinMaxAntSystemPermutation: MinMaxAntSystem<TSPInstance>
{
    public MinMaxAntSystemPermutation(TSPProblem problem, TSPInstance instance) : base(problem)
    {
        NumAnts = problem.Dimension;
        Alpha = 1.0;
        Beta = 5.0;
        Rho = 0.002;
        TauMax = 1.0 / (Rho * Problem.Fitness(NearestNeighbour.Solve(instance, problem)));
        TauMin = TauMax * (1 - Math.Pow(0.05, 1.0/NumAnts)) / ((NumAnts/2.0 - 1.0) * Math.Pow(0.05, 1.0/NumAnts));
        InitialPheromone = TauMax;
        SearchPoint = instance;
        BSFF = GetFitness();
        _edgePheromones = new double[Problem.Dimension, Problem.Dimension];
    }
    private double[,] _edgePheromones;
    private (List<int>, int) _iterationBest;
    public override void UpdatePheromones()
    {
        // update TauMax and TauMin with new BSFF
        TauMax = 1.0/(Rho * BSFF);
        TauMin = TauMax * (1 - Math.Pow(0.05, 1.0/NumAnts)) / ((NumAnts/2.0 - 1.0) * Math.Pow(0.05, 1.0/NumAnts));
        // Evaporate all (only upper triangle since TSP is symmetric)
        for (int i = 0; i < Problem.Dimension; i++)
            for (int j = i + 1; j < Problem.Dimension; j++)
                _edgePheromones[i, j] = Math.Max(TauMin, _edgePheromones[i, j] * (1 - Rho));

        // Deposit on best tour edges
        var tour = _iterationBest.Item1;
        double deposit = 1.0 / _iterationBest.Item2;
    
        for (int i = 0; i < tour.Count - 1; i++)
        {
            int row = Math.Min(tour[i], tour[i + 1]);
            int col = Math.Max(tour[i], tour[i + 1]);
            _edgePheromones[row, col] = Math.Min(TauMax, _edgePheromones[row, col] + deposit);
        }
        // closing edge
        int r = Math.Min(tour[^1], tour[0]);
        int c = Math.Max(tour[^1], tour[0]);
        _edgePheromones[r, c] = Math.Min(TauMax, _edgePheromones[r, c] + deposit);
    }

    public override void InitializePheromones()
    {
        for (int i = 0; i < Problem.Dimension; i++)
            for (int j = i + 1; j < Problem.Dimension; j++)
                _edgePheromones[i, j] = InitialPheromone;
        
    }

    public override void ConstructAntSolutions() //ordered construction
    {
        int iterationBestFitness = int.MaxValue;
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
                _iterationBest = (antPermutation, tourLength);
                iterationBestFitness = tourLength;
            }
        }

        if (iterationBestFitness < BSFF)
        {
            SearchPoint = new TSPInstance(_iterationBest.Item1, SearchPoint.Graph);
            BSFF = iterationBestFitness;
        }
    }
    
    private int ChooseComponent(int currentVertex, HashSet<int> antNeighbourhood)
    {
        double roll = _random.NextDouble(); // random between 0.0 and 1.0
        double r = 0; // sum of pheromone * heuristic for all edges in neighbourhood
        foreach (var potentialEdgeVertex in antNeighbourhood)
        {
            r += Math.Pow(GetEdgePheromones(currentVertex, potentialEdgeVertex),Alpha) 
                 * Math.Pow(Heuristic(currentVertex, potentialEdgeVertex), Beta);
        }
        double cum = 0;
        int chosenVertex = Problem.Dimension + 2; // fallback in case of floating point imprecision, ensure error
        foreach (var potentialEdgeVertex in antNeighbourhood)
        {
            double probability = Math.Pow(GetEdgePheromones(currentVertex, potentialEdgeVertex), Alpha)
                                 * Math.Pow(Heuristic(currentVertex, potentialEdgeVertex), Beta)
                                 / r;
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
        return _edgePheromones[row, col];
    }

    private HashSet<int> InitializeNeighbourhood()
    {
        return new HashSet<int>(SearchPoint.Permutation);
    }

    private double Heuristic(int currentVertex, int potentialEdgeVertex)
    {
        return 1.0/((TSPProblem) Problem).GetEuclidianDistance(currentVertex, potentialEdgeVertex, SearchPoint);
    }

    public override void InitializeCore()
    {
        SearchPoint.Shuffle();
        BSFF = GetFitness();
        base.InitializeCore();
    }
}