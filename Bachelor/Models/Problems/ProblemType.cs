namespace Bachelor.Models.Problems;


public abstract class ProblemType<T>
{
    internal int dimension { get; set; }


    public abstract int Fitness(T c);
}