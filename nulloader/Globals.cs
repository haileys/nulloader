using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nulloader
{
    public static class Globals
    {
        public static Form NullularGrapherMainForm { get; internal set; }

        static TabControl editorTabs = null;
        internal static TabControl EditorTabs { get { return editorTabs ?? (editorTabs = grapherControls.Where(x => x.Name == "editorTabs").First() as TabControl); } }

        static MenuStrip mainMenu = null;
        internal static MenuStrip MainMenu { get { return mainMenu ?? (mainMenu = grapherControls.Where(x => x.Name == "mainMenu").First() as MenuStrip); } }

        internal static IEnumerable<Control> grapherControls { get; set; }

        static TwoDimensionalGraph twoDgraph = null;
        internal static TwoDimensionalGraph TwoDGraph { get { return twoDgraph ?? (twoDgraph = new TwoDimensionalGraph(grapherControls.Where(x => x.Name == "graphPanel").First())); } }

        static ThreeDimensionalGraph threeDgraph = null;
        internal static ThreeDimensionalGraph ThreeDGraph { get { return threeDgraph ?? (threeDgraph = new ThreeDimensionalGraph((A.J)grapherControls.Where(x => x.Name == "scenePanel").First())); } }

        internal static bool CreatedPluginMenuItemYet = false;
    }
}
