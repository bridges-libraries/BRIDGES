﻿using System;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Variable of the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public sealed class Variable : IEquatable<Variable>
    {
        #region Fields

        /// <summary>
        /// Segment of the global vector X representing this variable. 
        /// </summary>
        private ArraySegment<double> _components;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the component at the specified index in this variable.
        /// </summary>
        /// <param name="index"> The zero-based index of the variable's component to get. </param>
        /// <returns> The component at the specified index. </returns>
        public double this[int index]
        {
            get 
            {
                return index < _components.Count ? _components.Array[_components.Offset + index] :
                    throw new IndexOutOfRangeException("The index must be positive and smaller than the dimmension of the variable.");
            }
        }

        /// <summary>
        /// Get the number of components of this variable.
        /// </summary>
        public int Dimension => _components.Count;


        /// <summary>
        /// Gets the index of the variable's first component, relative to the start of the global vector X.
        /// </summary>
        internal int Offset => _components.Offset;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initialises a new instance of the <see cref="Variable"/> class from its components.
        /// </summary>
        /// <param name="components"> Components of the variable. </param>
        public Variable(params double[] components)
        {
            if (components is null) { throw new ArgumentNullException(nameof(components)); }
            if (components.Length == 0) { throw new RankException("The variable must be initialised with at least one component value."); }

            double[] array = (double[])components.Clone();

            _components = new ArraySegment<double>(array);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an array representation of the variable.
        /// </summary>
        /// <returns> The double-precision array representing the variable. </returns>
        public double[] ToArray()
        {
            double[] array = new double[Dimension];
            for (int i = 0; i < Dimension; i++) 
            { 
                array[i] = this[i]; 
            }

            return array;
        }


        /// <summary>
        /// Creates a <see cref="Span{T}"/> representation of the variable
        /// </summary>
        /// <returns> The <see cref="Span{T}"/> representation of the variable. </returns>
        public Span<double> ToSpan()
        {
            return new Span<double>(_components.Array, _components.Offset, _components.Count);
        }

        /// <summary>
        /// Creates a <see cref="Span{T}"/> representation of the variable
        /// </summary>
        /// <param name="start"> Zero-based index of the first component to include in the <see cref="Span{T}"/>. </param>
        /// <param name="count"> Number of components to include in the <see cref="Span{T}"/>. </param>
        /// <returns> The <see cref="Span{T}"/> representation of the variable. </returns>
        public Span<double> ToSpan(int start, int count)
        {
            return new Span<double>(_components.Array, _components.Offset + start, count);
        }


        /// <summary>
        /// Changes the reference of this variable.
        /// </summary>
        /// <remarks> Interfering with the variable's reference can create issues with the solving. </remarks>
        /// <param name="array"> New array to refer to. </param>
        /// <param name="offset"> Index of the variable's first component in the array. </param>
        internal void ChangeReference(double[] array, int offset)
        {
            _components = new ArraySegment<double>(array, offset, Dimension);
        }


        // ---------- Implement IEquatable<.> ---------- //

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        /// <summary> Indicates whether this <see cref="Variable"/> is equal to another <see cref="Variable"/>. </summary>
        /// <param name="other"> A <see cref="Variable"/> to compare with this one. </param>
        /// <returns> <see langword="true"/> if their <see cref="ArraySegment{T}"/> field are equal, <see langword="false"/> otherwise. </returns>
        public bool Equals(Variable other) => _components.Equals(other._components);

        #endregion


        #region Override : Object

        /// <inheritdoc cref="Object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return obj is Variable variable && Equals(variable);
        }

        /// <inheritdoc cref="Object.GetHashCode()"/>
        public override int GetHashCode()
        {
            return 1716805434 + _components.GetHashCode();
        }

        #endregion
    }
}
