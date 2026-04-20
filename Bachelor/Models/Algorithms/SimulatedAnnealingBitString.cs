namespace Bachelor.Models.Algorithms;

using Bachelor.Models.Problems;

using System;
using System.Collections;
using System.Collections.Generic;  

public class SimulatedAnnealingBitString(ProblemType<BitArray> problem) : OnePlusOne<BitArray>(problem)
{
    
    public override BitArray CloneSearchPoint()
    {
        return SearchPoint.Clone() as BitArray;
    }
    
    public override int UpdateSearchPoint(BitArray old)
    {
        double temperature = 5; // Temperatur skal udregnes ud fra tid og cooling function
        
        if (Problem.Fitness(SearchPoint) > Problem.Fitness(old))
        {
            return 0;
        }
        
        var difference = Problem.Fitness(old) - Problem.Fitness(SearchPoint);
        var prob = Math.Exp(difference/temperature);
        
        var random = new Random();
        if (!(random.Next(101) > (int)(Math.Min(1, prob)*100)))
        {
            return 0;
        }
        
        SearchPoint = old;
        return 1;
    }

    public override void MutateSearchPoint(Random random)
    {
        // UUHhhh skal laves megeet om her
        SearchPoint = ((BitStringProblem)Problem).MutateBitArray(SearchPoint, random);
    }

    public override int GetFitness()
    {
        return Problem.Fitness(SearchPoint);
    }
    public override void Initialize() 
    {
        var dim = Problem.Dimension;
        var bits = new bool[dim];
        var random = new Random();
        
        for (int i = 0; i < dim; i++)
        {
            bits[i] = random.Next(2) == 1;  // Random true or false
        }
        SearchPoint = new BitArray(bits);
    }
    
}