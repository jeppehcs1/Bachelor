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

    public double XCoordinate(BitArray bits)
    {
        var (left, right) = BitDistribution(bits);
        
        var xNonAdjusted = (right - left) / (double)(right + left);

        var difference = Math.Abs(left - right) * 2;
        var adjustment = difference / (double)bits.Length;
        
        return xNonAdjusted * adjustment;
    }
    
    public double YCoordinate(BitArray bits)
    {
        var (left, right) = BitDistribution(bits);
        var mid = bits.Length % 2 == 0 ? 0 : (bits[bits.Length / 2] ? 1 : 0);
        
        return (left + mid + right) / (double)(bits.Length);
    }
    
    public HypercubeViewModel(Algorithm<BitArray> algorithm)
    {                                                                      
        Algorithm = algorithm;                                             
        Algorithm.Initialize();
                                                                       
        Points = new ObservableCollection<DataPoint>();                    
        
        for (int i = 1; i < 10000; i++)                                       
        {    
            Algorithm.Iterate();
            //Console.WriteLine("x: " + XCoordinate(Algorithm.SearchPoint) + "  and y: " + YCoordinate(Algorithm.SearchPoint));
            
            Points.Add(new DataPoint{ x = XCoordinate(Algorithm.SearchPoint), y = YCoordinate(Algorithm.SearchPoint) });
        }
                                  
    }      
    
    
}