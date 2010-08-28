using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nulloader;

namespace TimeToggleButton 
{
    [PluginName("Toggle Button for Time")]
    public class TimeToggleButton : Plugin
    {
        Button start;
        Button stop;
        public TimeToggleButton()
        {
            start = (Button)FindControlByName("startTimeButton");
            stop = (Button)FindControlByName("stopTimeButton");

            start.Click += start_Click;
            stop.Click += stop_Click;

            OperateOnControl(stop, _ =>
            {
                stop.Width = start.Width = (stop.Left + stop.Width) - start.Left;
                stop.Left = start.Left;
                stop.Hide();
                start.Focus();
            });
        }

        void stop_Click(object sender, EventArgs e)
        {
            stop.Hide();
            start.Show();
            start.Focus();
        }

        void start_Click(object sender, EventArgs e)
        {
            start.Hide();
            stop.Show();
            stop.Focus();
        }
    }
}
