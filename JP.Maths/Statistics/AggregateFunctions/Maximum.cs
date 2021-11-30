namespace JP.Maths.Statistics
{
	public sealed class Maximum : SimpleAggregateFunction
	{
		private double Result = double.NegativeInfinity;

		public override void Aggregate(double samplePoint)
		{
			if (samplePoint > Result)
				Result = samplePoint;
		}

		public override double GetResult() => Result;
	}
}
