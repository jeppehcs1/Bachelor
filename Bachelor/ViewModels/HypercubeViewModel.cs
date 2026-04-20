using System;
using System.Collections;
using System.Collections.ObjectModel;
using Bachelor.Models.Algorithms;

namespace Bachelor.ViewModels;

public class HypercubeViewModel
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
        
        return (right - left) / (double)(right + left);
    }

    public HypercubeViewModel()
    {
        Points = new ObservableCollection<DataPoint>();
    }
    public HypercubeViewModel(Algorithm<BitArray> algorithm)                    
    {                                                                      
        Algorithm = algorithm;                                             
        Algorithm.Initialize();                                            
                                                                       
        Points = new ObservableCollection<DataPoint>();                    
        Points.Add(new DataPoint{ x = XCoordinate(Algorithm.SearchPoint), y = (double)Algorithm.GetFitness()/(double)Algorithm.Problem.Dimension });    
        for (int i = 1; i < 50; i++)                                       
        {                                                                  
            Algorithm.Iterate();
            Console.WriteLine(XCoordinate(Algorithm.SearchPoint));   
            Points.Add(new DataPoint{ x = XCoordinate(Algorithm.SearchPoint), y = (double)Algorithm.GetFitness()/(double)Algorithm.Problem.Dimension });
        }                                                                  
                                        
    }      
    
    
}