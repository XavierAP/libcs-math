namespace JP.Maths.Statistics
{
	public sealed class Max : IAggregateFunction
	{
		public double Result { get; private set; } = double.NegativeInfinity;

		public void Aggregate(double samplePoint)
		{
			if (samplePoint > Result)
				Result = samplePoint;
		}
	}
}
