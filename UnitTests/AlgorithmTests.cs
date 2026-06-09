using System.Collections;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace UnitTests;

public class AlgorithmTests
{
    [TestFixture]
    public class OnePlusOneTests
    {
        [Test]
        public void OnePlusOneBitString_RevertsWhenMutationIsWorse()
        {
            var problem = new OneMax(5);
            var algo = new OnePlusOneBitString(problem);
            algo.Initialize();
    
            // old (pre-mutation) is good
            var old = new BitArray(new bool[] { true, true, true, true, true });
            algo.BSFF = 5;
    
            // SearchPoint (post-mutation) is worse
            algo.SearchPoint = new BitArray(new bool[] { false, false, false, false, false });
    
            algo.UpdateSearchPoint(old);
    
            // Should revert to old, no improvement
            Assert.That(algo.BSFF, Is.EqualTo(5));
            Assert.That(algo.SearchPoint, Is.EqualTo(old));
        }

        [Test]
        public void OnePlusOneBitString_AcceptsBetterMutation()
        {
            var problem = new OneMax(5);
            var algo = new OnePlusOneBitString(problem);
            algo.Initialize();
    
            // old (pre-mutation) is worse
            var old = new BitArray(new bool[] { false, false, false, false, false });
            algo.BSFF = 0;
    
            // SearchPoint (post-mutation) is better
            algo.SearchPoint = new BitArray(new bool[] { true, true, true, true, true });
    
            algo.UpdateSearchPoint(old);
    
            
            Assert.That(algo.BSFF, Is.EqualTo(5));
        }

        [Test]
        public void OnePlusOneBitString_AcceptsBetterSolution()
        {
            var problem = new OneMax(5);
            var algo = new OnePlusOneBitString(problem);
            algo.Initialize();
            
            algo.SearchPoint = new BitArray(new bool[] { false, false, false, false, false });
            algo.BSFF = 0;
            
            var better = new BitArray(new bool[] { true, true, true, true, true });
            // Mutate to better manually
            algo.SearchPoint = better;
            algo.UpdateSearchPoint(new BitArray(new bool[] { false, false, false, false, false }));
            
            Assert.That(algo.BSFF, Is.EqualTo(5));
            
        }

        [Test]
        public void OnePlusOneBitString_BsffNeverDecreases()
        {
            var problem = new OneMax(20);
            var algo = new OnePlusOneBitString(problem);
            algo.Initialize();
            
            int prev = algo.BSFF;
            for (int i = 0; i < 1000; i++)
            {
                algo.Iterate();
                Assert.That(algo.BSFF, Is.GreaterThanOrEqualTo(prev));
                prev = algo.BSFF;
            }
        }

        [Test]
        public void OnePlusOnePermutation_KeepsLowerTourLength()
        {
            var problem = new TSPProblem(4);
            var instance = new TSPInstance([0,1,2,3], [(0,0),(0,3),(3,3),(3,0)]);
            var algo = new OnePlusOnePermutation(problem, instance);
            algo.Initialize();
            
            int initialBSFF = algo.BSFF;
            int prev = algo.BSFF;
            for (int i = 0; i < 1000; i++)
            {
                algo.Iterate();
                Assert.That(algo.BSFF, Is.LessThanOrEqualTo(prev)); // TSP: lower is better
                prev = algo.BSFF;
            }
        }

        [Test]
        public void OnePlusOnePermutation_BsffNeverIncreases()
        {
            var problem = new TSPProblem(4);
            var instance = new TSPInstance([0,1,2,3], [(0,0),(0,3),(3,3),(3,0)]);
            var algo = new OnePlusOnePermutation(problem, instance);
            algo.Initialize();
            
            int prev = algo.BSFF;
            for (int i = 0; i < 1000; i++)
            {
                algo.Iterate();
                Assert.That(algo.BSFF, Is.LessThanOrEqualTo(prev));
                prev = algo.BSFF;
            }
        }
        

        [Test]
        public void OnePlusOneBitString_IterationsIncrementCorrectly()
        {
            var problem = new OneMax(5);
            var algo = new OnePlusOneBitString(problem);
            algo.Initialize();
            
            for (int i = 0; i < 10; i++)
                algo.Iterate();
            
            Assert.That(algo.Iterations, Is.EqualTo(10));
        }
    }
    [TestFixture]
    public class MuPlusLambdaTests
    {
        // ===== BIT STRING TESTS =====

        [Test]
        public void Initialize_BitString_PopulationHasMuIndividuals()
        {
            var problem = new OneMax(10);
            var algorithm = new MuPlusLambdaBitString(problem);
            algorithm.Initialize();

            Assert.That(algorithm.Population.Count, Is.EqualTo(algorithm.Mu));
        }

