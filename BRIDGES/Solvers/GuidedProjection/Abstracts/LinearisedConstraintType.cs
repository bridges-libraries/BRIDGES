using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection.Abstracts
{
    /// <summary>
    /// Abstract class defining a linearised constraint type.
    /// </summary>
    public abstract class LinearisedConstraintType : ConstraintType
    {
        #region Methods

        /// <summary>
        /// Updates the local members (i.e. LocalHi and LocalBi) and the value Ci of the linearised constraint using the local variables.
        /// </summary>
        /// <param name="variables"> Actualised value of the constraint's local variables. </param>
        public abstract void UpdateLocal(IReadOnlyList<Variable> variables);

        #endregion
    }
}
