using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bachelor.Models.Algorithms;
using Bachelor.Models.Problems;

namespace Bachelor.Models.Utility;
// author Jeppe and Claude.ai
public class AlgorithmRunner
{
    private CancellationTokenSource _cts = new();
    private SemaphoreSlim _pauseSemaphore = new(1, 1);
    private bool _isPaused = false;
    private int _iterationCount = 0;
    public int UpdateInterval = 1000;
    public event Action<IAlgorithm>? OnIteration;
    public event Action? OnInitialization;
    // Co-authored by Claude.ai
    public async Task Run(IAlgorithm algorithm, CancellationToken ct = default)
    {
        algorithm.Initialize();
        OnInitialization?.Invoke();
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, ct);
        var token = linkedCts.Token;

        await Task.Run(() =>
        {
            double totalTime = 0;
            while (!algorithm.StoppingCondition() && !token.IsCancellationRequested)
            {
                _pauseSemaphore.Wait(token);
                _pauseSemaphore.Release();
                long startTime = Stopwatch.GetTimestamp();
                algorithm.Iterate();
                totalTime += Stopwatch.GetElapsedTime(startTime).TotalSeconds;
                _iterationCount++;
                if (_iterationCount % UpdateInterval == 0)
                    OnIteration?.Invoke(algorithm);
            }

            algorithm.Runtime = totalTime;
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
    // Co-authored by Claude.ai
    public void Restart()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
        _iterationCount = 0;
    
        if (_isPaused)
        {
            _pauseSemaphore.Release();
            _isPaused = false;
        }
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