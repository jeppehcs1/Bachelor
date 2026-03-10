using System.Collections;

namespace Bachelor.Models.Algorithms;

public class OnePlusOne : Algorithm<BitArray>
{
    public override void Run()
    {
        searchPointString = inputString;
        while (problem.Fitness(searchPointString) <= problem.dimension)
        {
            
        }
    }
}