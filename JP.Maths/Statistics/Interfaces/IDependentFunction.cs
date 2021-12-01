namespace JP.Maths.Statistics
{
	public interface IDependentFunction : IFunction
	{
		void SetDependencies(IBatchAggregator parent);
	}
}
