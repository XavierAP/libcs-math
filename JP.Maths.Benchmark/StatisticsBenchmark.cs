using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace JP.Maths.Statistics
{
	using Scalar = Int32;

	[MemoryDiagnoser]
	public class StatisticsBenchmark
	{
		static void Main() => BenchmarkRunner.Run<StatisticsBenchmark>();
		
		IEnumerable<Scalar> populationIterator;
		Scalar[] populationArray;

		[GlobalSetup]
		public void Setup()
		{
			populationIterator = Enumerable.Range(-9999999, 99999999);
			populationArray = populationIterator.ToArray();
		}

		[Benchmark]
		public void LinqArray()
		{
			var min = populationArray.Min();
			var max = populationArray.Max();
			var avg = populationArray.Average();

			WriteLine($"Min: {min}, Max: {max}, Average: {avg}");
		}

		[Benchmark]
		public void LinqIterator()
		{
			var min = populationIterator.Min();
			var max = populationIterator.Max();
			var avg = populationIterator.Average();

			WriteLine($"Min: {min}, Max: {max}, Average: {avg}");
		}

		[Benchmark]
		public void LinqIteratorCached()
		{
			var population = populationIterator.ToArray();
			var min = population.Min();
			var max = population.Max();
			var avg = population.Average();

			WriteLine($"Min: {min}, Max: {max}, Average: {avg}");
		}
		
		[Benchmark]
		public void MyArray()
		{
			var stats = new BatchAggregator();
			var minStat = stats.Add<Min>();
			var maxStat = stats.Add<Max>();
			var avgStat = stats.Add<Average>();

			foreach (var point in populationArray)
				stats.Aggregate(point);

			var min = minStat.GetResult();
			var max = maxStat.GetResult();
			var avg = avgStat.GetResult();

			WriteLine($"Min: {min}, Max: {max}, Average: {avg}");
		}
		
		[Benchmark]
		public void MyIterator()
		{
			var stats = new BatchAggregator();
			var minStat = stats.Add<Min>();
			var maxStat = stats.Add<Max>();
			var avgStat = stats.Add<Average>();

			foreach (var point in populationIterator)
				stats.Aggregate(point);
			
			var min = minStat.GetResult();
			var max = maxStat.GetResult();
			var avg = avgStat.GetResult();

			WriteLine($"Min: {min}, Max: {max}, Average: {avg}");
		}
	}
}
