using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nulloader
{
    public partial class PluginManagerForm : Form
    {
        public PluginManagerForm()
        {
            InitializeComponent();

            PluginManager.RestartNeeded += () => restartalert.Visible = restartbutton.Visible = true;
            PopulateLists();
        }

        void PopulateLists()
        {
            runningList.Clear();
            disabledList.Clear();

            runningList.SmallImageList = new ImageList();
            foreach (var plugin in PluginManager.LoadedPlugins)
                runningList.Items.Add(new ListViewItem(plugin.GetType().FullName, runningList.SmallImageList.Images.Add(plugin.Icon, Color.White)) { Tag = plugin.GetType() });

            foreach (var plugin in PluginManager.AvailablePlugins.Where(p => !p.Enabled))
                disabledList.Items.Add(new ListViewItem(plugin.PluginType.FullName) { Tag = plugin.PluginType });
        }

        static PluginManagerForm self = null;
        public static void Open()
        {
            (self ?? (self = new PluginManagerForm())).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem sel in runningList.SelectedItems)
            {
                PluginManager.Disable(sel.Tag as Type);
                runningList.Items.Remove(sel);
                sel.ImageIndex = -1;
                disabledList.Items.Add(sel);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem sel in disabledList.SelectedItems)
            {
                var plugin = PluginManager.Enable(sel.Tag as Type);
                disabledList.Items.Remove(sel);
                sel.ImageIndex = runningList.SmallImageList.Images.Add(plugin.Icon, Color.White);
                runningList.Items.Add(sel);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem sel in disabledList.SelectedItems)
            {
                var plugin = PluginManager.Start(sel.Tag as Type);
                disabledList.Items.Remove(sel);
                sel.ImageIndex = runningList.SmallImageList.Images.Add(plugin.Icon, Color.White);
                runningList.Items.Add(sel);
            }
        }

        private void PluginManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
