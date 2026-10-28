using System;
using System.Numerics;
using System.Collections.Generic;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;
using System.Collections.Immutable;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages
{
    /// <summary>
    /// Represents a generic compressed column storage for a sparse matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix components. </typeparam>
    public class CompressedColumn<T> : SparseStorage<T>
        where T : INumberBase<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public List<T> Values { get; init; }

        /// <summary>
        /// Gets or sets the row indices.
        /// </summary>
        public List<int> RowIndices { get; init; }

        /// <summary>
        /// Gets or sets the column pointers.
        /// </summary>
        public int[] ColumnPointers { get; init; }


        /// <inheritdoc/>
        public override int Count => Values.Count;


        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The sparse storage does not contain an entry for the component at the given row and column. </exception>
        public override T this[int row, int column]
        { 
            get
            {
                for (int i = ColumnPointers[column]; i < ColumnPointers[column + 1]; i++)
                {
                    if (RowIndices[i] < row) { continue; }
                    else if (RowIndices[i] == row) { return Values[i]; }
                    else { break; }
                }

                throw new ArgumentException("The sparse storage does not contain an entry for the component at the given row and column.");
            }
        }

        
        /// <inheritdoc/>
        public override SparseStorageType StorageType => SparseStorageType.CompressedColumn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn{T}"/> class with its size and capacity.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="capacity"> Number of values that the matrix can hold. </param>
        public CompressedColumn(int rowCount, int columnCount, int capacity)
            : base(rowCount, columnCount)
        {
            // Instantiate fields
            Values = new List<T>(capacity);
            RowIndices = new List<int>(capacity);
            ColumnPointers = new int[columnCount + 1];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn{T}"/> class with its size and capacity.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="values"> Non-zero values in the matrix, arranged column by column.</param>
        /// <param name="rowIndices"> Row indices of each non-zero value. </param>
        /// <param name="columnPointers"> Column pointers for the matrix. It starts at zero and then stores for each column the number of values preceding that column. </param>
        public CompressedColumn(int rowCount, int columnCount, ref List<T> values, ref List<int> rowIndices, ref int[] columnPointers) 
            : base(rowCount, columnCount)
        {
            //     -----     Verifications
            if (columnPointers.Length != columnCount + 1) { throw new ArgumentException($"The length of the {nameof(columnPointers)} is different from '{nameof(columnCount)} + 1'."); } 


            // Initialises properties
            Values = values;
            RowIndices = rowIndices;
            ColumnPointers = columnPointers;
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn{T}"/> class by deep copying another <see cref="CompressedColumn{T}"/>.
        /// </summary>
        /// <param name="compressedColumn"> <see cref="CompressedColumn{T}"/> to deep copy. </param>
        public CompressedColumn(CompressedColumn<T> compressedColumn)
            : base(compressedColumn.RowCount, compressedColumn.ColumnCount)
        {
            // Initialises properties
            Values = new List<T>(compressedColumn.Values);

            RowIndices = new List<int> (compressedColumn.RowIndices);

            ColumnPointers = new int[compressedColumn.ColumnPointers.Length];
            Array.Copy(compressedColumn.ColumnPointers, ColumnPointers, compressedColumn.ColumnPointers.Length);
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
        public override bool Add(int row, int column, T value)
        {
            int index = ColumnPointers[column];

            if (ColumnPointers[column] != ColumnPointers[column + 1])
            {
                while (index < ColumnPointers[column + 1] && RowIndices[index] < row) { index++; }
            }

            if (index < ColumnPointers[column + 1] && RowIndices[index] == row) { return false; }
            else
            {
                Values.Insert(index, value);
                RowIndices.Insert(index, row);
                for (int i_Column = column + 1; i_Column < ColumnPointers.Length; i_Column++)
                {
                    ColumnPointers[i_Column] += 1;
                }
                return true;
            }
        }

        /// <inheritdoc/>
        public override bool Add(int row, int column, T value, bool nonZeroCheck)
        {
            if (nonZeroCheck && value == T.Zero) { return false; }
            else { return Add(row, column, value); }
        }


        /// <inheritdoc/>
        public override bool Replace(int row, int column, T value)
        {
            for (int i = ColumnPointers[column]; i < ColumnPointers[column + 1]; i++)
            {
                if (RowIndices[i] < row) { continue; }
                else if (RowIndices[i] == row) { Values[i] = value; return true; }
                else { break; }
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool Replace(int row, int column, T value, bool nonZeroCheck)
        {
            if(nonZeroCheck && value == T.Zero) { return false; }

            return Replace(row, column, value);
        }


        /// <inheritdoc/>
        public override bool Remove(int row, int column)
        {
            for (int i = ColumnPointers[column]; i < ColumnPointers[column + 1]; i++)
            {
                if (RowIndices[i] < row) { continue; }
                else if (RowIndices[i] == row) 
                {
                    Values.RemoveAt(i);
                    RowIndices.RemoveAt(i);
                    for (int i_Column = column + 1; i_Column < ColumnPointers.Length; i_Column++)
                    {
                        ColumnPointers[i_Column] -= 1;
                    }
                    return true;
                }
                else { break; }
            }

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



        /// <inheritdoc/>
        public override CompressedColumn<T> ToCompressedColumn()
        {
            return new CompressedColumn<T>(this);
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

        #region Private Methods

        //     -----     AdditionOperator(SparseStorage<T> right)     -----     //

        private CompressedColumn<T> AdditionOperator(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount || ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The sparse matrix storages must have the same number of rows and colunms as this sparse matrix storage.");
            }


            int maxCapacity = Values.Count + right.Values.Count;

            List<T> resultValues = new List<T>(maxCapacity);
            List<int> resultRowIndices = new List<int>(maxCapacity);
            int[] resultColumnPointers = new int[ColumnCount + 1];

            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                int i_Left = ColumnPointers[i_Column];
                int i_Right = right.ColumnPointers[i_Column];

                while (i_Left < ColumnPointers[i_Column + 1] && i_Right < right.ColumnPointers[i_Column + 1])
                {
                    int leftRow = RowIndices[i_Left];
                    int rightRow = right.RowIndices[i_Right];

                    if (leftRow == rightRow)
                    {
                        T sum = Values[i_Left] + right.Values[i_Right];
                        if (sum != T.Zero)
                        {
                            resultValues.Add(sum);
                            resultRowIndices.Add(leftRow);
                        }
                        i_Left++;
                        i_Right++;
                    }
                    else if (leftRow < rightRow)
                    {
                        resultValues.Add(Values[i_Left]);
                        resultRowIndices.Add(leftRow);
                        i_Left++;
                    }
                    else
                    {
                        resultValues.Add(right.Values[i_Right]);
                        resultRowIndices.Add(rightRow);
                        i_Right++;
                    }
                }

                while (i_Left < ColumnPointers[i_Column + 1])
                {
                    resultValues.Add(Values[i_Left]);
                    resultRowIndices.Add(RowIndices[i_Left]);
                    i_Left++;
                }

                while (i_Right < right.ColumnPointers[i_Column + 1])
                {
                    resultValues.Add(right.Values[i_Right]);
                    resultRowIndices.Add(right.RowIndices[i_Right]);
                    i_Right++;
                }

                resultColumnPointers[i_Column + 1] = resultValues.Count;
            }

            return new CompressedColumn<T>(RowCount, ColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }

        private CompressedColumn<T> AdditionOperator(DictionaryOfKeys<T> right)
        {
            CompressedColumn<T> rightCC = right.ToCompressedColumn();

            return AdditionOperator(rightCC);
        }


        //     -----     SubtractionOperator(SparseStorage<T> right)     -----     //

        private CompressedColumn<T> SubtractionOperator(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount || ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The sparse matrix storages must have the same number of rows and colunms as this sparse matrix storage.");
            }


            int maxCapacity = Values.Count + right.Values.Count;

            List<T> resultValues = new List<T>(maxCapacity);
            List<int> resultRowIndices = new List<int>(maxCapacity);
            int[] resultColumnPointers = new int[ColumnCount + 1];

            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                int i_Left = ColumnPointers[i_Column];
                int i_Right = right.ColumnPointers[i_Column];

                while (i_Left < ColumnPointers[i_Column + 1] && i_Right < right.ColumnPointers[i_Column + 1])
                {
                    int leftRow = RowIndices[i_Left];
                    int rightRow = right.RowIndices[i_Right];

                    if (leftRow == rightRow)
                    {
                        T sum = Values[i_Left] - right.Values[i_Right];
                        if (sum != T.Zero)
                        {
                            resultValues.Add(sum);
                            resultRowIndices.Add(leftRow);
                        }
                        i_Left++;
                        i_Right++;
                    }
                    else if (leftRow < rightRow)
                    {
                        resultValues.Add(Values[i_Left]);
                        resultRowIndices.Add(leftRow);
                        i_Left++;
                    }
                    else
                    {
                        resultValues.Add(-right.Values[i_Right]);
                        resultRowIndices.Add(rightRow);
                        i_Right++;
                    }
                }

                while (i_Left < ColumnPointers[i_Column + 1])
                {
                    resultValues.Add(Values[i_Left]);
                    resultRowIndices.Add(RowIndices[i_Left]);
                    i_Left++;
                }

                while (i_Right < right.ColumnPointers[i_Column + 1])
                {
                    resultValues.Add(right.Values[i_Right]);
                    resultRowIndices.Add(right.RowIndices[i_Right]);
                    i_Right++;
                }

                resultColumnPointers[i_Column + 1] = resultValues.Count;
            }

            return new CompressedColumn<T>(RowCount, ColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }

        private CompressedColumn<T> SubtractionOperator(DictionaryOfKeys<T> right)
        {
            CompressedColumn<T> rightCC = right.ToCompressedColumn();

            return SubtractionOperator(rightCC);
        }


        //     -----     MultiplyOperator(SparseStorage<T> right)     -----     //

        private CompressedColumn<T> MultiplyOperator(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the sparse matrix storage must be equal to the number of columns of this sparse matrix storage.");
            }


            int resultRowCount = RowCount;
            int resultColumnCount = right.ColumnCount;
            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];

            
            T[] tmp_ResultColumn = new T[resultRowCount]; 
            bool[] tmp_IsUsed = new bool[resultRowCount];

            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++)
            {
                Array.Clear(tmp_ResultColumn, 0, resultRowCount);
                Array.Clear(tmp_IsUsed, 0, resultRowCount);

                for (int i_Right = right.ColumnPointers[i_ResultColumn]; i_Right < right.ColumnPointers[i_ResultColumn + 1]; i_Right++)
                {
                    int rightRow = right.RowIndices[i_Right];
                    T rightComponent = right.Values[i_Right];

                    for (int i_Left = ColumnPointers[rightRow]; i_Left < ColumnPointers[rightRow + 1]; i_Left++)
                    {
                        int leftRow = RowIndices[i_Left];
                        T leftComponent = Values[i_Left];

                        if (!tmp_IsUsed[leftRow])
                        {
                            tmp_ResultColumn[leftRow] = leftComponent * rightComponent;
                            tmp_IsUsed[leftRow] = true;
                        }
                        else
                        {
                            tmp_ResultColumn[leftRow] += leftComponent * rightComponent;
                        }
                    }
                }

                for (int i = 0; i < resultRowCount; i++)
                {
                    if (tmp_IsUsed[i] && tmp_ResultColumn[i] != T.Zero)
                    {
                        resultValues.Add(tmp_ResultColumn[i]);
                        resultRowIndices.Add(i);
                    }
                }

                resultColumnPointers[i_ResultColumn + 1] = resultValues.Count;
            }

            return new CompressedColumn<T>(resultRowCount, resultColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }

        private CompressedColumn<T> MultiplyOperator(DictionaryOfKeys<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the sparse matrix storage must be equal to the number of columns of this sparse matrix storage.");
            }


            int resultRowCount = RowCount;
            int resultColumnCount = right.ColumnCount;

            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];

            resultColumnPointers[0] = 0;
            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++)
            {
                T[] resultColumnValues = new T[resultRowCount];

                for (int i_LeftColumn = 0; i_LeftColumn < ColumnCount; i_LeftColumn++)
                {
                    for (int i_Left = ColumnPointers[i_LeftColumn]; i_Left < ColumnPointers[i_LeftColumn + 1]; i_Left++)
                    {
                        if (right.Storage.TryGetValue((i_LeftColumn, i_ResultColumn), out T? rightValue))
                        {
                            resultColumnValues[RowIndices[i_Left]] += Values[i_Left] * rightValue;
                        }
                    }
                }

                for (int i = 0; i < resultColumnValues.Length; i++)
                {
                    if (resultColumnValues[i] != T.Zero)
                    {
                        resultRowIndices.Add(i);
                        resultValues.Add(resultColumnValues[i]);
                    }
                }
                resultColumnPointers[i_ResultColumn + 1] = resultValues.Count;
            }

            return new CompressedColumn<T>(resultRowCount, resultColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }


        //     -----     TransposeMultiply(SparseStorage<T> right)     -----     //

        private CompressedColumn<T> TransposeMultiply(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the sparse matrix storage must be equal to the number of rows of this sparse matrix storage");
            }


            int resultRowCount = ColumnCount;
            int resultColumnCount = right.ColumnCount;

            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];

            resultColumnPointers[0] = 0;

            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < resultRowCount; i_ResultRow++)
                {
                    int i_Left = ColumnPointers[i_ResultRow];
                    int i_Right = right.ColumnPointers[i_ResultColumn];

                    T resultComponent = T.Zero;
                    while (i_Left < ColumnPointers[i_ResultRow + 1] && i_Right < right.ColumnPointers[i_ResultColumn + 1])
                    {
                        int leftRow = RowIndices[i_Left];
                        int rightRow = right.RowIndices[i_Right];

                        if (leftRow == rightRow)
                        {
                            resultComponent += Values[i_Left] * right.Values[i_Right];
                            i_Left++;
                            i_Right++;
                        }
                        else if (leftRow < rightRow)
                        {
                            i_Left++;
                        }
                        else
                        {
                            i_Right++;
                        }
                    }

                    if (resultComponent != T.Zero)
                    {
                        resultValues.Add(resultComponent);
                        resultRowIndices.Add(i_ResultRow);
                    }
                }

                resultColumnPointers[i_ResultColumn + 1] = resultValues.Count;
            }

            return new CompressedColumn<T>(resultRowCount, resultColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }

        private CompressedColumn<T> TransposeMultiply(DictionaryOfKeys<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the sparse matrix storage must be equal to the number of rows of this sparse matrix storage");
            }


            int resultRowCount = ColumnCount;
            int resultColumnCount = right.ColumnCount;

            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];

            resultColumnPointers[0] = 0;
            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < resultRowCount; i_ResultRow++)
                {
                    T resultValue = T.Zero;
                    for (int i_Left = ColumnPointers[i_ResultRow]; i_Left < ColumnPointers[i_ResultRow + 1]; i_Left++)
                    {
                        if (right.Storage.TryGetValue((RowIndices[i_Left], i_ResultColumn), out T? rightValue))
                        {
                            resultValue += Values[i_Left] * rightValue;
                        }
                    }

                    if(resultValue != T.Zero) 
                    {
                        resultRowIndices.Add(i_ResultRow);
                        resultValues.Add(resultValue);
                    }
                }

                resultColumnPointers[i_ResultColumn + 1] = resultValues.Count;
            }

            return new CompressedColumn<T>(resultRowCount, resultColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }


        //     -----     MultiplyTranspose(SparseStorage<T> right)     -----     //

        private CompressedColumn<T> MultiplyTranspose(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException($"The number of columns of the sparse matrix storage must be equal to the number of columns of this sparse matrix storage");
            }

            
            int resultRowCount = RowCount;
            int resultColumnCount = right.RowCount;

            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];


            bool[] isValid = new bool[right.ColumnCount];
            int[] i_Right_ByColumn = new int[right.ColumnCount];
            for (int i_RightColumn = 0; i_RightColumn < right.ColumnCount; i_RightColumn++)
            {
                isValid[i_RightColumn] = true;
                i_Right_ByColumn[i_RightColumn] = right.ColumnPointers[i_RightColumn];
            }


            T[] tmp_ResultColumn = new T[resultRowCount];
            bool[] tmp_IsUsed = new bool[resultRowCount];


            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++) 
            {
                Array.Clear(tmp_ResultColumn, 0, resultRowCount);
                Array.Clear(tmp_IsUsed, 0, resultRowCount);

                for(int i = 0; i < right.ColumnCount; i++)
                {
                    if (isValid[i] && i_ResultColumn != right.RowIndices[i_Right_ByColumn[i]]) { continue; }

                    T rightComponent = right.Values[i_Right_ByColumn[i]];

                    for (int i_Left = ColumnPointers[i]; i_Left < ColumnPointers[i + 1]; i_Left++)
                    {
                        int leftRow = RowIndices[i_Left];
                        T leftComponent = Values[i_Left];

                        if (!tmp_IsUsed[leftRow])
                        {
                            tmp_ResultColumn[leftRow] = leftComponent * rightComponent;
                            tmp_IsUsed[leftRow] = true;
                        }
                        else
                        {
                            tmp_ResultColumn[leftRow] += leftComponent * rightComponent;
                        }
                    }
                    i_Right_ByColumn[i]++;
                    if (i_Right_ByColumn[i] == ColumnPointers[i + 1]) { isValid[i] = false; }
                }


                for (int i = 0; i < resultRowCount; i++)
                {
                    if (tmp_IsUsed[i] && tmp_ResultColumn[i] != T.Zero)
                    {
                        resultValues.Add(tmp_ResultColumn[i]);
                        resultRowIndices.Add(i);
                    }
                }

                resultColumnPointers[i_ResultColumn + 1] = resultValues.Count;
            }

            return new CompressedColumn<T>(resultRowCount, resultColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }

        private CompressedColumn<T> MultiplyTranspose(DictionaryOfKeys<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException($"The number of columns of the sparse matrix storage must be equal to the number of columns of this sparse matrix storage");
            }


            int resultRowCount = RowCount;
            int resultColumnCount = right.RowCount;

            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];

            resultColumnPointers[0] = 0;
            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++)
            {
                T[] resultColumnValues = new T[resultRowCount];

                for (int i_LeftColumn = 0; i_LeftColumn < ColumnCount; i_LeftColumn++)
                {
                    for (int i_Left = ColumnPointers[i_LeftColumn]; i_Left < ColumnPointers[i_LeftColumn + 1]; i_Left++)
                    {
                        if (right.Storage.TryGetValue((i_ResultColumn, i_LeftColumn), out T? rightValue))
                        {
                            resultColumnValues[RowIndices[i_Left]] += Values[i_Left] * rightValue;
                        }
                    }
                }

                for (int i = 0; i < resultColumnValues.Length; i++)
                {
                    if (resultColumnValues[i] != T.Zero)
                    {
                        resultRowIndices.Add(i);
                        resultValues.Add(resultColumnValues[i]);
                    }
                }
                resultColumnPointers[i_ResultColumn + 1] = resultValues.Count;
            }

            return new CompressedColumn<T>(resultRowCount, resultColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }

        #endregion


        #region Protected Inheritence : SparseStorage<T>

        //     -----     -----     Algebraic Near Ring : ISparseStorage<T>     -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The addition of this sparse storage with <paramref name="right"/> as a <see cref="SparseStorage{T}"/> is not implemented. 
        /// </exception>
        protected override SparseStorage<T> AdditionOperator(SparseStorage<T> right)
        {
            if (right is CompressedColumn<T> rightCC) { return AdditionOperator(rightCC); }
            else if (right is DictionaryOfKeys<T> rightDOK) { return AdditionOperator(rightDOK); }
            else
            {
                throw new NotImplementedException($"The addition of this sparse matrix storage with a {right.GetType()} as a {typeof(SparseStorage<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of this sparse storage with <paramref name="right"/> as a <see cref="SparseStorage{T}"/> is not implemented. 
        /// </exception>
        protected override SparseStorage<T> SubtractionOperator(SparseStorage<T> right)
        {
            if (right is CompressedColumn<T> rightCC) { return SubtractionOperator(rightCC); }
            else if (right is DictionaryOfKeys<T> rightDOK) { return SubtractionOperator(rightDOK); }
            else
            {
                throw new NotImplementedException($"The subtraction of this sparse matrix storage with a {right.GetType()} as a {typeof(SparseStorage<T>)} is not implemented.");
            }
        }


        /// <inheritdoc/>
        protected override SparseStorage<T> UnaryNegationOperator()
        {
            List<T> resultValues = new List<T>(Values.Count);
            for (int i = 0; i < Values.Count; i++) { resultValues.Add(-Values[i]); }

            List<int> resultRowIndices = new List<int>(RowIndices);

            int[] resultColumnPointers = new int[ColumnPointers.Length];
            Array.Copy(ColumnPointers, resultColumnPointers, ColumnPointers.Length);

            return new CompressedColumn<T>(RowCount, ColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }


        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of this sparse storage with <paramref name="right"/> as a <see cref="SparseStorage{T}"/> is not implemented. 
        /// </exception>
        protected override SparseStorage<T> MultiplyOperator(SparseStorage<T> right)
        {
            if (right is CompressedColumn<T> rightCC) { return MultiplyOperator(rightCC); }
            else if (right is DictionaryOfKeys<T> rightDOK) { return MultiplyOperator(rightDOK); }
            else
            {
                throw new NotImplementedException($"The multiplication of this sparse matrix storage with a {right.GetType()} as a {typeof(SparseStorage<T>)} is not implemented.");
            }
        }


        //     -----     Other Operations : ISparseStorage<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of this transposed sparse storage with <paramref name="right"/> as a <see cref="SparseStorage{T}"/> is not implemented. 
        /// </exception>
        protected override SparseStorage<T> TransposeMultiply(SparseStorage<T> right)
        {
            if (right is CompressedColumn<T> rightCC) { return TransposeMultiply(rightCC); }
            else if (right is DictionaryOfKeys<T> rightDOK) { return TransposeMultiply(rightDOK); }
            else
            {
                throw new NotImplementedException($"The multiplication of this transposed sparse matrix storage with a {right.GetType()} as a {typeof(SparseStorage<T>)} is not implemented.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of this sparse storage with transposed <paramref name="right"/> as a <see cref="SparseStorage{T}"/> is not implemented. 
        /// </exception>
        protected override SparseStorage<T> MultiplyTranspose(SparseStorage<T> right)
        {
            if (right is CompressedColumn<T> rightCC) { return MultiplyTranspose(rightCC); }
            else if (right is DictionaryOfKeys<T> rightDOK) { return MultiplyTranspose(rightDOK); }
            else
            {
                throw new NotImplementedException($"The multiplication of this sparse matrix storage with a transposed {right.GetType()} as a {typeof(SparseStorage<T>)} is not implemented.");
            }
        }


        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The dense matrix must have the same number of rows and colunms as this sparse storage. </exception>
        protected override DenseMatrix<T> RightAdditionOperator(DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount || ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The dense matrix must have the same number of rows and colunms as this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(right);
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i = ColumnPointers[i_Column]; i < ColumnPointers[i_Column + 1]; i++)
                {
                    result[RowIndices[i], i_Column] = Values[i] + right[RowIndices[i], i_Column];
                }
            }

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The dense matrix must have the same number of rows and colunms as this sparse storage. </exception>
        protected override DenseMatrix<T> RightSubtractionOperator(DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount || ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The dense matrix must have the same number of rows and colunms as this sparse matrix storage.");
            }

            DenseMatrix<T> result = -right;
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i = ColumnPointers[i_Column]; i < ColumnPointers[i_Column + 1]; i++)
                {
                    result[RowIndices[i], i_Column] = Values[i] - right[RowIndices[i], i_Column];
                }
            }

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of rows of the dense matrix must be equal to the number of columns of this sparse storage. </exception>
        protected override DenseMatrix<T> RightMultiplyOperator(DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the dense matrix must be equal to the number of columns of this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(RowCount, right.ColumnCount);
            for (int i_LeftColumn = 0; i_LeftColumn < ColumnCount; i_LeftColumn++)
            {
                for (int i_Left = ColumnPointers[i_LeftColumn]; i_Left < ColumnPointers[i_LeftColumn + 1]; i_Left++)
                {
                    T leftComponent = Values[i_Left];
                    int i_ResultRow = RowIndices[i_Left];

                    for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                    {
                        result[i_ResultRow, i_ResultColumn] += leftComponent * right[i_LeftColumn, i_ResultColumn];
                    }
                }
            }

            return result;
        }


        //     -----     Other Right Operations : DenseMatrix<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of rows of the dense matrix must be equal to the number of rows of this sparse storage. </exception>
        protected override DenseMatrix<T> RightTransposeMultiply(DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the dense matrix must be equal to the number of rows of this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(ColumnCount, right.ColumnCount);
            for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
                {
                    T component = T.Zero;
                    for (int i_Left = ColumnPointers[i_ResultRow]; i_Left < ColumnPointers[i_ResultRow + 1]; i_Left++)
                    {
                        component += Values[i_Left] * right[RowIndices[i_Left], i_ResultColumn];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of rows of the dense matrix must be equal to the number of rows of this sparse storage. </exception>
        protected override DenseMatrix<T> RightMultiplyTranspose(DenseMatrix<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException($"The number of columns of the dense matrix must be equal to the number of columns of this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(RowCount, right.RowCount);
            for (int i_LeftColumn = 0; i_LeftColumn < ColumnCount; i_LeftColumn++)
            {
                for (int i_Left = ColumnPointers[i_LeftColumn]; i_Left < ColumnPointers[i_LeftColumn + 1]; i_Left++)
                {
                    T i_LeftComponent = Values[i_Left];
                    int i_ResultRow = RowIndices[i_Left];

                    for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                    {
                        result[i_ResultRow, i_ResultColumn] += i_LeftComponent * right[i_ResultColumn, i_LeftColumn];
                    }
                }
            }

            return result;
        }


        //     -----     -----     Left Embedding : DenseMatrix<T>     -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The dense matrix must have the same number of rows and colunms as this sparse storage. </exception>
        protected override DenseMatrix<T> LeftAdditionOperator(DenseMatrix<T> left)
        {
            //     -----     Verifications

            if (RowCount != left.RowCount || ColumnCount != left.ColumnCount)
            {
                throw new ArgumentException("The dense matrix must have the same number of rows and colunms as this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(left);
            for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
            {
                for (int i_Right = ColumnPointers[i_ResultColumn]; i_Right < ColumnPointers[i_ResultColumn + 1]; i_Right++)
                {
                    result[RowIndices[i_Right], i_ResultColumn] = left[RowIndices[i_Right], i_ResultColumn] + Values[i_Right];
                }
            }

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The dense matrix must have the same number of rows and colunms as this sparse storage. </exception>
        protected override DenseMatrix<T> LeftSubtractionOperator(DenseMatrix<T> left)
        {
            //     -----     Verifications

            if (RowCount != left.RowCount || ColumnCount != left.ColumnCount)
            {
                throw new ArgumentException("The dense matrix must have the same number of rows and colunms as this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(left);
            for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
            {
                for (int i_Right = ColumnPointers[i_ResultColumn]; i_Right < ColumnPointers[i_ResultColumn + 1]; i_Right++)
                {
                    result[RowIndices[i_Right], i_ResultColumn] = left[RowIndices[i_Right], i_ResultColumn] - Values[i_Right];
                }
            }

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of columns of the dense matrix must be equal to the number of rows of this sparse storage. </exception>
        protected override DenseMatrix<T> LeftMultiplyOperator(DenseMatrix<T> left)
        {
            //     -----     Verifications

            if (left.ColumnCount != RowCount)
            {
                throw new ArgumentException($"The number of columns of the dense matrix must be equal to the number of rows of this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, ColumnCount);
            for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
                {
                    T component = T.Zero;
                    for (int i_Right = ColumnPointers[i_ResultColumn]; i_Right < ColumnPointers[i_ResultColumn + 1]; i_Right++)
                    {
                        component += left[i_ResultRow, RowIndices[i_Right]] * Values[i_Right];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }


        //     -----     Other Left Operations : DenseMatrix<T>     -----     //

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of rows of the dense matrix must be equal to the number of rows of this sparse storage. </exception>
        protected override DenseMatrix<T> LeftTransposeMultiply(DenseMatrix<T> left)
        {
            //     -----     Verifications

            if (left.RowCount != RowCount)
            {
                throw new ArgumentException($"The number of rows of the dense matrix must be equal to the number of rows of this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(left.ColumnCount, ColumnCount);
            for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
                {
                    T component = T.Zero;
                    for (int i_Right = ColumnPointers[i_ResultColumn]; i_Right < ColumnPointers[i_ResultColumn + 1]; i_Right++)
                    {
                        component += left[RowIndices[i_Right], i_ResultRow] * Values[i_Right];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of columns of the dense matrix must be equal to the number of columns of this sparse storage. </exception>
        protected override DenseMatrix<T> LeftMultiplyTranspose(DenseMatrix<T> left)
        {
            //     -----     Verifications

            if (left.ColumnCount != ColumnCount)
            {
                throw new ArgumentException($"The number of columns of the dense matrix must be equal to the number of columns of this sparse matrix storage.");
            }


            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, RowCount);
            for (int i_RightColumn = 0; i_RightColumn < ColumnCount; i_RightColumn++)
            {
                for (int i_Right = ColumnPointers[i_RightColumn]; i_Right < ColumnPointers[i_RightColumn + 1]; i_Right++)
                {
                    T rightComponent = Values[i_Right];
                    int i_ResultColumn = RowIndices[i_Right];
                    for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
                    {
                        result[i_ResultRow, i_ResultColumn] += left[i_ResultRow, i_RightColumn] * rightComponent;
                    }
                }
            }

            return result;
        }


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc/>
        protected override SparseStorage<T> RightMultiplyOperator(T factor)
        {
            List<T> resultValues = new List<T>(Values.Count);
            for (int i = 0; i < Values.Count; i++) { resultValues.Add(Values[i] * factor); }

            List<int> resultRowIndices = new List<int>(RowIndices);

            int[] resultColumnPointers = new int[ColumnPointers.Length];
            Array.Copy(ColumnPointers, resultColumnPointers, ColumnPointers.Length);

            return new CompressedColumn<T>(RowCount, ColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }

        /// <inheritdoc/>
        protected override SparseStorage<T> LeftMultiplyOperator(T factor)
        {
            List<T> resultValues = new List<T>(Values.Count);
            for (int i = 0; i < Values.Count; i++) { resultValues.Add(factor * Values[i]); }

            List<int> resultRowIndices = new List<int>(RowIndices);

            int[] resultColumnPointers = new int[ColumnPointers.Length];
            Array.Copy(ColumnPointers, resultColumnPointers, ColumnPointers.Length);

            return new CompressedColumn<T>(RowCount, ColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }

        /// <inheritdoc/>
        protected override SparseStorage<T> DivisionOperator(T divisor)
        {
            List<T> resultValues = new List<T>(Values.Count);
            for (int i = 0; i < Values.Count; i++) { resultValues.Add(Values[i] / divisor); }

            List<int> resultRowIndices = new List<int>(RowIndices);

            int[] resultColumnPointers = new int[ColumnPointers.Length];
            Array.Copy(ColumnPointers, resultColumnPointers, ColumnPointers.Length);

            return new CompressedColumn<T>(RowCount, ColumnCount, ref resultValues, ref resultRowIndices, ref resultColumnPointers);
        }


        //     -----      -----     Vectors     -----     -----     //

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of columns of this sparse storage and the size of <paramref name="vector"/> must be equal. </exception>
        protected override Vect.SparseVector<T> MultiplyOperator(Vect.SparseVector<T> vector)
        {
            //     -----     Verifications

            if (this.ColumnCount != vector.Size)
            {
                throw new ArgumentException($"The number of columns of this sparse storage and the size of {nameof(vector)} must be equal.");
            }

            T[] array = new T[this.RowCount];
            for (int i_StorageColumn = 0; i_StorageColumn < this.ColumnCount; i_StorageColumn++)
            {
                if (!vector.Contains(i_StorageColumn)) { continue; }

                T vectorValue = vector[i_StorageColumn];
                for (int i = ColumnPointers[i_StorageColumn]; i < ColumnPointers[i_StorageColumn + 1]; i++)
                {
                    array[RowIndices[i]] += Values[i] * vectorValue;
                }
            }


            Vect.SparseVector<T> result = new Vect.SparseVector<T>(this.RowCount);
            for (int i = 0; i < result.Size; i++)
            {
                if (array[i] != T.Zero) { result.Add(i, array[i]); }
            }

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of columns of this sparse storage and the size of <paramref name="vector"/> must be equal. </exception>
        protected override Vect.DenseVector<T> MultiplyOperator(Vect.DenseVector<T> vector)
        {
            //     -----     Verifications

            if (this.ColumnCount != vector.Size)
            {
                throw new ArgumentException($"The number of columns of this sparse storage and the size of {nameof(vector)} must be equal.");
            }

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(this.RowCount);
            for (int i_StorageColumn = 0 ; i_StorageColumn < this.ColumnCount; i_StorageColumn++)
            {
                T vectorValue = vector[i_StorageColumn];
                for (int i = ColumnPointers[i_StorageColumn]; i < ColumnPointers[i_StorageColumn + 1]; i++)
                {
                    result[RowIndices[i]] += Values[i] * vectorValue;
                }
            }

            return result;
        }



        //     -----      Other Operations : Vectors     -----     //

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of rows of this sparse storage and the size of <paramref name="vector"/> must be equal. </exception>
        protected override Vect.SparseVector<T> TransposeMultiply(Vect.SparseVector<T> vector)
        {
            //     -----     Verifications

            if (this.RowCount != vector.Size)
            {
                throw new ArgumentException($"The number of rows of this sparse storage and the size of {nameof(vector)} must be equal.");
            }

            Vect.SparseVector<T> result = new Vect.SparseVector<T>(this.ColumnCount);
            for (int i_StorageColumn = 0; i_StorageColumn < this.ColumnCount; i_StorageColumn++)
            {
                T component = default!;
                for (int i = ColumnPointers[i_StorageColumn]; i < ColumnPointers[i_StorageColumn + 1]; i++)
                {
                    if (vector.TryGet(RowIndices[i], out T? value))
                    {
                        component += Values[i] * value!;
                    }
                }

                if (component != T.Zero) { result.Add(i_StorageColumn, component); }
            }

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The number of rows of this sparse storage and the size of <paramref name="vector"/> must be equal. </exception>
        protected override Vect.DenseVector<T> TransposeMultiply(Vect.DenseVector<T> vector)
        {
            //     -----     Verifications

            if (this.RowCount != vector.Size)
            {
                throw new ArgumentException($"The number of rows of this sparse storage and the size of {nameof(vector)} must be equal.");
            }

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(this.ColumnCount);
            for (int i_StorageColumn = 0; i_StorageColumn < this.ColumnCount; i_StorageColumn++)
            {
                T component = default!;
                for (int i = ColumnPointers[i_StorageColumn]; i < ColumnPointers[i_StorageColumn + 1]; i++)
                {
                    component += Values[i] * vector[RowIndices[i]];
                }
                result[i_StorageColumn] = component;
            }

            return result;
        }


        #endregion
    }
}
