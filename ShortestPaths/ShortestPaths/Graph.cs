using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPaths
{
	class Graph
	{
		List<PointF> points = new List<PointF>();
		List<Barrier> barriers = new List<Barrier>();
		public Graph(List<PointF> points)
		{
			foreach (var p in points)
				this.points.Add(p);
		}
		public void AddBarrier(List<PointF> barrierPoints)
		{
			barriers.Add(new Barrier(barrierPoints));
		}
		public void Draw(Graphics g, float w, float h)
		{
			Pen p = new Pen(Color.Purple, 2);
			for (int i = 0; i < points.Count - 1; i++)
				g.DrawLine(p, w + points[i].X * 50, h - points[i].Y * 50, w + points[i + 1].X * 50, h - points[i + 1].Y * 50);
			g.DrawLine(p, w + points[points.Count - 1].X * 50, h - points[points.Count - 1].Y * 50, w + points[0].X * 50, h - points[0].Y * 50);
			foreach (var b in barriers)
				b.Draw(g, w, h);
		}
		double[,] CreateGraph(PointF startPoint, PointF finishPoint)
		{
			int barriersCount = 0;
			foreach (var b in barriers)
				barriersCount += b.Points.Count;
			int n = points.Count + barriersCount + 2;
			var result = new double[n, n];
			double w = 0;
			for(int i = 0; i < points.Count; i++)
			{
				w = 0;
				for(int j = i; j < points.Count; j++)
				{
					w = FindWeight(points[i], points[j]);
					result[i, j] = w;
					result[j, i] = w;
				}
				int k = points.Count;
				foreach(var b in barriers)
				{
					for(int j = 0; j < b.Points.Count; j++)
					{
						w = FindWeight(points[i], b.Points[j]);
						result[i, k] = w;
						result[k, i] = w;
						k++;
					}
				}
				w = FindWeight(points[i], startPoint);
				result[i, n - 2] = w;
				result[n - 2, i] = w;
				w = FindWeight(points[i], finishPoint);
				result[i, n - 1] = w;
				result[n - 1, i] = w;
			}

			int m = points.Count;
			for(int i = 0; i < barriers.Count; i++)
			{
				for(int j = 0; j < barriers[i].Points.Count; j++)
				{
					for(int k = i + 1; k < barriers.Count; k++)
					{
						int o = barriers[i].Points.Count - j;
						for(int l = 0; l < barriers[k].Points.Count; l++)
						{
							w = FindWeight(barriers[i].Points[j], barriers[k].Points[l]);
							result[m, m + o] = w;
							result[m + o, m] = w;
							o++;
						}
					}
					m++;
				}
			}

			m = points.Count;
			foreach (var b in barriers)
			{
				for (int j = 0; j < b.Points.Count; j++)
				{
					if (j != b.Points.Count - 1)
					{
						PointF ab = new PointF(b.Points[j + 1].X - b.Points[j].X, b.Points[j + 1].Y - b.Points[j].Y);
						w = Math.Sqrt(ab.X * ab.X + ab.Y * ab.Y);
						result[m, m + 1] = w;
						result[m + 1, m] = w;
					}
					else
					{
						PointF ab = new PointF(b.Points[j].X - b.Points[0].X, b.Points[j].Y - b.Points[0].Y);
						w = Math.Sqrt(ab.X * ab.X + ab.Y * ab.Y);
						result[m, m - b.Points.Count + 1] = w;
						result[m - b.Points.Count + 1, m] = w;
					}
					w = FindWeight(startPoint, b.Points[j]);
					result[n - 2, m] = w;
					result[m, n - 2] = w;
					w = FindWeight(finishPoint, b.Points[j]);
					result[n - 1, m] = w;
					result[m, n - 1] = w;
					m++;
				}
			}
			w = FindWeight(startPoint, finishPoint);
			result[n - 1, n - 2] = w;
			result[n - 2, n - 1] = w;
			return result;
		}
		double Determinant(double a, double b, double c, double d)
		{
			return a * d - b * c;
		}
		bool IsOnLine(PointF a, PointF b, double x, double y)
		{
			double minx, maxx, miny, maxy;
			if(a.X > b.X)
			{
				minx = b.X;
				maxx = a.X;
			}
			else
			{
				minx = a.X;
				maxx = b.X;
			}
			if (a.Y > b.Y)
			{
				miny = b.Y;
				maxy = a.Y;
			}
			else
			{
				miny = a.Y;
				maxy = b.Y;
			}
			if (maxx == minx)
				return x == maxx && miny < y && y < maxy;
			else if (miny == maxy)
				return y == maxy && minx < x && x < maxx;
			else return minx < x && x < maxx && miny < y && y < maxy;
		}
		bool IsEqual(double a, double b, double c, double d)
		{
			if(a > b)
			{
				double tmp = a;
				a = b;
				b = tmp;
			}
			if(c > d)
			{
				double tmp = c;
				c = d;
				d = tmp;
			}
			double max, min;
			if (a > c)
				max = a;
			else max = c;
			if (b > d)
				min = d;
			else min = b;
			return max <= min;
		}
		bool IsIntersect(PointF a, PointF b, PointF c, PointF d)
		{
			double a1 = b.Y - a.Y;
			double b1 = a.X - b.X;
			double c1 = -a1 * a.X - b1 * a.Y;

			double a2 = d.Y - c.Y;
			double b2 = c.X - d.X;
			double c2 = -a2 * c.X - b2 * c.Y;

			double det = Determinant(a1, b1, a2, b2);
			if(Math.Abs(det) > 0.00001)
			{
				double x = Determinant(-c1, b1, -c2, b2) / det;
				double y = Determinant(a1, -c1, a2, -c2) / det;
				return IsOnLine(a, b, x, y) && IsOnLine(c, d, x, y);
			}
			else if(Math.Abs(Determinant(a1, c1, a2, c2)) < 0.00001 && Math.Abs(Determinant(b1, c1, b2, c2)) < 0.00001)
				return IsEqual(a.X, b.X, c.X, d.X) && IsEqual(a.Y, b.Y, c.Y, d.Y);
			else return false;
		}
		double FindWeight(PointF a, PointF b)
		{
			// TODO: Проверка на принадлежность
			foreach(var barrier in barriers)
			{
				for (int i = 0; i < barrier.Points.Count - 1; i++)
					if (IsIntersect(a, b, barrier.Points[i], barrier.Points[i + 1]))
						return 0;
				if (IsIntersect(a, b, barrier.Points[barrier.Points.Count - 1], barrier.Points[0]))
					return 0;
			}
			PointF ab = new PointF(b.X - a.X, b.Y - a.Y);
			return Math.Sqrt(ab.X * ab.X + ab.Y * ab.Y);
		}
		public void FindShortestPath(PointF startPoint, PointF finishPoint, Graphics g, float w, float h)
		{
			var graph = CreateGraph(startPoint, finishPoint);
			int n = graph.GetLength(0);
			var coords = new PointF[n];
			for (int i = 0; i < points.Count; i++)
				coords[i] = points[i];
			int m = points.Count;
			for(int i = 0; i < barriers.Count; i++)
				for(int j = 0; j < barriers[i].Points.Count; j++)
				{
					coords[m] = barriers[i].Points[j];
					m++;
				}
			coords[n - 2] = startPoint;
			coords[n - 1] = finishPoint;

			double[] mark = new double[n];
			int[] prev = new int[n];
			int[] state = new int[n];
			for (int i = 0; i < n; i++)
			{
				mark[i] = 10000;
				prev[i] = -1;
				state[i] = 0;
			}
			mark[n - 2] = 0;
			int k = n - 2;
			while(!isStateOnes(state))
			{
				double min = 10000;
				for (int i = 0; i < n; i++)
				{
					if (graph[k, i] != 0 && state[i] != 1 && mark[i] >  graph[k, i] + mark[k])
					{
						mark[i] = graph[k, i] + mark[k];
						prev[i] = k;
					}
				}
				state[k] = 1;
				for(int i = 0; i < n; i++)
					if(mark[i] < min && state[i] != 1)
					{
						min = mark[i];
						k = i;
					}
			}
			Pen p = new Pen(Color.Red, 2);
			k = n - 1;
			while(mark[k] != 0)
			{
				int l = prev[k];
				g.DrawLine(p, w + coords[k].X * 50, h - coords[k].Y * 50, w + coords[l].X * 50, h - coords[l].Y * 50);
				k = l;
			}
		}
		bool isStateOnes(int[] state)
		{
			for (int i = 0; i < state.Length; i++)
				if (state[i] == 0)
					return false;
			return true;
		}
	}
}
