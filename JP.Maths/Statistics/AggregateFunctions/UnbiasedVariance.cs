namespace JP.Maths.Statistics
{
	public sealed class UnbiasedVariance : IDependentFunction
	{
		private UncorrectedVariance UncorrectedVariance;
		private Count Count;

		public void SetDependencies(IBatchAggregator parent)
		{
			UncorrectedVariance = parent.Add<UncorrectedVariance>();
			Count = parent.Add<Count>();
		}

		public double GetResult()
		{
			var count = Count.GetResult();
			return UncorrectedVariance.GetResult() * count / (count - 1);
		}
	}
}
