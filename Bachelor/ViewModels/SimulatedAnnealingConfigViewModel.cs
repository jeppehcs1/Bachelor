using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bachelor.ViewModels;

public partial class SimulatedAnnealingConfigViewModel : ViewModelBase
{
    [ObservableProperty] private double _alpha;
    [ObservableProperty] private double _temperature;

    public Dictionary<string, object> ToDictionary() => new()
    {
        { "Alpha", Alpha },
        { "Temperature", Temperature },
    };
}