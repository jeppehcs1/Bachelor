using System;
using Avalonia.Controls;
using Bachelor.ViewModels;

namespace Bachelor.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is MainWindowViewModel _vm)
        {
            int[] xs = new int[_vm.Points.Count];
            for (int i = 0; i < _vm.Points.Count; i++)
            {
                xs[i] = i;
            }
            int[] ys = new int[_vm.Points.Count];
            for (int i = 0; i < _vm.Points.Count; i++)
            {
                ys[i] = (int)_vm.Points[i].y;
            }
            
            PlotControl.Plot.Clear();
            PlotControl.Plot.Add.Scatter(xs, ys);
            PlotControl.Refresh();
        }
    }
}