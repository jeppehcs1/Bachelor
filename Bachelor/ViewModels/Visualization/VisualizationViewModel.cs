using Bachelor.Models.Algorithms;
using Bachelor.Models.Utility;

namespace Bachelor.ViewModels.Visualization;

public abstract class VisualizationViewModel : ViewModelBase
{
    public string Name { get; }

    protected VisualizationViewModel(string name)
    {
        Name = name;
    }

    public abstract void Update(AlgorithmSnapshot snapshot);
}