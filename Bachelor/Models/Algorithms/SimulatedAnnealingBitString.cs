namespace Bachelor.Models.Algorithms;

using Bachelor.Models.Problems;

using System;
using System.Collections;
using System.Collections.Generic;  

public class SimulatedAnnealingBitString(IProblemType<BitArray> problem) : SimulatedAnnealing<BitArray>(problem)
{
    private double alpha = 1 - 1/((double) (problem.Dimension) * 10);
    private double temperature = 0.8;
    private Random random = new Random();
    
    public override BitArray CloneSearchPoint()
    {
        return SearchPoint.Clone() as BitArray;
    }
    
    public override bool UpdateSearchPoint(BitArray old)
    {
        temperature = temperature * alpha;
        
        if (Problem.Fitness(SearchPoint) > Problem.Fitness(old))
        {
            return false;
        }
        
        var delta = Problem.Fitness(SearchPoint) - Problem.Fitness(old);
        var prob = Math.Exp(delta/temperature);
        
        if (random.NextDouble() < prob)
        {
            return false;
        }
        
        SearchPoint = old;
        return true;
    }

    public override void MutateSearchPoint()
    {
        var index = random.Next(0, SearchPoint.Length);
        SearchPoint[index] = !SearchPoint[index];
    }

    
    public override void Initialize() 
    {
        var dim = Problem.Dimension;
        var bits = new bool[dim];
        
        for (int i = 0; i < dim; i++)
        {
            //bits[i] = random.Next(2) == 1;  // Random true or false
            bits[i] = false;
        }
        SearchPoint = new BitArray(bits);
    }
    
}