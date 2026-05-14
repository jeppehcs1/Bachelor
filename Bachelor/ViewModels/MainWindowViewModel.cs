using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bachelor.Models;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace Bachelor.ViewModels;

using ScottPlot;
using ScottPlot.Avalonia;



public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private object currentView;

    private readonly PlotViewModel _plotViewModel;
    private readonly HypercubeViewModel _hypercubeViewModel;
    private readonly TSPViewModel _tspViewModel;
    private readonly CreateScheduleViewModel _createScheduleViewModel;

    public MainWindowViewModel()
    {
        // Create ViewModels once
        _plotViewModel = new PlotViewModel();
        _hypercubeViewModel = new HypercubeViewModel(new OnePlusOneBitString(new OneMax(200)));
        _tspViewModel = new TSPViewModel();
        _createScheduleViewModel = new CreateScheduleViewModel(this);
        
        // Default view
        CurrentView = new HomeView();
    }
    [RelayCommand]
    private void ShowPlot() => CurrentView = _plotViewModel;
    [RelayCommand]
    private void ShowHome() => CurrentView = new HomeView(); // or create HomeViewModel
    [RelayCommand]
    private void ShowCube() => CurrentView = _hypercubeViewModel;
    [RelayCommand]
    private void ShowTSP() => CurrentView = _tspViewModel;
    [RelayCommand]
    private void ShowCreateSchedule() => CurrentView = _createScheduleViewModel;
    
    
    //public ObservableCollection<DataPoint> Points { get; }
    
    
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    

    
    
    
}