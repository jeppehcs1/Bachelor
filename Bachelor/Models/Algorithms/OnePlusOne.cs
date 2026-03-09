using System.Collections;

namespace Bachelor.Models;

public class OnePlusOne : Algorithm
{
    public override void Run()
    {
        searchPointString = inputString;
        while (problem.Fitness(searchPointString) <= problem.dimension)
        {
            
        }
    }
}