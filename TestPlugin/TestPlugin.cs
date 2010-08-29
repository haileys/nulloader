using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using nulloader;

namespace TestPlugin
{
    [PluginName("Test Plugin")]
    public class TestPlugin : Plugin, IPluginIcon
    {
        public TestPlugin()
        {
            RegisterMenuItem((s, e) => MessageBox.Show("Hello, world!"));

            OperateOnControl(CreateEditorTab("test tab :D"), tabpage =>
            {
                new Label { Text = "Hello, World!", Parent = tabpage, Left = 50, Top = 50 };
            });

            TwoDimensionalGraph.XAxisColor = Color.Black;
            TwoDimensionalGraph.YAxisColor = Color.Black;
            TwoDimensionalGraph.DrawGridlines = false;

            RegisterConstant("charlie", 5);
            RegisterFunction("hurr", 1, Hurr);
        }

        public float Hurr(float[] x)
        {
            return (float)Math.Sin(x[0]);
        }

        public System.Drawing.Image GetIcon()
        {
            return Properties.Resources.emoticon_tongue;
        }
    }
}
