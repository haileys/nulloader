using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nulloader;
using System.Reflection;

namespace Regression
{
    public partial class TableSelect : UserControl
    {
        Func<string, Control> ctlFinder;

        string curTable;

        public TableSelect(Func<string, Control> ctlFinder, object self, IEnumerable<MethodInfo> Actions)
        {
            InitializeComponent();

            this.ctlFinder = ctlFinder;

            foreach (var action in Actions.Select(x => new 
            { 
                Name = (x.GetCustomAttributes(typeof(UserAction), true).Single() as UserAction).Name ?? x.Name,
                InternalName = x.Name,
            }))
            {
                var method = self.GetType().GetMethod(action.InternalName);
                var button = new Button { Text = action.Name, Width = 120 };
                button.Click += (s, e) => method.Invoke(self, new[] { GetTable(curTable) });
                curveLayout.Controls.Add(button);
            }
        }

        int createNewTableIndex = int.MinValue;
        private void TableSelect_Enter(object sender, EventArgs e)
        {
            var items = comboBox1.Items;
            items.Clear();
            foreach (var tabletab in (ctlFinder("tablesTabs") as TabControl).TabPages.Cast<TabPage>())
            {
                items.Add(tabletab.Text);
            }
            createNewTableIndex = items.Add("(create new table)");
        }

        public PointF[] GetTable(string Name)
        {
            var ie = (ctlFinder("tablesTabs") as TabControl).TabPages.Cast<TabPage>();
            var tab = ie.Where(t => t.Name == Name).FirstOrDefault();
            if (tab == null)
                return new PointF[0];

            var grid = tab.Controls.Cast<Control>().Where(x => x is DataGridView).Single() as DataGridView;
            var rows = grid.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null);
            return rows.Select(r => new PointF(float.Parse(r.Cells[0].Value.ToString()), float.Parse(r.Cells[1].Value.ToString()))).ToArray();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            curTable = (string)comboBox1.Items[comboBox1.SelectedIndex];
            if (comboBox1.SelectedIndex == createNewTableIndex)
            {
                var tabs = (ctlFinder("editorTabs") as TabControl);
                tabs.SelectTab(4);
            }
        }
    }
}
