namespace Bachelor.Models;
using System.Collections;

public class OneMax : BitStringProblem
{
    public override int Fitness(BitArray c)
    {
        int count = 0;

        foreach (bool b in c)
            if (b) count++;

        return count;
    }
}