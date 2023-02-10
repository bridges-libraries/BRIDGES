using System;
using System.Collections.Generic;
using System.Text;

namespace BRIDGES.DiscreteElementModelling.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ParticleOrientation : ICloneable
    {
        #region Constructor
        /// <summary>
        /// Create a new instance of the class <see cref="ParticleOrientation"/>.
        /// </summary>
        public ParticleOrientation()
        {
        }
        #endregion

        #region ICloneable
        /// <summary>
        /// Creates a new ParticleOrientation class that is a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new ParticleOrientation();
        }
        #endregion
    }
}