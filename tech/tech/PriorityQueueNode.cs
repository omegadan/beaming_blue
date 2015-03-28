using System;

namespace tech
{
	/// <summary>
	/// A node for the priortiy queue -- we could have used generics but its a bit overkill for this problem.
	/// </summary>
	public class PriorityQueueNode
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="tech.PriorityQueueNode"/> class.
		/// </summary>
		/// <param name="key">Priority (lower is first).</param>
		public PriorityQueueNode(int key)
		{
			this.Key = key;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="tech.PriorityQueueNode"/> class.
		/// </summary>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		/// <param name="isBlue">If set to <c>true</c> represents a Blue pixel.</param>
		public PriorityQueueNode(UInt16 x, UInt16 y, bool isBlue)
		{
			this.X = x;
			this.Y = y;

			this.IsBlue = isBlue;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="tech.PriorityQueueNode"/> class.
		/// </summary>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		/// <param name="isBlue">If set to <c>true</c> represents a Blue pixel.</param>
		/// <param name="key">Priority (lower is first).</param>
		public PriorityQueueNode(UInt16 x, UInt16 y, bool isBlue, int key)
		{
			this.X = x;
			this.Y = y;

			this.IsBlue = isBlue;
			this.Key = key;
		}

		#endregion Constructors

		#region Public Properties

		public bool IsBlue
		{
			get; set;
		}

		/// <summary>
		/// The key by which the heap property is computed.
		/// </summary>
		/// <value>The key.</value>
		public int Key
		{
			get; set;
		}

		public UInt16 X
		{
			get; set;
		}

		public UInt16 Y
		{
			get; set;
		}

		#endregion Public Properties

		#region Public Methods

		/*
		* Taxi cost results were inferior to path length
		public void ComputeKey(PriorityQueueNode goal) {
		this.Key = _compute_taxi_cost (X, Y, goal.X, goal.Y);
		}

		protected float _compute_taxi_cost( int X1, int Y1, int X2, int Y2 ) {
		return Math.Abs (X1 - X2) + Math.Abs (Y1 - Y2);
		}
		*/
		/// <summary>
		/// Hash the X,Y coordinates for Hash Table insertions
		/// </summary>
		/// <returns>A hash of the XY coords.</returns>
		public static UInt32 EncodeXY(UInt16 x, UInt16 y)
		{
			return ( ((UInt32) x)  << 16) + y;
		}

		/// <summary>
		/// Return an encoding for this instance.
		/// </summary>
		public UInt32 Encoding()
		{
			return EncodeXY(X, Y);
		}

		#endregion Public Methods
	}
}