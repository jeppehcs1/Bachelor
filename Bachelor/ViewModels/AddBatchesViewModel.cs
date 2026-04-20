using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using Bachelor.Models.Scheduling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bachelor.ViewModels;

public partial class AddBatchesViewModel : ViewModelBase
{
    private ObservableCollection<BatchItem> _items;
    private BatchItem? _selectedItem;
    private string _newBatchName;
    private string _newBatchRuns;
    
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

    public int GetNewBatchRunsAsInt() => int.TryParse(_newBatchRuns, out var result) ? result : 0;
    public AddBatchesViewModel()
    {
        _items = new ObservableCollection<BatchItem>();
    }

    public void AddItem(string name,  int runs)
    {
        Items.Add(new BatchItem(name, runs));
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

    
}

public class BatchItem(string name, int runs)
{
    public string Name { get; set; } = name;
    public int Runs { get; set; } = runs;
}

