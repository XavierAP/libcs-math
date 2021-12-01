namespace JP.Maths.Statistics
{
	public interface IBatchAggregator : IAggregator
	{
		void Clear();

		F Add<F>() where F : class, IFunction, new();

		double GetResult<F>() where F : class, IFunction;
	}
}
