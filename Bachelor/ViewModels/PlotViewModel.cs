namespace Bachelor.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bachelor.Models;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;

public class DataPoint : IEnumerable
{
        public double x { get; set; }
        public double y  { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
}
public class PlotViewModel : ViewModelBase
{
    public Algorithm<BitArray> Algorithm = null;
    public ObservableCollection<DataPoint> Points { get; }

    public PlotViewModel(Algorithm<BitArray> algorithm)
    {
        Algorithm = algorithm;
        Algorithm.Initialize();
        
        Points = new ObservableCollection<DataPoint>();
        Points.Add(new DataPoint{ x = 0, y = Algorithm.GetFitness() });
        for (int i = 1; i < 10; i++)
        {
            Algorithm.Iterate();
            Points.Add(new DataPoint{ x = i, y = Algorithm.GetFitness() });
        }
    }
}