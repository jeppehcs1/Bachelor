using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bachelor.Models;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
namespace Bachelor.ViewModels;

using ScottPlot;
using ScottPlot.Avalonia;



public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private object currentView;

    public MainWindowViewModel()
    {
        // Default view
        CurrentView = new HomeView();
    }
    [RelayCommand]
    private void ShowPlot() => CurrentView = new PlotView();
    [RelayCommand]
    private void ShowHome() => CurrentView = new HomeView();
    [RelayCommand]
    private void ShowCube() => CurrentView = new HypercubeView();
    
    //public ObservableCollection<DataPoint> Points { get; }
    
    
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    

    
    
    
}