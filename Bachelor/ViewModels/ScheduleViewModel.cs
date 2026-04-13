using System.Collections.ObjectModel;


using CommunityToolkit.Mvvm;
namespace Bachelor.ViewModels;

public class ScheduleViewModel : ViewModelBase
{
    private string _selectedSearchSpace;
    private string _selectedAlgorithm;
    private string _selectedProblem;
    private ObservableCollection<string> _algorithms;
    private ObservableCollection<string> _problems;
    
    
    public ScheduleViewModel()
    {
        _algorithms = new ObservableCollection<string>();
        SearchSpaces = new ObservableCollection<string> { "Bit Strings", "Permutations" };
    }
    
    public ObservableCollection<string> SearchSpaces { get; }
    
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