        [Test]
        public void Initialize_BitString_SearchPointIsInPopulation()
        {
            var problem = new OneMax(10);
            var algorithm = new MuPlusLambdaBitString(problem);
            algorithm.Initialize();

            var bestFitness = algorithm.Population.Max(x => x.Fitness);
            Assert.That(problem.Fitness(algorithm.SearchPoint), Is.EqualTo(bestFitness));
        }

        [Test]
        public void UpdateSearchPoint_BitString_KeepsMuBestIndividuals()
        {
            var problem = new OneMax(10);
            var algorithm = new MuPlusLambdaBitString(problem);
            algorithm.Initialize();

            var old = algorithm.ClonePopulation();
            algorithm.MutateSearchPoint();
            algorithm.UpdateSearchPoint(old);

            Assert.That(algorithm.Population.Count, Is.EqualTo(algorithm.Mu));
        }

        [Test]
        public void UpdateSearchPoint_BitString_SearchPointIsBestInPopulation()
        {
            var problem = new OneMax(10);
            var algorithm = new MuPlusLambdaBitString(problem);
            algorithm.Initialize();

            var old = algorithm.ClonePopulation();
            algorithm.MutateSearchPoint();
            algorithm.UpdateSearchPoint(old);

            var bestFitness = algorithm.Population.Max(x => x.Fitness);
            Assert.That(problem.Fitness(algorithm.SearchPoint), Is.EqualTo(bestFitness));
        }
        [Test]
        public void MutateSearchPoint_BitString_EvaluatesExactlyLambdaTimes()
        {
            var problem = new OneMax(10);
            var algorithm = new MuPlusLambdaBitString(problem);
            algorithm.Initialize();

            problem.FuncEvals = 0; // Reset after initialization

            algorithm.MutateSearchPoint();

            Assert.That(problem.FuncEvals, Is.EqualTo(algorithm.Lambda));
        }
        [Test]
        public void UpdateSearchPoint_BitString_EvaluatesZeroTimes()
        {
            var problem = new OneMax(10);
            var algorithm = new MuPlusLambdaBitString(problem);
            algorithm.Initialize();

            var old = algorithm.ClonePopulation();
            algorithm.MutateSearchPoint();
            problem.FuncEvals = 0; // Reset after mutation, before update

            algorithm.UpdateSearchPoint(old);

            Assert.That(problem.FuncEvals, Is.EqualTo(0));
        }

        [Test]
        public void MutateSearchPoint_BitString_PopulationGrowsToMuPlusLambda()
        {
            var problem = new OneMax(10);
            var algorithm = new MuPlusLambdaBitString(problem);
            algorithm.Initialize();

            algorithm.MutateSearchPoint();

            Assert.That(algorithm.Population.Count, Is.EqualTo(algorithm.Mu + algorithm.Lambda));
        }

        [Test]
        public void Iterate_BitString_FitnessNeverDecreases()
        {
            var problem = new OneMax(10);
            var algorithm = new MuPlusLambdaBitString(problem);
            algorithm.Initialize();

            int previousFitness = problem.Fitness(algorithm.SearchPoint);
            for (int i = 0; i < 50; i++)
            {
                algorithm.Iterate();
                int currentFitness = problem.Fitness(algorithm.SearchPoint);
                Assert.That(currentFitness, Is.GreaterThanOrEqualTo(previousFitness));
                previousFitness = currentFitness;
            }
        }

        // ===== TSP TESTS =====

        [Test]
        public void Initialize_TSP_PopulationHasMuIndividuals()
        {
            var problem = new TSPProblem(6);
            var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
            var algorithm = new MuPlusLambdaPermutation(problem, instance);
            algorithm.Initialize();

            Assert.That(algorithm.Population.Count, Is.EqualTo(algorithm.Mu));
        }
        [Test]
        public void Initialize_TSP_SearchPointIsBestInPopulation()
        {
            var problem = new TSPProblem(6);
            var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
            var algorithm = new MuPlusLambdaPermutation(problem, instance);
            algorithm.Initialize();

            var bestFitness = algorithm.Population.Min(x => x.Fitness);
            Assert.That(problem.Fitness(algorithm.SearchPoint), Is.EqualTo(bestFitness));
        }
        [Test]
        public void UpdateSearchPoint_TSP_KeepsMuBestIndividuals()
        {
            var problem = new TSPProblem(6);
            var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
            var algorithm = new MuPlusLambdaPermutation(problem, instance);
            algorithm.Initialize();

            var old = algorithm.ClonePopulation();
            algorithm.MutateSearchPoint();
            algorithm.UpdateSearchPoint(old);

            Assert.That(algorithm.Population.Count, Is.EqualTo(algorithm.Mu));
        }

