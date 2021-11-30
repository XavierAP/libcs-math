namespace JP.Maths.Statistics
{
	public abstract class ComplexAggregateFunction : IAggregateFunction
	{
		public abstract void AddDependenciesTo(IBatchAggregator batchAggregator);

		public void Aggregate(double samplePoint)
		{ }

		public abstract double GetResult(IBatchAggregator parent);
	}
}
