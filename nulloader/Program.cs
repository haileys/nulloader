using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Threading;
using nulloader.Properties;
using A;

namespace nulloader
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var grapher = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory,"grapher.exe"));

            new Thread(() => grapher.EntryPoint.Invoke(null, new object[0])).Start();

            while(Application.OpenForms.Count == 0)
                Thread.Sleep(10);

            Globals.NullularGrapherMainForm = Application.OpenForms[0];

            PopulateControlsList(Globals.NullularGrapherMainForm);

            // cross-thread setup stuff here
            Globals.NullularGrapherMainForm.Invoke((MethodInvoker)(() =>
            {
                foreach (var ilist in new[] {
                    (Globals.EditorTabs.ImageList = new ImageList()),
                    (Globals.MainMenu.ImageList = new ImageList()) })
                {
                    ilist.Images.Add("nulloader_plugin", Resources.plugin);
                }

                var graph = Globals.grapherControls.Where(x => x.Name == "graphPanel").Single();

                Globals.MainMenu.Items.Add(
                    new ToolStripMenuItem("Plugins", null,
                        new ToolStripMenuItem("(none)") { Enabled = false }
                        ));
            }));

            if (!Directory.Exists("plugins"))
                return;
            
            var noargs = new Type[0];

            Plugin[] Plugins = Directory.GetFiles("plugins", "*.dll", SearchOption.TopDirectoryOnly)
                .Select(f => { try { return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, f)); } catch { return null; } })
                .Where(a => a != null)
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsSubclassOf(typeof(Plugin)))
                .Select(t => t.GetConstructor(noargs))
                .Where(c => c != null)
                .Select(c => (Plugin)c.Invoke(noargs))
                .ToArray();
        }

        public static void PopulateControlsList(Control c)
        {
            if (!(string.IsNullOrEmpty(c.Name) || Globals.grapherControls.ContainsKey(c.Name)))
                Globals.grapherControls.Add(c.Name, c);

            foreach (var child in c.Controls)
                PopulateControlsList(child as Control);
        }
    }
}
