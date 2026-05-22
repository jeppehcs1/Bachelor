namespace Bachelor.Models.Problems;
using System.Collections;

public class LeadingOnes(int dimension) : BitStringProblem(dimension)
{
    

    protected override int FitnessCore(BitArray c)
    {
        int count = 0;

        foreach (bool b in c)
        {
            if (!b) {return count;}
            count++;
        }
        return count;
    }
    
}