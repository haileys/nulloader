using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace nulloader
{
    public class GraphView
    {
        A.F graph;

        internal GraphView(A.F graph)
        {
            this.graph = graph;
        }

        public Font Font { get { return graph.K; } set { graph.K = value; } }

        public ContextMenuStrip ContextMenu { get { return (ContextMenuStrip)graph.Field("A").Get(); } }

        public Color BackgroundColor { get { return graph.M; } set { graph.M = value; Redraw(); } }
        public Color ForegroundColor { get { return graph.N; } set { graph.N = value; Redraw(); } }
        public Color TextColor { get { return graph.O; } set { graph.O = value; Redraw(); } }

        public string NumberFormatString { get { return graph.L; } set { graph.L = value; Redraw(); } }

        public bool DrawGridlines { get { return graph.P; } set { graph.P = value; Redraw(); } }
        public bool DrawAxes { get { return graph.R; } set { graph.R = value; Redraw(); } }
        public bool DrawLabels { get { return graph.S; } set { graph.S = value; Redraw(); } }
        public bool Antialiasing { get { return graph.T; } set { graph.T = value; Redraw(); } }


        public Image TakeSnapshot()
        {
            return graph.DC();
        }

        public void Redraw()
        {
            graph.CrossCall("JC");
        }

        public void CopySnapshot()
        {
            Clipboard.SetImage(TakeSnapshot());
        }

        public enum ColorScheme
        {
            Light,
            Dark,
            Custom,
        }
        public void ChangeColorScheme(ColorScheme scheme)
        {
            graph.GC(scheme.ToString());
        }


    }
}
