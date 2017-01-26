using System;

namespace SquaresPage
{
	public class Point
	{
		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point(Point p)
		{
			X = p.X;
			Y = p.Y;
		}

		public string CoordsToString(bool nice=false)
		{
			if(nice)
				return "(" + X.ToString() + ", " + Y.ToString() + ")";
			else
				return X.ToString() + "\t" + Y.ToString();
		}

		//		public Point DistVect(Point p)
		//		{
		//			return new Point(p.X - X, p.Y - Y);
		//		}

		public static Point operator-(Point a, Point b)
		{
			return new Point(b.X - a.X, b.Y - a.Y);
		}

		public static int operator * (Point a, Point b)
		{
			return a.X * b.X + a.Y * b.Y;
		}

		public static bool operator == (Point a, Point b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator != (Point a, Point b)
		{
			return a.X != b.X || a.Y != b.Y;
			//return !(a==b);
		}

		public override bool Equals(Object o)
		{
			Point p = o as Point;
			return p == this;
		}

		public override int GetHashCode()
		{
			// most miafasz
			return base.GetHashCode();
		}

		public double Abs()
		{
			double x = X;
			double y = Y;
			return Math.Sqrt(x * x + y * y);
		}
	}
}
