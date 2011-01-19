using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nulloader;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace calculus
{
    public class UserAction : Attribute
    {
        public string Name { get; private set; }

        public UserAction()
        {
            Name = null;
        }

        public UserAction(string Name)
        {
            this.Name = Name;
        }
    }
    public class Calculus : Plugin, IPluginIcon
    {
        public Calculus()
        {
            //expressions = (RichTextBox)FindControlByName("expressionTextBox");

            OperateOnControl(CreateEditorTab("Calculus"), tab =>
            {
                var calculuspanel = new CalculusPanel { Parent = tab, Dock = DockStyle.Fill };

                Expressions.CurrentChanged += (s, e) =>
                {
                    calculuspanel.textBox1.Text = Expressions.CurrentExpression;
                };
                
                foreach (var method_attrs in GetType().GetMethods()
                    .Select(x => new { Method = x, Attr = x.GetCustomAttributes(typeof(UserAction), true).FirstOrDefault() as UserAction })
                    .Where(x => x.Attr != null))
                {
                    var method = method_attrs.Method;

                    var button = new Button { Text = method_attrs.Attr.Name ?? method.Name, Width = 120 };
                    button.Click += (s, e) => method.Invoke(this, new object[0]);
                    calculuspanel.flowLayoutPanel1.Controls.Add(button);
                }
            });

            TwoDimensionalGraph.RegisterDrawHook(DrawHook);
            TwoDimensionalGraph.GetControl().DoubleClick += Calculus_DoubleClick;
        }

        void Calculus_DoubleClick(object sender, EventArgs e)
        {
            if (tracing != null)
            {
                Expressions.Add(FindTangentLine(tracing, (decimal)tracing_x));
            }
        }

        [UserAction("Find Tangent")]
        public void Tangent()
        {
            var dlg = new Tangent(Expressions.CurrentExpression);
            dlg.Ok += Tangent_Ok;
            dlg.ShowDialog();
        }


        string tracing = null;
        float tracing_x;
        [UserAction("Trace Tangent")]
        public void TraceTangent()
        {
            if (tracing == null)
                tracing = Expressions.CurrentExpression;
            else
                tracing = null;

            (FindControlByName("tracingCheckBox")as CheckBox).Checked = tracing != null;
        }

        void DrawHook(Graphics g)
        {
            if (tracing == null)
                return;

            var window = TwoDimensionalGraph.Window;

            foreach(var expr in (FindControlByName("tracingTextBox") as TextBox).Lines
                .Where(s => s.Contains(tracing))
                .Select(s => Regex.Split(s, "=>").Select(spl => spl.Trim()))
                .Select(sa => new { Expression = sa.First(), Point = sa.Last() }))
            {
                var match = Regex.Match(expr.Point, @"([\-0-9.]+)\s*,\s*([\-0-9.]+)");
                if (!match.Success)
                    continue;

                var x = tracing_x = float.Parse(match.Groups[1].Value);
                var y = float.Parse(match.Groups[2].Value);

                Func<PointF,PointF> tsc = TwoDimensionalGraph.TranslateToScreenCoords;

                var tang = FindTangentLine(expr.Expression, (decimal)x).Replace("y=","");

                PointF topleft = new PointF(x - window.Width, (float)Expressions.Evaluate(Expressions.SubIn(tang, "x", (x - window.Width).ToString())));
                PointF bottomright = new PointF(x + window.Width, (float)Expressions.Evaluate(Expressions.SubIn(tang, "x", (x + window.Width).ToString())));

                g.DrawLine(Pens.Red, tsc(topleft), tsc(bottomright));
            }
        }

        void Tangent_Ok(Tangent.TangentDialogResponse Response)
        {
            Expressions.Add(FindTangentLine(Response.Expression, Response.X));
        }

        string FindTangentLine(string Expression, decimal X)
        {
            var derivative = Expressions.SubIn(Derive(Expression), "x", X.ToString());
            var y = Expressions.Evaluate(Expressions.SubIn(Expression, "x", X.ToString()).Replace("y=", ""));

            var gradient = Expressions.Evaluate(derivative.Replace("y=", ""));
            return "y=" + gradient + "x-" + (gradient * (float)X - y);
        }

        [UserAction]
        public void Derive()
        {
            Expressions.Add(Derive(Expressions.CurrentExpression));
        }

        [UserAction("Find Stationary Points")]
        public void FindStationaryPoints()
        {
            Expressions.Add((Derive(Expressions.CurrentExpression).Replace("y=", "0=")));
            (FindControlByName("intersectionsCheckBox") as CheckBox).Checked = true;
        }

        [UserAction]
        public void Antiderive()
        {
            Expressions.Add(Antiderive(Expressions.CurrentExpression));
        }

        [UserAction]
        public void Integrate()
        {
            var dlg = new Integrate(Expressions.CurrentExpression);
            dlg.Ok += Integrate_Ok;
            dlg.ShowDialog();
        }

        void Integrate_Ok(Integrate.IntegrationDialogResponse Response)
        {
            var antiderivative = Antiderive(Response.Expression).Replace("y=", "");

            // pretty highlighting
            Expressions.Add(string.Format("y<({0})&y>0&x>{1}&x<{2}", Response.Expression, Response.XMin, Response.XMax));
            Expressions.Add(string.Format("y>({0})&y<0&x>{1}&x<{2}", Response.Expression, Response.XMin, Response.XMax));

            // calculating the signed area bound by the x axis
            var min_sub_in = Expressions.SubIn(antiderivative, "x", Response.XMin.ToString());
            var max_sub_in = Expressions.SubIn(antiderivative, "x", Response.XMax.ToString());

            Expressions.Add(string.Format("({0})-({1})", max_sub_in, min_sub_in));
        }

        string Antiderive(string Expression)
        {
            return Regex.Replace(Expressions.MakeFirstDegreeExplicit(Expressions.MakeConstantExplicit(Expression)), @"([0-9\.]*)x\^([0-9\.]+)", m =>
            {
                var coefficient = decimal.Parse(string.IsNullOrEmpty(m.Groups[1].Value) ? "1" : m.Groups[1].Value);
                var degree = decimal.Parse(m.Groups[2].Value);

                return (coefficient / (degree + 1)) + "x^" + (degree + 1).ToString();
            },
            RegexOptions.Compiled);
        }

        string Derive(string Expression)
        {
            return Regex.Replace(Expressions.MakeFirstDegreeExplicit(Expressions.MakeConstantExplicit(Expression)), @"([0-9\.]*)x\^([0-9\.]+)", m =>
            {
                var coefficient = decimal.Parse(string.IsNullOrEmpty(m.Groups[1].Value) ? "1" : m.Groups[1].Value);
                var degree = decimal.Parse(m.Groups[2].Value);

                //if (degree == 0)
                //    return "0";

                return coefficient + "(" + degree + ")x^" + (degree - 1).ToString();
            },
            RegexOptions.Compiled);
        }

        public Image GetIcon()
        {
            return Properties.Resources.chart_curve;
        }
    }
}
