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
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
		}
		public List<PointF> Points
		{
			get
			{
				if (string.IsNullOrEmpty(textBox1.Text))
					return null;
				var strings = textBox1.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				List<PointF> points = new List<PointF>();
				foreach (var s in strings)
				{
					var str = s.Split(' ');
					points.Add(new PointF(Single.Parse(str[0]), Single.Parse(str[1])));
				}
				return points;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
