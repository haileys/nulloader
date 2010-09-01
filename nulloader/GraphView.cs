using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace nulloader
{
    public class GraphView
    {
        protected A.F graph;

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

        public Color XAxisColor { get { return graph.U; } set { graph.U = value; Redraw(); } }
        public Color YAxisColor { get { return graph.V; } set { graph.V = value; Redraw(); } }
        public Color ZAxisColor { get { return graph.W; } set { graph.W = value; Redraw(); } }

        public bool DrawGridlines { get { return graph.P; } set { graph.P = value; Redraw(); } }
        public bool DrawAxes { get { return graph.R; } set { graph.R = value; Redraw(); } }
        public bool DrawLabels { get { return graph.S; } set { graph.S = value; Redraw(); } }
        public bool Antialiasing { get { return graph.T; } set { graph.T = value; Redraw(); } }


        public Image TakeSnapshot()
        {
            if (graph.Field("BC").Get() == null)
                Redraw();

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
