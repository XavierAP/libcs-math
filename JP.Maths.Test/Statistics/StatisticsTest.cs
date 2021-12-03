using NUnit.Framework;

namespace JP.Maths.Statistics.Test
{
	[TestFixture]
	public class StatisticsTest
	{
		[Test]
		public void AllFunctions()
		{
			double[] data = { 0, 2, -9, 99, -3 };

			var stats = new BatchAggregator();
			var min = stats.Add<Min>();
			var max = stats.Add<Max>();
			var avg = stats.Add<Average>();
			var count = stats.Add<Count>();
			var sum = stats.Add<Sum>();
			var sumOfSquares = stats.Add<SumOfSquares>();
			var uncorrectedVariance = stats.Add<UncorrectedVariance>();
			var unbiasedVariance = stats.Add<UnbiasedVariance>();

			foreach (var point in data)
				stats.Aggregate(point);

			const double tolerance = 1e-9;
			Assert.AreEqual(-9, min.GetResult());
			Assert.AreEqual(99, max.GetResult());
			Assert.AreEqual(data.Length, count.GetResult());
			Assert.AreEqual(17.8, avg.GetResult(), tolerance);
			Assert.AreEqual(89, sum.GetResult(), tolerance);
			Assert.AreEqual(9895, sumOfSquares.GetResult(), tolerance);
			Assert.AreEqual(1662.16, uncorrectedVariance.GetResult(), tolerance);
			Assert.AreEqual(2077.7, unbiasedVariance.GetResult(), tolerance);
		}
	}
}
