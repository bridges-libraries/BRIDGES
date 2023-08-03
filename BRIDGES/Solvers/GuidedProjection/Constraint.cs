using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a quadratic constraint for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public sealed class Constraint
    {
        #region Fields

        /// <summary>
        /// Variables composing the local vector LocalX on which the <see cref="Type"/> is defined.
        /// </summary>
        private readonly List<Variable> _variables;

        #endregion

        #region Properties

        /// /// <summary>
        /// Gets the constraint type defining the reduced matrix <see cref="ConstraintType.LocalHi"/>, the reduced vector <see cref="ConstraintType.LocalBi"/> and the scalar value <see cref="ConstraintType.Ci"/>.
        /// </summary>
        public ConstraintType Type { get; private set; }

        /// <summary>
        /// Gets the variables composing the local vector LocalX.
        /// </summary>
        public IReadOnlyList<Variable> Variables => _variables;


        /// <summary>
        /// Gets or sets the value of the weight for the constraint.
        /// </summary>
        public double Weight { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        /// <param name="constraintType"> Constraint type defining the local quantities of the constraint. </param>
        /// <param name="variables"> Variables composing the local vector LocalX on which the local symmetric matrix Hi and the local vector Bi are defined.</param>
        /// <param name="weight"> Weight of the constraint. </param>
        internal Constraint(ConstraintType constraintType, List<Variable> variables, double weight)
        {
            this.Type = constraintType;
            this._variables = variables;

            Weight = weight;
        }

        #endregion
    }
}
