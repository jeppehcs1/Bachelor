namespace Bachelor.Models.Problems;


public abstract class ProblemType<T>(int dimension)
{
    internal int Dimension { get; set; } = dimension;


    public abstract int Fitness(T c);
}