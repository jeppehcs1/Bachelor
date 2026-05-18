using Bachelor.Models.Problems;

namespace Bachelor.Models.Utility;

public interface ITSPFileReader
{
    public TSPInstance Read(string filename);
}