using System;
using System.Drawing;
using System.IO;

using NUnit.Framework;

using tech;

namespace tech_test
{
	[TestFixture]
	public class GraphilatorInternalTests : Graphilator
	{
		#region Public Methods

		[Test]
		public void LoadTest()
		{
			Clear ();
			var p = maze_path ("maze3.png");

			Assert.True (Load (p));

			Assert.AreEqual (this.FullPath, p, "Load should set FullPath");
			Assert.IsNotNull (this.Source, "Source should load bitmap");

			//TODO: Test 64k length, width constraint
		}

		public string maze_path(string string_path)
		{
			var s = Directory.GetCurrentDirectory ();
			var path = Path.Combine (s, string_path);

			return path;
		}

		[Test]
		public void SearchTest()
		{
			//Full round trip test.
			Clear ();

			Assert.True(Load (maze_path ("maze3.png")));
			Assert.True( Setup ()) ;
			Search ();
			Assert.True (DrawResults ());
		}

		[Test]
		public void SetupTest()
		{
			Clear ();

			Assert.IsTrue(Load (maze_path ("maze3.png")));

			Assert.AreEqual (0, _blue.Count, "Blue count should be zero.");
			Assert.AreEqual (0, _red.Count, "Red count should be zero.");

			Assert.IsTrue (Setup ());

			Assert.Greater (_blue.Count, 0, "Should have located at least one blue pixel.");
			Assert.Greater (_red.Count, 0, "Should have located at least one red pixel.");
		}

		[Test]
		public void _get_neighborsTest()
		{
			Clear ();
			Source = new Bitmap (3, 3);

			//Set all pixels to white
			for (int x = 0; x < Source.Width; x++) {
				for (int y = 0; y < Source.Height; y++) {
					Source.SetPixel (x, y, Color.White);
				}
			}

			var pqn_top_left = new PriorityQueueNode (0, 0, false);
			var l = _get_neighbors (pqn_top_left);
			Assert.AreEqual (l.Count, 2, "Two pixels should have been returned from top left.");
			Assert.IsTrue (l [0].X == 1 && l[0].Y == 0, "Right pixel should be first on list.");
			Assert.IsTrue (l [1].X == 0 && l[1].Y == 1, "Bottom pixel should be second on list.");

			var pqn_bottom_right = new PriorityQueueNode (2, 2, false);
			l = _get_neighbors (pqn_bottom_right);
			Assert.AreEqual (l.Count, 2, "Two pixels should have been returned from bottom right.");
			Assert.IsTrue (l [0].X == 2 && l[0].Y == 1, "Top pixel should be first on list.");
			Assert.IsTrue (l [1].X == 1 && l[1].Y == 2, "Left pixel should be first on list.");

			//Set all pixels to Black
			for (int x = 0; x < Source.Width; x++) {
				for (int y = 0; y < Source.Height; y++) {
					Source.SetPixel (x, y, Color.Black);
				}
			}

			var pqn_dead_center = new PriorityQueueNode (1, 1, false);
			l = _get_neighbors (pqn_dead_center);
			Assert.AreEqual (l.Count, 0, "Pixel should have no neighbors.");
		}

		[Test]
		public void _sort_pixelTest()
		{
			Clear ();
			Source = new Bitmap (1, 2);

			Source.SetPixel (0, 0, Color.Blue);
			Source.SetPixel (0, 1, Color.Red);

			_sort_pixel (0, 0);
			_sort_pixel (0, 1);

			Assert.AreEqual(1, _blue.Count, "One blue pixel should have been found");
			Assert.IsTrue (_blue [0].X == 0 && _blue [0].Y == 0, "Correct pixel should have been identified");

			Assert.AreEqual(1, _red.Count, "One red pixel should have been found");
			Assert.IsTrue (_red [0].X == 0 && _red [0].Y == 1, "Correct pixel should have been identified");
		}

		#endregion Public Methods
	}
}