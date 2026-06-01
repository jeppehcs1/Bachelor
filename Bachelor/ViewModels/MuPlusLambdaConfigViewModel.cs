using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bachelor.ViewModels;

public partial class MuPlusLambdaConfigViewModel : ViewModelBase
{
    [ObservableProperty] private int _mu = 5;
    [ObservableProperty] private int _lambda = 20;

    public Dictionary<string, object> ToDictionary() => new()
    {
        { "Mu", Mu },
        { "Lambda", Lambda },
    };
}