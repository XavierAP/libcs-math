using System;

namespace JP.Maths
{
	using fn = Func<double, double>; // scalar function type

	/// <summary>Solvers for scalar equations.</summary>
	public static class Solve
	{
		/// <summary>Tries to find a root (zero) of f, by Newton's method.</summary>
		/// <param name="f">Function whose root is to be found.</param>
		/// <param name="df">Derivative of f.</param>
		/// <param name="x0">Initial value. Ideally close to the root.</param>
		/// <param name="precision">Precision digits with which the root is to be found.</param>
		/// <returns>Root found, rounded off according to 'precision'.</returns>
		/// <remarks>On failure to find a root, it may return a non-real number, or never
		/// return (consider running asynchronously and aborting on time out).</remarks>
		public static double
		Newton(fn f, fn df, double x0, byte precision)
		{
			double tol = GetPrecisionTolerance(precision);
			double x1 = x0;
			do
			{
				x0 = x1;
				x1 = x0 - f(x0) / df(x0);
			}
			while(tol <= Math.Abs(x1 - x0));

			return Math.Round(x1, precision);
		}
		
		/// <summary>Tries to find a root (zero) of f, by the secant method.</summary>
		/// <param name="f">Function whose root is to be found.</param>
		/// <param name="x0">Initial value. Ideally close to the root.</param>
		/// <param name="x1">Another initial value. Ideally close to the root.</param>
		/// <param name="precision">Precision digits with which the root is to be found.</param>
		/// <returns>Root found, rounded off according to 'precision'.</returns>
		/// <remarks>On failure to find a root, it may return a non-real number, or never
		/// return (consider running asynchronously and aborting on time out).</remarks>
		public static double
		Secant(fn f, double x0, double x1, byte precision)
		{
			if(x0 == x1) throw new DivideByZeroException();

			double tol = GetPrecisionTolerance(precision);
			while(tol <= Math.Abs(x1 - x0))
			{
				double
					f0 = f(x0),
					f1 = f(x1);

				double x2 = x1 - f1 * (x1 - x0) / (f1 - f0);

				x0 = x1;
				x1 = x2;
			}
			return Math.Round(x1, precision);
		}

		private static double GetPrecisionTolerance(byte precision) => Math.Pow(10, -precision) / 2;
	}
}
