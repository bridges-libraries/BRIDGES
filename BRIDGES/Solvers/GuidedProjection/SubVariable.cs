using System;
using System.Collections.Generic;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// SubVariable for the <see cref="Solver"/>, whose retained components are referenced from a <see cref="Variable"/>.
    /// </summary>
    /// <remarks> <see cref="SubVariable"/> don't need to be add to the <see cref="Solver"/> (as long as the <see cref="Variable"/> is. </remarks>
    public class SubVariable : IVariable, IEquatable<SubVariable>
    {
        #region Fields

        /// <summary>
        /// Indices of the components in the variable which are referenced in the SubVariable
        /// </summary>
        private readonly int[] _indices;

        /// <summary>
        /// Variable to which the SubVariable refers for its components.
        /// </summary>
        private Variable _variable;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public bool IsReference => true;


        /// <inheritdoc/>
        public double this[int index] => _variable[_indices[index]];

        /// <inheritdoc/>
        public int Dimension => _indices.Length;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SubVariable"/> class from the reference variable and the retained indices.
        /// </summary>
        /// <param name="variable"> Reference variable. </param>
        /// <param name="indices"> Retained indices of the components in the variable. </param>
        /// <exception cref="IndexOutOfRangeException"> One of the retained index is larger the dimension of the reference array. </exception>
        /// <exception cref="ArgumentException"> Two retained indices can not be equal. </exception>
        public SubVariable(Variable variable, IReadOnlyList<int> indices)
        {
            this._variable = variable;

            this._indices = new int[indices.Count];
            for (int i = 0; i < indices.Count; i++) { _indices[i] = indices[i]; }

            // Verifications
            for (int i = 0; i < _indices.Length; i++)
            {
                if (!(_indices[i] < variable.Dimension))
                {
                    throw new IndexOutOfRangeException("One of the retained index is larger the dimension of the reference array.");
                }
                for (int j = 0; j < i; j++)
                {
                    if (_indices[i] == _indices[j])
                    {
                        throw new ArgumentException("Two retained indices can not be equal.");
                    }
                }
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SubVariable"/> class from the reference variable and a range of indices.
        /// </summary>
        /// <param name="variable"> Reference variable. </param>
        /// <param name="start"> Zero-based index of the first component to retain in the variable. </param>
        /// <param name="count"> Number of component to retain. </param>
        public SubVariable(Variable variable, int start, int count)
        {
            this._variable = variable;

            this._indices = new int[count];
            for (int i = 0; i < count; i++) { _indices[i] = start + i; }
        }

        #endregion

        #region Public Methods

        // ---------- Implement IVariable ---------- //

        /// <inheritdoc/>
        public int ReferenceIndex(int index) => _variable.ReferenceIndex(_indices[index]);

        /// <inheritdoc/>
        public double[] ToArray()
        {
            double[] components = new double[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                components[i] = _variable[_indices[i]];
            }

            return components;
        }


        // ---------- Implement IEquatable<.> ---------- //

        /// <summary> Indicates whether this <see cref="Variable"/> is equal to another <see cref="Variable"/>. </summary>
        /// <param name="other"> A <see cref="Variable"/> to compare with this one. </param>
        /// <returns> <see langword="true"/> if their <see cref="ArraySegment{T}"/> field are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(SubVariable other) => _variable.Equals(other._variable) & _indices.Equals(other._indices);

        #endregion


        #region Override : Object

        /// <inheritdoc cref="Object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is SubVariable subVariable && Equals(subVariable);
        }

        /// <inheritdoc cref="Object.GetHashCode()"/>
        public override int GetHashCode()
        {
            int hashCode = -924498043;
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(_indices);
            hashCode = hashCode * -1521134295 + EqualityComparer<Variable>.Default.GetHashCode(_variable);
            return hashCode;
        }

        #endregion
    }
}
