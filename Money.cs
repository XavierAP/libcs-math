using System;
using System.Collections.Generic;
using System.Linq;

namespace JP.Maths
{
	/// <summary>Collection of financial utility methods.</summary>
	/// <remarks>Rates are per 1, not per cent.</remarks>
	public static class Money
	{
		private const byte
			precisionDefault = 2, // default number of decimal places for money amounts
			precisionDefaultPer1 = precisionDefault + 2; // default number of decimal places for rates per 1

		/// <summary>Calculates the discount/interest rate that makes the net present
		/// value of a set of cash flows equal to a given present value.</summary>
		/// <param name="flows">Past or future cash flows. Needn't be ordered.
		/// Must not be empty, and not all elements may have the same Day.
		/// Cash: positive means money earned, and negative spent.
		/// Day: only whole days elapsed from this to 'present.Day'
		/// will be considered in the calculation, not fractions.</param>
		/// <param name="present">Present Cash value and Day.</param>
		/// <param name="precision">Precision digits with which the rate is to be found.</param>
		/// <param name="guess">Initial guess of rate for the solver.</param>
		/// <returns>Yearly rate per 1, rounded off according to 'precision';
		/// NaN if 'flows' is empty; or possibly infinite or nonsense value on failure to solve.</returns>
		public static double
		SolveRateInvest(IEnumerable<(double Cash, DateTime Day)> flows,
			(double Cash, DateTime Day) present,
			byte precision = precisionDefaultPer1,
			double guess = 0 )
		{
			// Cache flows into array and translate for more efficient iteration:
			var flowsDiff = TranslateFlowToDiff(present.Day, flows).ToArray();
			if(flowsDiff.Length < 1) return double.NaN;
			
			return Solve.Newton(
				r => present.Cash + CalcNetPresentValue(r, flowsDiff),
				r => DerivNetPresentValue(r, flowsDiff),
				guess, precision);
		}

		/// <summary>Calculates the discount/interest rate that makes zero
		/// the net present value of a set of cash flows.</summary>
		/// <param name="flows">Cash flows. Needn't be ordered.
		/// Must contain more than one element, and not all may have the same Day.
		/// Cash: positive means money earned, and negative spent.
		/// Day: fractions of days are not considered.</param>
		/// <param name="precision">Precision digits with which the rate is to be found.</param>
		/// <returns>Yearly rate per 1, rounded off according to 'precision'.</returns>
		public static double
		SolveRateInvest(IEnumerable<(double Cash, DateTime Day)> flows,
			byte precision = precisionDefaultPer1,
			double guess = 0 )
		{
			return SolveRateInvest(flows.Skip(1), flows.First(), precision, guess);  // which time is considered "present" may be arbitrary, it shouldn't affect the solution -- perhaps the convergence.
		}


		/// <summary>Calculates the net present value/worth (NPV) of a set of cash flows.</summary>
		/// <param name="rate">Interest/discount rate.</param>
		/// <param name="flowsDiff">Past or future cash flows. Needn't be ordered.
		/// Cash: positive means money earned, and negative spent.
		/// Years: elapsed from the time of the flow to the present.</param>
		public static double
		CalcNetPresentValue(double rate, IEnumerable<(double Cash, double Years)> flowsDiff)
		{
			rate += 1;
			return (
				from cf in flowsDiff
				select cf.Cash * Math.Pow(rate, cf.Years)
				).Sum();
		}

		/// <summary>Calculates the net present value/worth of a set of cash flows.</summary>
		/// <param name="rate">Interest/discount rate.</param>
		/// <param name="flowsDiff">Past or future cash flows. Needn't be ordered.
		/// Cash: positive means money earned, and negative spent.
		/// Day: only whole days elapsed from this to 'presentDate'
		/// will be considered in the calculation, not fractions.</param>
		/// <param name="presentDate">Present time.</param>
		public static double
		CalcNetPresentValue(double rate, IEnumerable<(double Cash, DateTime Day)> flows, DateTime presentDate)
		{
			return CalcNetPresentValue(rate, TranslateFlowToDiff(presentDate, flows));
		}

		/// <summary>Calculates the derivative of the NPV with respect to the interest rate.</summary>
		/// <param name="rate">Interest/discount rate; independent variable of the derivative.</param>
		/// <param name="flowsDiff">Past or future cash flows. Needn't be ordered; parameters.</param>
		private static double
		DerivNetPresentValue(double rate, IEnumerable<(double Cash, double Years)> flowsDiff)
		{
			rate += 1;
			return (
				from cf in flowsDiff
				select cf.Cash * cf.Years * Math.Pow(rate, cf.Years - 1)
				).Sum();
		}


		/// <summary>Translates a set of cash flows from (double, DateTime) to (double, Years).
		/// This improves efficiency in case the NPV of the same cash flows at different rates
		/// needs to be calculated e.g. by an interative solver.</summary>
		/// <param name="presentDate">Present time.</param>
		/// <param name="flows">Cash flows.
		/// Cash: positive means money earned, and negative spent.
		/// Day: only whole days elapsed from this to 'presentDate'
		/// will be considered in the calculation, not fractions.</param>
		private static IEnumerable<(double Cash, double Years)>
		TranslateFlowToDiff(DateTime presentDate, IEnumerable<(double Cash, DateTime Day)> flows)
		{
			presentDate = presentDate.ToLocalTime(); // DateTimes must be alike referenced before subtracting
			return (
				from cf in flows
				select ( Cash: cf.Cash, Years: (double)(presentDate - cf.Day.ToLocalTime()).Days / 365.25 )
				);
		}
	}
}
