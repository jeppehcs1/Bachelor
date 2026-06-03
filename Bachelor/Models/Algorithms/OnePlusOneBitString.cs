using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;  


public class OnePlusOneBitString(ProblemType<BitArray> problem) : OnePlusOne<BitArray>(problem)
{

    public override BitArray CloneSearchPoint()
    {
        return SearchPoint.Clone() as BitArray;
    }
    public override bool UpdateSearchPoint(BitArray old)
    {
        int oldFitness = Problem.Fitness(old);
        if (BSFF < oldFitness)
        {
            BSFF = oldFitness;
            SearchPoint = old;
            return true;
        }
        return false;
    }

    
    public override void MutateSearchPoint()
    {
        //SearchPoint = ((BitStringProblem)Problem).MutateBitArray(SearchPoint, random);
        
        for (var i = 0; i < Problem.Dimension; i++)
        {
            if (_random.Next(Problem.Dimension) == 0) // 1/dim chance of being 0, i.e. flipping a bit
            {
                SearchPoint[i] = !SearchPoint[i];
            }
        }
        //return SearchPoint;
    }

    
    

    public override void InitializeCore()
    {
        var dim = Problem.Dimension;
        var bits = new bool[dim];
        var random = new Random();
        
        for (int i = 0; i < dim; i++)
        {
            //bits[i] = random.Next(2) == 1;  // Random true or false
            bits[i] = false;
        }

        BSFF = 0;
        SearchPoint = new BitArray(bits);
    }
}