using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nulloader;
using System.Windows.Forms;

namespace Inspector
{
    [PluginName("Inspector")]
    public class Inspector : Plugin, IPluginIcon
    {
        public Inspector()
        {
            RegisterMenuItem((s, e) => new InspectorForm().Show());
        }

        public System.Drawing.Image GetIcon()
        {
            return Properties.Resources.magnifier;
        }
    }
}
