using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace UnitTests;

public class AlgorithmTests
{
    [Test]
    public void UpdateSearchPoint_WhenNewFitnessIsBetter_KeepsNewSearchPoint()
    {
        // Arrange: Set up problem and instances
        var problem = new TSPProblem(6);
        var newInstance = new TSPInstance([1, 0, 4, 5, 2, 3], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
        var oldInstance = new TSPInstance([0, 5, 3, 4, 2, 1], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);

        
        var algorithm = new OnePlusOnePermutation(problem,  newInstance);
    
          // Assume this has better fitness
    
        algorithm.SearchPoint = newInstance;  // Simulate mutated point
        Assert.That(algorithm.GetFitness(), Is.EqualTo(21));
        // Act
        algorithm.UpdateSearchPoint(oldInstance);
    
        // Assert: Should keep newInstance if its fitness <= oldInstance's fitness
        
        Assert.That(newInstance.Permutation, Is.EqualTo(algorithm.SearchPoint.Permutation)); //, "Should keep the new SearchPoint if fitness is better or equal."
    }

    [Test]
    public void UpdateSearchPoint_WhenNewFitnessIsWorse_RevertsToOldSearchPoint()
    {
        // Arrange
        var problem = new TSPProblem(6);
        
        var oldInstance = new TSPInstance([0, 1, 3, 2, 5, 4], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
        var worseInstance = new TSPInstance([3, 4, 2, 1, 5, 0], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);  // Assume worse fitness

        var algorithm = new OnePlusOnePermutation(problem, oldInstance);
        Assert.That(algorithm.GetFitness(), Is.EqualTo(21));
        algorithm.SearchPoint = worseInstance;
        
        // Act
        algorithm.UpdateSearchPoint(oldInstance);
    
        // Assert: Should revert to oldInstance
        Assert.That(oldInstance.Permutation, Is.EqualTo(algorithm.SearchPoint.Permutation)); //, "Should revert to old SearchPoint if new fitness is worse."
    }
}