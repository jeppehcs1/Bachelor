using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bachelor.ViewModels;

public partial class MMASConfigViewModel : ViewModelBase
{
    [ObservableProperty] private double _rho = 1.0;
    [ObservableProperty] private double _alpha;
    [ObservableProperty] private double _beta;
    [ObservableProperty] private int _ants;

    public Dictionary<string, object> ToDictionary() => new()
    {
        { "Rho", Rho },
        { "Alpha", Alpha },
        { "Beta", Beta },
        { "Ants", Ants },
    };
}