
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class Algorithm<T> : IAlgorithm
{
    protected Random _random = new Random();
    public int FuncEvals => Problem.FuncEvals;
    public double Runtime { get; set; }
    public int Iterations { get; private set; }
    public int BSFF { get; set; } // Best So Far Fitness

    protected ProblemType<T> Problem  { get; set; }

    public T SearchPoint;

    public int GetFitness()
    {
        return Problem.Fitness(SearchPoint);
    }

    public int? Optimum => Problem.OptimalFitness;
    public bool Iterate() // return true if the mutation is better than before
    {
        Iterations++;
        return IterateCore();
    }

    
    public abstract bool IterateCore();

    public void Initialize()
    {
        Iterations = 0;
        Problem.FuncEvals = 0;
        InitializeCore();
    }

    public abstract void InitializeCore();
    protected  Algorithm(ProblemType<T> problem)
    {
        this.Problem = problem;
        StoppingCondition = () => FuncEvals >= 10000000;
    }

    public Func<bool> StoppingCondition  { get; set; }
    public void Run()
    {
        
        BSFF = GetFitness();
        long startTime = Stopwatch.GetTimestamp();
        while (!StoppingCondition())
        {
            int newFitness = GetFitness();
            if (Iterate())
                BSFF = newFitness;
        }
        Runtime = Stopwatch.GetElapsedTime(startTime).TotalSeconds;
        Console.WriteLine($"Runtime set inside Run(): {Runtime}, FuncEvals: {FuncEvals}");
    }
    public abstract void Configure(Dictionary<string, object> config);
    public static string BitArrayToString(BitArray bitArray)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bitArray.Length; i++)
        {
            sb.Append(bitArray[i] ? "1" : "0");
        }
        return(sb.ToString());
    }
}