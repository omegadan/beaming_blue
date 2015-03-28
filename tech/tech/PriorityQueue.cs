using System;

namespace tech
{
	/// <summary>
	/// A prority 2ueue. 
	/// 
	/// Based on: Introduction to Algorithms 3rd edition, page 151.
	/// 
	/// Not going to extensively document a PQ.
	/// </summary>
	public class PriorityQueue
	{
		#region Fields

		protected int _heap_size = 0;
		protected PriorityQueueNode[] _nodes;

		#endregion Fields

		#region Constructors

		public PriorityQueue()
		{
			Clear ();
		}

		#endregion Constructors

		#region Public Properties

		public int Length
		{
			get { return _heap_size; }
		}

		#endregion Public Properties

		#region Protected Methods

		protected void _exchange( int i, int j )
		{
			var pqn = _nodes[i];
			_nodes [i] = _nodes [j];
			_nodes [j] = pqn;
		}

		protected int _left(int i)
		{
			return 2*i;
		}

		protected int _parent(int i)
		{
			return i/2;
		}

		protected int _right(int i)
		{
			return 2*i+1;
		}

		#endregion Protected Methods

		#region Public Methods

		/// <summary>
		/// Reset the Priority Queue
		/// </summary>
		public void Clear()
		{
			_heap_size = 0;
			_nodes = new PriorityQueueNode[2];
		}

		/// <summary>
		/// Remove and return the minimum Key'd value.
		/// </summary>
		/// <returns>The minimum.</returns>
		public PriorityQueueNode ExtractMin()
		{
			var min_val = _nodes [1];
			_nodes [1] = _nodes [_heap_size];
			_nodes [_heap_size] = null;
			_heap_size--;

			int i = 1;

			while (i < _heap_size) {

				int min = i;
				int l = _left(i);
				int r = _right(i);

				if ( l <= _heap_size &&  _nodes[ l ].Key < _nodes[min].Key )
					min = l;

				if ( r <= _heap_size && _nodes[r].Key < _nodes[min].Key )
					min = r;

				if( min != i ) {
					_exchange(i, min);
					i = min;
				} else break;
			}

			return min_val;
		}

		/// <summary>
		/// Insert node into the Queue.
		/// </summary>
		/// <param name="node">The node..</param>
		public void Insert(PriorityQueueNode node)
		{
			_heap_size += 1;
			if (_heap_size >= _nodes.Length) {
				Array.Resize<PriorityQueueNode> (ref _nodes, _nodes.Length * 2);
			}

			_nodes [_heap_size] = node;

			int i = _heap_size;
			while (i > 1)
			{
				if (_nodes[ _parent(i) ].Key <= _nodes[i].Key )
					break;

				_exchange(_parent(i), i);
				i = _parent (i);
				}
		}

		/// <summary>
		/// Return the minimum Key'd value.
		/// </summary>
		public PriorityQueueNode Min()
		{
			return _nodes[1];
		}

		#endregion Public Methods
	}
}