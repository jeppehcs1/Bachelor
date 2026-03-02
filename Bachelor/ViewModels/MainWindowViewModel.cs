using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bachelor.Models;

namespace Bachelor.ViewModels;

using ScottPlot;
using ScottPlot.Avalonia;

public class DataPoint
{
    public double x { get; set; }
    public double y  { get; set; }
}

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<DataPoint> Points { get; }
    public List<int> Iters { get; }
    
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    public Ea11Naive Ea11 = new Ea11Naive();

    public MainWindowViewModel()
    {
        Iters = Ea11.Run("");
        Points = new ObservableCollection<DataPoint>();

        for (int i = 0; i < Iters.Count; i++)
        {
            Points.Add(new DataPoint{ x = i, y = Iters[i] });
        }
    }
    
    
    
}