        [Test]
        public void Iterate_TSP_FitnessNeverIncreases()
        {
            var problem = new TSPProblem(6);
            var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
            var algorithm = new MuPlusLambdaPermutation(problem, instance);
            algorithm.Initialize();

            double previousFitness = problem.Fitness(algorithm.SearchPoint);
            for (int i = 0; i < 50; i++)
            {
                algorithm.Iterate();
                double currentFitness = problem.Fitness(algorithm.SearchPoint);
                Assert.That(currentFitness, Is.LessThanOrEqualTo(previousFitness));
                previousFitness = currentFitness;
            }
        }
    }
        
    [TestFixture]
    public class SimulatedAnnealingTests
    {
        // ===== BIT STRING TESTS =====

    [Test]
    public void Initialize_BitString_SearchPointHasCorrectDimension()
    {
        var problem = new OneMax(10);
        var algorithm = new SimulatedAnnealingBitString(problem);
        algorithm.Initialize();

        Assert.That(algorithm.SearchPoint.Length, Is.EqualTo(10));
    }

    [Test]
    public void Initialize_BitString_TemperatureIsPositive()
    {
        var problem = new OneMax(10);
        var algorithm = new SimulatedAnnealingBitString(problem);
        algorithm.Initialize();

        Assert.That(algorithm._temperature, Is.GreaterThan(0));
    }

    [Test]
    public void UpdateSearchPoint_BitString_TemperatureDecreasesEachIteration()
    {
        var problem = new OneMax(10);
        var algorithm = new SimulatedAnnealingBitString(problem);
        algorithm.Initialize();

        double previousTemperature = algorithm._temperature;
        algorithm.Iterate();

        Assert.That(algorithm._temperature, Is.LessThan(previousTemperature));
    }

    [Test]
    public void Iterate_BitString_SearchPointRemainsValidLength()
    {
        var problem = new OneMax(10);
        var algorithm = new SimulatedAnnealingBitString(problem);
        algorithm.Initialize();

        for (int i = 0; i < 50; i++)
        {
            algorithm.Iterate();
            Assert.That(algorithm.SearchPoint.Length, Is.EqualTo(10));
        }
    }

    [Test]
    public void MutateSearchPoint_BitString_ChangesExactlyOneBit()
    {
        var problem = new OneMax(10);
        var algorithm = new SimulatedAnnealingBitString(problem);
        algorithm.Initialize();

        var before = algorithm.CloneSearchPoint();
        algorithm.MutateSearchPoint();

        int differences = 0;
        for (int i = 0; i < before.Length; i++)
            if (before[i] != algorithm.SearchPoint[i]) differences++;

        Assert.That(differences, Is.EqualTo(1));
    }

    // ===== TSP TESTS =====

