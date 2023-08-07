using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;
using static System.Console;

namespace JP.Maths.Statistics
{
	using Scalar = Int32;

	public class StatisticsBenchmark
	{
		static void Main() => BenchmarkRunner.Run<StatisticsBenchmark>();

		Scalar[] population;

		[GlobalSetup]
		public void Setup()
		{
			population = Enumerable.Range(-9999999, 99999999)
				.Select(x => (Scalar)x)
				.ToArray();
		}

		[Benchmark]
		public void Linq()
		{
			var min = population.Min();
			var max = population.Max();
			var avg = population.Average();

			WriteLine($"Min: {min}, Max: {max}, Average: {avg}");
		}
		
		[Benchmark]
		public void Mine()
		{
			var stats = new BatchAggregator();
			stats.Add<Min>();
			stats.Add<Max>();
			stats.Add<Average>();

			foreach (var point in population)
				stats.Aggregate(point);

			var min = stats.GetResult<Min>();
			var max = stats.GetResult<Max>();
			var avg = stats.GetResult<Average>();

			WriteLine($"Min: {min}, Max: {max}, Average: {avg}");
		}
	}
}
