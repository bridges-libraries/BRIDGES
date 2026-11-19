using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors
{
    /// <summary>
    /// Represents a generic vector.
    /// </summary>
    /// <typeparam name="T"> Type of the vector components. </typeparam>
    public abstract class Vector<T> : IEnumerable<(int Row, T Value)>
        where T : INumberBase<T>
    {
        #region Properties

        /// <summary>
        /// Number of row of this vector.
        /// </summary>
        public int RowCount { get; init; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Vector{T}"/> class from its size.
        /// </summary>
        /// <param name="rowCount"> Total number of component. </param>
        internal Vector(int rowCount)
        {
            RowCount = rowCount;
        }

        #endregion

        #region Public Static Methods

        //     -----     -----     Additive Abelian Group : Vector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(Vector{T}, Vector{T})"/>
        public static Vector<T> Add(Vector<T> left, Vector<T> right) => left + right;

        /// <inheritdoc cref="operator -(Vector{T}, Vector{T})"/>
        public static Vector<T> Subtract(Vector<T> left, Vector<T> right) => left - right;


        /// <inheritdoc cref="operator -(Vector{T})"/>
        public static Vector<T> Opposite(Vector<T> operand) => -operand;


        //     -----     Other Operations : Vector<T>     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.TransposeMultiply{T}(Vector{T}, Vector{T})"/>
        public static T TransposeMultiply(Vector<T> left, Vector<T> right) => Operations.VectorOperations.TransposeMultiply(left, right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(Vector{T}, T)"/>
        public static Vector<T> Multiply(Vector<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, Vector{T})"/>
        public static Vector<T> Multiply(T factor, Vector<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(Vector{T}, T)"/>
        public static Vector<T> Divide(Vector<T> operand, T divisor) => operand / divisor;

        #endregion

        #region Operators

        //     -----     -----     Additive Abelian Group : Vector<T>     -----     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.Add{T}(Vector{T}, Vector{T})"/>
        public static Vector<T> operator +(Vector<T> left, Vector<T> right) => Operations.VectorOperations.Add(left, right);

        /// <inheritdoc cref="Operations.VectorOperations.Subtract{T}(Vector{T}, Vector{T})"/>
        public static Vector<T> operator -(Vector<T> left, Vector<T> right) => Operations.VectorOperations.Subtract(left, right);


        /// <inheritdoc cref="Operations.VectorOperations.Negate{T}(Vector{T})"/>
        public static Vector<T> operator -(Vector<T> operand) => Operations.VectorOperations.Negate(operand);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.Multiply{T}(Vector{T}, T)"/>
        public static Vector<T> operator *(Vector<T> operand, T factor) => Operations.VectorOperations.Multiply(operand, factor);

        /// <inheritdoc cref="Operations.VectorOperations.Multiply{T}(T, Vector{T})"/>
        public static Vector<T> operator *(T factor, Vector<T> operand) => Operations.VectorOperations.Multiply(factor, operand);


        /// <inheritdoc cref="Operations.VectorOperations.Divide{T}(Vector{T}, T)"/>
        public static Vector<T> operator /(Vector<T> operand, T divisor) => Operations.VectorOperations.Divide(operand, divisor);

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the value of the component at the given index.
        /// </summary>
        /// <param name="index"> Index of the component. </param>
        /// <returns> The value of the component. </returns>
        public abstract T GetComponent(int index);


        /// <summary>
        /// Translates this vector into its array representation.
        /// </summary>
        /// <returns> The array representing the vector. </returns>
        public abstract T[] ToArray();

        #endregion


        #region Implement : IEnumerable<T>

        /// <inheritdoc/>
        public abstract IEnumerator<(int, T)> GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
