using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Bachelor.Models;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.ViewModels.Visualization;
using Bachelor.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace Bachelor.ViewModels;

using ScottPlot;
using ScottPlot.Avalonia;

// author Jeppe and Clement

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private object currentView;
    private readonly PlotViewModel _plotViewModel;
    private readonly HypercubeViewModel _hypercubeViewModel;
    private readonly TSPViewModel _tspViewModel;
    private readonly CreateScheduleViewModel _createScheduleViewModel;
    private readonly AddBatchesViewModel _addBatchesViewModel;
    private readonly VisualizationHostViewModel _visualizationHostViewModel;
    public LogViewModel LogViewModel { get; } = new();
    public MainWindowViewModel(Window parentWindow)
    {
        // Create ViewModels once
        _plotViewModel = new PlotViewModel();
        _hypercubeViewModel = new HypercubeViewModel();
        _tspViewModel = new TSPViewModel();
        _visualizationHostViewModel = new VisualizationHostViewModel();
        _addBatchesViewModel = new AddBatchesViewModel(_visualizationHostViewModel, this);
        _createScheduleViewModel = new CreateScheduleViewModel(this, _addBatchesViewModel, parentWindow);
        
        // Default view
        CurrentView = new HomeView();
    }
    [RelayCommand]
    private void ShowHome() => CurrentView = new HomeView(); // or create HomeViewModel
    [RelayCommand]
    private void ShowCreateSchedule() => CurrentView = _createScheduleViewModel;
    [RelayCommand]
    private void ShowVisualization() => CurrentView = _visualizationHostViewModel;
    [RelayCommand]
    private void ShowLog() => CurrentView = LogViewModel;
    
    
    
    
    
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    

    
    
    
}