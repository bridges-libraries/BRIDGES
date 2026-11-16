using System;
using System.Numerics;
using System.Collections.Generic;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;
using SBuilders = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseBuilders;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages
{
    /// <summary>
    /// Represents a generic compressed column storage for a sparse matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix components. </typeparam>
    public class CompressedColumn<T> : SparseStorage<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Column pointers for the compressed column storage. It starts at zero and then stores for each column the number of values preceding that column.
        /// </summary>
        internal readonly int[] _columnPointers;

        /// <summary>
        /// Stores the row index of each non-zero entry.
        /// </summary>
        internal readonly int[] _rowIndices;

        /// <summary>
        /// Non-zero values.
        /// </summary>
        internal readonly T[] _values;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the column pointers.
        /// </summary>
        public IReadOnlyList<int> ColumnPointers => _columnPointers;

        /// <summary>
        /// Gets or sets the row indices.
        /// </summary>
        public IReadOnlyList<int> RowIndices => _rowIndices;

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public IReadOnlyList<T> Values => _values;


        /// <inheritdoc/>
        public override int Count => Values.Count;


        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The sparse storage does not contain an entry for the component at the given row and column. </exception>
        public override T this[int row, int column]
        { 
            get
            {
                for (int i = _columnPointers[column]; i < _columnPointers[column + 1]; i++)
                {
                    if (_rowIndices[i] < row) { continue; }
                    else if (_rowIndices[i] == row) { return _values[i]; }
                    else { break; }
                }

                throw new ArgumentException("The sparse storage does not contain an entry for the component at the given row and column.");
            }
        }

        
        /// <inheritdoc/>
        public override SparseStorageFormat StorageFormat => SparseStorageFormat.CompressedColumn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn{T}"/> class with its size and capacity.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="values"> Non-zero values in the matrix, arranged column by column.</param>
        /// <param name="rowIndices"> Row indices of each non-zero value. </param>
        /// <param name="columnPointers"> Column pointers for the matrix. It starts at zero and then stores for each column the number of values preceding that column. </param>
        public CompressedColumn(int rowCount, int columnCount, int[] columnPointers, int[] rowIndices, T[] values)
            : base(rowCount, columnCount)
        {
            //     -----     Verifications
            if (columnPointers.Length != columnCount + 1) { throw new ArgumentException($"The length of the {nameof(columnPointers)} is different from '{nameof(columnCount)} + 1'."); }


            // Initialises properties
            _columnPointers = columnPointers;
            _rowIndices = rowIndices;
            _values = values;
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn{T}"/> class by deep copying another <see cref="CompressedColumn{T}"/>.
        /// </summary>
        /// <param name="compressedColumn"> <see cref="CompressedColumn{T}"/> to deep copy. </param>
        internal CompressedColumn(CompressedColumn<T> compressedColumn)
            : base(compressedColumn.RowCount, compressedColumn.ColumnCount)
        {
            // Initialises properties
            _columnPointers = new int[compressedColumn._columnPointers.Length];
            Array.Copy(compressedColumn._columnPointers, _columnPointers, compressedColumn._columnPointers.Length);

            _rowIndices = new int[compressedColumn._rowIndices.Length];
            Array.Copy(compressedColumn._rowIndices, _rowIndices, compressedColumn._rowIndices.Length);

            _values = new T[compressedColumn._values.Length];
            Array.Copy(compressedColumn._values, _values, compressedColumn._values.Length);
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Builds a <see cref="CompressedColumn{T}"/> storage from the components in the <see cref="SBuilders.DictionaryOfKeys{T}"/> sparse matrix builder.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="dok"> Sparse matrix builder containing the unsorted components for the matrix.</param>
        /// <param name="validityChecks"> Evaluates whether the component indices and the component values should be verified.</param>
        /// <returns> The <see cref="CompressedColumn{T}"/> storage.</returns>
        public static CompressedColumn<T> Build(int rowCount, int columnCount, SBuilders.DictionaryOfKeys<T> dok, bool validityChecks)
        {
            List<(int RowIndex, T Value)>[] columns;

            // Sort by columns
            int nonZeroCount = validityChecks ? SortByColumn_Check(rowCount, columnCount, dok, out columns) :
                SortByColumn_NoCheck(columnCount, dok, out columns);

            // Sort by row within each column
            for (int i_Column = 0; i_Column < columns.Length; i_Column++)
            {
                columns[i_Column].Sort((x, y) => x.RowIndex.CompareTo(y.RowIndex));
            }


            int[] columnPointers = new int[columnCount + 1];
            int[] rowIndices = new int[nonZeroCount];
            T[] values = new T[nonZeroCount];

            columnPointers[0] = 0;

            int counter = 0;
            for (int i_Column = 0; i_Column < columnCount; i_Column++)
            {
                List<(int RowIndex, T Value)> rightColumn = columns[i_Column];
                for (int i_Right = 0; i_Right < rightColumn.Count; i_Right++)
                {
                    rowIndices[counter] = rightColumn[i_Right].RowIndex;
                    values[counter] = rightColumn[i_Right].Value;
                    counter++;
                }
                columnPointers[i_Column + 1] = counter;
            }

            return new CompressedColumn<T>(rowCount, columnCount, columnPointers, rowIndices, values);
        }

        #endregion

        #region Other Static Methods

        /// <summary>
        /// Sorts the values in the <see cref="SBuilders.DictionaryOfKeys{T}"/> into columns.
        /// </summary>
        /// <remarks> The row and column indices verified to be within the admissible range and the values and verified for the non-zero condition. </remarks>
        /// <remarks> The components are not sorted within each columns. </remarks>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="dok"> The unsorted components for the matrix.</param>
        /// <param name="columns"> Components sorted by columns. </param>
        /// <returns> The number of non-zero components in the resulting <paramref name="columns"/>. </returns>
        private static int SortByColumn_Check(int rowCount, int columnCount, SBuilders.DictionaryOfKeys<T> dok, out List<(int RowIndex, T Value)>[] columns)
        {
            columns = new List<(int RowIndex, T Value)>[columnCount];
            for (int i_Column = 0; i_Column < columns.Length; i_Column++) { columns[i_Column] = new List<(int, T)>(); }

            int nonZeroCount = 0;
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in dok.Storage)
            {
                if (0 <= rowCount | rowCount <= component.Key.RowIndex)
                {
                    throw new ArgumentOutOfRangeException("The row index of a component must be positive and smaller that the number of rows.");
                }
                else if (component.Value != T.Zero )
                {
                    columns[component.Key.ColumnIndex].Add((component.Key.RowIndex, component.Value));
                    nonZeroCount++;
                }
            }

            return nonZeroCount;
        }

        /// <summary>
        /// Sorts the values in the <see cref="SBuilders.DictionaryOfKeys{T}"/> into columns.
        /// </summary>
        /// <remarks> The components are not sorted within each columns. </remarks>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="dok"> The unsorted components for the matrix.</param>
        /// <param name="columns"> Components sorted by columns. </param>
        /// <returns> The number of non-zero components in the resulting <paramref name="columns"/>. </returns>
        private static int SortByColumn_NoCheck(int columnCount, SBuilders.DictionaryOfKeys<T> dok, out List<(int RowIndex, T Value)>[] columns)
        {
            columns = new List<(int RowIndex, T Value)>[columnCount];
            for (int i_Column = 0; i_Column < columns.Length; i_Column++) { columns[i_Column] = new List<(int, T)>(); }


            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in dok.Storage)
            {
                columns[component.Key.ColumnIndex].Add((component.Key.RowIndex, component.Value));
            }

            return dok.Count;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override bool TryGet(int row, int column, out T? value)
        {
            for (int i = ColumnPointers[column]; i < ColumnPointers[column + 1]; i++)
            {
                if (RowIndices[i] < row) { continue; }
                else if (RowIndices[i] == row) { value = Values[i]; return true; }
                else { break; }
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public override bool Contains(int row, int column)
        {
            for (int i = ColumnPointers[column]; i < ColumnPointers[column + 1]; i++)
            {
                if (RowIndices[i] < row) { continue; }
                else if (RowIndices[i] == row) { return true; }
                else { break; }
            }

            return false;
        }

        #endregion

        #region Internal Methods

        /// <inheritdoc/>
        internal override T[,] ToArray()
        {
            T[,] result = new T[RowCount, ColumnCount];
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i = ColumnPointers[i_Column]; i < ColumnPointers[i_Column + 1]; i++)
                {
                    int i_Row = RowIndices[i];

                    result[i_Row, i_Column] = Values[i];
                }
            }

            return result;
        }

        /// <inheritdoc/>
        internal override T[] ToRowMajorArray()
        {
            T[] result = new T[RowCount * ColumnCount];
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i = ColumnPointers[i_Column]; i < ColumnPointers[i_Column + 1]; i++)
                {
                    int i_Row = RowIndices[i];

                    result[(i_Row * ColumnCount) + i_Column] = Values[i];
                }
            }

            return result;
        }

        /// <inheritdoc/>
        internal override T[] ToColumnMajorArray()
        {
            T[] result = new T[RowCount * ColumnCount];
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i = ColumnPointers[i_Column]; i < ColumnPointers[i_Column + 1]; i++)
                {
                    int i_Row = RowIndices[i];

                    result[i_Row + (i_Column * RowCount)] = Values[i];
                }
            }

            return result;
        }

        /// <inheritdoc/>
        internal override Vect.SparseVector<T>[] RowVectors()
        {
            List<int>[] indices = new List<int>[RowCount];
            List<T>[] values = new List<T>[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                indices[i] = new List<int>();
                values[i] = new List<T>();
            }

            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i = ColumnPointers[i_Column]; i < ColumnPointers[i_Column + 1]; i++)
                {
                    int i_Row = RowIndices[i];

                    indices[i_Row].Add(i_Column);
                    values[i_Row].Add(Values[i]);
                }
            }

            Vect.SparseVector<T>[] vectors = new Vect.SparseVector<T>[RowCount];
            for (int i_Row = 0; i_Row < vectors.Length; i_Row++)
            {
                vectors[i_Row] = new Vect.SparseVector<T>(ColumnCount, indices[i_Row], values[i_Row]);
            }

            return vectors;
        }

        /// <inheritdoc/>
        internal override Vect.SparseVector<T>[] ColumnVectors()
        {
            Vect.SparseVector<T>[] vectors = new Vect.SparseVector<T>[ColumnCount];

            List<int> indices = new List<int>();
            List<T> values = new List<T>();
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                indices.Clear();
                values.Clear();

                for (int i = ColumnPointers[i_Column]; i < ColumnPointers[i_Column + 1]; i++)
                {
                    indices.Add(RowIndices[i]);
                    values.Add(Values[i]);
                }

                vectors[i_Column] = new Vect.SparseVector<T>(RowCount, indices, values);
            }

            return vectors;
        }

        #endregion


        #region Implement : IEnumerable<T>

        /// <inheritdoc/>
        public override IEnumerator<(int, int, T)> GetEnumerator()
        {
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i = ColumnPointers[i_Column]; i < ColumnPointers[i_Column + 1]; i++)
                {
                    yield return (RowIndices[i], i_Column, Values[i]);
                }
            }
        }

        #endregion
    }
}
