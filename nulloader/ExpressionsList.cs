using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nulloader
{
    public class ExpressionsList
    {
        RichTextBox rtb;
        internal ExpressionsList(RichTextBox ExpressionsBox)
        {
            rtb = ExpressionsBox;

            rtb.SelectionChanged += (s, e) => { if (CurrentChanged != null) CurrentChanged(s, e); };
        }

        public float? Evaluate(string Expression, ref string Error)
        {
            return new A.QD(Expression).M(null, null, ref Error);
        }
        public float? Evaluate(string Expression)
        {
            string error = "";
            return Evaluate(Expression, ref error);
        }

        void Update()
        {
            Globals.NullularGrapherMainForm.CrossCall("TN", null, new KeyEventArgs(Keys.None));
        }

        public string CurrentExpression
        {
            get { return this[rtb.GetLineFromCharIndex(rtb.SelectionStart)]; }
        }

        public event EventHandler CurrentChanged;

        public void Add(string Expression)
        {
            var lines = rtb.Lines.ToList();
            lines.Add(Expression);
            rtb.Lines = lines.ToArray();
            Update();
        }

        public int Find(string Expression)
        {
            for (int i = 0; i < rtb.Lines.Length; i++)
                if (Expression == rtb.Lines[i])
                    return i;

            return -1;
        }

        public void Remove(string Expression)
        {
            var lines = rtb.Lines.ToList();
            lines.Remove(Expression);
            rtb.Lines = lines.ToArray();
            Update();
        }
        public void Remove(int Index)
        {
            var lines = rtb.Lines.ToList();
            lines.RemoveAt(Index);
            rtb.Lines = lines.ToArray();
            Update();
        }

        public string this[int index]
        {
            get
            {
                if (rtb.Lines.Length == 0)
                    return "";
                try
                {
                    return rtb.Lines[index];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                rtb.Lines[index] = value;
                Update();
            }
        }
    }
}
