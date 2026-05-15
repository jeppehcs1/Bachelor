using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bachelor.Models.Algorithms;
using Bachelor.Views;
using CommunityToolkit.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Bachelor.Models.Problems;
using Bachelor.Models.Scheduling;
using CommunityToolkit.Mvvm.Input;


namespace Bachelor.ViewModels;

public partial class CreateScheduleViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly AddBatchesViewModel _addBatchesViewModel;
    private string _selectedSearchSpace = "";
    private string _selectedAlgorithm = "";
    private string _selectedProblem = "";
    private string _selectedFinishCondition = "";
    private string _selectedVisualization = "";
    private string _dimension = "";
    private ObservableCollection<string> _algorithms;
    private ObservableCollection<string> _problems;
    private ObservableCollection<string> _finishConditions;
    private ObservableCollection<string> _visualizations;
    public ObservableCollection<string> SearchSpaces { get; }
    
    public CreateScheduleViewModel(MainWindowViewModel mainViewModel, AddBatchesViewModel addBatchesViewModel)
    {
        _mainViewModel = mainViewModel;
        _addBatchesViewModel = addBatchesViewModel;
        _algorithms = new ObservableCollection<string>();
        SearchSpaces = ["Bit Strings", "Permutations"];
        UpdateAlgorithms();
        UpdateProblems();
        UpdateFinishConditions();
        UpdateVisualizations();
    }

    [RelayCommand]
    private void NextOnClick()
    {
        Schedule schedule = new Schedule(_selectedSearchSpace, _selectedAlgorithm, _selectedProblem, _selectedFinishCondition, _selectedVisualization, GetDimensionAsInt());
        _addBatchesViewModel.Schedule = schedule;
        _mainViewModel.CurrentView = _addBatchesViewModel;  
    } 
    public string Dimension
    {
        get => _dimension;
        set => this.SetProperty(ref _dimension, value);
    }
    
    public int GetDimensionAsInt() => int.TryParse(_dimension, out var result) ? result : 0;
    public string SelectedSearchSpace
    {
        get => _selectedSearchSpace;
        set
        {
            SetProperty(ref _selectedSearchSpace, value);
            UpdateAlgorithms();
        }
    }
    public string SelectedAlgorithm
    {
        get => _selectedAlgorithm;
        set
        {
            SetProperty(ref _selectedAlgorithm, value);
            UpdateProblems();
        }
    }
    public string SelectedProblem
    {
        get => _selectedProblem;
        set
        {
            SetProperty(ref _selectedProblem, value);
            UpdateFinishConditions();
            UpdateVisualizations();
            
        }
    }

    public string SelectedFinishCondition
    {
        get => _selectedFinishCondition;
        set => SetProperty(ref _selectedFinishCondition, value);
    }
    public string SelectedVisualization
    {
        get => _selectedVisualization;
        set => SetProperty(ref _selectedVisualization, value);
    }
    public ObservableCollection<string> Algorithms
    {
        get => _algorithms;
        set => SetProperty(ref _algorithms, value);
    }
    public ObservableCollection<string> Problems
    {
        get => _problems;
        set => SetProperty(ref _problems, value);
    }
    public ObservableCollection<string> FinishConditions
    {
        get => _finishConditions;
        set => SetProperty(ref _finishConditions, value);
    }
    public ObservableCollection<string> Visualizations
    {
        get => _visualizations;
        set => SetProperty(ref _visualizations, value);
    }
    private void UpdateAlgorithms()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var suffix = SelectedSearchSpace switch
        {
            "Bit Strings" => "BitString",
            "Permutations" => "Permutation",
            _ => string.Empty
        };

        var algorithmList = assembly.GetTypes()
            .Where(t => t.Namespace == "Bachelor.Models.Algorithms"
                        && !t.IsAbstract
                        && t.Name.EndsWith(suffix))
            .Select(t => t.Name.EndsWith(suffix) ? t.Name[..^suffix.Length] : t.Name)
            .ToList();
        Algorithms = new ObservableCollection<string>(algorithmList);
    }
    
    private void UpdateProblems()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var problemTypes = SelectedSearchSpace switch
        {
            "Bit Strings" => assembly.GetTypes()
                .Where(t => t.Namespace == "Bachelor.Models.Problems"
                            && typeof(BitStringProblem).IsAssignableFrom(t)
                            && !t.IsAbstract)
                .Select(t => t.Name)
                .ToList(),
            "Permutations" => assembly.GetTypes()
                .Where(t => t.Namespace == "Bachelor.Models.Problems"
                            && typeof(PermutationProblem).IsAssignableFrom(t)
                            && !t.IsAbstract)
                .Select(t => t.Name)
                .ToList(),
            _ => new List<string>()
        };
        Problems = new ObservableCollection<string>(problemTypes);
    }
    private void UpdateFinishConditions()
    {
        var finishList = SelectedProblem switch
        {
            "Bo" => new ObservableCollection<string> { "Onefkx", "Lefewnes"},
            _ => new ObservableCollection<string> { "Function evaluations", "Optimum Reached"}
        };
        FinishConditions = finishList;
    }
    private void UpdateVisualizations()
    {
        var visualizationList = SelectedProblem switch
        {
            "Bo" => new ObservableCollection<string> { "Onefkx", "Lefewnes"},
            _ => new ObservableCollection<string> { "No visualization", "hypercube"}
        };
        Visualizations = visualizationList;
    }
    
}