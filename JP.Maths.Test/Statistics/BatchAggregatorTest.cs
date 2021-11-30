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
			stats.Add<Min>();
			stats.Add<Max>();
			stats.Add<Average>();

			foreach (var point in data)
				stats.Aggregate(point);

			const double tolerance = 1e-9;
			Assert.AreEqual(data.Min(), stats.GetResult<Min>(), tolerance);
			Assert.AreEqual(data.Max(), stats.GetResult<Max>(), tolerance);
			Assert.AreEqual(data.Average(), stats.GetResult<Average>(), tolerance);
			Assert.AreEqual(data.Count(), stats.GetResult<Count>(), tolerance);
			Assert.AreEqual(data.Sum(), stats.GetResult<Sum>(), tolerance);
		}
	}
}
