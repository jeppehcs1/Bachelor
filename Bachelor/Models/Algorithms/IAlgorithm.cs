using System;
using System.Collections.Generic;

namespace Bachelor.Models.Algorithms;
// author Jeppe and Clement
public interface IAlgorithm
{
    int BSFF { get; set; }
    double Runtime { get; set; }
    int FuncEvals { get; }
    Func<bool> StoppingCondition { get; set; }
    int GetFitness();
    int? Optimum { get; }
    void Iterate();
    int Iterations { get; }
    void Initialize();
    void Configure(Dictionary<string, object> config);
}