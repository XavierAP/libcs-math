namespace JP.Maths.Statistics
{
	public interface IAggregateFunction : IAggregator
	{
		void AddDependenciesTo(IBatchAggregator parent);

		double GetResult(IBatchAggregator parent);
	}
}