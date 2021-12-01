using System;
using System.Collections.Generic;

namespace JP.Maths.Statistics
{
	/// <summary>Calculates multiple <see cref="IAggregateFunction"/>s
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

		public void Add<F>()
			where F : IFunction, new()
		{
			if(IsAlreadyAdded<F>())
				return;

			var f = new F();

			if(f is IAggregateFunction af)
			{
				AggregateFunctions.Add(af);
			}
			else if(f is IDependentFunction df)
			{
				df.SetDependencies(this);
				DependentFunctions.Add(df);
			}
			else
			{
				throw new ArgumentException($"{nameof(BatchAggregator)} does not support {nameof(IFunction)} type {nameof(F)}.");
			}
		}

		public void Aggregate(double samplePoint)
		{
			for(int i = 0; i < AggregateFunctions.Count; ++i)
				AggregateFunctions[i].Aggregate(samplePoint);
		}

		public double GetResult<T>()
			where T : IFunction
		{
			var afunc = GetFunction<T>();
			if (null == afunc) throw new KeyNotFoundException(
				$"A {nameof(IFunction)} of type {nameof(T)} was not Added to this {nameof(BatchAggregator)}.");

			return afunc.Result;
		}

		/// <summary>Returns null if this type has not been Added yet.</summary>
		private IFunction GetFunction<F>()
			where F : IFunction
		{
			return
				AggregateFunctions.Find(a => a is F) ??
				DependentFunctions.Find(a => a is F) as IFunction ;
		}

		private bool IsAlreadyAdded<F>()
			where F : IFunction, new()
		{
			return null != GetFunction<F>();
		}
	}
}
