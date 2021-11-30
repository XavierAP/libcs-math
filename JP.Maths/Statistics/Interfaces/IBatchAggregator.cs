namespace JP.Maths.Statistics
{
	public interface IBatchAggregator : IAggregator
	{
		void Add<T>() where T : IAggregateFunction, new();

		double GetResult<T>() where T : IAggregateFunction;
	}
}