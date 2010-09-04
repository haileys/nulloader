using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace calculus
{
    public delegate void IntegrationOk(Integrate.IntegrationDialogResponse Response);

    public partial class Integrate : Form
    {
        public class IntegrationDialogResponse
        {
            public string Expression { get; private set; }
            public decimal XMax { get; private set; }
            public decimal XMin { get; private set; }

            public IntegrationDialogResponse(string expression, decimal xmax, decimal xmin)
            {
                Expression = expression;
                XMax = xmax;
                XMin = xmin;
            }
        }

        public event IntegrationOk Ok;

        public Integrate(string Expression)
        {
            InitializeComponent();

            equation.Text = Expression;
            xmin.Minimum = xmax.Minimum = decimal.MinValue;
            xmin.Maximum = xmax.Maximum = decimal.MaxValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Ok != null)
            {
                var max_x = Math.Max(xmax.Value, xmin.Value);
                var min_x = Math.Min(xmax.Value, xmin.Value);
                Ok(new IntegrationDialogResponse(equation.Text.Replace("y=", ""), max_x, min_x));
            }

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

}
