﻿namespace JP.Maths.Statistics
{
	public abstract class SimpleAggregateFunction : IAggregateFunction
	{
		public void AddDependenciesTo(IBatchAggregator parent)
		{ }

		public abstract void Aggregate(double samplePoint);

		public double GetResult(IBatchAggregator parent) => GetResult();

		public abstract double GetResult();
	}
}