# JP.Maths.Statistics
[BatchAggregator](/JP.Maths/Statistics/BatchAggregator.cs) allows to calculate multiple statistical aggregate functions
by iterating the sample or population only once and minimizing the number of operations.
See how to use in [StatisticsTest](/JP.Maths.Test/Statistics/StatisticsTest.cs).

More functions can be added.
Note that statistical aggregate functions are classified in two types:

* Those that implement [IAggregateFunction](/JP.Maths/Statistics/Interfaces/IAggregateFunction.cs):
they are calculated from the sample points. They cannot be calculated from other functions.
These classes can be used on their own (without [BatchAggregator](/JP.Maths/Statistics/BatchAggregator.cs))
however note "performance" below.

* Those that implement [IDependentFunction](/JP.Maths/Statistics/Interfaces/IDependentFunction.cs):
they can be calculated from other functions. So a more naive implementation directly based on their definition would be suboptimal.
	+ For example
	[(uncorrected) variance](/JP.Maths/Statistics/AggregateFunctions/UncorrectedVariance.cs)
	= sum((x - mean(x))²)/N = sum(x²)/N - mean(x)²

Currently implemented: see classes in [AggregateFunctions](/JP.Maths/Statistics/AggregateFunctions/).

## Performance
System.Linq (in modern .NET versions) _will_ outperform this (in most modern machines)
for calculating Min, Max, Sum and Average,
as long as the call is made on an array or List,
since System.Linq implements these with vectorization (SIMD).
See:
* [StatisticsBenchmark.results.txt](/JP.Maths.Benchmark/StatisticsBenchmark.results.txt)
* https://github.com/dotnet/runtime/pull/64624
