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
    public delegate void TangentOk(Tangent.TangentDialogResponse Response);

    public partial class Tangent : Form
    {
        public class TangentDialogResponse
        {
            public string Expression { get; private set; }
            public decimal X { get; private set; }

            public TangentDialogResponse(string expression, decimal x)
            {
                Expression = expression;
                X = x;
            }
        }

        public event TangentOk Ok;
        
        public Tangent(string Expression)
        {
            InitializeComponent();

            equation.Text = Expression;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Ok != null)
                Ok(new TangentDialogResponse(equation.Text, x.Value));

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
