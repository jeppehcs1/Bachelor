namespace Bachelor.Models.Problems;




public abstract class ProblemType<T>
{
    public int Dimension { get; set; }
    public int FuncEvals { get; set; }
    protected ProblemType(int dimension)
    {
        Dimension = dimension;
    }

    public int Fitness(T c)
    {
        FuncEvals++;
        return FitnessCore(c);
    }

    protected abstract int FitnessCore(T c);
}