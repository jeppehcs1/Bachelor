namespace Bachelor.Models.Problems;


public abstract class ProblemType<T>
{
    protected ProblemType(int dimension)
    {
        this.dimension = dimension;
    }
    internal int dimension { get; set; }


    public abstract int Fitness(T c);
}