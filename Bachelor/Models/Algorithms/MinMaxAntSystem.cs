using System.Collections.Generic;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;
// author Jeppe
public abstract class MinMaxAntSystem<T> : Algorithm<T>
{
   
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
    
    public override void IterateCore()
    {
        ConstructAntSolutions();
        // ApplyLocalSearch();
        UpdatePheromones();
        
    }
    
    public override void Configure(Dictionary<string, object> config)
    {
        if (config.TryGetValue("TauMin", out var taumin)) TauMin = (double)taumin;
        if (config.TryGetValue("TauMax", out var taumax)) TauMax= (double)taumax;
        if (config.TryGetValue("Rho", out var rho)) Rho = (double)rho;
        if (config.TryGetValue("Alpha", out var alpha)) Alpha = (double)alpha;
        if (config.TryGetValue("Beta", out var beta)) Beta = (double)beta;
        if (config.TryGetValue("Ants", out var ants)) NumAnts = (int)ants;
        
    }
    
    
    public abstract void ConstructAntSolutions(); // return iteration best as searchpoint
    public abstract void UpdatePheromones();
    
    public abstract void InitializePheromones();

    public override void InitializeCore()
    {
        InitializePheromones();
    }

    public abstract double GetEdgePheromones(int currentVertex, int potentialVertex);
}