
using System.Collections;
using Bachelor.Models.Problems;
using System.Text;
namespace Bachelor.Models.Algorithms;

public abstract class Algorithm<T>
{
    private int FuncEvals { get; set; }
    private double Runtime { get; set; }
    private int BSFF { get; set; }

    public ProblemType<T> problem  { get; set; }
    
    public BitArray searchPointString  { get; set; }
    private string searchPointGraph = "";
    public abstract int GetFitness();
    public abstract void Iterate();
    
    public abstract void Initialize();
    protected  Algorithm(ProblemType<T> problem)
    {
        this.problem = problem;
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