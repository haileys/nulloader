using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nulloader;
using System.Windows.Forms;

namespace highresrender
{
    public class highresrender : Plugin, IPluginIcon
    {
        public highresrender()
        {
            TwoDimensionalGraph.ContextMenu.Items.Add("Render in High Resolution", Properties.Resources.layers, Render);
        }

        void Render(object sender, EventArgs eargs)
        {
            var img = TwoDimensionalGraph.TakeSnapshot(1000, 1000);
            var sfd = new SaveFileDialog { DefaultExt = ".png", Filter = "PNG File|*.png|All Files|*.*", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    img.Save(sfd.FileName);
                }
                catch
                {
                    MessageBox.Show("Could not save image.", "High Resolution Render", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public System.Drawing.Image GetIcon()
        {
            return Properties.Resources.layers;
        }
    }
}
