using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;

namespace UnitTests;

public class ProblemTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void TSPFitnessTest()
    {
        PermutationProblem problem = new TSPProblem(6);
        TSPInstance instance = new TSPInstance([0,1,2,3,4,5],
            [(0,0),(0,3),(0,6),(3,6),(3,3),(3,0)]);
        int result = problem.Fitness(instance);
        
        Assert.That(result, Is.EqualTo(18));
    }
    
    [Test]
    public void MutateTSPTest()
    {
        PermutationProblem problem = new TSPProblem(6);
        TSPInstance instance = new TSPInstance([0,1,2,3,4,5],
            [(0,0),(0,3),(0,6),(3,6),(3,3),(3,0)]);
    }
}