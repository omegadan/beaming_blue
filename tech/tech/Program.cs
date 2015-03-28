using System;

namespace tech
{
	class MainClass
	{
		#region Public Methods

		public static void Main(string[] args)
		{
			if (args.Length < 2) {
				Console.WriteLine( "Usage: maze.exe source.[bmp,png,jpg] destination.[bmp,png,jpg]" );

				return;
			}

			var g = new Graphilator ();

			if ( !g.Load (args[0] ) )
			{
				Console.WriteLine ("Error loading image or size too large.");
				return;
			}

			if (!g.Setup ())
			{
				Console.WriteLine ("Graph does not contain start or endpoints.");
				return;
			}

			g.Search ();

			if (!g.DrawResults ())
			{
				Console.WriteLine ("No path through graph found.");
				return;
			}

			if (!g.SaveResults (args [1]))
			{
				Console.Write ("Error saving output.");
			}

			Console.WriteLine ("Wrote output to: " + args [1]);
		}

		#endregion Public Methods
	}
}