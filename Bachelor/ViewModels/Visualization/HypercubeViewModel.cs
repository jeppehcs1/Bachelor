using System;
using System.Collections;
using System.Collections.ObjectModel;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Utility;

namespace Bachelor.ViewModels.Visualization;

public class HypercubeViewModel : VisualizationViewModel
{
    public ObservableCollection<DataPoint> Points = new ObservableCollection<DataPoint>();
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
    
    public HypercubeViewModel()
    {
        
                                  
    }
    public override void Update(AlgorithmSnapshot snapshot)
    {
        var point = new DataPoint{ x = XCoordinate(snapshot.BitStringSearchPoint), y = YCoordinate(snapshot.BitStringSearchPoint) };
        Points.Add(point);
    }

    public override void Initialize()
    {
        Points.Clear();
    }
}