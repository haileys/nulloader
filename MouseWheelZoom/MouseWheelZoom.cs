using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nulloader;
using System.Windows.Forms;

namespace MouseWheelZoom
{
    [PluginName("Mousewheel Zoom")]
    public class MouseWheelZoom : Plugin
    {
        public MouseWheelZoom()
        {
            TwoDimensionalGraph.GetControl().MouseWheel += MouseWheelZoom_MouseWheel;
        }

        void MouseWheelZoom_MouseWheel(object sender, MouseEventArgs e)
        {
            double xMin = double.Parse(FindControlByName("xMinTextBox").Text);
            double xMax = double.Parse(FindControlByName("xMaxTextBox").Text);
            
            double yMin = double.Parse(FindControlByName("yMinTextBox").Text);
            double yMax = double.Parse(FindControlByName("yMaxTextBox").Text);

            var mw = Math.Sign(e.Delta);

            double xQuarter = (xMax - xMin) / (mw > 0 ? 4d : 2d);
            double yQuarter = (yMax - yMin) / (mw > 0 ? 4d : 2d);

            xMin += xQuarter * mw;
            xMax -= xQuarter * mw;

            yMin += xQuarter * mw;
            yMax -= xQuarter * mw;

            FindControlByName("xMinTextBox").Text = xMin.ToString();
            FindControlByName("xMaxTextBox").Text = xMax.ToString();

            FindControlByName("yMinTextBox").Text = yMin.ToString();
            FindControlByName("yMaxTextBox").Text = yMax.ToString();
        }
    }
}
