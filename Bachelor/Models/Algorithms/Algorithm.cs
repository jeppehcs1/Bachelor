
using System.Collections;
using Bachelor.Models.Problems;
namespace Bachelor.Models.Algorithms;

public abstract class Algorithm<T>
{
    private int FuncEvals { get; set; }
    private double Runtime { get; set; }
    private int BSFF { get; set; }

    public ProblemType<T> problem  { get; set; }
    public BitArray inputString  { get; set; }
    private string inputGraph = "";

    public BitArray searchPointString  { get; set; }
    private string searchPointGraph = "";
    
    public abstract void Run();
}