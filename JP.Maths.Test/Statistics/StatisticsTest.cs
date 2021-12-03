using NUnit.Framework;

namespace JP.Maths.Statistics.Test
{
	[TestFixture]
	public class StatisticsTest
	{
		[Test]
		public void AllFunctions()
		{
			double[] data = { 2, 0, -5, 7, -3 };

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
			Assert.AreEqual(-5.0, min.GetResult());
			Assert.AreEqual(7.0, max.GetResult());
			Assert.AreEqual((double)data.Length, count.GetResult());
			Assert.AreEqual(0.20, avg.GetResult(), tolerance);
			Assert.AreEqual(1.0, sum.GetResult(), tolerance);
			Assert.AreEqual(87.0, sumOfSquares.GetResult(), tolerance);
			Assert.AreEqual(17.36, uncorrectedVariance.GetResult(), tolerance);
			Assert.AreEqual(21.70, unbiasedVariance.GetResult(), tolerance);
		}
	}
}
