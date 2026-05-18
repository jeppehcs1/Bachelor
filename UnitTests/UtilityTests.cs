using Bachelor.Models.Problems;
using Bachelor.Models.Utility;

namespace UnitTests;

public class UtilityTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Euclid2DTSPFileReaderTest()
    {
        // first five lines of berlin tsp tour, followed by last five lines
        var confirmationGraph = new List<(int x, int y)>
        {
            (565, 575),
            (25,  185),
            (345, 750),
            (945, 685),
            (845, 655),
            
            (830, 610),
            (605, 625),
            (595, 360),
            (1340, 725),
            (1740, 245)
        };
        ITSPFileReader reader = new Euclid2DTSPFileReader();
        var tsp = reader.Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "berlin52.tsp"));
        // Check that the proper number of points have been loaded
        Assert.That(tsp.Graph.Count, Is.EqualTo(52));
        Assert.That(tsp.Permutation.Count, Is.EqualTo(52));
        // Check that the first five nodes match
        for (int i = 0; i < 5; i++)
        {
            Assert.That(tsp.Graph[i], Is.EqualTo(confirmationGraph[i]));
        }
        // Check that the last five nodes match
        for (int i = 5; i < 10; i++)
        {
            Assert.That(tsp.Graph[i+42], Is.EqualTo(confirmationGraph[i]));
        }
        //
    }
}