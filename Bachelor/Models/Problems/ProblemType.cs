namespace Bachelor.Models.Problems;


public interface IProblemType<T>
{
    internal int Dimension { get; set; }
    internal int FuncEvals { get; set; }
    public abstract int Fitness(T c);
}

public abstract class ProblemType<T> : IProblemType<T>
{
    public int Dimension { get; set; }
    public int FuncEvals { get; set; }
    

    public int Fitness(T c)
    {
        FuncEvals++;
        return FitnessCore(c);
    }

    protected abstract int FitnessCore(T c);
}