    [Test]
    public void Initialize_TSP_SearchPointIsValidPermutation()
    {
        var problem = new TSPProblem(6);
        var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
        var algorithm = new SimulatedAnnealingPermutation(problem, instance);
        algorithm.Initialize();

        var sorted = algorithm.SearchPoint.Permutation.OrderBy(x => x).ToList();
        Assert.That(sorted, Is.EqualTo(new List<int> { 0, 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void Initialize_TSP_TemperatureIsPositive()
    {
        var problem = new TSPProblem(6);
        var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
        var algorithm = new SimulatedAnnealingPermutation(problem, instance);
        algorithm.Initialize();

        Assert.That(algorithm._temperature, Is.GreaterThan(0));
    }

    [Test]
    public void UpdateSearchPoint_TSP_TemperatureDecreasesEachIteration()
    {
        var problem = new TSPProblem(6);
        var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
        var algorithm = new SimulatedAnnealingPermutation(problem, instance);
        algorithm.Initialize();

        double previousTemperature = algorithm._temperature;
        algorithm.Iterate();

        Assert.That(algorithm._temperature, Is.LessThan(previousTemperature));
    }

    [Test]
    public void Iterate_TSP_SearchPointRemainsValidPermutation()
    {
        var problem = new TSPProblem(6);
        var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
        var algorithm = new SimulatedAnnealingPermutation(problem, instance);
        algorithm.Initialize();

        for (int i = 0; i < 50; i++)
        {
            algorithm.Iterate();
            var sorted = algorithm.SearchPoint.Permutation.OrderBy(x => x).ToList();
            Assert.That(sorted, Is.EqualTo(new List<int> { 0, 1, 2, 3, 4, 5 }));
        }
    }

    [Test]
    public void UpdateSearchPoint_TSP_AcceptsWorseSolutionWithHighTemperature()
    {
        // With very high temperature, worse solutions should sometimes be accepted
        var problem = new TSPProblem(6);
        var instance = new TSPInstance([0, 1, 2, 3, 4, 5], [(2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2)]);
        var algorithm = new SimulatedAnnealingPermutation(problem, instance);
        algorithm.Initialize();

        // Run many iterations at high temperature and check it doesn't always revert
        bool everAcceptedWorse = false;
        for (int i = 0; i < 100; i++)
        {
            double fitnessBefore = problem.Fitness(algorithm.SearchPoint);
            algorithm.Iterate();
            double fitnessAfter = problem.Fitness(algorithm.SearchPoint);
            if (fitnessAfter > fitnessBefore) everAcceptedWorse = true;
        }

        Assert.That(everAcceptedWorse, Is.True);
    }
}
        
    
    [Test]
    public void UpdateSearchPoint_WhenNewFitnessIsBetter_KeepsNewSearchPoint()
    {
        var problem = new TSPProblem(6);
        var graph = new List<(int x, int y)> { (2, 4), (1, 4), (4, 2), (3, 1), (7, 7), (8, 2) };
    
        var oldInstance = new TSPInstance([0, 5, 3, 4, 2, 1], graph);
        var newInstance = new TSPInstance([1, 0, 4, 5, 2, 3], graph);
    
        var algorithm = new OnePlusOnePermutation(problem, oldInstance);
        algorithm.SearchPoint = newInstance;
    
        int newFitness = problem.Fitness(newInstance);
        int oldFitness = problem.Fitness(oldInstance);
    
        // Verify the test assumption — new must actually be better
        Assert.That(newFitness, Is.LessThan(oldFitness), "Test setup error: newInstance must have lower fitness");
    
        algorithm.BSFF = oldFitness; // BSFF reflects fitness before mutation
        algorithm.UpdateSearchPoint(oldInstance);
    
        Assert.That(algorithm.SearchPoint.Permutation, Is.EqualTo(newInstance.Permutation));
        Assert.That(algorithm.BSFF, Is.EqualTo(newFitness));
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
        algorithm.BSFF = 21;
        algorithm.SearchPoint = worseInstance;
        
        // Act
        algorithm.UpdateSearchPoint(oldInstance);
    
        // Assert: Should revert to oldInstance
        Assert.That(oldInstance.Permutation, Is.EqualTo(algorithm.SearchPoint.Permutation)); //, "Should revert to old SearchPoint if new fitness is worse."
    }
    
    
    // MMAS, largely based on output from Claude
    [TestFixture]
    public class MinMaxAntSystemPermutationTests
    {
        [Test]
    public void PheromonesInitialized()
    {
        var problem = new TSPProblem(4);
        var instance = new TSPInstance([0,1,2,3], [(0,0),(0,3),(3,3),(3,0)]);
        var algo = new MinMaxAntSystemPermutation(problem, instance);
        algo.Initialize();
    
        Assert.That(algo.GetEdgePheromones(0, 1), Is.EqualTo(algo.InitialPheromone));
        Assert.That(algo.GetEdgePheromones(1, 3), Is.EqualTo(algo.InitialPheromone));
    }
    [Test]
    public void PheromonesAreSymmetric()
    {
        var problem = new TSPProblem(4);
        var instance = new TSPInstance([0,1,2,3], [(0,0),(0,3),(3,3),(3,0)]);
        var algo = new MinMaxAntSystemPermutation(problem, instance);
        algo.Initialize();
    
        Assert.That(algo.GetEdgePheromones(0, 2), Is.EqualTo(algo.GetEdgePheromones(2, 0)));
        algo.Iterate();
        
    }
    [Test]
    public void FitnessDoesNotWorsenAfterIterations()
    {
        var problem = new TSPProblem(4);
        var instance = new TSPInstance([0,1,2,3], [(0,0),(0,3),(3,3),(3,0)]);
        var algo = new MinMaxAntSystemPermutation(problem, instance);
        algo.Initialize();
    
        int initialFitness = algo.GetFitness();
        for (int i = 0; i < 100000; i++)
        {
            algo.Iterate();
            Assert.That(algo.GetFitness(), Is.LessThanOrEqualTo(initialFitness));
        }
            
    
        
    }
    [Test]
    public void ConstructAntSolutions_ProducesValidPermutation()
    {
        var problem = new TSPProblem(4);
        var instance = new TSPInstance([0,1,2,3], [(0,0),(0,3),(3,3),(3,0)]);
        var algo = new MinMaxAntSystemPermutation(problem, instance);
        algo.Initialize();
        for (int i = 0; i < 100000; i++)
        {
            algo.ConstructAntSolutions();  
            Assert.That(algo.SearchPoint.Permutation.Count, Is.EqualTo(4));
            Assert.That(algo.SearchPoint.Permutation.OrderBy(x => x), Is.EqualTo(new[] {0,1,2,3}));
        }
        
    }
    [Test]
    public void ChooseComponent_NeverReturnsVertexOutsideNeighbourhood()
    {
        var problem = new TSPProblem(4);
        var instance = new TSPInstance([0,1,2,3], [(0,0),(0,3),(3,3),(3,0)]);
        var algo = new MinMaxAntSystemPermutation(problem, instance);
        algo.Initialize();
    
        // Run many times to catch edge cases
        for (int i = 0; i < 100000; i++)
            algo.ConstructAntSolutions();
    
        // If we get here without an exception or invalid permutation, the neighbourhood logic is likely to be correct
        Assert.That(algo.SearchPoint.Permutation.OrderBy(x => x), Is.EqualTo(new[] {0,1,2,3}));
    }
    [Test]
    public void ConstructAntSolutions_IncrementsEvalsByNumAnts()
    {
        var problem = new TSPProblem(4);
        var instance = new TSPInstance([0,1,2,3], [(0,0),(0,3),(3,3),(3,0)]);
        var algo = new MinMaxAntSystemPermutation(problem, instance);
        algo.Initialize();
    
        int evalsBefore = problem.FuncEvals;
        algo.ConstructAntSolutions();
    
        Assert.That(problem.FuncEvals - evalsBefore, Is.EqualTo(algo.NumAnts));
    }
    // bit string mmas
    [Test]
    public void BitString_PheromonesInitializedToHalf()
    {
        var problem = new OneMax(10);
        var algo = new MinMaxAntSystemBitString(problem);
        algo.Initialize();
        
        for (int i = 0; i < 10; i++)
            Assert.That(algo.EdgePheromones[i], Is.EqualTo(0.5));
    }

    [Test]
    public void BitString_PheromonesStayWithinBounds()
    {
        var problem = new OneMax(10);
        var algo = new MinMaxAntSystemBitString(problem);
        algo.Initialize();
        
        for (int i = 0; i < 1000; i++)
        {
            algo.ConstructAntSolutions();
            algo.UpdatePheromones();
            for (int j = 0; j < 10; j++)
                Assert.That(algo.EdgePheromones[j], Is.InRange(algo.TauMin, algo.TauMax));
        }
        
        
    }

    [Test]
    public void BitString_PheromonesIncreaseForSetBits()
    {
        var problem = new OneMax(10);
        var algo = new MinMaxAntSystemBitString(problem);
        algo.Initialize();
        
        // Force a known SearchPoint
        algo.SearchPoint = new BitArray(new bool[] { true, true, true, true, true, true, true, true, true, true });
        double before = algo.EdgePheromones[0];
        algo.UpdatePheromones();
        
        Assert.That(algo.EdgePheromones[0], Is.GreaterThan(before));
    }

    [Test]
    public void BitString_PheromonesDecreaseForUnsetBits()
    {
        var problem = new OneMax(10);
        var algo = new MinMaxAntSystemBitString(problem);
        algo.Initialize();
        
        // Force all bits unset
        algo.SearchPoint = new BitArray(10, false);
        double before = algo.EdgePheromones[0];
        algo.UpdatePheromones();
        
        Assert.That(algo.EdgePheromones[0], Is.LessThan(before));
    }

    [Test]
    public void BitString_ConstructAntSolutions_IncrementsEvalsByNumAnts()
    {
        var problem = new OneMax(10);
        var algo = new MinMaxAntSystemBitString(problem);
        algo.Initialize();
        
        int evalsBefore = problem.FuncEvals;
        algo.ConstructAntSolutions();
        
        Assert.That(problem.FuncEvals - evalsBefore, Is.EqualTo(algo.NumAnts));
    }

    [Test]
    public void BitString_BsffNeverDecreases()
    {
        var problem = new OneMax(10);
        var algo = new MinMaxAntSystemBitString(problem);
        algo.Initialize();
        
        int previous = algo.BSFF;
        for (int i = 0; i < 100; i++)
        {
            algo.ConstructAntSolutions();
            algo.UpdatePheromones();
            Assert.That(algo.BSFF, Is.GreaterThanOrEqualTo(previous));
            previous = algo.BSFF;
        }
    }
    }
    
}












