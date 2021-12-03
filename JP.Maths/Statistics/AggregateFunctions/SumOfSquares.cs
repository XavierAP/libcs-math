namespace JP.Maths.Statistics
{
	public sealed class SumOfSquares : IAggregateFunction
	{
		private double Result = 0;

		public void Aggregate(double samplePoint) => Result += samplePoint * samplePoint;

		public double GetResult() => Result;
	}
}
