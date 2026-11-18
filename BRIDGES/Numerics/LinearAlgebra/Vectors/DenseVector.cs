using BRIDGES.Numerics.LinearAlgebra.Matrices;
using System;
using System.Collections.Generic;
using System.Numerics;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors
{
    /// <summary>
    /// Represents a generic dense vector.
    /// </summary>
    /// <typeparam name="T"> Type of the vector components. </typeparam>
    public class DenseVector<T> : Vector<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Components of this <see cref="DenseVector{T}"/>.
        /// </summary>
        private readonly T[] _components;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the component at a given index.
        /// </summary>
        /// <param name="index"> Index of the component to get or set. </param>
        /// <returns> The value of the component. </returns>
        public T this[int index]
        {
            get { return _components[index]; }
            set { _components[index] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector{T}"/> class from its size.
        /// </summary>
        /// <remarks> The dense vector is initialised with <see langword="default"/> values. </remarks>
        /// <param name="rowCount"> Number of components of the <see cref="DenseVector{T}"/>. </param>
        public DenseVector(int rowCount)
            : base(rowCount)
        {
            _components = new T[rowCount];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector{T}"/> class from its components.
        /// </summary>
        /// <param name="components"> Components of the <see cref="DenseVector{T}"/>. </param>
        public DenseVector(IList<T> components)
            : base(components.Count)
        {
            _components = new T[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                _components[i] = components[i];
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector{T}"/> class from another <see cref="DenseVector{T}"/>.
        /// </summary>
        /// <param name="vector"> <see cref="DenseVector{T}"/> to deep copy. </param>
        public DenseVector(DenseVector<T> vector)
            : base(vector.RowCount)
        {
            _components = new T[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                _components[i] = vector._components[i];
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseVector{T}"/> class from a <see cref="SparseVector{T}"/>.
        /// </summary>
        /// <param name="vector"> <see cref="SparseVector{T}"/> to copy. </param>
        public DenseVector(SparseVector<T> vector)
            : base(vector.RowCount)
        {
            _components = new T[RowCount];
            foreach ((int row, T value) in vector)
            {
                _components[row] = value;
            }
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the zero dense vector.
        /// </summary>
        /// <param name="rowCount"> Number of component of the <see cref="DenseVector{T}"/>. </param>
        /// <returns> The <see cref="DenseVector{T}"/> of the given size, with zero on each components. </returns>
        public static DenseVector<T> Zero(int rowCount) => new DenseVector<T>(rowCount);

        /// <summary>
        /// Returns the i-th unit dense vector.
        /// </summary>
        /// <remarks> It corresponds to the vector with <paramref name="rowCount"/> components, with one at the given row <paramref name="index"/> and zeros elsewhere. </remarks>
        /// <param name="rowCount"> Number of components for the <see cref="DenseVector{T}"/>. </param>
        /// <param name="index"> Index of the standard vector, which corresponds to the coordinate of the component equal to one. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> representing the standard vector. </returns>
        public static DenseVector<T> StandardVector(int rowCount, int index) => new DenseVector<T>(rowCount) { [index] = T.One };

        #endregion

        #region Public Static Methods

        //     -----     -----     Additive Abelian Group : DenseVector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(DenseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> Add(DenseVector<T> left, DenseVector<T> right) => left + right;

        /// <inheritdoc cref="operator -(DenseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> Subtract(DenseVector<T> left, DenseVector<T> right) => left - right;


        /// <inheritdoc cref="operator -(DenseVector{T})"/>
        public static DenseVector<T> Opposite(DenseVector<T> operand) => -operand;


        //     -----     Other Operations : DenseVector<T>     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.TransposeMultiply{T}(DenseVector{T}, DenseVector{T})"/>
        public static T TransposeMultiply(DenseVector<T> left, DenseVector<T> right) => Operations.VectorOperations.TransposeMultiply(left, right);


        //     -----     -----     Right Embedding : SparseVector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(DenseVector{T}, SparseVector{T})"/>
        public static DenseVector<T> Add(DenseVector<T> left, SparseVector<T> right) => left + right;

        /// <inheritdoc cref="operator -(DenseVector{T}, SparseVector{T})"/>
        public static DenseVector<T> Subtract(DenseVector<T> left, SparseVector<T> right) => left - right;


        //     -----     Other Right Operations : SparseMatrix<T>     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.TransposeMultiply{T}(DenseVector{T}, SparseVector{T})"/>
        public static T TransposeMultiply(DenseVector<T> left, SparseVector<T> right) => Operations.VectorOperations.TransposeMultiply(left, right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(DenseVector{T}, T)"/>
        public static DenseVector<T> Multiply(DenseVector<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, DenseVector{T})"/>
        public static DenseVector<T> Multiply(T factor, DenseVector<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(DenseVector{T}, T)"/>
        public static DenseVector<T> Divide(DenseVector<T> operand, T divisor) => operand / divisor;

        #endregion

        #region Operators

        //     -----     -----     Additive Abelian Group : DenseVector<T>     -----     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.Add{T}(DenseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> operator +(DenseVector<T> left, DenseVector<T> right) => Operations.VectorOperations.Add(left, right);

        /// <inheritdoc cref="Operations.VectorOperations.Subtract{T}(DenseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> operator -(DenseVector<T> left, DenseVector<T> right) => Operations.VectorOperations.Subtract(left, right);


        /// <inheritdoc cref="Operations.VectorOperations.Negate{T}(DenseVector{T})"/>
        public static DenseVector<T> operator -(DenseVector<T> operand) => Operations.VectorOperations.Negate(operand);


        //     -----     -----     Right Embedding : SparseVector<T>     -----     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.Add{T}(DenseVector{T}, SparseVector{T})"/>
        public static DenseVector<T> operator +(DenseVector<T> left, SparseVector<T> right) => Operations.VectorOperations.Add(left, right);

        /// <inheritdoc cref="Operations.VectorOperations.Subtract{T}(DenseVector{T}, SparseVector{T})"/>
        public static DenseVector<T> operator -(DenseVector<T> left, SparseVector<T> right) => Operations.VectorOperations.Subtract(left, right);


        //     -----      -----      Group Action : T      -----     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.Multiply{T}(DenseVector{T}, T)"/>
        public static DenseVector<T> operator *(DenseVector<T> operand, T factor) => Operations.VectorOperations.Multiply(operand, factor);

        /// <inheritdoc cref="Operations.VectorOperations.Multiply{T}(T, DenseVector{T})"/>
        public static DenseVector<T> operator *(T factor, DenseVector<T> operand) => Operations.VectorOperations.Multiply(factor, operand);


        /// <inheritdoc cref="Operations.VectorOperations.Divide{T}(DenseVector{T}, T)"/>
        public static DenseVector<T> operator /(DenseVector<T> operand, T divisor) => Operations.VectorOperations.Divide(operand, divisor);

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override T Get(int index)
        {
            return _components[index];
        }


        /// <inheritdoc cref="Vector{T}.ToArray()"/>
        public override T[] ToArray() => (T[])_components.Clone();

        #endregion


        #region Implement : IEnumerable<T>

        /// <inheritdoc/>
        public override IEnumerator<(int, T)> GetEnumerator()
        {
            for (int i = 0; i < RowCount; i++)
            {
                yield return (i, _components[i]);
            }
        }

        #endregion
    }
}
