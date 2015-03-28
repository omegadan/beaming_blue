using System;

using NUnit.Framework;

using tech;

namespace tech_test
{
	[TestFixture]
	public class TreeNodeTests
	{
		#region Public Methods

		[Test]
		public void AddChildTest()
		{
			var t = new TreeNode<int>();

			Assert.AreEqual(t.Children.Count, 0, "There should be zero children upon creation.");

			var c = t.AddChild (2);
			Assert.AreEqual(t.Children.Count, 1, "There should be one child after AddChild().");

			Assert.AreSame(t.Children[0], c, "Child node should appear in children list.");
		}

		[Test]
		public void ConstructorTest()
		{
			var t = new TreeNode<int> (32);
			Assert.AreEqual (t.Payload, 32, "Constructor should set payload property.");
		}

		#endregion Public Methods
	}
}