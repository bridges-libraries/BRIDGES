using System;
using System.Numerics;
using System.Collections.Generic;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors
{
    /// <summary>
    /// Represents a generic sparse vector.
    /// </summary>
    /// <typeparam name="T"> Type of the vector values. </typeparam>
    public class SparseVector<T> : Vector<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Storage for the non-zero components of this <see cref="SparseVector{T}"/>.
        /// </summary>
        /// <remarks> 
        /// <list type="bullet"> 
        ///     <item>
        ///         <term>Key</term>
        ///         <description> Row index of the non-zero component. </description>
        ///     </item>
        ///     <item>
        ///         <term>Value</term>
        ///         <description> Value of the non-zero component at the given row index. </description>
        ///     </item>
        /// </list> 
        /// </remarks>
        private readonly Dictionary<int, T> _components;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of non-zero components in this sparse vector. <br/>
        /// It obtained as the number of entries in this sparse storage.
        /// </summary>
        /// <remarks> The total number of components is given by <see cref="Vector{T}.RowCount"/>. </remarks>
        public int NonZeroCount => _components.Count;

        /// <summary>
        /// Gets or sets the component at a given index in this sparse vector. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>.
        /// </remarks>
        /// <param name="index"> Index of the component to get or set. </param>
        /// <returns> The <typeparamref name="T"/> number at the given index. </returns>
        public T this[int index]
        {
            get { return _components[index]; }
            set { _components[index] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from its number of .
        /// </summary>
        /// <remarks> The sparse storage is initialised empty. </remarks>
        /// <param name="rowCount"> Number of rows of the <see cref="SparseVector{T}"/>. </param>
        public SparseVector(int rowCount)
            : base(rowCount)
        {
            _components = new Dictionary<int, T>(RowCount / 4);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from its size and sets of indices and values.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, no checks are performed on the non-zero condition of the <paramref name="values"/>.
        /// </remarks>
        /// <param name="rowCount"> Number of components for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="indices"> Indices of the non-zero values for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="values"> Non-zero values for the <see cref="SparseVector{T}"/>. </param>
        /// <exception cref="ArgumentException"> The numbers of elements in <paramref name="indices"/> and <paramref name="values"/> must be the same. </exception>
        public SparseVector(int rowCount, IList<int> indices, IList<T> values)
            : base(rowCount)
        {
            //     -----     Verifications

            if (indices.Count != values.Count)
            {
                throw new ArgumentException($"The numbers of elements in {nameof(indices)} and {nameof(values)} must be the same.");
            }


            _components = new Dictionary<int, T>(RowCount);
            for (int i = 0; i < values.Count; i++) { _components.Add(indices[i], values[i]); }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from its size and sets of indices and values.
        /// </summary>
        /// <param name="rowCount"> Number of components for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="indices"> Indices of the non-zero values for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="values"> Non-zero values for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="nonZeroCheck"> Evaluates whether the <paramref name="values"/> should be checked for the non-zero condition. </param>
        /// <exception cref="ArgumentException"> The numbers of elements in <paramref name="indices"/> and <paramref name="values"/> must be the same. </exception>
        public SparseVector(int rowCount, IList<int> indices, IList<T> values, bool nonZeroCheck)
            : base(rowCount)
        {
            //     -----     Verifications

            if (indices.Count != values.Count)
            {
                throw new ArgumentException($"The numbers of elements in {nameof(indices)} and {nameof(values)} must be the same.");
            }

            _components = new Dictionary<int, T>(RowCount);

            if (nonZeroCheck)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i] == T.Zero) { continue; }

                    _components.Add(indices[i], values[i]);
                }
            }
            else
            {
                for (int i = 0; i < values.Count; i++) { _components.Add(indices[i], values[i]); }
            }
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from another <see cref="SparseVector{T}"/>.
        /// </summary>
        /// <param name="other"> <see cref="SparseVector{T}"/> to deep copy. </param>
        public SparseVector(SparseVector<T> other)
            : base(other.RowCount)
        {
            _components = new Dictionary<int, T>(other._components);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="SparseVector{T}"/> class from its components.
        /// </summary>
        /// <param name="rowCount"> Number of components of the <see cref="SparseVector{T}"/>. </param>
        /// <param name="components"> The components of the <see cref="SparseVector{T}"/>. </param>
        internal SparseVector(int rowCount, Dictionary<int, T> components)
            : base(rowCount)
        {
            _components = components;
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Returns the zero sparse vector.
        /// </summary>
        /// <returns> The <see cref="SparseVector{T}"/> of the given size, with zero on each components. </returns>
        public static SparseVector<T> Zero(int rowCount) => new SparseVector<T>(rowCount);

        /// <summary>
        /// Returns the i-th unit sparse vector.
        /// </summary>
        /// <remarks> Corresponds to the vector with <paramref name="rowCount"/> components, with one at the given row <paramref name="index"/> and zeros elsewhere. </remarks>
        /// <param name="rowCount"> Number of components for the <see cref="SparseVector{T}"/>. </param>
        /// <param name="index"> Index of the standard vector, which corresponds to the coordinate of the component equal to one. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> representing the standard vector. </returns>
        public static SparseVector<T> StandardVector(int rowCount, int index) => new SparseVector<T>(rowCount) { [index] = T.One };

        #endregion

        #region Public Static Methods

        //     -----     -----     Additive Abelian Group : SparseVector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(SparseVector{T}, SparseVector{T})"/>
        public static SparseVector<T> Add(SparseVector<T> left, SparseVector<T> right) => left + right;

        /// <inheritdoc cref="operator -(SparseVector{T}, SparseVector{T})"/>
        public static SparseVector<T> Subtract(SparseVector<T> left, SparseVector<T> right) => left - right;


        /// <inheritdoc cref="operator -(SparseVector{T})"/>
        public static SparseVector<T> Opposite(SparseVector<T> operand) => -operand;


        //     -----     Other Operations : SparseVector<T>     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.TransposeMultiply{T}(SparseVector{T}, SparseVector{T})"/>
        public static T TransposeMultiply(SparseVector<T> left, SparseVector<T> right) => Operations.VectorOperations.TransposeMultiply(left, right);
        

        //     -----     -----     Right Embedding : DenseVector<T>     -----     -----     //

        /// <inheritdoc cref="operator +(SparseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> Add(SparseVector<T> left, DenseVector<T> right) => left + right;

        /// <inheritdoc cref="operator -(SparseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> Subtract(SparseVector<T> left, DenseVector<T> right) => left - right;


        //     -----     Other Right Operations : SparseMatrix<T>     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.TransposeMultiply{T}(SparseVector{T}, DenseVector{T})"/>
        public static T TransposeMultiply(SparseVector<T> left, DenseVector<T> right) => Operations.VectorOperations.TransposeMultiply(left, right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(SparseVector{T}, T)"/>
        public static SparseVector<T> Multiply(SparseVector<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, SparseVector{T})"/>
        public static SparseVector<T> Multiply(T factor, SparseVector<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(SparseVector{T}, T)"/>
        public static SparseVector<T> Divide(SparseVector<T> operand, T divisor) => operand / divisor;


        #endregion

        #region Operators

        //     -----     -----     Additive Abelian Group : SparseVector<T>     -----     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.Add{T}(SparseVector{T}, SparseVector{T})"/>
        public static SparseVector<T> operator +(SparseVector<T> left, SparseVector<T> right) => Operations.VectorOperations.Add(left, right);

        /// <inheritdoc cref="Operations.VectorOperations.Subtract{T}(SparseVector{T}, SparseVector{T})"/>
        public static SparseVector<T> operator -(SparseVector<T> left, SparseVector<T> right) => Operations.VectorOperations.Subtract(left, right);


        /// <inheritdoc cref="Operations.VectorOperations.Negate{T}(SparseVector{T})"/>
        public static SparseVector<T> operator -(SparseVector<T> operand) => Operations.VectorOperations.Negate(operand);


        //     -----     -----     Right Embedding : DenseVector<T>     -----     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.Add{T}(SparseVector{T}, SparseVector{T})"/>
        public static DenseVector<T> operator +(SparseVector<T> left, DenseVector<T> right) => Operations.VectorOperations.Add(left, right);

        /// <inheritdoc cref="Operations.VectorOperations.Subtract{T}(SparseVector{T}, DenseVector{T})"/>
        public static DenseVector<T> operator -(SparseVector<T> left, DenseVector<T> right) => Operations.VectorOperations.Subtract(left, right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="Operations.VectorOperations.Multiply{T}(SparseVector{T}, T)"/>
        public static SparseVector<T> operator *(SparseVector<T> operand, T factor) => Operations.VectorOperations.Multiply(operand, factor);

        /// <inheritdoc cref="Operations.VectorOperations.Multiply{T}(T, SparseVector{T})"/>
        public static SparseVector<T> operator *(T factor, SparseVector<T> operand) => Operations.VectorOperations.Multiply(factor, operand);


        /// <inheritdoc cref="Operations.VectorOperations.Divide{T}(SparseVector{T}, T)"/>
        public static SparseVector<T> operator /(SparseVector<T> operand, T divisor) => Operations.VectorOperations.Divide(operand, divisor);

        #endregion

        #region Public Methods

        /// <summary>
        /// Attemps to get the components at the given index.
        /// </summary>
        /// <param name="index"> Index of the component to get. </param>
        /// <param name="value"> Value of the component at the given index if it was found, <see langword="default"/> otherwise. </param>
        /// <returns> <see langword="true"/> if the component was found, <see langword="false"/> otherwise. </returns>
        public bool TryGet(int index, out T? value)
        {
            return _components.TryGetValue(index, out value);
        }

        /// <summary>
        /// Evaluates whether the sparse storage contains an entry for the component at the given index.
        /// </summary>
        /// <param name="index"> Index of the component to retrieve. </param>
        /// <returns> <see langword="true"/> if the sparse storage contains an entry at the given index, <see langword="false"/> otherwise.  </returns>
        public bool Contains(int index)
        {
            return _components.ContainsKey(index);
        }


        /// <inheritdoc/>
        /// <remarks> If the sparse storage does not have an existing entry for the component, zero is returned. </remarks>
        public override T Get(int index)
        {
            return _components.TryGetValue(index, out T? value) ? value : T.Zero;
        }


        /// <inheritdoc cref="Vector{T}.ToArray()"/>
        public override T[] ToArray()
        {
            T[] result = new T[RowCount];
            foreach ((int index, T value) in this) { result[index] = value; }

            return result;
        }

        #endregion


        #region Implement : IEnumerable<T>

        /// <inheritdoc/>
        public override IEnumerator<(int, T)> GetEnumerator()
        {
            foreach(var kvp in _components)
            {
                yield return (kvp.Key, kvp.Value);
            }
        }

        #endregion
    }
}
