using System;
using System.Collections.Generic;

namespace SquaresPage
{
	public struct Qdr
	{
		List<Point> PQ;

		public Qdr(Point p1, Point p2, Point p3, Point p4)
		{
			PQ = new List<Point>();
			PQ.Add(p1);
			PQ.Add(p2);
			PQ.Add(p3);
			PQ.Add(p4);
		}

		public string GetPointsText()
		{
			string retVal = "";
			for (int i = 0; i < PQ.Count; i++)
				retVal += PQ[i].CoordsToString(true) + " ";
			return retVal;
		}

		public bool IsSquare()
		{
			int diag0 = -1; // index of diagonal-to-0 vertex
			double dist = 0;
			for (int i = 1; i < 4; i++)
			{
				if ((PQ[0] - PQ[i]).Abs() > dist)
				{
					diag0 = i;
					dist = (PQ[0] - PQ[i]).Abs();
				}
			}

			int a = -1, b = -1;

			for (int i = 1; i < 4; i++)
			{
				if(i!=diag0)
				{

					if ((PQ[i] - PQ[0]) * (PQ[i] - PQ[diag0]) != 0)
						return false;
					if (a < 0)
						a = i;
					else
						b = i;
				}
			}


			if ((PQ[a] - PQ[0]) * (PQ[0] - PQ[b]) != 0)
				return false;
			if ((PQ[a] - PQ[diag0]) * (PQ[diag0] - PQ[b]) != 0)
				return false;

			if (PQ[a] - PQ[0] != PQ[diag0] - PQ[b])
				return false;
			if (PQ[0] - PQ[b] != PQ[a] - PQ[diag0])
				return false;

			// this is not very nice, but works.
			if ((PQ[0] - PQ[a]).Abs() != (PQ[0] - PQ[b]).Abs())
				return false;

			// elvileg elég

			return true;
		}
	}

	public class Qdrs
	{
		public List<Qdr> QS;
		List<Point> pts;

		int[] srcIdx;
		Stack<int> cmb;
		int n;// number of points

		private void Comb(int offset, int k)
		{
			if (k == 0)
			{
				int[] que = cmb.ToArray();
				QS.Add(new Qdr(pts[que[0]], pts[que[1]], pts[que[2]], pts[que[3]]));
				return;
			}
			for(int i=offset; i<=n-k; ++i)// kiprobalini, hogy mivan ha i++!!!
			{
				cmb.Push(srcIdx[i]);
				Comb(i + 1, k - 1);
				cmb.Pop();
			}
		}

		public Qdrs(List<Point> points)
		{
			QS=new List<Qdr>();
			pts = points;
			cmb = new Stack<int>();
			n = pts.Count;
			srcIdx = new int[n];
			for (int i = 0; i < n; i++)
				srcIdx[i] = i;
			Comb(0, 4);
		}
	}
}

