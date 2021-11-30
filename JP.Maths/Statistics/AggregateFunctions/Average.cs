namespace JP.Maths.Statistics
{
	public sealed class Average : ComplexAggregateFunction
	{
		public override void AddDependenciesTo(IBatchAggregator parent)
		{
			parent.Add<Sum>();
			parent.Add<Count>();
		}

		public override double GetResult(IBatchAggregator parent)
		{
			return
				parent.GetResult<Sum>() /
				parent.GetResult<Count>() ;
		}
	}
}
