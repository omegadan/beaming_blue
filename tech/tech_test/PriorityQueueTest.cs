using System;

using NUnit.Framework;

using tech;

namespace tech_test
{
	[TestFixture]
	public class PriorityQueueTest : PriorityQueue
	{
		#region Public Methods

		[Test]
		public void InsertExpandsArray()
		{
			Insert (new PriorityQueueNode (10));
			Insert (new PriorityQueueNode (2));

			Assert.IsTrue (_nodes.Length > 2);
		}

		[Test]
		public void PQOperationsTest()
		{
			Clear ();

			Insert (new PriorityQueueNode (7));
			Insert (new PriorityQueueNode (2));
			Insert (new PriorityQueueNode (5));
			Insert (new PriorityQueueNode (9));
			Insert (new PriorityQueueNode (13));
			Insert (new PriorityQueueNode (1));

			Assert.AreEqual (6, this.Length, "Length property should return the number of elements in the heap");

			Assert.AreEqual (1, this.ExtractMin().Key, "Heap is not ordered correctly.");
			Assert.AreEqual (5, this.Length, "Extract Min should reduce element count by 1.");

			Assert.AreEqual (2, this.ExtractMin().Key, "Heap is not ordered correctly.");
			Assert.AreEqual (4, this.Length, "Extract Min should reduce element count by 1.");

			Assert.AreEqual (5, this.ExtractMin().Key, "Heap is not ordered correctly.");
			Assert.AreEqual (3, this.Length, "Extract Min should reduce element count by 1.");

			Assert.AreEqual (7, this.ExtractMin().Key, "Heap is not ordered correctly.");
			Assert.AreEqual (2, this.Length, "Extract Min should reduce element count by 1.");

			Assert.AreEqual (9, this.ExtractMin().Key, "Heap is not ordered correctly.");
			Assert.AreEqual (1, this.Length, "Extract Min should reduce element count by 1.");

			Assert.AreEqual (13, this.ExtractMin().Key, "Heap is not ordered correctly.");
			Assert.AreEqual (0, this.Length, "Extract Min should reduce element count by 1.");
		}

		#endregion Public Methods
	}
}