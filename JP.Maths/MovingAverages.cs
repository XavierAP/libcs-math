using System;
using System.Collections.Generic;
using System.Linq;

namespace JP.Maths
{
	/// <summary>Base interface for all moving averages.</summary>
	public interface IMovingAverage
	{
		/// <summary>Add a new data point to average.</summary>
		void AddDatum(double newDatum);
		/// <summary>The current value of the average.</summary>
		double Value { get; }
		/// <summary>Number of data points currently averaged.</summary>
		int SizeDiscrete { get; }
		/// <summary>Offset of the moving average relative to the
		/// original signal, in number of data points.</summary>
		double DelayDiscrete { get; }
	}

	/// <summary><see cref="IMovingAverage"/> that includes
	/// dimensional information for the abscissa, e.g. time.</summary>
	public interface IMovingAverageDimensional
		:IMovingAverage
	{
		/// <summary>Units of the abscissa of this signal, e.g. "seconds".</summary>
		string XUnits { get; }
		/// <summary>Distance between each two consecutive
		/// data points, in <see cref="XUnits"/>.</summary>
		double Step  { get; }
		/// <summary>Range currently averaged, in <see cref="XUnits"/>.</summary>
		double Size { get; }
		/// <summary>Offset of the moving average relative to the
		/// original signal, in <see cref="XUnits"/>.</summary>
		double Delay { get; }
	}

	internal static class MovingAverageCalculator
	{
		internal static int GetSizeDiscrete(double intendedSize, double stepSize)
		{
			if(intendedSize <= 0d || stepSize <= 0d) throw new ArgumentOutOfRangeException(
				"intendedSize and stepSize must be greater than zero");
			
			var size = (int)Math.Round(intendedSize / stepSize);
			if(size < 1) size = 1;
			return size;
		}

		internal static double GetDelay(int sizeDiscrete)
			=> 0.5d * (double)(sizeDiscrete - 1);
	}

	/// <summary><see cref="IMovingAverage"/> with a
	/// constant number of averaged data points.</summary>
	/// <remarks>Optimized for continuous retrieval of <see cref="Value"/>.
	/// Before as many points as the <see cref="SizeDiscrete"/> are added,
	/// they are initialized to zero.</remarks>
	public class MovingAverageFixed
		:IMovingAverage
	{
		/// <summary>Creates a new <see cref="MovingAverageFixed"/>
		/// with the specified <see cref="SizeDiscrete"/>.</summary>
		public MovingAverageFixed(int size)
		{
			if(size < 1) throw new ArgumentOutOfRangeException(
				"size", size, "Cannot average less than 1 value!");

			this.SizeDiscrete = size;
			this.DelayDiscrete = MovingAverageCalculator.GetDelay(size);
			this.Weight = 1d / (double)size;

			_Value = 0d;
			// I have to initialize the queue for AddPoint() to work.
			Buffer = new Queue<double>(new double[size]); // zeroes--see C# spec.
		}

		private readonly object Locker = new object();

		/// <summary>Add a new data point to average -- while de-queuing the oldest one.</summary>
		public void AddDatum(double newDatum)
		{
			lock(Locker)
			{
				_Value += (newDatum - Buffer.Dequeue()) * Weight;
				Buffer.Enqueue(newDatum);
			}
		}

		private readonly Queue<double> Buffer;
		private readonly double Weight;

		/// <summary>Number of data points currently averaged.</summary>
		public int SizeDiscrete { get; private set; }
		/// <summary>Offset of the moving average relative to the
		/// original signal, in number of data points.</summary>
		public double DelayDiscrete { get; private set; }


		/// <summary>The current value of the average.</summary>
		public double Value
		{
			get { lock(Locker) { return _Value; } }
		}
		private double _Value;
	}

	/// <summary><see cref="MovingAverageFixed"/> that
	/// is also a <see cref="IMovingAverageDimensional"/>.</summary>
	public class MovingAverageFixedDimensional
		:MovingAverageFixed, IMovingAverageDimensional
	{
		/// <summary>Creates a new <see cref="MovingAverageFixedDimensional"/> with the
		/// specified units and the <see cref="Size"/> closest to intendedSize
		/// that is an exact multiple of stepSize and greater than zero.</summary>
		public MovingAverageFixedDimensional(string xUnits, double intendedSize, double stepSize)
			:base(MovingAverageCalculator.GetSizeDiscrete(intendedSize, stepSize))
		{
			this.XUnits = xUnits;
			this.Step = stepSize;
			this.Size = this.SizeDiscrete * stepSize;
			this.Delay = this.DelayDiscrete * stepSize;
		}

		/// <summary>Units of the abscissa of this signal, e.g. "seconds".</summary>
		public string XUnits { get; private set; }
		/// <summary>Distance between each two consecutive
		/// data points, in <see cref="XUnits"/>.</summary>
		public double Step { get; private set; }
		/// <summary>Range currently averaged, in <see cref="XUnits"/>.</summary>
		public double Size { get; private set; }
		/// <summary>Offset of the moving average relative to the
		/// original signal, in <see cref="XUnits"/>.</summary>
		public double Delay { get; private set; }
	}

