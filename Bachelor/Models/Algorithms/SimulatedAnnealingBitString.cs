namespace Bachelor.Models.Algorithms;

using Bachelor.Models.Problems;

using System;
using System.Collections;
using System.Collections.Generic;  

public class SimulatedAnnealingBitString(ProblemType<BitArray> problem) : SimulatedAnnealing<BitArray>(problem)
{
    public override BitArray CloneSearchPoint()
    {
        return SearchPoint.Clone() as BitArray;
    }
    
    public override int UpdateSearchPoint(BitArray old)
    {
        _temperature = _temperature * _alpha;
        
        if (Problem.Fitness(SearchPoint) > Problem.Fitness(old))
        {
            return 0;
        }
        
        var delta = Problem.Fitness(SearchPoint) - Problem.Fitness(old);
        var prob = Math.Exp(delta/_temperature);
        
        if (_random.NextDouble() < prob)
        {
            return 0;
        }
        
        SearchPoint = old;
        return 1;
    }

    public override void MutateSearchPoint()
    {
        var index = _random.Next(0, SearchPoint.Length);
        SearchPoint[index] = !SearchPoint[index];
    }

    public override int GetFitness()
    {
        return Problem.Fitness(SearchPoint);
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