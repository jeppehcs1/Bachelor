using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Utility;

public class AlgorithmRunner
{
    private CancellationTokenSource _cts = new();
    private SemaphoreSlim _pauseSemaphore = new(1, 1);
    private bool _isPaused = false;
    private int _iterationCount = 0;
    private const int UpdateInterval = 1000;
    public event Action<IAlgorithm>? OnIteration;
    public event Action? OnInitialization;

    public async Task Run(IAlgorithm algorithm)
    {
        algorithm.Initialize();
        OnInitialization?.Invoke();
        var token = _cts.Token;

        await Task.Run(() =>
        {
            long startTime = Stopwatch.GetTimestamp();
            while (!algorithm.StoppingCondition() && !token.IsCancellationRequested)
            {
                _pauseSemaphore.Wait(token);
                _pauseSemaphore.Release();

                algorithm.Iterate();
                _iterationCount++;
                if (_iterationCount % UpdateInterval == 0)
                    OnIteration?.Invoke(algorithm);
            }
            algorithm.Runtime = Stopwatch.GetElapsedTime(startTime).TotalSeconds;
        }, token);
    }

    public AlgorithmSnapshot TakeSnapshot(IAlgorithm algorithm)
    {
        if (algorithm is Algorithm<TSPInstance> tspAlgo)
            return new AlgorithmSnapshot
            {
                BSFF = algorithm.BSFF,
                FuncEvals = algorithm.FuncEvals,
                Iterations = algorithm.Iterations,
                Runtime = algorithm.Runtime,
                TSPSearchPoint = tspAlgo.SearchPoint.DeepCopy()
            };

        if (algorithm is Algorithm<BitArray> bitAlgo)
            return new AlgorithmSnapshot
            {
                BSFF = algorithm.BSFF,
                FuncEvals = algorithm.FuncEvals,
                Iterations = algorithm.Iterations,
                Runtime = algorithm.Runtime,
                BitStringSearchPoint = bitAlgo.SearchPoint.Clone() as BitArray
            };
        return new AlgorithmSnapshot { BSFF = algorithm.BSFF, FuncEvals = algorithm.FuncEvals,  Iterations = algorithm.Iterations };
    }


public void Pause()
    {
        if (!_isPaused)
        {
            _pauseSemaphore.Wait();
            _isPaused = true;
        }
    }

    public void Play()
    {
        if (_isPaused)
        {
            _pauseSemaphore.Release();
            _isPaused = false;
        }
    }
}