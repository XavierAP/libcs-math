namespace JP.Maths.Statistics
{
	public sealed class Sum : IAggregateFunction
	{
		public double Result { get; private set; } = 0;

		public void Aggregate(double samplePoint) => Result += samplePoint;
	}
}
