using System;
using System.Collections.Generic;
using Avalonia.Markup.Xaml;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;

public abstract class MinMaxAntSystem<T> : Algorithm<T>
{
    protected Random _random = new Random();
    public MinMaxAntSystem(ProblemType<T> problem) : base(problem)
    {
        
    }
    
    public int NumAnts;
    public double InitialPheromone;
    public double TauMax;
    public double TauMin;
    public double Alpha; // determines impact of pheromone
    public double Beta; // determines impact of heuristic
    public double Rho; // determines evaporation of pheromone
    
    public override bool Iterate()
    {
        ConstructAntSolutions();
        // ApplyLocalSearch();
        // UpdatePheromones();
        return true;
    }
    

    public List<(int, int)> Graph;
    public List<int> Permutation;
    public List<int> Neighbourhood;
    
    
    public abstract void ConstructAntSolutions(); // return iteration best as searchpoint
    public abstract void UpdatePheromones();
    
    public abstract void InitializePheromones();
    public abstract void ConstructionGraph();
    public override void InitializeCore()
    {
        InitializePheromones();
    }
}