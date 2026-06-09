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
    
    public override void UpdateSearchPoint(BitArray old)
    {
        _temperature = _temperature * _alpha;
        int oldFitness = CurrentFitness;
        int newFitness = GetFitness();
        if (newFitness > oldFitness)
        {
            BSFF = Math.Max(BSFF, newFitness);
            CurrentFitness = newFitness;
            return;
        }
        
        var delta = newFitness - oldFitness;
        var prob = Math.Exp(delta/_temperature);
        
        if (_random.NextDouble() < prob)
        {
            CurrentFitness = newFitness;
            return;
        }
        
        SearchPoint = old;
        CurrentFitness = oldFitness;
        
    }

    public override void MutateSearchPoint()
    {
        var index = _random.Next(0, SearchPoint.Length);
        SearchPoint[index] = !SearchPoint[index];
    }

    
    public override void InitializeCore() 
    {
        _temperature = _initialTemperature;
        var dim = Problem.Dimension;
        var bits = new bool[dim];
        
        for (int i = 0; i < dim; i++)
        {
            bits[i] = _random.Next(2) == 1;  // Random true or false
        }
        SearchPoint = new BitArray(bits);
        BSFF = GetFitness();
        CurrentFitness = BSFF;
    }
    
}