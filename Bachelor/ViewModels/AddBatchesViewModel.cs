using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Scheduling;
using Bachelor.Models.Utility;
using Bachelor.ViewModels.Visualization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bachelor.ViewModels;

public partial class AddBatchesViewModel : ViewModelBase
{
    public Schedule? Schedule { get; set; }
    private int Dimension;
    private ObservableCollection<BatchItem> _items = [];
    private BatchItem? _selectedItem;
    private string _newBatchName = "";
    private string _newBatchRuns = "";
    private readonly MainWindowViewModel _mainViewModel;
    private readonly VisualizationHostViewModel _visualizationHostViewModel;
    private int _visualizationAttached = 0;
    public AddBatchesViewModel(VisualizationHostViewModel visualizationHostViewModel, MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _visualizationHostViewModel = visualizationHostViewModel;
        
    }
    public ObservableCollection<BatchItem> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    
    public BatchItem? SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }
    public string NewBatchName
    {
        get => _newBatchName;
        set => this.SetProperty(ref _newBatchName, value);
    }
    public string NewBatchRuns
    {
        get => _newBatchRuns;
        set => this.SetProperty(ref _newBatchRuns, value);
    }

    private int GetNewBatchRunsAsInt() => int.TryParse(_newBatchRuns, out var result) ? result : 1;


    private void AddItem(string name,  int runs)
    {
        IAlgorithm algorithm = AlgorithmFactory.Create(Schedule);
        algorithm.StoppingCondition = Schedule.BuildStoppingCondition(algorithm);
        Batch batch = new Batch(algorithm , runs, name);
        Items.Add(new BatchItem(name, runs, batch));
    }

    public void RemoveItem(BatchItem item)
    {
        Items.Remove(item);
    }
    [RelayCommand]
    private void AddBatchOnClick() => AddItem(_newBatchName, GetNewBatchRunsAsInt());
    [RelayCommand]
    private void CopyBatchOnClick() => AddItem(_selectedItem.Name, _selectedItem.Runs);

    [RelayCommand]
    private void DeleteBatchOnClick()
    {
        BatchItem item = SelectedItem;
        if (item != null)
        {
            RemoveItem(item);
        }
    }
    [RelayCommand]
    private void MoveDownOnClick()
    {
        if (SelectedItem == null) return;
        int index = Items.IndexOf(SelectedItem);
        if (index < Items.Count - 1)
        {
            Items.Move(index, index + 1);
            SelectedItem = Items[index+1];
        }
    }
    [RelayCommand]
    private void MoveUpOnClick()
    {
        if (SelectedItem == null) return;
        int index = Items.IndexOf(SelectedItem);
        if (index > 0)
        {
            Items.Move(index, index - 1);
            SelectedItem = Items[index-1];
        }
    }

    [ObservableProperty] private bool _isRunning;
    [RelayCommand]
    private async Task FinishSetupOnClick()
    {
        IsRunning = true;
        VisualizationViewModel viewModel = Schedule.Visualization switch
        {
            "TSPPlot" => new TSPViewModel(""),
            "HyperCube" => new HypercubeViewModel(""),
            "FitnessPlot" => new PlotViewModel(""),
            _ => null
        };
        _visualizationHostViewModel.CurrentVisualization = viewModel;
        _mainViewModel.CurrentView = _visualizationHostViewModel;
        await RunBatches();
        IsRunning =  false;
    }

    private Task RunBatches()
    {
        var options = new ParallelOptions 
        { 
            MaxDegreeOfParallelism = Environment.ProcessorCount 
        };
        
        return Parallel.ForEachAsync(_items, options, async (batchItem, ct) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => batchItem.Status = Status.Running);
            if (Interlocked.CompareExchange(ref _visualizationAttached, 1, 0) == 0)
                _visualizationHostViewModel.Attach(batchItem.Batch.Algorithm, batchItem.Batch.Runner);
            await batchItem.Batch.RunAll();
            Avalonia.Threading.Dispatcher.UIThread.Post(() => batchItem.Status = Status.Completed);
        });
    }
}

public partial class BatchItem(string name, int runs, Batch batch) : ObservableObject
{
    public string Name { get; set; } = name;
    public int Runs { get; set; } = runs;
    
    [ObservableProperty]
    private Status _status;
    
    public Batch Batch = batch;
}

