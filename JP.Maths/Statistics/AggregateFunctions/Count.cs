namespace JP.Maths.Statistics
{
	public sealed class Count : IAggregateFunction
	{
		private double Result = 0;

		public void Aggregate(double samplePoint) => ++Result;

		public double GetResult() => Result;
	}
}
