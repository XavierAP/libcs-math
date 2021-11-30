using System.Diagnostics;
using System.Linq;
using static System.Console;

namespace JP.Maths.Statistics
{
	class Benchmark
	{
		static void Main()
		{
			var stats = new BatchAggregator();
			stats.Add<Minimum>();
			stats.Add<Maximum>();
			stats.Add<Average>();
			
			var timer = new Stopwatch();
			for(int i = 0; i < 10; i++)
			{
				timer.Restart();

				foreach (var point in Enumerable.Range(-9999999, 99999999))
					stats.Aggregate(point);

				timer.Stop();
				WriteLine(timer.ElapsedMilliseconds.ToString());
			}
		}
	}
}
