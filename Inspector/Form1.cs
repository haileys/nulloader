using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Inspector
{
    public partial class InspectorForm : Form
    {
        public InspectorForm()
        {
            InitializeComponent();
            toolStripMenuItem1_Click(null, null);
        }

        public void AddObjectTree<T>(TreeNode Node, IEnumerable<KeyValuePair<string,T>> Objects, Func<T, IEnumerable<KeyValuePair<string,T>>> GetChildren)
        {
            foreach (var o in Objects)
            {
                var tn = Node.Nodes.Add(o.Key + " : " + o.Value.GetType().Name);
                tn.Tag = o.Value;
                AddObjectTree(tn, GetChildren(o.Value), GetChildren);
            }
        }

        private void formcontrols_Click(object sender, EventArgs e)
        {
            if (formcontrols.SelectedNode != null)
                controlprops.SelectedObject = formcontrols.SelectedNode.Tag;
            else
                controlprops.SelectedObject = null;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            formcontrols.Nodes.Clear();

            AddObjectTree(formcontrols.Nodes.Add("Forms and Controls"),
                Application.OpenForms.Cast<Form>().Select(x => new KeyValuePair<string, Control>(x.Name, x)),
                x => x.Controls.Cast<Control>().Select(c => new KeyValuePair<string, Control>(c.Name, c))
                );
        }
    }
}
