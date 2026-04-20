
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm;
namespace Bachelor.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bachelor.Models;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;

public partial class TSPViewModel : ViewModelBase
{
    public Algorithm<TSPInstance> Algorithm;
    private ObservableCollection<DataPoint> _points = new();

    public ObservableCollection<DataPoint> Points
    {
        get => _points; 
        set => SetProperty(ref _points, value);
    }

    public TSPViewModel()
    {
        
    }
    public TSPViewModel(Algorithm<TSPInstance> algorithm)
    {
        Algorithm = algorithm;
        Algorithm.SearchPoint = new TSPInstance([0,1,2,3,4,5],
            [(2,4),(1,4),(4,2),(3,1),(7,7),(8,2)]);
        Algorithm.Initialize();
        GenerateTSPPoints();
    }
    [RelayCommand] private void IterateTSPOnClick() { if( Algorithm.Iterate() == 1){ GenerateTSPPoints();} }
    public void GenerateTSPPoints()
    {
        var newPoints = new ObservableCollection<DataPoint>();
        for (int i = 0; i < Algorithm.SearchPoint.Permutation.Count; i++)                                                               
        {                                                                                         
            int j = Algorithm.SearchPoint.Permutation[i];                                         
            var (x, y) = Algorithm.SearchPoint.Graph[j];                                        
            newPoints.Add(new DataPoint{  x = x, y = y });                                           
        }                                                                                  
        var (xl, yl) = Algorithm.SearchPoint.Graph[Algorithm.SearchPoint.Permutation[0]];       
        newPoints.Add(new DataPoint { x = xl, y = yl });
        Points = new ObservableCollection<DataPoint>(newPoints);
    }
    
    
    
    
}