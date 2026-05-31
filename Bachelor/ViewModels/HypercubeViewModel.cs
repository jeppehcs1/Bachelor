using System;
using System.Collections;
using System.Collections.ObjectModel;
using Bachelor.Models.Algorithms;
using SkiaSharp;

namespace Bachelor.ViewModels;

public class HypercubeViewModel : ViewModelBase
{
    public Algorithm<BitArray> Algorithm = null;
    public ObservableCollection<DataPoint> Points { get; }
    
    public (int, int) BitDistribution(BitArray bits) // Returns number of 1-bit left and right of center, respectively
    {
        int mid = bits.Length / 2;
        int leftCount = 0;
        int rightCount = 0;

        // left side
        for (int i = 0; i < mid; i++)
            if (bits[i]) leftCount++;

        // right side
        for (int i = mid; i < bits.Length; i++)
            if (bits[i]) rightCount++;

        return (leftCount, rightCount);
    }

    /*public double XCoordinate(BitArray bits)
    {
        var (left, right) = BitDistribution(bits);
        
        double xNonAdjusted = 0;
        for (int i = 0; i < bits.Length; i++)
        {
            if (bits[i]) xNonAdjusted += i - bits.Length/(double)2;
        }
        double max = ((bits.Length /(double) 2) * (bits.Length /(double) 2) + (bits.Length /(double) 2)) / 2;
        xNonAdjusted /= max;
        
        var difference = Math.Abs(left - right) * 2;
        var adjustment = difference / (double)bits.Length;
        
        return xNonAdjusted * adjustment;
    }*/
    
    public double XCoordinate(BitArray bits)
    {
        int onemax = 0, sumOfIndices = 0;
        for (int i = 0; i < bits.Length; i++)
            if (bits[i]) { onemax++; sumOfIndices += i; }

        int minSum = (onemax * (onemax - 1)) / 2;
        int maxSum = (bits.Length * (bits.Length - 1)) / 2
                     - ((bits.Length - 1 - onemax) * (bits.Length - onemax)) / 2;
        int range = maxSum - minSum;

        double xNormalized = (range == 0) ? 0 : (double)(2 * sumOfIndices - 2 * minSum - range) / range;
        double y = onemax / (double)bits.Length;
        
        return xNormalized * Math.Sin(Math.PI * y);
    }
    
    public double YCoordinate(BitArray bits)
    {
        int onemax = 0;
        for (int i = 0; i < bits.Length; i++)
            if (bits[i]) onemax++;
        return onemax / (double)bits.Length;
    }
    
    public HypercubeViewModel(Algorithm<BitArray> algorithm)
    {             
        
        Algorithm = algorithm;                                             
        Algorithm.Initialize();
        Points = new ObservableCollection<DataPoint>();                    
        
        for (int i = 1; i < 100; i++)                                       
        {    
            Algorithm.Iterate();
            var point = new DataPoint{ x = XCoordinate(Algorithm.SearchPoint), y = YCoordinate(Algorithm.SearchPoint) };
            Points.Add(point);
            Console.WriteLine($"i={i} x={point.x:F4} y={point.y:F4}");
        }
        
                                  
    }      
    
    
}