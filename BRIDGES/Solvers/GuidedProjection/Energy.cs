using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining an energy for the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public sealed class Energy
    {
        #region Fields

        /// <summary>
        /// Variables composing the local vector LocalX on which the <see cref="Type"/> is defined.
        /// </summary>
        private readonly List<Variable> _variables;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the energy type defining the local vector <see cref="EnergyType.LocalKi"/> and the scalar value <see cref="EnergyType.Si"/>.
        /// </summary>
        public EnergyType Type { get; private set; }

        /// <summary>
        /// Gets the variables composing the local vector LocalX.
        /// </summary>
        public IReadOnlyList<Variable> Variables => _variables;


        /// <summary>
        /// Gets or sets the weight of the energy.
        /// </summary>
        public double Weight { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Energy"/> class.
        /// </summary>
        /// <param name="energyType"> Energy type defining the local quantities of the energy. </param>
        /// <param name="variablesKi"> Variables composing the local vector LocalX. </param>
        /// <param name="weight"> Weight of the energy. </param>
        public Energy(EnergyType energyType, List<Variable> variablesKi, double weight)
        {
            this.Type = energyType;
            this._variables = variablesKi;

            Weight = weight;
        }

        #endregion
    }
}
