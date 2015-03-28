using System;
using System.Collections.Generic;

namespace tech
{
	/// <summary>
	/// The solution tree node.
	/// </summary>
	public class TreeNode<Type>
	{
		#region Constructors

		/// <summary>
		/// Create a new instance of TreeNode with Payload set.
		/// </summary>
		/// <param name="Payload">The payload for this node to carry.</param>
		public TreeNode(Type Payload)
		{
			this.Payload = Payload;
			Children = new List<TreeNode<Type>>();
		}

		/// <summary>
		/// Create a new instance of TreeNode.
		/// </summary>
		public TreeNode()
		{
			Children = new List<TreeNode<Type>>();
		}

		#endregion Constructors

		#region Public Properties

		/// <summary>
		/// A list of this Nodes Children.
		/// </summary>
		public List<TreeNode<Type>> Children
		{
			get; set;
		}

		/// <summary>
		/// This node's parent.
		/// </summary>
		public TreeNode<Type> Parent
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the payload.
		/// </summary>
		/// <value>The payload.</value>
		public Type Payload
		{
			get; set;
		}

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Add a child to this tree.
		/// </summary>
		/// <returns>The newly created child node.</returns>
		/// <param name="Payload">The payload for the child to carry.</param>
		public TreeNode<Type> AddChild( Type Payload )
		{
			TreeNode<Type> node = new TreeNode<Type> (Payload);
			node.Parent = this;
			Children.Add(node);

			return node;
		}

		/// <summary>
		/// Clear node of all references.
		/// </summary>
		public void Clear()
		{
			Parent = null;
			Children.Clear();
			Payload = default(Type);
		}

		#endregion Public Methods
	}
}