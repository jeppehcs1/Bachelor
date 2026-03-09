using System.Collections;

namespace Bachelor.Models;

public abstract class Algorithm
{
    private int funcEvals { get; set; }
    private double runtime { get; set; }
    private int BSFF { get; set; }

    public ProblemType<BitArray> problem  { get; set; }
    public BitArray inputString  { get; set; }
    private string inputGraph = "";

    public BitArray searchPointString  { get; set; }
    private string searchPointGraph = "";
    
    public abstract void Run();
}