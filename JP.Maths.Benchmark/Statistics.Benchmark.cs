using System.Diagnostics;
using System.Linq;
using static System.Console;

namespace JP.Maths.Statistics
{
	class Benchmark
	{
		static void Main()
		{
			var timer = new Stopwatch();
			const int repeats = 10;
			var population = Enumerable.Range(-9999999, 99999999).ToArray();

			WriteLine("        System.Linq:");
			for (int i = 0; i < repeats; i++)
			{
				timer.Restart();

				var min = population.Min();
				var max = population.Max();
				var avg = population.Average();

				timer.Stop();
				WriteLine($"Min: {min}, Max: {max}, Average: {avg}, TIME: {timer.ElapsedMilliseconds:N}");
			}

			WriteLine("        JP.Maths.Statistics:");

			for (int i = 0; i < repeats; i++)
			{
				var stats = GetMinMaxAverageCalculator();
				timer.Restart();

				foreach (var point in population)
					stats.Aggregate(point);

				timer.Stop();
				var (min, max, avg) = (stats.GetResult<Min>(), stats.GetResult<Max>(), stats.GetResult<Average>());
				WriteLine($"Min: {min}, Max: {max}, Average: {avg}, TIME: {timer.ElapsedMilliseconds:N}");
			}
		}

		private static BatchAggregator GetMinMaxAverageCalculator()
		{
			var stats = new BatchAggregator();
			stats.Add<Min>();
			stats.Add<Max>();
			stats.Add<Average>();
			return stats;
		}
	}
}
