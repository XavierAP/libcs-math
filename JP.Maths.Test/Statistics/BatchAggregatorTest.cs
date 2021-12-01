using NUnit.Framework;
using System.Linq;

namespace JP.Maths.Statistics.Test
{
	[TestFixture]
	public class BatchAggregatorTest
	{
		[Test]
		public void AllFunctions()
		{
			double[] data = { 0, 2, -9, 99, -3 };

			var stats = new BatchAggregator();
			var min = stats.Add<Min>();
			var max = stats.Add<Max>();
			var avg = stats.Add<Average>();

			foreach (var point in data)
				stats.Aggregate(point);

			const double tolerance = 1e-9;
			Assert.AreEqual(data.Min(), min.Result, tolerance);
			Assert.AreEqual(data.Max(), max.Result, tolerance);
			Assert.AreEqual(data.Average(), avg.Result, tolerance);
			Assert.AreEqual(data.Sum(), stats.GetResult<Sum>(), tolerance);
			Assert.AreEqual(data.Count(), stats.GetResult<Count>());
		}
	}
}
