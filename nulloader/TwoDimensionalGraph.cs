using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using A;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;

namespace nulloader
{
    public class TwoDimensionalGraph : GraphView
    {
        internal TwoDimensionalGraph(object graph)
            : base((F)graph)
        {
            var field_ZB = graph.Field("ZB");

            drawHooks.Add((Delegate)field_ZB.Get());
            var delegate_type = Util.GetNullsType("A.ZD+WD");

            field_ZB.Set(DelegateUtility.Cast((Action<Graphics>)OnDraw, delegate_type));
        }

        List<Delegate> drawHooks = new List<Delegate>();

        public void RegisterDrawHook(Action<Graphics> callback)
        {
            drawHooks.Add(callback);
        }

        void OnDraw(Graphics g)
        {
            foreach (var hook in drawHooks)
                hook.DynamicInvoke(g);
        }

        public Image TakeSnapshot(int Width, int Height)
        {
            // emulates ZD's high OnPaint, but with the set height and width.
            // !!! subject to breakage !!!

            Size tSz = Size.Empty;
            Size gSz = Size.Empty;


            graph.Invoke((MethodInvoker)(() =>
            {
                Globals.NullularGrapherMainForm.UseWaitCursor = true;
                graph.Lock();

                var tabs = (TabControl)graph.Parent.Parent;

                tSz = tabs.Size;
                gSz = graph.Size;

                tabs.Dock = DockStyle.None;
                tabs.Size = new Size(Width + (tSz.Width - gSz.Width), Height + (tSz.Height - gSz.Height));
            }));

            Redraw();

            Image img = null;
            graph.Invoke((MethodInvoker)(() => img = TakeSnapshot()));

            while (img == null)
                Thread.Sleep(10);

            graph.Invoke((MethodInvoker)(() =>
            {
                var tabs = (TabControl)graph.Parent.Parent;

                tabs.Dock = DockStyle.Fill;

                tabs.Size = tSz;
                graph.Size = gSz;

                Globals.NullularGrapherMainForm.UseWaitCursor = false;
            }));

            graph.Unlock();

            return img;
        }
    }
}
