using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using A;

namespace nulloader
{
    public class Plugin
    {
        internal Form grapher;

        public Guid PluginGuid { get; private set; }

        public string Name { get; private set; }
        public Image Icon { get; private set; }

        protected Plugin()
        {
            grapher = Globals.NullularGrapherMainForm;

            var nameattr = GetType().GetCustomAttributes(typeof(PluginName), false).FirstOrDefault();
            Name = nameattr != null ? (nameattr as PluginName).Name : GetType().Name;

            Icon = this is IPluginIcon? (this as IPluginIcon).GetIcon() : Properties.Resources.plugin;

            PluginGuid = Guid.NewGuid();
        }

        protected TabPage CreateEditorTab(string Title)
        {
            return new PluginTabPage(Title, this, Globals.EditorTabs);
        }

        protected Control FindControlByName(string Name)
        {
            if (Globals.grapherControls.ContainsKey(Name))
                return Globals.grapherControls[Name];
            return null;
        }

        protected void RegisterFunction(string Name, int NumParams, Func<float[], float> Callback)
        {
            var fields = typeof(FB).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var dictobj = Globals.NullularGrapherMainForm.Field("LG").Get();

            var del = DelegateUtility.Cast(Callback, Util.GetNullsType("A.FB+AB"));

            dictobj.GetType().GetMethod("Add").Invoke(dictobj, new object[] { Name, del });
            (Globals.NullularGrapherMainForm.Field("MG").Get() as Dictionary<string, int>).Add(Name, NumParams);
        }

        protected void RegisterConstant(string Name, float Value)
        {
            (Globals.NullularGrapherMainForm.Field("KG").Get() as Dictionary<string, float>).Add(Name, Value);
        }

        protected void RegisterMenuItem(EventHandler Click)
        {
            OperateOnMenu("Plugins", m =>
            {
                if (!Globals.CreatedPluginMenuItemYet)
                {
                    Globals.CreatedPluginMenuItemYet = true;
                    m.DropDownItems.Clear();
                }

                var tsi = m.DropDownItems.Add(Name, Icon);

                tsi.Click += Click;
            });
        }

        protected void OperateOnMenu(Action<MenuStrip> Operations)
        {
            Globals.NullularGrapherMainForm.Invoke((MethodInvoker)(() => Operations(Globals.MainMenu)));
        }
        protected bool OperateOnMenu(string Path, Action<ToolStripMenuItem> Operations)
        {
            var pathlets = Path.Split('/').Select(s => s.Trim());
            var ms = Globals.MainMenu.Items.Cast<ToolStripMenuItem>().Where(mi => mi.Text == pathlets.First()).FirstOrDefault();
            if(ms == null)
                return false;

            foreach (var pathlet in pathlets.Skip(1))
            {
                var subitems = ms.DropDownItems.Cast<ToolStripItem>().Select(x => x as ToolStripMenuItem).Where(x => x != null).ToArray();
                ms = subitems.Where(x => x.Text == pathlet).FirstOrDefault();
                if (ms == null)
                    return false;
            }

            Globals.NullularGrapherMainForm.Invoke((MethodInvoker)(() => Operations(ms)));
            
            return true;
        }

        protected void OperateOnControl(Control control, Action<Control> action)
        {
            control.Invoke((MethodInvoker)(() => action(control)));
        }

        protected TwoDimensionalGraph TwoDimensionalGraph { get { return Globals.TwoDGraph; } }
        protected ThreeDimensionalGraph ThreeDimensionalGraph { get { return Globals.ThreeDGraph; } }
    }
}
