using System;
using System.Collections.Generic;
using System.Drawing;

namespace tech
{
	/// <summary>
	/// Graphilator Solves mazes.
	/// 
	/// It does this by implementing a Uniform Cost Search.
	/// 
	/// Constraints:
	/// Both dimensions of source image must be less than 64k.
	/// Loaded image must containe at least one blue and red pixel.
	/// Paths must be white.
	/// Walls must be black.
	/// 
	/// Process:
	/// 
	/// 1. Call Load() with a file name of a bitmap. 
	/// 2. Call Setup() to searche for start and end nodes.
	/// 3. Call Search() Performs the Uniform Cost Search (more about that below) outputs the child leaf node to _solution.
	/// 4. DrawResults() Draws the results back onto the image.                                      
	/// 
	/// </summary>
	public class Graphilator
	{
		#region Fields

		/// <summary>
		/// List of Blue nodes (populated by Setup()).
		/// </summary>
		protected List<PriorityQueueNode> _blue = new List<PriorityQueueNode>();

		/// <summary>
		/// A hash which allows us to compute membership in explored quickly. Indexed by XY coordinate encoding.
		/// </summary>
		protected Dictionary<UInt32, PriorityQueueNode> _explored = new Dictionary<UInt32, PriorityQueueNode> ();

		/// <summary>
		/// Priority queue which holds unexplored nodes.
		/// </summary>
		protected PriorityQueue _frontier = new PriorityQueue ();

		/// <summary>
		/// A hash which allows us to compute membership in frontier quickly. Indexed by XY coordinate encoding.
		/// </summary>
		protected Dictionary<UInt32, PriorityQueueNode> _frontier_membership = new Dictionary<UInt32, PriorityQueueNode> ();

		/// <summary>
		/// List of Red nodes (populated by Setup()).
		/// </summary>
		protected List<PriorityQueueNode> _red = new List<PriorityQueueNode> ();

		/// <summary>
		/// The end node of the solution, populated by Search().
		/// </summary>
		protected TreeNode<PriorityQueueNode> _solution;

		/// <summary>
		/// Search tree to track results of traversal.
		/// </summary>
		protected Tree<PriorityQueueNode> _tree = new Tree<PriorityQueueNode> ();

		/// <summary>
		/// A hash of tree_nodes for fast lookup. Indexed by XY coordinate encoding.
		/// </summary>
		protected Dictionary<UInt32, TreeNode<PriorityQueueNode>> _tree_parts = new Dictionary<uint, TreeNode<PriorityQueueNode>>();

		#endregion Fields

		#region Protected Properties

		protected Bitmap Source
		{
			get; set;
		}

		#endregion Protected Properties

		#region Public Properties

		/// <summary>
		/// The full path name to the bitmap to load.                              
		/// </summary>
		/// <value>The full path.</value>
		public string FullPath
		{
			get; set;
		}

		#endregion Public Properties

		#region Protected Methods

		/// <summary>
		/// Get the minimum PriorityQueue Node value on the queue keeping _frontier and _frontier_membership in sync.
		/// </summary>
		/// <returns>The minimum PriorityQueueNode</returns>
		protected PriorityQueueNode _frontier_get_min()
		{
			var queue_node = _frontier.ExtractMin ();
			_frontier_membership.Remove (queue_node.Encoding());

			return queue_node;
		}

		/// <summary>
		/// Insert an item into the frontier keeping membership and prioritty queue in sync.
		/// </summary>
		protected void _frontier_insert(PriorityQueueNode node)
		{
			_frontier.Insert (node);
			_frontier_membership.Add (node.Encoding(), node);
		}

		/// <summary>
		/// Compute a list of neighboring pixels that are a part of the path. In order N, E, S, W.
		/// </summary>
		/// <returns>The neighbors.</returns>
		/// <param name="node">Node.</param>
		protected List<PriorityQueueNode> _get_neighbors(PriorityQueueNode node)
		{
			UInt16 X = node.X;
			UInt16 Y = node.Y;

			var ret = new List<PriorityQueueNode> ();

			int north_y = Y - 1;
			if ( north_y >= 0 && _pixel_is_not_black(X, (UInt16)north_y) ) {
				ret.Add(new PriorityQueueNode(X, (UInt16)north_y, _pixel_is_blue(X, (UInt16)north_y), node.Key+1));
			}

			int east_x = X + 1;
			if ( east_x < Source.Width && _pixel_is_not_black((UInt16)east_x, Y) ) {
				ret.Add(new PriorityQueueNode((UInt16)east_x, Y, _pixel_is_blue((UInt16)east_x, Y), node.Key+1));
			}

			int south_y = Y + 1;
			if ( south_y < Source.Height && _pixel_is_not_black(X, (UInt16)south_y) ) {
				ret.Add(new PriorityQueueNode(X, (UInt16)south_y, _pixel_is_blue(X, (UInt16)south_y), node.Key+1));
			}

			int west_x = X - 1;
			if ( west_x >= 0 && _pixel_is_not_black((UInt16)west_x, Y) ) {
				ret.Add(new PriorityQueueNode((UInt16)west_x, Y, _pixel_is_blue((UInt16)west_x, Y), node.Key+1));
			}

			return ret;
		}

		protected bool _is_not_explored(PriorityQueueNode node)
		{
			return !_explored.ContainsKey (node.Encoding());
		}

		protected bool _is_not_in_frontier(PriorityQueueNode node)
		{
			return !_frontier_membership.ContainsKey (node.Encoding());
		}

		protected bool _pixel_is_blue(int color)
		{
			return (color & 0x0000FF) != 0;
		}

		protected bool _pixel_is_blue(UInt16 X, UInt16 Y)
		{
			var color = Source.GetPixel (X, Y);
			return color.B > 0 && color.R == 0 && color.G == 0;
		}

