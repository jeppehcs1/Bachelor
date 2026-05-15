
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;
public interface IAlgorithm
{
    int GetFitness();
    int Iterate();
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
        return Problem.Fitness(SearchPoint);
    }
    public abstract int Iterate(); // return 1 if the mutation is better than before
    
    public abstract void Initialize();
    protected  Algorithm(IProblemType<T> problem)
    {
        this.Problem = problem;
    }

    public void Run()
    {
        Console.WriteLine(BSFF);
        while (BSFF < Problem.Dimension)
        {
            Iterate();
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