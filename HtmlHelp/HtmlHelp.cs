using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using nulloader;

namespace HtmlHelp
{
    public class HtmlHelpPlugin : Plugin
    {
        public HtmlHelpPlugin()
        {
            OperateOnMenu("Help/Manual", ms => ms.DropDownItems.Add("View in HTML").Click += (s, e) => Process.Start("http://nullular.com/grapher/documentation.html"));
        }
    }
}
