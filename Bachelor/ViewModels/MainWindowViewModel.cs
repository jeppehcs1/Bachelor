using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bachelor.Models;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using CommunityToolkit.Mvvm.Input;

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
    
    
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    public Algorithm<BitArray> One = new OnePlusOneBitString(new LeadingOnes(5));

    public MainWindowViewModel()
    {
        One.Initialize();
        
        Points = new ObservableCollection<DataPoint>();
        Points.Add(new DataPoint{ x = 0, y = One.GetFitness() });
        for (int i = 1; i < 10; i++)
        {
            One.Run();
            Points.Add(new DataPoint{ x = i, y = One.GetFitness() });
        }
        
    }
    [RelayCommand]
    private void Run()
    {
        Console.WriteLine("bo");
    }
    
}