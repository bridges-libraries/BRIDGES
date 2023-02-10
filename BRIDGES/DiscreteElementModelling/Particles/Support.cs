using System;
using System.Collections.Generic;
using System.Text;

using BRIDGES.Geometry.Euclidean3D;

namespace BRIDGES.DiscreteElementModelling.Particles
{
    /// <summary>
    /// 
    /// </summary>
    public class Support
    {
        #region Constructor
        public Support()
        {
        } 

        public Support(Plane plane, bool x, bool y, bool z)
        {
            BasePlane = plane;
            Xdirection = x;
            Ydirection = y;
            Zdirection = z;
        }
        #endregion

        #region Properties
        public Plane BasePlane { get; set; }

        public bool Xdirection { get; set; }
        public bool Ydirection { get; set; }
        public bool Zdirection { get; set; } 
        #endregion
    }
}
