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
            Expressions.Add(Derive(Expressions[0]));
        }

        [UserAction("Find Stationary Points")]
        public void FindStationaryPoints()
        {
            Expressions.Add((Derive(Expressions[0]).Replace("y=", "0=")));
            (FindControlByName("intersectionsCheckBox") as CheckBox).Checked = true;
        }

        [UserAction]
        public void Integrate()
        {
            var expr = Expressions[0];

            expr = Regex.Replace(expr, @"x(^){0}", "x^1");
            var derivative = Regex.Replace(expr, @"([0-9\.]*)x\^([0-9\.]+)", m =>
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
            return Regex.Replace(Expression, @"([0-9\.]*)x\^([0-9\.]+)", m =>
            {
                var coefficient = decimal.Parse(string.IsNullOrEmpty(m.Groups[1].Value) ? "1" : m.Groups[1].Value);
                var degree = decimal.Parse(m.Groups[2].Value);

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
