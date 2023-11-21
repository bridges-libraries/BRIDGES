using System;
using System.Collections.Generic;

using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining an energy for the <see cref="Solver"/>.
    /// </summary>
    public sealed class Energy
    {
        #region Fields

        /// <summary>
        /// Variables composing the local vector localX on which the <see cref="EnergyType"/> is defined.
        /// </summary>
        private readonly IVariable[] _variables;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the energy type defining the local vector <see cref="EnergyType.LocalKi"/> and the scalar value <see cref="EnergyType.Si"/>.
        /// </summary>
        public EnergyType Type { get; private set; }

        /// <summary>
        /// Gets the variables composing the local vector localX.
        /// </summary>
        public IReadOnlyList<IVariable> Variables => _variables;


        /// <summary>
        /// Gets or sets the weight of this energy.
        /// </summary>
        public double Weight { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Energy"/> class.
        /// </summary>
        /// <param name="energyType"> Energy type defining the local quantities of the energy. </param>
        /// <param name="variablesKi"> Variables composing the local vector localX. </param>
        /// <param name="weight"> Weight of the energy. </param>
        public Energy(EnergyType energyType, IReadOnlyList<IVariable> variablesKi, double weight)
        {
            this.Type = energyType;

            this._variables = new IVariable[variablesKi.Count];
            for (int i = 0; i < variablesKi.Count; i++)
            {
                _variables[i] = variablesKi[i];
            }

            Weight = weight;
        }

        #endregion
    }
}
