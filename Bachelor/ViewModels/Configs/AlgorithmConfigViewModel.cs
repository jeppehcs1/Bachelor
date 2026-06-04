using System.Collections.Generic;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bachelor.ViewModels.Configs;

public partial class AlgorithmConfigViewModel : ViewModelBase
{
    public ViewModelBase? ConfigViewModel { get; }
    public bool Confirmed { get; private set; }
    [ObservableProperty] private string _algorithmName;
    public Dictionary<string, object> Config { get; private set; } = new();

    private readonly Window _dialog;

    public AlgorithmConfigViewModel(string algorithmName, string searchSpace, Window dialog)
    {
        AlgorithmName = algorithmName;
        _dialog = dialog;
        ConfigViewModel = algorithmName switch
        {
            "MinMaxAntSystem" => new MMASConfigViewModel(),
            "SimulatedAnnealing" => new SimulatedAnnealingConfigViewModel(),
            "MuPlusLambda" => new MuPlusLambdaConfigViewModel(),
            _ => null
        };
    }

    [RelayCommand]
    private void Confirm()
    {
        Config = ConfigViewModel switch
        {
            MMASConfigViewModel vm => vm.ToDictionary(),
            SimulatedAnnealingConfigViewModel vm => vm.ToDictionary(),
            MuPlusLambdaConfigViewModel vm => vm.ToDictionary(),
            _ => new()
        };
        Confirmed = true;
        _dialog.Close();
    }

    [RelayCommand]
    private void Cancel()
    {
        Confirmed = false;
        _dialog.Close();
    }
}