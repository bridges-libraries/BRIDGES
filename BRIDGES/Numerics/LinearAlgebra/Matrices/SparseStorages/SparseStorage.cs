using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages
{
    /// <summary>
    /// Represents a generic sparse storage for sparse matrices.
    /// </summary>
    /// <typeparam name="T"> Type of the values. </typeparam>
    public abstract class SparseStorage<T> : IEnumerable<(int Row, int Column, T Value)>
        where T : INumberBase<T>
    {
        #region Properties

        /// <summary>
        /// Gets the number of rows of this sparse storage.
        /// </summary>
        public int RowCount { get; init; }

        /// <summary>
        /// Gets the number of columns of this sparse storage.
        /// </summary>
        public int ColumnCount { get; init; }


        /// <summary>
        /// Gets the number of non-zero components in this sparse storage.
        /// </summary>
        /// <remarks> It is obtained as the number of entries in this sparse storage. </remarks>
        public abstract int Count { get; }


        /// <summary>
        /// Gets the component value of this sparse storage at the given row and column. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <param name="row"> Row index of the component to get or set. </param>
        /// <param name="column"> Column index of the component to get or set. </param>
        /// <returns> The <typeparamref name="T"/> number at the given row and column. </returns>
        public abstract T this[int row, int column] { get; }


        /// <summary>
        /// Gets the storage type of this sparse storage
        /// </summary>
        public abstract SparseStorageFormat StorageFormat { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseStorage{T}"/> class from its size.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the <see cref="SparseStorage{T}"/>. </param>
        /// <param name="columnCount"> Number of columns for the <see cref="SparseStorage{T}"/>. </param>
        /// <exception cref="ArgumentOutOfRangeException"> The number of rows and columns must be strictly larger than zero. </exception>
        internal SparseStorage(int rowCount, int columnCount)
        {
            //     -----     Verifications

            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), "The number of rows must be strictly larger than zero.");
            }
            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "The number of columns must be strictly larger than zero.");
            }

            // Initialise properties
            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Attemps to get the value of a component in this sparse storage at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to get. </param>
        /// <param name="column"> Column index of the component to get. </param>
        /// <param name="value"> Value of the component at the given row and column if it was found, <see langword="default"/> otherwise. </param>
        /// <returns> <see langword="true"/> if the component was found, <see langword="false"/> otherwise. </returns>
        public abstract bool TryGet(int row, int column, out T? value);

        /// <summary>
        /// Evaluates whether the sparse storage contains an entry for the component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to retrieve. </param>
        /// <param name="column"> Column index of the component to retrieve. </param>
        /// <returns> <see langword="true"/> if the sparse storage has an entry, <see langword="false"/> otherwise.  </returns>
        public abstract bool Contains(int row, int column);

        #endregion

        #region Internal Methods

        /// <summary>
        /// Provides an array representation of this sparse storage.
        /// </summary>
        /// <returns> The two-dimensional array. </returns>
        internal abstract T[,] ToArray();

        /// <summary>
        /// Provides an array representation of this sparse storage, with the values arranged row by row.
        /// </summary>
        /// <returns> The one-dimensional array, resulting from the concatenation of the rows. </returns>
        internal abstract T[] ToRowMajorArray();

        /// <summary>
        /// Provides an array representation of this sparse storage, with the values arranged column by column.
        /// </summary>
        /// <returns> The one-dimensional array, resulting from the concatenation of the columns. </returns>
        internal abstract T[] ToColumnMajorArray();

        /// <summary>
        /// Arranges each sparse storage row into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.SparseVector{T}"/>.</returns>
        internal abstract Vect.SparseVector<T>[] RowVectors();

        /// <summary>
        /// Arranges each sparse storage column into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.SparseVector{T}"/>.</returns>
        internal abstract Vect.SparseVector<T>[] ColumnVectors();

        #endregion


        #region Implement : IEnumerable<T>

        /// <inheritdoc/>
        public abstract IEnumerator<(int, int, T)> GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
