using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.ViewModels;
using ScottPlot.Avalonia;

namespace Bachelor.Views;

public partial class PlotView : UserControl
{
    
    private int[] xs = Array.Empty<int>();
    private double[] ys = Array.Empty<double>();
    public PlotView()
    {
        InitializeComponent();
        DataContext = new PlotViewModel(new OnePlusOneBitString(new OneMax(5)));
    }
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is PlotViewModel vm)
        {
            if (xs.Length != vm.Points.Count)
            {
                xs = new int[vm.Points.Count];
            }
            for (int i = 0; i < vm.Points.Count; i++)
            {
                xs[i] = i;
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
}