		protected bool _pixel_is_not_black(UInt16 X, UInt16 Y)
		{
			var color = Source.GetPixel (X, Y).ToArgb ();
			return !((color & 0x00FFFFFF) == 0);
		}

		protected bool _pixel_is_not_black(int color)
		{
			return !((color & 0x00FFFFFF) == 0);
		}

		/// <summary>
		/// Sorts a pixel into the _blue or _red bin.
		/// </summary>
		/// <param name="x">X coordinate of pixel.</param>
		/// <param name="y">Y cooridnate of pixel.</param>
		protected void _sort_pixel(UInt16 x, UInt16 y)
		{
			var c = Source.GetPixel(x, y);
			var pix = c.ToArgb () & 0x00FFFFF;

			bool r = (pix & 0xFF0000) != 0;
			bool g = (pix & 0x00FF00) != 0;
			bool b = (pix & 0x0000FF) != 0;

			//Pixel is black, we don't need it on our graph
			if( r && g && b )
				return;

			var node = new PriorityQueueNode (x, y, _pixel_is_blue(pix));

			if (r)
				_red.Add (node);

			if (b)
				_blue.Add (node);
		}

		#endregion Protected Methods

		#region Public Methods

		public void Clear()
		{
			_blue.Clear ();
			_red.Clear ();
			_solution = null;
			_tree.Clear ();
			_explored.Clear ();
			_frontier_membership.Clear ();
			_frontier.Clear ();
			Source = null;
			FullPath = null;

			foreach (var t in _tree_parts.Values) {
				t.Clear ();
			}

			_tree_parts.Clear ();
		}

		/// <summary>
		/// Draws the results of the saerch onto the Bitmap.
		/// </summary>
		/// <returns><c>true</c>, true if successful, <c>false</c> otherwise.</returns>
		public bool DrawResults()
		{
			if (_solution == null) {
				return false;
			}

			//Traverse the solution tree backwards
			while ( _solution != null ) {
				Source.SetPixel (_solution.Payload.X, _solution.Payload.Y, Color.Green);
				_solution = _solution.Parent;
			}

			return true;
		}

		/// <summary>
		/// Load the specified image.
		/// </summary>
		/// <param name="fullPath">The image.</param>
		public bool Load(string fullPath)
		{
			if( String.IsNullOrEmpty(fullPath) )
				return false;

			FullPath = fullPath;

			try {
				Source = (Bitmap)Bitmap.FromFile (fullPath);
			} catch(Exception)
			{
				return false;
			}

			//
			if (Source.Width > 65535 || Source.Height > 65535)
				return false;

			return true;
		}

		/// <summary>
		/// Save the results of the path search as a bitmap.
		/// </summary>
		/// <returns><c>true</c>, if results was saved, <c>false</c> otherwise.</returns>
		/// <param name="fullPath">Full path to save bitmap to.</param>
		public bool SaveResults(string fullPath)
		{
			if( String.IsNullOrEmpty(fullPath) )
				return false;

			try {
				Source.Save (fullPath);
			} catch (Exception)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Search the Graph, must have called Load() and Setup() first. (This is the big daddy).
		/// </summary>
		public bool Search()
		{
			if (_blue.Count == 0 || _red.Count == 0)
				return false;

			//Grab a start and end goal.
			var goal = _blue [0];
			var start = _red [0];

			int count = 0;

			//Initial cost is 0
			start.Key = 0;

			//Set initial conditions
			var graph_node = start;											//graph_node is the current graph node we're working on
			_tree.AddRoot (start);											//the root of solution tree is the start node
			var tree_node = _tree.Root;										//tree_node is the current graph nodes solution node
			_tree_parts.Add (graph_node.Encoding (), tree_node);			//store all pieces of the tree

			_frontier_insert (graph_node);									//graph_node is the first piece of the frontier

			do {
				if (_frontier.Length == 0) {
					//No solution found
					_solution = null;
					return false;
				}

				//Pull the lowest node from the priortiy queue, and grab its
				graph_node = _frontier_get_min();
				tree_node  = _tree_parts[graph_node.Encoding()];

				//Output a status for long output
				count++;
				if( count % 1000 == 0 ) { Console.Write("."); }

				//Solution has been found, terminate search
				if( graph_node.IsBlue ) {
					_solution = tree_node;
					Console.WriteLine ("");
					return true;
				}

				_explored.Add ( graph_node.Encoding(), graph_node) ;
				//Source.SetPixel (graph_node.X, graph_node.Y, Color.Green);

				//Add each neighbor to the fontier if they haven't been explored
				foreach(var neighbor in _get_neighbors(graph_node) ) {

					if( _is_not_explored( neighbor ) ) {

						var t = tree_node.AddChild( neighbor) ;
						if (!_tree_parts.ContainsKey( neighbor.Encoding() ))
							_tree_parts.Add( neighbor.Encoding(), t);

						if ( _is_not_in_frontier (neighbor) ) {
							_frontier_insert(neighbor);
						}

					} 	//else
						//if ( !_is_not_in_frontier(neighbor) && _frontier_membership[neighbor.Encoding()].Key > neighbor.Key) {
						//queue_node = neighbor;
						//Reduce key cost in official algo, not needed here.
						//}

				}

			} while (true);
		}

		/// <summary>
		/// Set the initial conditions for the search by looking for blue and red pixels.
		/// </summary>
		public bool Setup()
		{
			if (Source == null)
				return false;

			for (UInt16 x = 0; x < Source.Width; x++) {
				for (UInt16 y = 0; y < Source.Height; y++) {
					_sort_pixel (x, y);
				}
			}

			//We must find at least one starting and ending point
			return _red.Count != 0 && _blue.Count != 0;
		}

		#endregion Public Methods
	}
}