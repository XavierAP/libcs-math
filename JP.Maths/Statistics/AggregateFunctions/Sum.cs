namespace JP.Maths.Statistics
{
	public sealed class Sum : IAggregateFunction
	{
		private double Result = 0;

		public void Aggregate(double samplePoint) => Result += samplePoint;

		public double GetResult() => Result;
	}
}
