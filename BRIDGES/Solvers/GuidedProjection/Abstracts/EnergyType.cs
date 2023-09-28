using System;

using BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.Solvers.GuidedProjection.Abstracts
{
    /// <summary>
    /// Abstract class defining an energy type.
    /// </summary>
    public abstract class EnergyType
    {
        #region Properties

        /// <summary>
        /// Gets the local vector Ki of this energy.
        /// </summary>
        public SparseVector LocalKi { get; protected set; }

        /// <summary>
        /// Gets the scalar value Si of this energy.
        /// </summary>
        public double Si { get; protected set; }

        #endregion
    }
}
