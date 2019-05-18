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
		public void Draw(Graphics g)
		{
			Pen p = new Pen(Color.Black, 2);
			for (int i = 0; i < points.Count; i++)
				g.DrawLine(p, points[i], points[i + 1]);
			g.DrawLine(p, points[points.Count - 1], points[0]);
			foreach (var b in barriers)
				b.Draw(g);
		}
	}
}
