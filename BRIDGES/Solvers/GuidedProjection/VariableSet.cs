using System;
using System.Collections;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Class defining a set of variables with the same dimension (i.e. the same number of components).
    /// </summary>
    public class VariableSet : IEnumerable<Variable>
    {
        #region Fields
        
        /// <summary>
        /// List of th variables in the set.
        /// </summary>
        private readonly List<Variable> _variables;

        #endregion

        #region Properties

        /// <summary>
        /// Number of variables in this set.
        /// </summary>
        public int Count => _variables.Count;

        /// <summary>
        /// Common dimension of the variables in this set. 
        /// </summary>
        public int Dimension { get; private set; }

        /// <summary>
        /// Gets a variable at the specified index in this set.
        /// </summary>
        /// <param name="index"> The zero-based index of the variable to get. </param>
        /// <returns> The variable at the specified index. </returns>
        public Variable this[int index] => _variables[index];

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSet"/> class.
        /// </summary>
        /// <param name="dimension"> Common dimension of the variables in the set. </param>
        /// <param name="capacity"> Number of variables that the set can initially store. </param>
        public VariableSet(int dimension, int capacity)
        {
            Dimension = dimension;

            _variables = new List<Variable>(capacity);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Adds a variable in this set.
        /// </summary>
        /// <param name="components"> Components of the variable to add. </param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"> The number of components of the variable must be equal to the common dimension of the variables in this set. </exception>
        public Variable AddVariable(params double[] components)
        {
            if ( Dimension != components.Length)
            {
                throw new ArgumentOutOfRangeException("The number of components of the variable must be equal to the common dimension of the variables in this set.");
            }

            Variable variable = new Variable(components);
            _variables.Add(variable);

            return variable;
        }


        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
        public IEnumerator<Variable> GetEnumerator() =>_variables.GetEnumerator();

        #endregion


        #region Explicit Implementations : IEnumerable

        /// <inheritdoc cref="IEnumerable.GetEnumerator()"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
