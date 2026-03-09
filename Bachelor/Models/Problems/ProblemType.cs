namespace Bachelor.Models;


public abstract class ProblemType<T>
{
    private int dimension { get; set; }


    public abstract int Fitness(T c);
}