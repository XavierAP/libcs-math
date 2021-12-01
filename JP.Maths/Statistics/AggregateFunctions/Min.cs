namespace JP.Maths.Statistics
{
	public sealed class Min : IAggregateFunction
	{
		public double Result { get; private set; } = double.PositiveInfinity;

		public void Aggregate(double samplePoint)
		{
			if (samplePoint < Result)
				Result = samplePoint;
		}
	}
}
