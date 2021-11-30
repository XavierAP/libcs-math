namespace JP.Maths.Statistics
{
	public sealed class Sum : SimpleAggregateFunction
	{
		private double Result = 0;

		public override void Aggregate(double samplePoint) => Result += samplePoint;

		public override double GetResult() => Result;
	}
}
