namespace JP.Maths.Statistics
{
	public interface IAggregateFunction : IAggregator
	{
		void AddDependenciesTo(IBatchAggregator batchAggregator);

		double GetResult(IBatchAggregator parent);
	}
}