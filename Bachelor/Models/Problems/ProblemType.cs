namespace Bachelor.Models.Problems;


public abstract class ProblemType<T>
{
    protected ProblemType(int dimension)
    {
        this.Dimension = dimension;
    }
    internal int Dimension { get; set; }


    public abstract int Fitness(T c);
}