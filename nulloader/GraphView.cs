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

        public Color BackgroundColor { get { return graph.M; } set { graph.M = value; graph.CrossCall("JC"); } }
        public Color ForegroundColor { get { return graph.N; } set { graph.N = value; graph.CrossCall("JC"); } }
        public Color TextColor { get { return graph.O; } set { graph.O = value; graph.CrossCall("JC"); } }
    }
}
