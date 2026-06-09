using System.Collections.Generic;

namespace Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;

public abstract class MuPlusLambda<T> : Algorithm<T>
{
    public int Mu;
    public int Lambda;
    public List<(T Individual, double Fitness)> Population;

    protected MuPlusLambda(ProblemType<T> problem) : base(problem)
    {
        Mu = 5;
        Lambda = 20;
    }

    public override void Configure(Dictionary<string, object> config)
    {
        if (config.TryGetValue("Mu", out var mu)) Mu = (int)mu;
        if (config.TryGetValue("Lambda", out var lambda)) Lambda = (int)lambda;
    }

    public override bool IterateCore()
    {
        var old = ClonePopulation();
        MutateSearchPoint();
        return UpdateSearchPoint(old);
    }

    public abstract List<(T Individual, double Fitness)> ClonePopulation();
    public abstract bool UpdateSearchPoint(List<(T Individual, double Fitness)> old);
    public abstract void MutateSearchPoint();
}