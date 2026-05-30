
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;
public interface IAlgorithm
{
    int BSFF { get; set; }
    double Runtime { get; set; }
    int FuncEvals { get; }
    int GetFitness();
    bool Iterate();
    void Initialize();
    void Run();
    
}
public abstract class Algorithm<T> : IAlgorithm
{
    public int FuncEvals => Problem.FuncEvals;

    public double Runtime { get; set; }
    public int BSFF { get; set; } // Best So Far Fitness

    protected ProblemType<T> Problem  { get; set; }

    public T SearchPoint;

    public int GetFitness()
    {
        return Problem.Fitness(SearchPoint);
    }
    public abstract bool Iterate(); // return true if the mutation is better than before

    public virtual void Initialize()
    {
        Problem.FuncEvals = 0;
        InitializeCore();
    }

    public abstract void InitializeCore();
    protected  Algorithm(ProblemType<T> problem)
    {
        this.Problem = problem;
    }

    public void Run()
    {
        
        BSFF = GetFitness();
        long startTime = Stopwatch.GetTimestamp();
        while (BSFF != 7542 && FuncEvals < 10000000)
        {
            int newFitness = GetFitness();
            if (Iterate())
                BSFF = newFitness;
        }
        Runtime = Stopwatch.GetElapsedTime(startTime).TotalSeconds;
    }
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