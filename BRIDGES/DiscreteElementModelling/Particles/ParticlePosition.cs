using System;
using System.Collections.Generic;
using System.Text;
using BRIDGES.Geometry.Euclidean3D;

namespace BRIDGES.DiscreteElementModelling.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ParticlePosition : ICloneable
    {
        #region Constructor
        /// <summary>
        /// Create a new instance of the class <see cref="ParticlePosition"/>.
        /// </summary>
        public ParticlePosition()
        {
            Current = new Point();
            Previous = new Point();
        }

        /// <summary>
        /// Create a particle Position from a Point3D.
        /// </summary>
        /// <param name="point">A point3D defining the position of the particle.</param>
        public ParticlePosition(Point point)
        {
            Current = point;
            Previous = point;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Current position of the particle
        /// </summary>
        public Point Current { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Point Previous { get; set; }

        #endregion

        #region ICloneable
        /// <summary>
        /// Creates a new ParticlePosition that is a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            ParticlePosition pp = new ParticlePosition();
            pp.Current = Current;
            pp.Previous = Previous;
            return pp;
        }
        #endregion
    }
}
