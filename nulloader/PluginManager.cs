using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace nulloader
{
    public static class PluginManager
    {
        internal static PluginWrapper[] AvailablePlugins { get; private set; }
        internal static Plugin[] LoadedPlugins { get { return loadedPlugins.ToArray(); } }

        static List<Plugin> loadedPlugins = new List<Plugin>();

        static bool restartRequired = false;
        public static bool RestartRequired { get { return restartRequired; } private set { restartRequired = value; if (value && RestartNeeded != null) RestartNeeded(); } }
        public static event Action RestartNeeded;

        public static void Init()
        {
            var empty_type_array = new Type[0];

            if (!Directory.Exists("plugins"))
                return;

            AvailablePlugins = Directory.GetFiles("plugins", "*.dll", SearchOption.TopDirectoryOnly)
                .Select(f => { try { return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, f)); } catch { return null; } })
                .Where(a => a != null)
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsSubclassOf(typeof(Plugin)))
                .Where(t => t.GetConstructor(empty_type_array) != null)
                .Select(t => new PluginWrapper(t))
                .ToArray();

            foreach (var plugin in AvailablePlugins.Where(w => w.Enabled))
            {
                    Start(plugin.PluginType);
            }
        }

        public static void Disable(Type PluginType)
        {
            File.WriteAllLines("disabled.cfg", File.ReadAllLines("disabled.cfg").Union(new[] { PluginType.FullName }).ToArray());
            RestartRequired = true;
        }

        public static Plugin Start(Type PluginType)
        {
            Plugin plugin;
            try
            {
                plugin = PluginType.GetConstructor(new Type[0]).Invoke(new Type[0]) as Plugin;
            }
            catch (Exception ex)
            {
                List<string> log = new List<string>();
                log.Add(string.Format("** Crash: {0} - {1}", PluginType.FullName, DateTime.Now));
                log.AddRange(ex.InnerException.ToString().Split('\n').Select(s => "    " + s));
                log.Add("\n\n");
                File.AppendAllLines("plugin_crash.log", log);

                Disable(PluginType);
                MessageBox.Show(string.Format("An error occurred while loading the plugin '{0}'.\nThis plugin has been disabled and Nullular Grapher will now restart.\n\n\n{1}",
                    PluginType.FullName,
                    ex.InnerException.ToString()), "Plugin Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Application.Restart();
                Environment.Exit(1);
                return null;
            }
            if (!loadedPlugins.Contains(plugin))
                loadedPlugins.Add(plugin);
            return plugin;
        }

        public static Plugin Enable(Type PluginType)
        {
            File.WriteAllLines("disabled.cfg", File.ReadAllLines("disabled.cfg").Where(s => s != PluginType.FullName).ToArray());
            return Start(PluginType);
        }
    }
    public class PluginWrapper
    {
        public Type PluginType { get; private set; }
        public bool Enabled { get; private set; }

        static string[] disabledTypes = File.ReadAllLines("disabled.cfg");

        internal PluginWrapper(Type PluginType)
        {
            this.PluginType = PluginType;
            Enabled = !disabledTypes.Contains(PluginType.FullName);
        }
    }
}
