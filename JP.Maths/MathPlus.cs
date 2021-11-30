using System;

namespace JP.Maths
{
	/// <summary>Miscellaneous math methods.</summary>
	public static class MathPlus
	{
		/// <summary>Linear interpolation (or extrapolation).</summary>
		public static double Interpolate(double x1, double y1, double x2, double y2, double x)
		{
			if(x1 == x2) throw new DivideByZeroException();
			return y1 + (y2 - y1) * (x - x1) / (x2 - x1);
		}
	}
}
