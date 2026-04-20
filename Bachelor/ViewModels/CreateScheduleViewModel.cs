using System.Collections.ObjectModel;
using Bachelor.Models.Algorithms;
using Bachelor.Views;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;


namespace Bachelor.ViewModels;

public partial class CreateScheduleViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private string _selectedSearchSpace = "";
    private string _selectedAlgorithm = "";
    private string _selectedProblem = "";
    private ObservableCollection<string> _algorithms;
    private ObservableCollection<string> _problems;
    public ObservableCollection<string> SearchSpaces { get; }
    
    public CreateScheduleViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _algorithms = new ObservableCollection<string>();
        SearchSpaces = new ObservableCollection<string> { "Bit Strings", "Permutations" };
    }
    
    [RelayCommand]
    private void NextOnClick() => _mainViewModel.CurrentView = new AddBatchesViewModel();
    
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
        set => SetProperty(ref _selectedProblem, value);
            
        
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
    private void UpdateAlgorithms()
    {
        var algorithmList = SelectedSearchSpace switch
        {
            "Bit Strings" => new ObservableCollection<string> { "1+1",  "SA", "MMAS" },
            "Permutations" => new ObservableCollection<string> { "1+1", "SA", "MMAS" },
            _ => new ObservableCollection<string>()
        };
        Algorithms = algorithmList;
    }

    private void UpdateProblems()
    {
        var problemList = SelectedSearchSpace switch
        {
            "Bit Strings" => new ObservableCollection<string> { "OneMax", "LeadingOnes"},
            "Permutations" => new ObservableCollection<string> { "TSP" },
            _ => new ObservableCollection<string>()
        };
        Problems = problemList;
    }
    
}