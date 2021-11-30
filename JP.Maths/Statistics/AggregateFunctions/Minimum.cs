namespace JP.Maths.Statistics
{
	public sealed class Minimum : SimpleAggregateFunction
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
