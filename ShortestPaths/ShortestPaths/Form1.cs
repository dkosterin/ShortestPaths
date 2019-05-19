using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortestPaths
{
	public partial class Form1 : Form
	{
		Graphics g;
		Bitmap bmp;
		Graph graph;
		public Form1()
		{
			InitializeComponent();
			bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			g = Graphics.FromImage(bmp);
			pictureBox1.Image = bmp;
			Init();
			pictureBox1.Invalidate();
		}
		void Init()
		{
			g.Clear(Color.White);
			var p = new Pen(Color.Black, 2);
			g.DrawLine(p, 20, 0, 20, pictureBox1.Height);
			g.DrawLine(p, 0, pictureBox1.Height - 20, pictureBox1.Width, pictureBox1.Height - 20);
			p = new Pen(Color.Gray);
			for (int i = 0; 20 + i * 50 < pictureBox1.Width; i++)
				g.DrawLine(p, 20 + i * 50, 0, 20 + i * 50, pictureBox1.Height);
			for (int i = 0; pictureBox1.Height - 20 - i * 50 > 0; i++)
				g.DrawLine(p, 0, pictureBox1.Height - 20 - i * 50, pictureBox1.Width, pictureBox1.Height - 20 - i * 50);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var strings = textBox1.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<PointF> points = new List<PointF>();
			foreach(var s in strings)
			{
				var str = s.Split(' ');
				points.Add(new PointF(Single.Parse(str[0]), Single.Parse(str[1])));
			}
			graph = new Graph(points);
			graph.Draw(g, 20, pictureBox1.Height - 20);
			pictureBox1.Invalidate();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Form2 f = new Form2();
			f.ShowDialog();
			graph.AddBarrier(f.Points);
			graph.Draw(g, 20, pictureBox1.Height - 20);
			pictureBox1.Invalidate();
		}
	}
}
