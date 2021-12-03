namespace JP.Maths.Statistics
{
	public sealed class UncorrectedVariance : IDependentFunction
	{
		private SumOfSquares SumOfSquares;
		private Count Count;
		private Average Average;

		public void SetDependencies(IBatchAggregator parent)
		{
			SumOfSquares = parent.Add<SumOfSquares>();
			Count = parent.Add<Count>();
			Average = parent.Add<Average>();
		}

		public double GetResult()
		{
			var mean = Average.GetResult();
			return SumOfSquares.GetResult() / Count.GetResult() - mean * mean;
		}
	}
}
