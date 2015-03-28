using System;
using System.Collections.Generic;

namespace tech
{
	/// <summary>
	/// A very simple solution tree.
	/// </summary>
	public class Tree<Type>
	{
		#region Constructors

		public Tree()
		{
			Clear ();
		}

		#endregion Constructors

		#region Public Properties

		/// <summary>
		/// The root node for this tree.
		/// </summary>
		public TreeNode<Type> Root
		{
			get; set;
		}

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Add a root node.
		/// </summary>
		/// <param name="payload">The payload for the node to carry.</param>
		public void AddRoot(Type payload)
		{
			Root = new TreeNode<Type> (payload);
		}

		/// <summary>
		/// Remove the root node.
		/// </summary>
		public void Clear()
		{
			Root = null;
		}

		#endregion Public Methods
	}
}