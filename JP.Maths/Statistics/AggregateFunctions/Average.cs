namespace JP.Maths.Statistics
{
	public sealed class Average : IDependentFunction
	{
		private IBatchAggregator Parent;

		public void SetDependencies(IBatchAggregator parent)
		{
			Parent = parent;
			Parent.Add<Sum>();
			Parent.Add<Count>();
		}

		public double Result =>
			Parent.GetResult<Sum>() /
			Parent.GetResult<Count>() ;
	}
}
