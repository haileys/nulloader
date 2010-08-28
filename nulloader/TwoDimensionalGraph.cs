using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using A;

namespace nulloader
{
    public class TwoDimensionalGraph : GraphView
    {
        internal TwoDimensionalGraph(object graph)
            : base((F)graph)
        { }
    }
}
