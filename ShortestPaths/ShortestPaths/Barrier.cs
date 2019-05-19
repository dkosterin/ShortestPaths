using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPaths
{
	class Barrier
	{
		List<PointF> points = new List<PointF>();
		public Barrier(List<PointF> points)
		{
			foreach (var p in points)
				this.points.Add(p);
		}
		public List<PointF> Points
		{
			get
			{
				return points;
			}
		}
		public void Draw(Graphics g, float w, float h)
		{
			Pen p = new Pen(Color.Black, 2);
			for (int i = 0; i < points.Count - 1; i++)
				g.DrawLine(p, w + points[i].X * 50, h - points[i].Y * 50, w + points[i + 1].X * 50, h - points[i + 1].Y * 50);
			g.DrawLine(p, w + points[points.Count - 1].X * 50, h - points[points.Count - 1].Y * 50, w + points[0].X * 50, h - points[0].Y * 50);
		}
	}
}
