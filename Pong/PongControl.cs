using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pong
{
    public partial class PongControl : UserControl
    {
        public event EventHandler Player1Up;
        public event EventHandler Player1Down;
        public event EventHandler Player2Up;
        public event EventHandler Player2Down;

        public PongControl()
        {
            InitializeComponent();
        }

        public void SetScore(int A, int B)
        {
            label1.Invoke((MethodInvoker)(() => label1.Text = A.ToString() + " - " + B.ToString()));
        }

        private void PongControl_Resize(object sender, EventArgs e)
        {
            panel1.Top = Height - panel1.Height - 16;
            panel1.Left = (Width - panel1.Width) / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Player1Up != null)
                Player1Up(this,e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Player1Down != null)
                Player1Down(this, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Player2Up != null)
                Player2Up(this, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Player2Down != null)
                Player2Down(this, e);
        }
    }
}