	/// <summary><see cref="IMovingAverage"/> with a flexible
	/// number of averaged data points.</summary>
	/// <remarks>All data points are counted towards the average,
	/// no point is de-queued automatically, but they can be de-queued
	/// manually by setting the size.</remarks>
	public class MovingAverageFlexible
		:IMovingAverage
	{
		/// <summary>Creates a new <see cref="MovingAverageFlexible"/>.</summary>
		public MovingAverageFlexible()
		{
			Buffer = new Queue<double>();
		}

		/// <summary>Creates a new <see cref="MovingAverageFlexible"/>, allocating
		/// the specified initial capacity for its internal storage.</summary>
		public MovingAverageFlexible(int capacity)
		{
			Buffer = new Queue<double>(capacity);
		}

		private readonly object Locker = new object();

		private Queue<double> Buffer;

		/// <summary>Add a new data point to average.</summary>
		public void AddDatum(double newDatum)
		{ lock(Locker) { Buffer.Enqueue(newDatum); } }

		/// <summary>Gets the current value of the average.
		/// If no data points have been added, returns double.NaN.</summary>
		public double Value
		{ get { lock(Locker) {
			if(SizeDiscrete > 0)
				return Buffer.Average();
			else
				return double.NaN;
		} } }

		/// <summary>Gets or sets the number of data points. Trying
		/// to increase the current size has no effect.</summary>
		public int SizeDiscrete
		{
			get { lock(Locker) { return Buffer.Count; } }
			set
			{ lock(Locker) {
				if(value < 0) throw new ArgumentOutOfRangeException("SizeDiscrete",
					value, "This property may not be set to less than 0.");

				for(int i = Buffer.Count - value; i > 0; --i)
					Buffer.Dequeue();
			} }
		}

		/// <summary>Removes all data points. It has the same effect
		/// as setting <see cref="SizeDiscrete"/> to 0.</summary>
		public void Clear()
		{ lock(Locker) { Buffer.Clear(); } }

		/// <summary>Offset of the moving average relative to the
		/// original signal, in number of data points.</summary>
		public double DelayDiscrete
		{ get { return MovingAverageCalculator.GetDelay(SizeDiscrete); } }
	}

	/// <summary><see cref="MovingAverageFlexible"/> that is
	/// also a <see cref="IMovingAverageDimensional"/>.</summary>
	public class MovingAverageFlexibleDimensional
		:MovingAverageFlexible, IMovingAverageDimensional
	{
		/// <summary>Creates a new <see cref="MovingAverageFlexibleDimensional"/>
		/// with the specified <see cref="XUnits"/> and <see cref="Step"/>.</summary>
		public MovingAverageFlexibleDimensional(string xUnits, double stepSize)
			:base()
		{
			this.XUnits = xUnits;
			this.Step = stepSize;
		}

		/// <summary>Creates a new <see cref="MovingAverageFlexibleDimensional"/> with
		/// the specified <see cref="XUnits"/> and <see cref="Step"/>, allocating enough
		/// internal capacity for approximately capacity <see cref="XUnits"/>.</summary>
		public MovingAverageFlexibleDimensional(string xUnits, double capacity, double stepSize)
			:base(MovingAverageCalculator.GetSizeDiscrete(capacity, stepSize))
		{
			this.XUnits = xUnits;
			this.Step = stepSize;
		}

		/// <summary>Units of the abscissa of this signal, e.g. "seconds".</summary>
		public string XUnits { get; private set; }

		/// <summary>Distance between each two consecutive
		/// data points, in <see cref="XUnits"/>.</summary>
		public double Step { get; private set; }

		/// <summary>Range currently averaged, in <see cref="XUnits"/>.</summary>
		public double Delay
		{ get { return DelayDiscrete * Step; } }

		/// <summary>Offset of the moving average relative to the
		/// original signal, in <see cref="XUnits"/>.</summary>
		public double Size
		{
			get { return SizeDiscrete * Step; }
			set { SizeDiscrete = (int)Math.Ceiling(Size/Step); }
		}
	}
}