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
        public void Integrate()
        {
            var expr = Expressions.CurrentExpression;

            var derivative = Regex.Replace(MakeFirstDegreeExplicit(expr), @"([0-9\.]*)x\^([0-9\.]+)", m =>
            {
                var coefficient = decimal.Parse(string.IsNullOrEmpty(m.Groups[1].Value) ? "1" : m.Groups[1].Value);
                var degree = decimal.Parse(m.Groups[2].Value);

                return (coefficient/(degree + 1)) + "x^" + (degree + 1).ToString();
            },
            RegexOptions.Compiled);

            Expressions.Add(derivative);
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

        string MakeConstantExplicit(string polynomial)
        {
            return Regex.Replace(polynomial, @"([+\-=])([0-9.])([+-]|$)", m => m.Groups[1].Value + "(" + m.Groups[2].Value + "x^0)" + m.Groups[3].Value);
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
