using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using A;
using System.Runtime.InteropServices;

namespace nulloader
{
    public class TwoDimensionalGraph : GraphView
    {
        internal TwoDimensionalGraph(object graph)
            : base((F)graph)
        {
            var field_ZB = graph.Field("ZB");

            drawHooks.Add((Delegate)field_ZB.Get());
            var delegate_type = Assembly.GetAssembly(typeof(A.F))
                // public delegate in A.ZD (internal class)
                .GetType("A.ZD+WD");

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
    }
}
