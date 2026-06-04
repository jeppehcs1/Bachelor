namespace Bachelor.Models.Problems;
using System.Collections;

public class OneMax(int dimension) : BitStringProblem(dimension)
{
    public override int? OptimalFitness => Dimension;
    protected override int FitnessCore(BitArray c)
    {
        int count = 0;

        foreach (bool b in c)
            if (b) count++;

        return count;
    }
}