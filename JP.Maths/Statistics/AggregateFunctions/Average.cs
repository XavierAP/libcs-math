namespace JP.Maths.Statistics
{
	public sealed class Average : ComplexAggregateFunction
	{
		public override void AddDependenciesTo(IBatchAggregator batchAggregator)
		{
			batchAggregator.Add<Sum>();
			batchAggregator.Add<Count>();
		}

		public override double GetResult(IBatchAggregator parent)
		{
			return
				parent.GetResult<Sum>() /
				parent.GetResult<Count>() ;
		}
	}
}
