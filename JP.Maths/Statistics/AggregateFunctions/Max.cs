namespace JP.Maths.Statistics
{
	public sealed class Max : IAggregateFunction
	{
		private double Result = double.NegativeInfinity;

		public void Aggregate(double samplePoint)
		{
			if (samplePoint > Result)
				Result = samplePoint;
		}

		public double GetResult() => Result;
	}
}
