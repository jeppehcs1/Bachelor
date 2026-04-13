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
    public void MutateTSP_2optTest()
    {
        TSPProblem problem = new TSPProblem(6);
        TSPInstance instance = new TSPInstance([0,1,2,3,4,5],
            [(0,0),(0,3),(0,6),(3,6),(3,3),(3,0)]);
        
        var random = new Random(100); // random.Next with this seed gives 5 and 0 in that order This means it reverses the list from index 1 to 5
        
        TSPInstance newTSP = problem.MutateTSP_2opt(instance, random);
        
        
        Assert.That(newTSP.Permutation, Is.EqualTo([0,5,4,3,2,1]));
    }

    [Test]
    public void MutateTSP_3optTest()
    {
        TSPProblem problem = new TSPProblem(6);
        TSPInstance instance = new TSPInstance([0,1,2,5,3,4],
            [(0,0),(0,3),(0,6),(3,6),(3,3),(3,0)]);
        
        var random = new Random(100);
        
        TSPInstance newTSP = problem.MutateTSP_3opt(instance, random);
        
        Assert.That(newTSP.Permutation, Is.EqualTo([4,3,2,1,0,5]));
    }

    [Test]
    public void ThreeOptTest()
    {
        PermutationProblem problem = new TSPProblem(6);
        TSPInstance instance = new TSPInstance([0,1,2,3,4,5],
            [(0,0),(0,3),(0,6),(3,6),(3,3),(3,0)]);
        
    }
}