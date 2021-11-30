namespace JP.Maths.Statistics
{
	public sealed class Count : SimpleAggregateFunction
	{
		private double Result = 0;

		public override void Aggregate(double samplePoint) => ++Result;

		public override double GetResult() => Result;
	}
}
