using System;
using System.Collections;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bachelor.ViewModels.Visualization;

public partial class VisualizationHostViewModel : ViewModelBase
{
    [ObservableProperty]
    private VisualizationViewModel? currentVisualization;
    private IAlgorithm? _algorithm;
    private AlgorithmRunner? _runner;

    public VisualizationHostViewModel()
    {
        currentVisualization = new HypercubeViewModel("bo");
    }

    public int IterationCounter { get; set; }
    public int BSFFCounter { get; set; }


    public void Attach(IAlgorithm algorithm, AlgorithmRunner runner)
    {
        _runner = runner;
        _runner.OnIteration += OnAlgorithmIteration;
        _runner.OnInitialization += OnAlgorithmInitialization;
        _algorithm = algorithm;
    }
    private void OnAlgorithmIteration(IAlgorithm algoritm)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            CurrentVisualization?.Update(_runner.TakeSnapshot(_algorithm));
        });
    }
    private void OnAlgorithmInitialization()
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            CurrentVisualization?.Initialize();
        });
    }
    [RelayCommand]
    private void Pause() => _runner?.Pause();
    
    [RelayCommand]
    private void Resume() => _runner?.Resume();
    
    [RelayCommand]
    private void Restart() => _runner?.Restart();
}
