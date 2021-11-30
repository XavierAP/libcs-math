namespace JP.Maths.Statistics
{
	public sealed class Min : SimpleAggregateFunction
	{
		private double Result = double.PositiveInfinity;

		public override void Aggregate(double samplePoint)
		{
			if (samplePoint < Result)
				Result = samplePoint;
		}

		public override double GetResult() => Result;
	}
}
