using System.Collections;
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
    public void LeadingOnesFitnessTest_AllLeading()
    {
        LeadingOnes problem = new LeadingOnes(5);
        BitArray bits = new BitArray(new bool[] { true, true, true, true, true });
    
        Assert.That(problem.Fitness(bits), Is.EqualTo(5));
    }

    [Test]
    public void LeadingOnesFitnessTest_NoneLeading()
    {
        LeadingOnes problem = new LeadingOnes(5);
        BitArray bits = new BitArray(new bool[] { false, true, true, true, true });
    
        Assert.That(problem.Fitness(bits), Is.EqualTo(0));
    }

    [Test]
    public void LeadingOnesFitnessTest_SomeLeading()
    {
        LeadingOnes problem = new LeadingOnes(5);
        BitArray bits = new BitArray(new bool[] { true, true, false, true, true });
    
        Assert.That(problem.Fitness(bits), Is.EqualTo(2));
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
        Assert.That(problem.FuncEvals, Is.EqualTo(0));
    }

    [Test]
    public void ThreeOptTest()
    {
        PermutationProblem problem = new TSPProblem(6);
        TSPInstance instance = new TSPInstance([0,1,2,3,4,5],
            [(0,0),(0,3),(0,6),(3,6),(3,3),(3,0)]);
        
    }
    // Claude
    [Test]
    public void DistanceMatrixCachesLazily()
    {
        TSPProblem problem = new TSPProblem(6);
        TSPInstance instance = new TSPInstance([0,1,2,3,4,5],
            [(0,0),(0,3),(0,6),(3,6),(3,3),(3,0)]);

        // Matrix should be unpopulated before any fitness call
        Assert.That(problem.DistanceMatrix[0, 1], Is.EqualTo(0));
        Assert.That(problem.DistanceMatrix[0, 2], Is.EqualTo(0)); 
        Assert.That(problem.DistanceMatrix[2, 3], Is.EqualTo(0));
        // Trigger fitness evaluation
        problem.Fitness(instance);

        // Entries that were needed should now be cached
        Assert.That(problem.DistanceMatrix[0, 1], Is.EqualTo(3)); // (0,0) to (0,3)
        Assert.That(problem.DistanceMatrix[0, 2], Is.EqualTo(0)); // hasnt been evaluated
        Assert.That(problem.DistanceMatrix[1, 2], Is.EqualTo(3)); // (0,3) to (0,6)
        Assert.That(problem.DistanceMatrix[2, 3], Is.EqualTo(3)); // (0,6) to (3,6)
    }
    // Claude
    [Test]
    public void DistanceMatrixSymmetrical()
    {
        TSPProblem problem = new TSPProblem(6);
        TSPInstance instance = new TSPInstance([0,1,2,3,4,5],
            [(0,0),(0,3),(0,6),(3,6),(3,3),(3,0)]);

        problem.Fitness(instance);

        // Confirm symmetry — GetDistance(i,j) == GetDistance(j,i)
        Assert.That(problem.GetDistance(0, 1, instance), Is.EqualTo(problem.GetDistance(1, 0, instance)));
        Assert.That(problem.GetDistance(2, 5, instance), Is.EqualTo(problem.GetDistance(5, 2, instance)));
    }
}