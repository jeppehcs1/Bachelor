using System;
using System.Collections;
using System.Collections.ObjectModel;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Utility;

namespace Bachelor.ViewModels.Visualization;

public class DataPoint : IEnumerable
{
        public double x { get; set; }
        public double y  { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
}
public class PlotViewModel : VisualizationViewModel
{
    public ObservableCollection<DataPoint> Points = new ObservableCollection<DataPoint>();

    
    public PlotViewModel(string name) : base(name)
    {
        
    }

    public override void Update(AlgorithmSnapshot snapshot)
    {
        
        Points.Add(new DataPoint{ x = snapshot.Iterations, y = snapshot.BSFF });
        
    }
}