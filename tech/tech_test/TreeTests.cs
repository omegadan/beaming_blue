using System;

using NUnit.Framework;

using tech;

namespace tech_test
{
	[TestFixture]
	public class TreeTests
	{
		#region Public Methods

		[Test]
		public void AddRootTest()
		{
			var t = new Tree<int>();

			Assert.IsNull(t.Root, "Root should start out as null.");

			t.AddRoot(1);

			Assert.IsNotNull(t.Root, "AddRoot should assign .Root");

			Assert.AreEqual(t.Root.Payload, 1, "AddRoot should set Payload.");
		}

		#endregion Public Methods
	}
}