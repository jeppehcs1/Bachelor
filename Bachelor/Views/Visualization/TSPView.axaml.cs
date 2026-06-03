using System;
using Avalonia.Controls;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.ViewModels.Visualization;

namespace Bachelor.Views.Visualization;

public partial class TSPView : UserControl
{
    private int[] xs = Array.Empty<int>();
    private double[] ys = Array.Empty<double>();
    private TSPViewModel? _currentVm;
    public TSPView()
    {
        InitializeComponent();
        PlotControl.UserInputProcessor.IsEnabled = false;
        PlotControl.Plot.Grid.IsVisible = false;
    }
    
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (_currentVm != null)
        {
            _currentVm.PropertyChanged -= OnViewModelPropertyChanged;
        }

        if (DataContext is TSPViewModel vm)
        {
            _currentVm = vm;
            _currentVm.PropertyChanged += OnViewModelPropertyChanged;
            UpdatePlot(vm);
        }
        
    }
    
    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TSPViewModel.Points) && _currentVm != null)
        {
            UpdatePlot(_currentVm);
        }
        
    }
    private void UpdatePlot(TSPViewModel vm)
    {
        if (xs.Length != vm.Points.Count)
        {
            xs = new int[vm.Points.Count];
        }
        for (int i = 0; i < vm.Points.Count; i++)
        {
            xs[i] = (int) vm.Points[i].x;
        }
        if (ys.Length != vm.Points.Count)
        {
            ys = new double[vm.Points.Count];
        }
        for (int i = 0; i < vm.Points.Count; i++)
        {
            ys[i] = vm.Points[i].y;
        }
            
        PlotControl.Plot.Clear();
        PlotControl.Plot.Add.Scatter(xs, ys);
        PlotControl.Refresh();
    }
}