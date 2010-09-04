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
        internal static TabControl EditorTabs { get { return editorTabs ?? (editorTabs = grapherControls["editorTabs"] as TabControl); } }

        static MenuStrip mainMenu = null;
        internal static MenuStrip MainMenu { get { return mainMenu ?? (mainMenu = grapherControls["mainMenu"] as MenuStrip); } }

        internal static Dictionary<string, Control> grapherControls = new Dictionary<string, Control>();

        static TwoDimensionalGraph twoDgraph = null;
        internal static TwoDimensionalGraph TwoDGraph { get { return twoDgraph ?? (twoDgraph = new TwoDimensionalGraph(grapherControls["graphPanel"])); } }

        static ThreeDimensionalGraph threeDgraph = null;
        internal static ThreeDimensionalGraph ThreeDGraph { get { return threeDgraph ?? (threeDgraph = new ThreeDimensionalGraph((A.J)grapherControls["scenePanel"])); } }
    }
}
