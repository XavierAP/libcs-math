namespace JP.Maths.Statistics
{
	public abstract class ComplexAggregateFunction : IAggregateFunction
	{
		public abstract void AddDependenciesTo(IBatchAggregator parent);

		public void Aggregate(double samplePoint)
		{ }

		public abstract double GetResult(IBatchAggregator parent);
	}
}
