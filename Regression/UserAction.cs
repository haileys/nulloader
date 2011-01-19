using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regression
{
    public class UserAction : Attribute
    {
        public string Name { get; private set; }

        public UserAction()
        {
            Name = null;
        }

        public UserAction(string Name)
        {
            this.Name = Name;
        }
    }
}
