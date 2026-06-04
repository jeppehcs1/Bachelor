using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bachelor.Views;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Controls;
using Bachelor.Models.Problems;
using Bachelor.Models.Scheduling;
using Bachelor.Models.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace Bachelor.ViewModels;

public partial class CreateScheduleViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly Window _parentWindow;
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
    public bool IsFuncEvals => SelectedFinishCondition == "Function evaluations";
    public bool IsExactFitness => SelectedFinishCondition == "Exact fitness";
    public ObservableCollection<string> SearchSpaces { get; }
    private Dictionary<string, object> _algorithmConfig = new();
    
    public CreateScheduleViewModel(MainWindowViewModel mainViewModel, AddBatchesViewModel addBatchesViewModel, Window parentWindow)
    {
        _parentWindow = parentWindow;
        _mainViewModel = mainViewModel;
        _addBatchesViewModel = addBatchesViewModel;
        _algorithms = new ObservableCollection<string>();
        SearchSpaces = ["Bit Strings", "Permutations"];
        
        UpdateProblems();
        UpdateFinishConditions();
        UpdateVisualizations();
    }
    [ObservableProperty]
    private string _filePathError = "";
    [ObservableProperty]
    private string _maxFuncEvals = "1000000";
    [ObservableProperty]
    private string _exactFitness = "";
    public int GetMaxFuncEvalsAsInt() => int.TryParse(MaxFuncEvals, out var result) ? result : 1000000;
    public int GetExactFitnessAsInt() => int.TryParse(ExactFitness, out var result) ? result : 10;
    partial void OnMaxFuncEvalsChanged(string value) => OnPropertyChanged(nameof(CanProceed));
    partial void OnExactFitnessChanged(string value) => OnPropertyChanged(nameof(CanProceed));
    [RelayCommand]
    private void NextOnClick()
    {
        Schedule schedule;
        if (IsTSP)
        {
            try
            {
                ITSPFileReader reader = new Euclid2DTSPFileReader();
                TSPInstance instance = reader.Read(FilePath);
                schedule = new Schedule(SelectedSearchSpace, SelectedAlgorithm, SelectedProblem, 
                    SelectedFinishCondition, SelectedVisualization, instance, GetMaxFuncEvalsAsInt(), GetExactFitnessAsInt());
            }
            catch (FileNotFoundException e)
            {
                FilePathError = e.Message;
                return;
            }
            catch (FormatException e)
            {
                FilePathError = e.Message;
                return;
            }
            catch (Exception)
            {
                FilePathError = "Could not read file. Make sure it is a valid .tsp file.";
                return;
            }
        }
        else
        {
            schedule = new Schedule(SelectedSearchSpace, SelectedAlgorithm, SelectedProblem, 
                SelectedFinishCondition, SelectedVisualization, GetDimensionAsInt(), GetMaxFuncEvalsAsInt(), GetExactFitnessAsInt());
        }
        
        schedule.AlgorithmConfig = _algorithmConfig;
        _addBatchesViewModel.Schedule = schedule;
        _mainViewModel.CurrentView = _addBatchesViewModel;
    }
    [RelayCommand]
    private async Task ConfigureAlgorithmOnClick()
    {
        var dialog = new AlgorithmConfigView();
        var vm = new Configs.AlgorithmConfigViewModel(SelectedAlgorithm, SelectedSearchSpace, dialog);
        dialog.DataContext = vm;
        await dialog.ShowDialog(_parentWindow);

        if (vm.Confirmed)
            _algorithmConfig = vm.Config;
    }
    
    [ObservableProperty]
    private string _filePath = "";
    public string Dimension
    {
        get => _dimension;
        set
        {
            OnPropertyChanged(nameof(CanProceed));
            this.SetProperty(ref _dimension, value);
        }
    }
    partial void OnFilePathChanged(string value) => OnPropertyChanged(nameof(CanProceed));
    public int GetDimensionAsInt() => int.TryParse(_dimension, out var result) ? result : 500;
    [ObservableProperty]
    private bool _isPermutations;
    public string SelectedSearchSpace
    {
        get => _selectedSearchSpace;
        set
        {
            SetProperty(ref _selectedSearchSpace, value);
            IsPermutations = value == "Permutations";
            OnPropertyChanged(nameof(CanProceed));
            UpdateAlgorithms();
        }
    }
    public string SelectedAlgorithm
    {
        get => _selectedAlgorithm;
        set
        {
            SetProperty(ref _selectedAlgorithm, value);
            _algorithmConfig = new();
            OnPropertyChanged(nameof(CanProceed));
            UpdateProblems();
        }
    }
    [ObservableProperty]
    private bool _isTSP;
    public string SelectedProblem
    {
        get => _selectedProblem;
        set
        {
            SetProperty(ref _selectedProblem, value);
            IsTSP = value == "TSPProblem";
            OnPropertyChanged(nameof(CanProceed));
            UpdateFinishConditions();
            UpdateVisualizations();
            
        }
    }

    public string SelectedFinishCondition
    {
        get => _selectedFinishCondition;
        set
        {
            SetProperty(ref _selectedFinishCondition, value); 
            OnPropertyChanged(nameof(IsFuncEvals));
            OnPropertyChanged(nameof(IsExactFitness));
            OnPropertyChanged(nameof(CanProceed));
        }
        
    }
    public string SelectedVisualization
    {
        get => _selectedVisualization;
        set
        {
            SetProperty(ref _selectedVisualization, value);
            OnPropertyChanged(nameof(CanProceed));
        }
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
            "TSPProblem" => new ObservableCollection<string> { "Function evaluations", "Optimum reached", "Exact fitness"},
            "LeadingOnes" => new ObservableCollection<string> { "Function evaluations", "Optimum reached", "Exact fitness"},
            "OneMax" => new ObservableCollection<string> { "Function evaluations", "Optimum reached", "Exact fitness"},
            _ => new ObservableCollection<string>()
        };
        FinishConditions = finishList;
    }
    private void UpdateVisualizations()
    {
        var visualizationList = SelectedProblem switch
        {
            "TSPProblem" => new ObservableCollection<string> { "TSPPlot", "FitnessPlot", "None"},
            _ => new ObservableCollection<string> { "HyperCube", "FitnessPlot", "None"}
        };
        Visualizations = visualizationList;
    }
    public bool CanProceed => 
        !string.IsNullOrEmpty(SelectedSearchSpace) &&
        !string.IsNullOrEmpty(SelectedAlgorithm) &&
        !string.IsNullOrEmpty(SelectedProblem) &&
        !string.IsNullOrEmpty(SelectedFinishCondition) &&
        !string.IsNullOrEmpty(SelectedVisualization) &&
        (!IsFuncEvals || GetMaxFuncEvalsAsInt() > 0) &&
        (!IsExactFitness || GetExactFitnessAsInt() > 0) &&
        (IsTSP ? !string.IsNullOrEmpty(FilePath) : GetDimensionAsInt() > 0);

    
    
}