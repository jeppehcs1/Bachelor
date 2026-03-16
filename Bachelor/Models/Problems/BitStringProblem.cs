using System.Transactions;
using System.Collections;

namespace Bachelor.Models.Problems;

public abstract class BitStringProblem(int dimension) : ProblemType<BitArray>(dimension);