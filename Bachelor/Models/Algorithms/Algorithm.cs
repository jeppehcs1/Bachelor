
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class Algorithm<T>
{
    private int FuncEvals { get; set; }
    private double Runtime { get; set; }
    private int BSFF { get; set; } // Best So Far Fitness

    public ProblemType<T> Problem  { get; set; }

    public T SearchPoint;
    
    public abstract int GetFitness();
    public abstract void Iterate();
    
    public abstract void Initialize();
    protected  Algorithm(ProblemType<T> problem)
    {
        this.Problem = problem;
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