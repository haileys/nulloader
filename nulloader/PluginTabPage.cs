using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace nulloader
{
    public class PluginTabPage : TabPage
    {
        internal PluginTabPage(string Title, Plugin Owner, TabControl Parent)
            : base(Title)
        {
            this.Owner = Owner.PluginGuid;

            Parent.ImageList.Images.Add(Owner.PluginGuid.ToString(), Owner.Icon);
            ImageIndex = Parent.ImageList.Images.IndexOfKey(Owner.PluginGuid.ToString());

            Parent.Invoke((MethodInvoker)(() => Globals.EditorTabs.TabPages.Add(this)));
        }

        internal Guid Owner { get; private set; }
    }
}
