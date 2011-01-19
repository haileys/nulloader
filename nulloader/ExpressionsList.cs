using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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

        public string SubIn(string Expression, string Pronumeral, string Replacement)
        {
            return Regex.Replace(Expression, @"([^a-z]|^)" + Pronumeral + @"([^a-z]|$)", m => m.Groups[1].Value + "(" + Replacement + ")" + m.Groups[2].Value, RegexOptions.Compiled);
        }

        public string MakeConstantExplicit(string polynomial)
        {
            var s = Regex.Replace(polynomial, @"([+\-=]|^)([0-9.]+)([+-]|$)", m => m.Groups[1].Value + "(" + m.Groups[2].Value + "x^0)" + m.Groups[3].Value);
            return s;
        }

        public string MakeFirstDegreeExplicit(string polynomial)
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

                if (i + 1 != polynomial.Length && polynomial[i + 1] != '^')
                    sb.Append("^1");

            }

            return sb.ToString();
        }

        public float? Evaluate(string Expression, ref string Error)
        {
            try
            {
                return new A.QD(Expression).M(null, null, ref Error);
            }
            catch (NullReferenceException)
            { return 0; }
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
