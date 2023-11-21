using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection.Abstracts
{
    /// <summary>
    /// Abstract class defining a linearised constraint type.
    /// </summary>
    public abstract class UpdatedConstraintType : ConstraintType
    {
        #region Fields

        /// <summary>
        /// Objects used to update the local members (i.e. LocalHi and LocalBi) and the value Ci of the constraint.
        /// </summary>
        private readonly object[] _objects;

        #endregion

        #region Methods

        /// <summary>
        /// Updates the local members (i.e. LocalHi and LocalBi) and the value Ci of the constraint using the <see cref="_objects"/>.
        /// </summary>
        internal abstract void UpdateLocal();

        #endregion
    }
}
