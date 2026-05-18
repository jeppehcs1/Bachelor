using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using Bachelor.Models.Scheduling;
using Bachelor.Models.Utility;
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

    private int GetNewBatchRunsAsInt() => int.TryParse(_newBatchRuns, out var result) ? result : 0;


    private void AddItem(string name,  int runs, int dimension)
    {
        Batch batch = new Batch(AlgorithmFactory.Create(Schedule), runs, name);
        Items.Add(new BatchItem(name, runs, dimension, batch));
    }

    public void RemoveItem(BatchItem item)
    {
        Items.Remove(item);
    }
    [RelayCommand]
    private void AddBatchOnClick() => AddItem(_newBatchName, GetNewBatchRunsAsInt(), Dimension);
    [RelayCommand]
    private void CopyBatchOnClick() => AddItem(_selectedItem.Name, _selectedItem.Runs, _selectedItem.Dimension);

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
        await RunBatches();
        IsRunning =  false;
    }

    private Task RunBatches()
    {
        var options = new ParallelOptions 
        { 
            MaxDegreeOfParallelism = Environment.ProcessorCount 
        };
    
        return Task.Run(() =>
        {
            Parallel.ForEach(_items, options, batchItem =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() => batchItem.Status = Status.Running);
                batchItem.Batch.Run();
                Avalonia.Threading.Dispatcher.UIThread.Post(() => batchItem.Status = Status.Completed);
            });
        });
    }
}

public partial class BatchItem(string name, int runs, int dimension, Batch batch) : ObservableObject
{
    public string Name { get; set; } = name;
    public int Runs { get; set; } = runs;
    public int Dimension { get; set; } = dimension;
    
    [ObservableProperty]
    private Status _status;
    
    public Batch Batch = batch;
}

