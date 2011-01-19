using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nulloader;
using System.Windows.Forms;
using System.Drawing;

namespace Regression
{
    public class Regression : Plugin, IPluginIcon
    {
        public Regression()
        {
            OperateOnControl(CreateEditorTab("Regression"), tab =>
            {
                var methods = GetType().GetMethods()
                    .Where(x => x.GetCustomAttributes(typeof(UserAction), true).FirstOrDefault() != null);

                var tsel = new TableSelect(FindControlByName, this, methods) { Parent = tab, Dock = DockStyle.Fill };
            });
        }

        public Image GetIcon()
        {
            return Properties.Resources.chart_line;
        }

        [UserAction]
        public void Linear(PointF[] points)
        {
            if (points.Length < 2)
            {
                MessageBox.Show("Linear regression requires at least two points");
                return;
            }

            float sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

            foreach (var point in points)
            {
                sumX += point.X;
                sumY += point.Y;

                sumXY += point.X * point.Y;
                sumX2 += point.X * point.X;
            }

            float gradient = (points.Length * sumXY - sumX * sumY) / (points.Length * sumX2 - sumX * sumX);
            float constant = (sumY - gradient * sumX) / points.Length;

            Expressions.Add(string.Format("y={0}x+{1}", gradient, constant));
        }

        [UserAction]
        public void Quadratic(PointF[] points)
        {
            if (points.Length != 3)
            {
                MessageBox.Show("Quadratic regression requires exactly three points (for now, at least)");
                return;
            }

            var x1 = points[0].X;
            var x2 = points[1].X;
            var x3 = points[2].X;

            var y1 = points[0].Y;
            var y2 = points[1].Y;
            var y3 = points[2].Y;

            var a =     (x3 * (-y1 + y2) + x2 * (y1 - y3) + x1 * (-y2 + y3))
                    /   ((x1 - x2) * (x1 - x3) * (x2 - x3));

            var b =     (Math.Pow(x3, 2) * (y1 - y2) + Math.Pow(x1, 2) * (y2 - y3) + Math.Pow(x2, 2) * (-y1 + y3))
                    /   ((x1 - x2) * (x1 - x3) * (x2 - x3));

            /*
            var c =     (x3 * (x2 * (x2 - x3) * y1 + x1 * (-x1 + x3) * y2) + x1 * (x1 - x2) * Math.Pow(x2, 2) * y3)
                    /   ((x1 - x2) * (x1 - x3) * (x2 - x3));
            */

            var quadratic = string.Format("{0}x^2+{1}x", a, b);
            var curY3 = Expressions.Evaluate(Expressions.SubIn(quadratic, "x", x3.ToString()));
            var c = y3 - curY3;

            Expressions.Add(string.Format("y={0}x^2+{1}x+{2}", a, b, c));
        }
    }
}
