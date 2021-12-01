using System;
using System.Collections.Generic;

namespace JP.Maths.Statistics
{
	/// <summary>Calculates multiple <see cref="IFunction"/>s
	/// by iterating only once through the sample or population.</summary>
	public class BatchAggregator : IBatchAggregator
	{
		private readonly List<IAggregateFunction> AggregateFunctions = new List<IAggregateFunction>();
		private readonly List<IDependentFunction> DependentFunctions = new List<IDependentFunction>();
		
		public void Clear()
		{
			AggregateFunctions.Clear();
			DependentFunctions.Clear();
		}

		/// <summary>If the same type was already added, returns existing instance.</summary>
		public F Add<F>()
			where F : class, IFunction, new()
		{
			var func = GetFunction<F>(out var wasAlreadyAdded);
			if(wasAlreadyAdded)
				return func;

			if(func is IAggregateFunction af)
			{
				AggregateFunctions.Add(af);
			}
			else if(func is IDependentFunction df)
			{
				df.SetDependencies(this);
				DependentFunctions.Add(df);
			}
			else
			{
				throw new ArgumentException($"{nameof(BatchAggregator)} does not support {nameof(IFunction)} type {nameof(F)}.");
			}
			return func;
		}

		public void Aggregate(double samplePoint)
		{
			for(int i = 0; i < AggregateFunctions.Count; ++i)
				AggregateFunctions[i].Aggregate(samplePoint);
		}

		private F GetFunction<F>(out bool wasAlreadyAdded)
			where F : class, IFunction, new()
		{
			var func =
				AggregateFunctions.Find(a => a is F) as F ??
				DependentFunctions.Find(a => a is F) as F ;

			return (wasAlreadyAdded = func != null) ?
				func : new F();
		}
	}
}
