using System;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;

namespace Bachelor.Views;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Bachelor.ViewModels;



public partial class TSPView : UserControl
{
    private int[] xs = Array.Empty<int>();
    private double[] ys = Array.Empty<double>();
    private TSPViewModel? _currentVm;
    public TSPView()
    {
        InitializeComponent();
        DataContext = new TSPViewModel(new SimulatedAnnealingPermutation(
            new TSPProblem(6),
            new TSPInstance([0, 5, 3, 4, 2, 1], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)])));
        
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