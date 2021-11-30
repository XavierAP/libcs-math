using System;
using System.Collections.Generic;

namespace JP.Maths.Statistics
{
	/// <summary>Calculates multiple <see cref="IAggregateFunction"/>s
	/// by iterating only once through the sample or population.</summary>
	public class BatchAggregator : IBatchAggregator
	{
		private readonly List<IAggregateFunction> Functions = new List<IAggregateFunction>();
		
		public void Add<T>()
			where T : IAggregateFunction, new()
		{
			if(null != GetFunction<T>())
				return;

			var afunc = new T();
			Functions.Add(afunc);
			afunc.AddDependenciesTo(this);
		}

		public void Aggregate(double samplePoint)
		{
			for(int i = 0; i < Functions.Count; ++i)
				Functions[i].Aggregate(samplePoint);
		}

		public double GetResult<T>()
			where T : IAggregateFunction
		{
			var afunc = GetFunction<T>();
			if (null == afunc) throw new KeyNotFoundException(
				$"A {nameof(IAggregateFunction)} of type {nameof(T)} was not Added to this object.");

			return afunc.GetResult(this);
		}

		/// <summary>Returns null if this type has not been Added yet.</summary>
		private IAggregateFunction GetFunction<T>()
			where T : IAggregateFunction
		{
			return Functions.Find(a => a is T);
		}
	}
}
