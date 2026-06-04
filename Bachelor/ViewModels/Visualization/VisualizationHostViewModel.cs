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
    [ObservableProperty] private VisualizationViewModel? currentVisualization;
    private IAlgorithm? _algorithm;
    private AlgorithmRunner? _runner;
    [ObservableProperty] private int _updateInterval;
    [ObservableProperty] private int _iterationCounter;
    [ObservableProperty] private int _bSFFCounter;
    [ObservableProperty] private int _funcEvalCounter;
    public VisualizationHostViewModel()
    {
        _updateInterval = 1000;
    }
    

    public void Attach(IAlgorithm algorithm, AlgorithmRunner runner)
    {
        if (_runner != null)
        {
            _runner.OnIteration -= OnAlgorithmIteration;
            _runner.OnInitialization -= OnAlgorithmInitialization;
        }
        _runner = runner;
        _runner.UpdateInterval = UpdateInterval;
        _runner.OnIteration += OnAlgorithmIteration;
        _runner.OnInitialization += OnAlgorithmInitialization;
        _algorithm = algorithm;
    }
    private void OnAlgorithmIteration(IAlgorithm algoritm)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            AlgorithmSnapshot snapshot = _runner.TakeSnapshot(_algorithm);
            IterationCounter = snapshot.Iterations;
            BSFFCounter = snapshot.BSFF;
            FuncEvalCounter = snapshot.FuncEvals;
            CurrentVisualization?.Update(snapshot);
        });
    }
    partial void OnUpdateIntervalChanged(int value)
    {
        if (_runner != null)
            _runner.UpdateInterval = value;
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
    private void Play() => _runner?.Play();
    
}
