namespace Bachelor.Models.Problems;
using System.Collections;

public class LeadingOnes : BitStringProblem
{
    public override int Fitness(BitArray c)
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