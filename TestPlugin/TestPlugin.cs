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

            TwoDimensionalGraph.RegisterDrawHook(g => g.FillRectangle(Brushes.Red, new Rectangle(50, 50, 50, 50)));
            TwoDimensionalGraph.Redraw();

            RegisterConstant("charlie", 5);
            RegisterFunction("hurr", 1, args => (float)Math.Sin(args[0]));
        }

        public System.Drawing.Image GetIcon()
        {
            return Properties.Resources.emoticon_tongue;
        }
    }
}
