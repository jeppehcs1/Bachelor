namespace Bachelor.Models.Problems;


public interface IProblemType<T>
{
    internal int Dimension { get; set; }
    public abstract int Fitness(T c);
}