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
        }

        [UserAction("Find Tangent")]
        public void Tangent()
        {
            var dlg = new Tangent(Expressions.CurrentExpression);
            dlg.Ok += Tangent_Ok;
            dlg.ShowDialog();
        }

        void Tangent_Ok(Tangent.TangentDialogResponse Response)
        {
            var derivative = SubIn(Derive(Response.Expression), "x", Response.X.ToString());
            var y = Expressions.Evaluate(SubIn(Response.Expression, "x", Response.X.ToString()).Replace("y=",""));

            var gradient = Expressions.Evaluate(derivative.Replace("y=", ""));
            Expressions.Add("y=" + gradient + "x-" + (gradient * (float)Response.X - y));
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
            var min_sub_in = SubIn(antiderivative, "x", Response.XMin.ToString());
            var max_sub_in = SubIn(antiderivative, "x", Response.XMax.ToString());

            Expressions.Add(string.Format("({0})-({1})", max_sub_in, min_sub_in));
        }

        string Antiderive(string Expression)
        {
            return Regex.Replace(MakeFirstDegreeExplicit(MakeConstantExplicit(Expression)), @"([0-9\.]*)x\^([0-9\.]+)", m =>
            {
                var coefficient = decimal.Parse(string.IsNullOrEmpty(m.Groups[1].Value) ? "1" : m.Groups[1].Value);
                var degree = decimal.Parse(m.Groups[2].Value);

                return (coefficient / (degree + 1)) + "x^" + (degree + 1).ToString();
            },
            RegexOptions.Compiled);
        }

        string Derive(string Expression)
        {
            return Regex.Replace(MakeFirstDegreeExplicit(MakeConstantExplicit(Expression)), @"([0-9\.]*)x\^([0-9\.]+)", m =>
            {
                var coefficient = decimal.Parse(string.IsNullOrEmpty(m.Groups[1].Value) ? "1" : m.Groups[1].Value);
                var degree = decimal.Parse(m.Groups[2].Value);

                return coefficient + "(" + degree + ")x^" + (degree - 1).ToString();
            },
            RegexOptions.Compiled);
        }

        string SubIn(string Expression, string Pronumeral, string Replacement)
        {
            return Regex.Replace(Expression, @"([^a-z]|^)" + Pronumeral + @"([^a-z]|$)", m => m.Groups[1].Value + "(" + Replacement + ")" + m.Groups[2].Value, RegexOptions.Compiled);
        }

        string MakeConstantExplicit(string polynomial)
        {
            var s = Regex.Replace(polynomial, @"([+\-=]|^)([0-9.]+)([+-]|$)", m => m.Groups[1].Value + "(" + m.Groups[2].Value + "x^0)" + m.Groups[3].Value);
            return s;
        }

        string MakeFirstDegreeExplicit(string polynomial)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < polynomial.Length; i++)
            {
                sb.Append(polynomial[i]);

                if (polynomial[i] != 'x')
                    continue;

                if (i + 1 == polynomial.Length)
                {
                    sb.Append("^1");
                    break;
                }

                if (i != 0 && char.IsLetter(polynomial[i - 1]))
                    continue;

                if (i + 1 != polynomial.Length && char.IsLetter(polynomial[i + 1]))
                    continue;

                if(i + 1 != polynomial.Length && polynomial[i + 1] != '^')
                    sb.Append("^1");

            }

            return sb.ToString();
        }

        public Image GetIcon()
        {
            return Properties.Resources.chart_curve;
        }
    }
}
