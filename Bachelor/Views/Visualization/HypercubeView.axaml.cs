using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Bachelor.ViewModels.Visualization;

namespace Bachelor.Views.Visualization;
// author Clement
public partial class HypercubeView : UserControl
{
    private double[] xs = Array.Empty<double>();
    private double[] ys = Array.Empty<double>();
    public HypercubeView()
    {
        InitializeComponent();
        PlotControl.Plot.Axes.SetLimits(-1, 1, -0, 1);
        PlotControl.UserInputProcessor.IsEnabled = false;
        PlotControl.Plot.Grid.IsVisible = false;
        PlotControl.Refresh();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (DataContext is HypercubeViewModel vm)
            RenderPlot(vm);
    }
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is HypercubeViewModel vm)
        {
            vm.Points.CollectionChanged += (_, _) => RenderPlot(vm);
            RenderPlot(vm);
        }
    }

    private void RenderPlot(HypercubeViewModel vm)
    {
        if (xs.Length != vm.Points.Count)
        {
            xs = new double[vm.Points.Count];
        }
        for (int i = 0; i < vm.Points.Count; i++)
        {
            xs[i] = vm.Points[i].x;
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
        double[] boundY = Enumerable.Range(0, 300).Select(i => i / 299.0).ToArray();
        double[] rightX = boundY.Select(y => Math.Sin(Math.PI * y)).ToArray();
        double[] leftX  = boundY.Select(y => -Math.Sin(Math.PI * y)).ToArray();
        var r = PlotControl.Plot.Add.Scatter(rightX, boundY);
        var l = PlotControl.Plot.Add.Scatter(leftX, boundY);
        r.MarkerSize = 0;
        l.MarkerSize = 0;
        
        PlotControl.Refresh();
        
    }
}