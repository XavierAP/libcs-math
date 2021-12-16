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
			var func = GetFunction<F>();
			if(func != null)
				return func;

			func = new F();

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

		public double GetResult<F>()
			where F : class, IFunction
		{
			var afunc = GetFunction<F>();
			if (null == afunc) throw new KeyNotFoundException(
				$"A {nameof(IFunction)} of type {nameof(F)} was not Added to this {nameof(BatchAggregator)}.");

			return afunc.GetResult();
		}

		/// <summary>Returns null if this type has not been Added yet.</summary>
		private F GetFunction<F>()
			where F : class, IFunction
		{
			return
				AggregateFunctions.Find(a => a is F) as F ??
				DependentFunctions.Find(a => a is F) as F ;
		}
	}
}
