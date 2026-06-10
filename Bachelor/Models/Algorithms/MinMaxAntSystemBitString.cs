using System;
using System.Collections;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;
// author Jeppe
public class MinMaxAntSystemBitString : MinMaxAntSystem<BitArray>
{
    public MinMaxAntSystemBitString(ProblemType<BitArray> problem) : base(problem)
    {
        NumAnts = 1;
        TauMax = 1.0 - 1.0/problem.Dimension;
        TauMin = 1.0 / Problem.Dimension;
        Alpha = 1.0;
        Beta = 0;
        Rho = 0.002;
        InitialPheromone = 0.5;
        SearchPoint = new BitArray(Problem.Dimension);
        SearchPoint.SetAll(false);
        BSFF = GetFitness();
        EdgePheromones = new double [Problem.Dimension];
    }

    public double[] EdgePheromones;
    public override void ConstructAntSolutions()
    {
        BitArray solution =  new BitArray(Problem.Dimension);
        for (int i = 0; i < Problem.Dimension; i++)
        {
            if (_random.NextDouble() < GetEdgePheromones(i, i + 1))
                solution[i] = true;
        }

        int fitness = Problem.Fitness(solution);
        if (fitness > BSFF)
        {
            SearchPoint = solution;
            BSFF = fitness;
        }
            
    }

    public override void UpdatePheromones()
    {
        // Evaporate all and deposit
        for (int i = 0; i < SearchPoint.Count; i++)
            if (SearchPoint[i])
                EdgePheromones[i] = Math.Min(TauMax, EdgePheromones[i] * (1 - Rho) + Rho);
            else
            {
                EdgePheromones[i] = Math.Max(TauMin, EdgePheromones[i] * (1 - Rho));
            }
    }

    public override void InitializePheromones()
    {
        for (int i = 0; i < Problem.Dimension; i++)
            EdgePheromones[i] = InitialPheromone;
    }

    public override void InitializeCore()
    {
        base.InitializeCore();
        var dim = Problem.Dimension;
        var bits = new bool[dim];
        var random = new Random();
        
        for (int i = 0; i < dim; i++)
        {
            bits[i] = random.Next(2) == 1;  // Random true or false
        }

        SearchPoint = new BitArray(bits);
        BSFF = GetFitness();
    }

    public override double GetEdgePheromones(int currentVertex, int potentialVertex) //special case with implicit construction graph
    {
        return EdgePheromones[currentVertex];
    }
}