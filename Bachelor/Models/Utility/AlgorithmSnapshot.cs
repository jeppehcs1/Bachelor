using System.Collections;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Utility;
// author Jeppe and Clement
public class AlgorithmSnapshot
{
    public int BSFF { get; init; }
    public int FuncEvals { get; init; }
    public int Iterations { get; init; }
    public int Fitness { get; init; }
    public double Runtime { get; init; }
    public TSPInstance TSPSearchPoint { get; init; }
    public BitArray BitStringSearchPoint { get; init; }
}