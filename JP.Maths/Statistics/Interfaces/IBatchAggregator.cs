namespace JP.Maths.Statistics
{
	public interface IBatchAggregator : IAggregator
	{
		void Clear();

		void Add<F>() where F : IFunction, new();

		double GetResult<F>() where F : IFunction;
	}
}
