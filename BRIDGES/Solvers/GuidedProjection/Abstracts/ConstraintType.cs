using System;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;


namespace BRIDGES.Solvers.GuidedProjection.Abstracts
{
    /// <summary>
    /// Abstract class defining a constraint type.
    /// </summary>
    public abstract class ConstraintType
    {
        #region Properties

        /// <summary>
        /// Gets the local symmetric matrix Hi of the energy.
        /// </summary>
        public SparseMatrix LocalHi { get; protected set; }

        /// <summary>
        /// Gets the local vector Bi of the energy.
        /// </summary>
        public SparseVector LocalBi { get; protected set; }

        /// <summary>
        /// Gets the scalar value Ci of the energy.
        /// </summary>
        public double Ci { get; protected set; }

        #endregion
    }
}
