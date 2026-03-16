using System.Collections;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;

public abstract class OnePlusOne<T> : Algorithm<T>
{
    protected OnePlusOne(ProblemType<T> problem) : base(problem)
    {
        
    }
} 