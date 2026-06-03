using System.Collections.ObjectModel;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Utility;
using CommunityToolkit.Mvvm.Input;

namespace Bachelor.ViewModels.Visualization;

public partial class TSPViewModel : VisualizationViewModel
{
    //public Algorithm<TSPInstance> Algorithm;
    private ObservableCollection<DataPoint> _points = new();

    public ObservableCollection<DataPoint> Points
    {
        get => _points; 
        set => SetProperty(ref _points, value);
    }

    
    public TSPViewModel(string name) : base(name)
    {
        
    }
    //[RelayCommand] private void IterateTSPOnClick() { if( algorithm.Iterate()){ GenerateTSPPoints();} }
    public void GenerateTSPPoints(AlgorithmSnapshot snapshot)
    {
        var newPoints = new ObservableCollection<DataPoint>();
        for (int i = 0; i < snapshot.TSPSearchPoint.Permutation.Count; i++)                                                               
        {                                                                                         
            int j = snapshot.TSPSearchPoint.Permutation[i];                                         
            var (x, y) = snapshot.TSPSearchPoint.Graph[j];                                        
            newPoints.Add(new DataPoint{  x = x, y = y });                                           
        }                                                                                  
        var (xl, yl) = snapshot.TSPSearchPoint.Graph[snapshot.TSPSearchPoint.Permutation[0]];       
        newPoints.Add(new DataPoint { x = xl, y = yl });
        Points = new ObservableCollection<DataPoint>(newPoints);
    }


    public override void Update(AlgorithmSnapshot snapshot)
    {
        GenerateTSPPoints(snapshot);
    }

    public override void Initialize()
    {
        Points.Clear();
    }
}