using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace JP.Maths.Statistics
{
	using Scalar = Single;

	[MemoryDiagnoser]
	public class StatisticsBenchmark
	{
		static void Main() => BenchmarkRunner.Run<StatisticsBenchmark>();
		
		IEnumerable<Scalar> populationIterator;
		Scalar[] populationArray;

		[GlobalSetup]
		public void Setup()
		{
			populationIterator = Enumerable.Range(-999999, 9999999).Select(i => (Scalar)i);
			populationArray = populationIterator.ToArray();
		}

		[Benchmark]
		public void LinqArray()
		{
			var min = populationArray.Min();
			var max = populationArray.Max();
			var sum = populationArray.Sum();

			WriteLine($"Min: {min}, Max: {max}, Sum: {sum}");
		}

		[Benchmark]
		public void LinqIterator()
		{
			var min = populationIterator.Min();
			var max = populationIterator.Max();
			var sum = populationIterator.Sum();

			WriteLine($"Min: {min}, Max: {max}, Sum: {sum}");
		}

		[Benchmark]
		public void LinqIteratorCached()
		{
			var population = populationIterator.ToArray();
			var min = population.Min();
			var max = population.Max();
			var sum = population.Sum();

			WriteLine($"Min: {min}, Max: {max}, Sum: {sum}");
		}
		
		[Benchmark]
		public void MyArray()
		{
			var stats = new BatchAggregator();
			var minStat = stats.Add<Min>();
			var maxStat = stats.Add<Max>();
			var sumStat = stats.Add<Sum>();

			foreach (var point in populationArray)
				stats.Aggregate(point);

			var min = minStat.GetResult();
			var max = maxStat.GetResult();
			var sum = sumStat.GetResult();

			WriteLine($"Min: {min}, Max: {max}, Sum: {sum}");
		}
		
		[Benchmark]
		public void MyIterator()
		{
			var stats = new BatchAggregator();
			var minStat = stats.Add<Min>();
			var maxStat = stats.Add<Max>();
			var sumStat = stats.Add<Sum>();

			foreach (var point in populationIterator)
				stats.Aggregate(point);
			
			var min = minStat.GetResult();
			var max = maxStat.GetResult();
			var sum = sumStat.GetResult();

			WriteLine($"Min: {min}, Max: {max}, Sum: {sum}");
		}

		[Benchmark]
		public void SmartArray()
		{
			float
				min = Scalar.MaxValue,
				max = Scalar.MinValue,
				sum = 0;

			foreach (var point in populationArray)
			{
				sum += point;
				if(point < min) min = point;
				if(point > max) max = point;
			}
			WriteLine($"Min: {min}, Max: {max}, Sum: {sum}");
		}

		[Benchmark]
		public void SmartIterator()
		{
			float
				min = Scalar.MaxValue,
				max = Scalar.MinValue,
				sum = 0;

			foreach (var point in populationIterator)
			{
				sum += point;
				if(point < min) min = point;
				if(point > max) max = point;
			}
			WriteLine($"Min: {min}, Max: {max}, Sum: {sum}");
		}
	}
}
