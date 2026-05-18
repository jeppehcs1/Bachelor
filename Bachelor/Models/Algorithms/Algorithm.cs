
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;
public interface IAlgorithm
{
    int GetFitness();
    bool Iterate();
    void Initialize();
    void Run();
}
public abstract class Algorithm<T> : IAlgorithm
{
    private int FuncEvals { get; set; }
    private double Runtime { get; set; }
    private int BSFF { get; set; } // Best So Far Fitness

    public IProblemType<T> Problem  { get; set; }

    public T SearchPoint;

    public int GetFitness()
    {
        FuncEvals++;
        return Problem.Fitness(SearchPoint);
    }
    public abstract bool Iterate(); // return 1 if the mutation is better than before
    
    public abstract void Initialize();
    protected  Algorithm(IProblemType<T> problem)
    {
        this.Problem = problem;
    }

    public void Run()
    {
        BSFF = GetFitness();
        while (BSFF < Problem.Dimension)
        {
            
            int newFitness = GetFitness();
            if (Iterate())
                BSFF = newFitness;
                
        }
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