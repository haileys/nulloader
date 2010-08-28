using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace nulloader
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginName : Attribute
    {
        public string Name { get; private set; }

        public PluginName(string Name)
        {
            this.Name = Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
