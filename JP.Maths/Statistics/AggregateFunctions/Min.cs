namespace JP.Maths.Statistics
{
	public sealed class Min : IAggregateFunction
	{
		private double Result = double.PositiveInfinity;

		public void Aggregate(double samplePoint)
		{
			if (samplePoint < Result)
				Result = samplePoint;
		}

		public double GetResult() => Result;
	}
}
