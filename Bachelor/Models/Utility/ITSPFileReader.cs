using Bachelor.Models.Problems;

namespace Bachelor.Models.Utility;
// author Jeppe
public interface ITSPFileReader
{
    public TSPInstance Read(string filename);
}