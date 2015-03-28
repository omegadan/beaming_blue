#region Header

/*[Test]
public void _compute_taxi_cost_test() {

	//	Assert.AreEqual( 20, _compute_taxi_cost( 0,  0,  10, 10 ) );
	//	Assert.AreEqual( 20, _compute_taxi_cost( 10,  10,  0, 0 ) );
}
*/

#endregion Header

using System;

using NUnit.Framework;

using tech;

namespace tech_test
{
	[TestFixture]
	public class PriorityQueueNodeTests
	{
		#region Public Methods

		[Test]
		public void ConstructorTests()
		{
			var p = new PriorityQueueNode (2);
			Assert.AreEqual (p.Key, 2, "Constructor should set Key");

			p = new PriorityQueueNode (10, 20, true);
			Assert.AreEqual (p.X, 10, "Constructor should set X");
			Assert.AreEqual (p.Y, 20, "Constructor should set Y");
			Assert.AreEqual (p.IsBlue, true, "IsBlue should be set");

			p = new PriorityQueueNode(10, 20, true, 2);
			Assert.AreEqual (p.X, 10, "Constructor should set X");
			Assert.AreEqual (p.Y, 20, "Constructor should set Y");
			Assert.AreEqual (p.IsBlue, true, "IsBlue should be set");
			Assert.AreEqual (p.Key, 2, "Constructor should set Key");
		}

		[Test]
		public void EncodeXYTest()
		{
			Assert.AreEqual( 0, PriorityQueueNode.EncodeXY(0,0) );
			Assert.AreEqual(0xFFFFEEEE, PriorityQueueNode.EncodeXY (0xFFFF, 0xEEEE), "Encode should shift the bits of X into the high 16 bits.");
		}

		#endregion Public Methods
	}
}