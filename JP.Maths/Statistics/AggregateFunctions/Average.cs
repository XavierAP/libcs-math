namespace JP.Maths.Statistics
{
	public sealed class Average : IDependentFunction
	{
		private Sum Sum;
		private Count Count;

		public void SetDependencies(IBatchAggregator parent)
		{
			Sum = parent.Add<Sum>();
			Count = parent.Add<Count>();
		}

		public double GetResult() => Sum.GetResult() / Count.GetResult();
	}
}
