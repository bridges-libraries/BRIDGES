using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a quadratic constraint for the <see cref="Solver"/>.
    /// </summary>
    public sealed class Constraint
    {
        #region Fields

        /// <summary>
        /// Variables composing the local vector localX on which the <see cref="ConstraintType"/> is defined.
        /// </summary>
        private readonly IVariable[] _variables;

        #endregion

        #region Properties

        /// /// <summary>
        /// Gets the constraint type defining the local matrix <see cref="ConstraintType.LocalHi"/>, the local vector <see cref="ConstraintType.LocalBi"/> and the scalar value <see cref="ConstraintType.Ci"/>.
        /// </summary>
        public ConstraintType Type { get; private set; }

        /// <summary>
        /// Gets the variables composing the local vector localX.
        /// </summary>
        public IReadOnlyList<IVariable> Variables => _variables;


        /// <summary>
        /// Gets or sets the weight of this constraint.
        /// </summary>
        public double Weight { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        /// <param name="constraintType"> Constraint type defining the local quantities of this constraint. </param>
        /// <param name="variables"> Variables composing the local vector localX on which the local symmetric matrix Hi and the local vector Bi are defined.</param>
        /// <param name="weight"> Weight of this constraint. </param>
        public Constraint(ConstraintType constraintType, IReadOnlyList<IVariable> variables, double weight)
        {
            this.Type = constraintType;

            this._variables = new IVariable[variables.Count];
            for (int i = 0; i < variables.Count; i++)
            {
                _variables[i] = variables[i];
            }

            Weight = weight;
        }

        #endregion
    }
}
