using Bachelor.Models.Algorithms;
using Bachelor.Models.Utility;

namespace Bachelor.ViewModels.Visualization;

public abstract class VisualizationViewModel : ViewModelBase
{
    

    protected VisualizationViewModel()
    {
       
    }

    public abstract void Update(AlgorithmSnapshot snapshot);
    public abstract void Initialize();
}