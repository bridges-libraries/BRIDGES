using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Numerics;
using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages
{
    /// <summary>
    /// Represents a generic dictionary of keys storage for a sparse matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix components. </typeparam>
    public class DictionaryOfKeys<T> : SparseStorage<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Dictionary containing the values.
        /// <list type="table">
        ///     <listheader>
        ///         <term> Key </term>
        ///         <description> (RowIndex, ColumnIndex) pair of a component. </description>
        ///     </listheader>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Value of the component at the corresponding row and column  </description>
        ///     </item>
        /// </list>
        /// </summary>
        private Dictionary<(int RowIndex, int ColumnIndex), T> _storage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dictionary storing the component values.
        /// <list type="table">
        ///     <listheader>
        ///         <term> Key </term>
        ///         <description> (RowIndex, ColumnIndex) pair of a component. </description>
        ///     </listheader>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Value of the component at the corresponding row and column. </description>
        ///     </item>
        /// </list>
        /// </summary>
        public IReadOnlyDictionary<(int RowIndex, int ColumnIndex), T> Storage => _storage;


        /// <inheritdoc/>
        public override int Count => _storage.Count;


        /// <inheritdoc/>
        /// <exception cref="ArgumentException"> The sparse storage does not contain an entry for the component at the given row and column. </exception>
        public override T this[int row, int column]
        {
            get
            {
                return _storage.TryGetValue((row,column), out T? value) ? value! :
                    throw new ArgumentException("The sparse storage does not contain an entry for the component at the given row and column.");
            }
        }


        /// <inheritdoc/>
        public override SparseStorageType StorageType => SparseStorageType.DictionaryOfKeys;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryOfKeys{T}"/> class with its size and capacity.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="capacity"> Number of values that the matrix can hold. </param>
        public DictionaryOfKeys(int rowCount, int columnCount, int capacity)
            : base(rowCount, columnCount)
        {
            _storage = new Dictionary<(int, int), T>(capacity);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryOfKeys{T}"/> class by deep copying another <see cref="DictionaryOfKeys{T}"/>.
        /// </summary>
        /// <param name="dok"> <see cref="DictionaryOfKeys{T}"/> to deep copy. </param>
        public DictionaryOfKeys(DictionaryOfKeys<T> dok)
            : base(dok.RowCount, dok.ColumnCount)
        {
            _storage = new Dictionary<(int RowIndex, int ColumnIndex), T>(dok._storage);
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override bool TryGet(int row, int column, out T? value)
        {
            return _storage.TryGetValue((row, column), out value);
        }


        /// <inheritdoc/>
        public override bool Add(int row, int column, T value)
        {
            return _storage.TryAdd((row, column), value);
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
            if (_storage.ContainsKey((row, column)))
            {
                _storage[(row, column)] = value;
                return true;
            }
            else { return false; }
        }

        /// <inheritdoc/>
        public override bool Replace(int row, int column, T value, bool nonZeroCheck)
        {
            if (nonZeroCheck && value == T.Zero) { return false; }

            return Replace(row, column, value);
        }


        /// <inheritdoc/>
        public override bool Remove(int row, int column)
        {
            return _storage.Remove((row, column));
        }


        /// <inheritdoc/>
        public override bool Contains(int row, int column)
        {
            return _storage.ContainsKey((row, column));
        }



        /// <inheritdoc/>
        public override CompressedColumn<T> ToCompressedColumn()
        {
            List<(int RowIndex, T Value)>[] rightColumns = new List<(int RowIndex, T Value)>[ColumnCount];
            for (int i_RightColumn = 0; i_RightColumn < rightColumns.Length; i_RightColumn++) { rightColumns[i_RightColumn] = new List<(int, T)>(); }

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> rightComponent in _storage)
            {
                rightColumns[rightComponent.Key.ColumnIndex].Add((rightComponent.Key.RowIndex, rightComponent.Value));
            }

            for (int i_RightColumn = 0; i_RightColumn < rightColumns.Length; i_RightColumn++)
            {
                rightColumns[i_RightColumn].Sort((x, y) => x.RowIndex.CompareTo(y.RowIndex));
            }


            List<T> values = new List<T>(Count);
            List<int> rowIndices = new List<int>(Count);
            int[] columnPointers = new int[ColumnCount + 1];

            columnPointers[0] = 0;
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                List<(int RowIndex, T Value)> rightColumn = rightColumns[i_Column];
                for (int i_Right = 0; i_Right < rightColumn.Count; i_Right++)
                {
                    rowIndices.Add(rightColumn[i_Right].RowIndex);
                    values.Add(rightColumn[i_Right].Value);
                }
                columnPointers[i_Column + 1] = values.Count;
            }


            return new CompressedColumn<T>(RowCount, ColumnCount, ref values, ref rowIndices, ref columnPointers);
        }

        #endregion

        #region Internal Methods

        /// <inheritdoc/>
        internal override T[,] ToArray()
        {
            T[,] result = new T[RowCount, ColumnCount];
            foreach(KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[component.Key.RowIndex, component.Key.ColumnIndex] = component.Value;
            }

            return result;
        }

        /// <inheritdoc/>
        internal override T[] ToRowMajorArray()
        {
            T[] result = new T[RowCount * ColumnCount];
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[(component.Key.RowIndex * ColumnCount) + component.Key.ColumnIndex] = component.Value;
            }

            return result;
        }

        /// <inheritdoc/>
        internal override T[] ToColumnMajorArray()
        {
            T[] result = new T[RowCount * ColumnCount];
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[component.Key.RowIndex + (component.Key.ColumnIndex * RowCount)] = component.Value;
            }

            return result;
        }

        /// <inheritdoc/>
        internal override Vect.SparseVector<T>[] RowVectors()
        {
            // Sort the keys of the storage by row, and then by column.
            List<(int RowIndex, int ColumnIndex)> keys = new List<(int, int)> (_storage.Keys);
            keys.Sort((a , b) =>
            {
                if (a.RowIndex > b.RowIndex) { return 1; }
                else if (a.RowIndex < b.RowIndex) { return -1; }
                else
                {
                    if (a.ColumnIndex > b.ColumnIndex) { return 1; }
                    else if (a.ColumnIndex < b.ColumnIndex) { return -1; }
                    else { return 0; }
                }
            });
            

            // Fill the value list and the index list the sparse vectors
            List<int>[] indices = new List<int>[RowCount];
            List<T>[] values = new List<T>[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                indices[i] = new List<int>();
                values[i] = new List<T>();
            }

            for (int i = 0; i < keys.Count; i++)
            {
                (int RowIndex, int ColumnIndex) key = keys[i];

                indices[key.RowIndex].Add(key.ColumnIndex);
                values[key.RowIndex].Add(_storage[key]);
            }


            // Instanciate and initalise the sparse vectors
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
            // Sort the keys of the storage by column, and then by row.
            List<(int RowIndex, int ColumnIndex)> keys = new List<(int, int)>(_storage.Keys);
            keys.Sort((a, b) =>
            {
                if (a.ColumnIndex > b.ColumnIndex) { return 1; }
                else if (a.ColumnIndex < b.ColumnIndex) { return -1; }
                else
                {
                    if (a.RowIndex > b.RowIndex) { return 1; }
                    else if (a.RowIndex < b.RowIndex) { return -1; }
                    else { return 0;  }
                }
            });


            // Fill the value list and the index list the sparse vectors
            List<int>[] indices = new List<int>[ColumnCount];
            List<T>[] values = new List<T>[ColumnCount];
            for (int i = 0; i < ColumnCount; i++)
            {
                indices[i] = new List<int>();
                values[i] = new List<T>();
            }

            for (int i = 0; i < keys.Count; i++)
            {
                (int RowIndex, int ColumnIndex) key = keys[i];

                indices[key.ColumnIndex].Add(key.RowIndex);
                values[key.ColumnIndex].Add(_storage[key]);
            }

            // Instanciate and initalise the sparse vectors
            Vect.SparseVector<T>[] vectors = new Vect.SparseVector<T>[ColumnCount];
            for (int i_Column = 0; i_Column < vectors.Length; i_Column++)
            {
                vectors[i_Column] = new Vect.SparseVector<T>(RowCount, indices[i_Column], values[i_Column]);
            }

            return vectors;
        }

        #endregion

        #region Private Methods

        //     -----     AdditionOperator(SparseStorage<T> right)     -----     //

        private DictionaryOfKeys<T> AdditionOperator(DictionaryOfKeys<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount || ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The sparse matrix storages must have the same number of rows and colunms as this sparse matrix storage.");
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(this);
            foreach (KeyValuePair<(int, int), T> rightComponent in right.Storage)
            {
                if (result._storage.ContainsKey(rightComponent.Key))
                {
                    T value = result._storage[rightComponent.Key] + rightComponent.Value;

                    if (value != T.Zero) { result._storage[rightComponent.Key] = value; }
                    else { result._storage.Remove(rightComponent.Key); }
                }
                else { result._storage.Add(rightComponent.Key, rightComponent.Value); }
            }

            return result;
        }

        private DictionaryOfKeys<T> AdditionOperator(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount || ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The sparse matrix storages must have the same number of rows and colunms as this sparse matrix storage.");
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(this);
            for (int i_Column = 0; i_Column < right.ColumnCount; i_Column++)
            {
                for (int i = right.ColumnPointers[i_Column]; i < right.ColumnPointers[i_Column + 1]; i++)
                {
                    (int, int) resultKey = (right.RowIndices[i], i_Column);

                    if (result._storage.ContainsKey(resultKey))
                    {
                        T value = result._storage[resultKey] + right.Values[i];

                        if (value != T.Zero) { result._storage[resultKey] = value; }
                        else { result._storage.Remove(resultKey); }
                    }
                    else { result._storage.Add(resultKey, right.Values[i]); }
                }
            }


            return result;
        }


        //     -----     SubtractionOperator(SparseStorage<T> right)     -----     //

        private DictionaryOfKeys<T> SubtractionOperator(DictionaryOfKeys<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount || ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The sparse matrix storages must have the same number of rows and colunms as this sparse matrix storage.");
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(this);
            foreach (KeyValuePair<(int, int), T> rightComponent in right.Storage)
            {
                if (result._storage.ContainsKey(rightComponent.Key))
                {
                    T value = result._storage[rightComponent.Key] - rightComponent.Value;

                    if (value != T.Zero) { result._storage[rightComponent.Key] = value; }
                    else { result._storage.Remove(rightComponent.Key); }
                }
                else { result._storage.Add(rightComponent.Key, -rightComponent.Value); }
            }

            return result;
        }

        private DictionaryOfKeys<T> SubtractionOperator(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount || ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException("The sparse matrix storages must have the same number of rows and colunms as this sparse matrix storage.");
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(this);
            for (int i_Column = 0; i_Column < right.ColumnCount; i_Column++)
            {
                for (int i = right.ColumnPointers[i_Column]; i < right.ColumnPointers[i_Column + 1]; i++)
                {
                    (int, int) resultKey = (right.RowIndices[i], i_Column);

                    if (result._storage.ContainsKey(resultKey))
                    {
                        T value = result._storage[resultKey] - right.Values[i];

                        if (value != T.Zero) { result._storage[resultKey] = value; }
                        else { result._storage.Remove(resultKey); }
                    }
                    else { result._storage.Add(resultKey, -right.Values[i]); }
                }
            }


            return result;
        }


        //     -----     MultiplyOperator(SparseStorage<T> right)     -----     //

        private DictionaryOfKeys<T> MultiplyOperator(DictionaryOfKeys<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the sparse matrix storage must be equal to the number of columns of this sparse matrix storage.");
            }


            HashSet<(int ColumnIndex, T Value)>[] rightRows = new HashSet<(int ColumnIndex, T Value)>[right.RowCount];
            for (int i_RightRow = 0; i_RightRow < rightRows.Length; i_RightRow++) { rightRows[i_RightRow] = new HashSet<(int, T)>(); }
            foreach (var rightComponent in right._storage)
            {
                rightRows[rightComponent.Key.RowIndex].Add((rightComponent.Key.ColumnIndex, rightComponent.Value));
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(RowCount, right.ColumnCount, (Count + right.Count) / 2);

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> leftComponent in _storage)
            {
                foreach((int rightColumnIndex, T rightValue) in rightRows[leftComponent.Key.ColumnIndex])
                {
                    (int, int) resultKey = (leftComponent.Key.RowIndex, rightColumnIndex);

                    if (result._storage.ContainsKey(resultKey)) { result._storage[resultKey] += leftComponent.Value * rightValue; }
                    else { result._storage.Add(resultKey, leftComponent.Value * rightValue); }
                }
            }

            // Remove zero-valued components
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> resultComponent in result._storage)
            {
                if (resultComponent.Value == T.Zero) { result._storage.Remove(resultComponent.Key); }
            }

            return result;
        }

        private DictionaryOfKeys<T> MultiplyOperator(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the sparse matrix storage must be equal to the number of columns of this sparse matrix storage.");
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(RowCount, right.ColumnCount, (Count + right.Count) / 2);

            HashSet<(int RowIndex, T Value)>[] leftColumns = new HashSet<(int RowIndex, T Value)>[ColumnCount];
            for (int i_LeftColumn = 0; i_LeftColumn < leftColumns.Length; i_LeftColumn++) { leftColumns[i_LeftColumn] = new HashSet<(int, T)>(); }
            foreach (var leftComponent in _storage)
            {
                leftColumns[leftComponent.Key.ColumnIndex].Add((leftComponent.Key.RowIndex, leftComponent.Value));
            }

            for (int i_RightColumn = 0; i_RightColumn < right.ColumnCount; i_RightColumn++)
            {
                for (int i = right.ColumnPointers[i_RightColumn]; i < right.ColumnPointers[i_RightColumn + 1]; i++)
                {
                    foreach ((int leftRowIndex, T leftValue) in leftColumns[right.RowIndices[i]])
                    {
                        (int, int) resultKey = (leftRowIndex, i_RightColumn);

                        if (result._storage.ContainsKey(resultKey)) { result._storage[resultKey] += leftValue * right.Values[i]; }
                        else { result._storage.Add(resultKey, leftValue * right.Values[i]); }
                    }
                }
            }


            // Remove zero-valued components
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> resultComponent in result._storage)
            {
                if (resultComponent.Value == T.Zero) { result._storage.Remove(resultComponent.Key); }
            }

            return result;
        }


        //     -----     TransposeMultiply(SparseStorage<T> right)     -----     //

        private DictionaryOfKeys<T> TransposeMultiply(DictionaryOfKeys<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the sparse matrix storage must be equal to the number of rows of this sparse matrix storage");
            }


            HashSet<(int ColumnIndex, T Value)>[] rightRows = new HashSet<(int ColumnIndex, T Value)>[right.RowCount];
            for (int i_RightRow = 0; i_RightRow < rightRows.Length; i_RightRow++) { rightRows[i_RightRow] = new HashSet<(int, T)>(); }
            foreach (var kvp in right._storage)
            {
                rightRows[kvp.Key.RowIndex].Add((kvp.Key.ColumnIndex, kvp.Value));
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(ColumnCount, right.ColumnCount, (Count + right.Count) / 2);

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> leftComponent in _storage)
            {
                foreach ((int rightColumnIndex, T rightValue) in rightRows[leftComponent.Key.RowIndex])
                {
                    (int, int) resultKey = (leftComponent.Key.ColumnIndex, rightColumnIndex);

                    if (result._storage.ContainsKey(resultKey)) { result._storage[resultKey] += leftComponent.Value * rightValue; }
                    else { result._storage.Add(resultKey, leftComponent.Value * rightValue); }
                }
            }

            // Remove zero-valued components
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> resultComponent in result._storage)
            {
                if(resultComponent.Value == T.Zero) { result._storage.Remove(resultComponent.Key); }
            }

            return result;
        }

        private DictionaryOfKeys<T> TransposeMultiply(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (RowCount != right.RowCount)
            {
                throw new ArgumentException($"The number of rows of the sparse matrix storage must be equal to the number of rows of this sparse matrix storage");
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(ColumnCount, right.ColumnCount, (Count + right.Count) / 2);

            HashSet<(int ColumnIndex, T Value)>[] leftRows = new HashSet<(int ColumnIndex, T Value)>[RowCount];
            for (int i_LeftRow = 0; i_LeftRow < leftRows.Length; i_LeftRow++) { leftRows[i_LeftRow] = new HashSet<(int, T)>(); }
            foreach (var leftComponent in _storage)
            {
                leftRows[leftComponent.Key.RowIndex].Add((leftComponent.Key.ColumnIndex, leftComponent.Value));
            }

            for (int i_RightColumn = 0; i_RightColumn < right.ColumnCount; i_RightColumn++)
            {
                for (int i = right.ColumnPointers[i_RightColumn]; i < right.ColumnPointers[i_RightColumn + 1]; i++)
                {
                    foreach ((int leftColumnIndex, T leftValue) in leftRows[right.RowIndices[i]])
                    {
                        (int, int) resultKey = (leftColumnIndex, i_RightColumn);

                        if (result._storage.ContainsKey(resultKey)) { result._storage[resultKey] += leftValue * right.Values[i]; }
                        else { result._storage.Add(resultKey, leftValue * right.Values[i]); }
                    }
                }
            }


            // Remove zero-valued components
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> resultComponent in result._storage)
            {
                if (resultComponent.Value == T.Zero) { result._storage.Remove(resultComponent.Key); }
            }

            return result;
        }


        //     -----     MultiplyTranspose(SparseStorage<T> right)     -----     //

        private DictionaryOfKeys<T> MultiplyTranspose(DictionaryOfKeys<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException($"The number of columns of the sparse matrix storage must be equal to the number of columns of this sparse matrix storage");
            }


            HashSet<(int RowIndex, T Value)>[] rightColumns = new HashSet<(int RowIndex, T Value)>[right.ColumnCount];
            for (int i_RightColumn = 0; i_RightColumn < rightColumns.Length; i_RightColumn++) { rightColumns[i_RightColumn] = new HashSet<(int, T)>(); }
            foreach (var rightComponent in right._storage)
            {
                rightColumns[rightComponent.Key.ColumnIndex].Add((rightComponent.Key.RowIndex, rightComponent.Value));
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(RowCount, right.RowCount, (Count + right.Count) / 2);

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> leftComponent in _storage)
            {
                foreach ((int rightRowIndex, T rightValue) in rightColumns[leftComponent.Key.ColumnIndex])
                {
                    (int, int) resultKey = (leftComponent.Key.RowIndex, rightRowIndex);

                    if (result._storage.ContainsKey(resultKey)) { result._storage[resultKey] += leftComponent.Value * rightValue; }
                    else { result._storage.Add(resultKey, leftComponent.Value * rightValue); }
                }
            }


            // Remove zero-valued components
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> resultComponent in result._storage)
            {
                if (resultComponent.Value == T.Zero) { result._storage.Remove(resultComponent.Key); }
            }

            return result;
        }

        private DictionaryOfKeys<T> MultiplyTranspose(CompressedColumn<T> right)
        {
            //     -----     Verifications

            if (ColumnCount != right.ColumnCount)
            {
                throw new ArgumentException($"The number of columns of the sparse matrix storage must be equal to the number of columns of this sparse matrix storage");
            }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(RowCount, right.RowCount, (Count + right.Count) / 2);

            HashSet<(int RowIndex, T Value)>[] leftColumns = new HashSet<(int RowIndex, T Value)>[ColumnCount];
            for (int i_LeftColumn = 0; i_LeftColumn < leftColumns.Length; i_LeftColumn++) { leftColumns[i_LeftColumn] = new HashSet<(int, T)>(); }
            foreach (var leftComponent in _storage)
            {
                leftColumns[leftComponent.Key.ColumnIndex].Add((leftComponent.Key.RowIndex, leftComponent.Value));
            }

            for (int i_RightColumn = 0; i_RightColumn < right.ColumnCount; i_RightColumn++)
            {
                for (int i = right.ColumnPointers[i_RightColumn]; i < right.ColumnPointers[i_RightColumn + 1]; i++)
                {
                    foreach ((int leftRowIndex, T leftValue) in leftColumns[i_RightColumn])
                    {
                        (int, int) resultKey = (leftRowIndex, right.RowIndices[i]);

                        if (result._storage.ContainsKey(resultKey)) { result._storage[resultKey] += leftValue * right.Values[i]; }
                        else { result._storage.Add(resultKey, leftValue * right.Values[i]); }
                    }
                }
            }


            // Remove zero-valued components
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> resultComponent in result._storage)
            {
                if (resultComponent.Value == T.Zero) { result._storage.Remove(resultComponent.Key); }
            }

            return result;
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
            if (right is DictionaryOfKeys<T> rightDOK) { return AdditionOperator(rightDOK); }
            else if (right is CompressedColumn<T> rightCC) { return AdditionOperator(rightCC); }
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
            if (right is DictionaryOfKeys<T> rightDOK) { return SubtractionOperator(rightDOK); }
            else if(right is CompressedColumn<T> rightCC) { return SubtractionOperator(rightCC); }
            else
            {
                throw new NotImplementedException($"The subtraction of this sparse matrix storage with a {right.GetType()} as a {typeof(SparseStorage<T>)} is not implemented.");
            }
        }


        /// <inheritdoc/>
        protected override SparseStorage<T> UnaryNegationOperator()
        {
            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(RowCount, ColumnCount, Count);

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage) { result._storage.Add(component.Key, -component.Value); }

            return result;
        }


        /// <inheritdoc/>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of this sparse storage with <paramref name="right"/> as a <see cref="SparseStorage{T}"/> is not implemented. 
        /// </exception>
        protected override SparseStorage<T> MultiplyOperator(SparseStorage<T> right)
        {
            if (right is DictionaryOfKeys<T> rightDOK) { return MultiplyOperator(rightDOK); }
            if (right is CompressedColumn<T> rightCC) { return MultiplyOperator(rightCC); }
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
            if (right is DictionaryOfKeys<T> rightDOK) { return TransposeMultiply(rightDOK); }
            if (right is CompressedColumn<T> rightCC) { return TransposeMultiply(rightCC); }
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
            if (right is DictionaryOfKeys<T> rightDOK) { return MultiplyTranspose(rightDOK); }
            if (right is CompressedColumn<T> rightCC) { return MultiplyTranspose(rightCC); }
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
            foreach(KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[component.Key.RowIndex, component.Key.ColumnIndex] = component.Value + result[component.Key.RowIndex, component.Key.ColumnIndex];
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[component.Key.RowIndex, component.Key.ColumnIndex] = component.Value + result[component.Key.RowIndex, component.Key.ColumnIndex];
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> leftComponent in _storage)
            {
                for (int i_RightColumn = 0; i_RightColumn < right.ColumnCount; i_RightColumn++)
                {
                    result[leftComponent.Key.RowIndex, i_RightColumn] += leftComponent.Value * right[leftComponent.Key.ColumnIndex, i_RightColumn];
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> leftComponent in _storage)
            {
                for (int i_RightColumn = 0; i_RightColumn < right.ColumnCount; i_RightColumn++)
                {
                    result[leftComponent.Key.ColumnIndex, i_RightColumn] += leftComponent.Value * right[leftComponent.Key.RowIndex, i_RightColumn];
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> leftComponent in _storage)
            {
                for (int i_RightRow = 0; i_RightRow < right.RowCount; i_RightRow++)
                {
                    result[leftComponent.Key.RowIndex, i_RightRow] += leftComponent.Value * right[i_RightRow, leftComponent.Key.ColumnIndex];
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[component.Key.RowIndex, component.Key.ColumnIndex] = result[component.Key.RowIndex, component.Key.ColumnIndex] + component.Value;
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[component.Key.RowIndex, component.Key.ColumnIndex] = result[component.Key.RowIndex, component.Key.ColumnIndex] - component.Value;
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> rightComponent in _storage)
            {
                for (int i_LeftRow = 0; i_LeftRow < left.RowCount; i_LeftRow++)
                {
                    result[i_LeftRow, rightComponent.Key.ColumnIndex] += left[i_LeftRow, rightComponent.Key.RowIndex] * rightComponent.Value;
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> rightComponent in _storage)
            {
                for (int i_LeftColumn = 0; i_LeftColumn < left.ColumnCount; i_LeftColumn++)
                {
                    result[i_LeftColumn, rightComponent.Key.ColumnIndex] += left[rightComponent.Key.RowIndex, i_LeftColumn] * rightComponent.Value;
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> rightComponent in _storage)
            {
                for (int i_LeftRow = 0; i_LeftRow < left.RowCount; i_LeftRow++)
                {
                    result[i_LeftRow, rightComponent.Key.RowIndex] += left[i_LeftRow, rightComponent.Key.ColumnIndex] * rightComponent.Value;
                }
            }


            return result;
        }


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc/>
        protected override SparseStorage<T> RightMultiplyOperator(T factor)
        {
            //     -----     Verification

            if(factor == T.Zero) { return new DictionaryOfKeys<T>(RowCount, ColumnCount, 0); }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(RowCount, ColumnCount, Count);

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage) { result._storage.Add(component.Key, component.Value * factor); }

            return result;
        }

        /// <inheritdoc/>
        protected override SparseStorage<T> LeftMultiplyOperator(T factor)
        {
            //     -----     Verification

            if (factor == T.Zero) { return new DictionaryOfKeys<T>(RowCount, ColumnCount, 0); }


            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(RowCount, ColumnCount, Count);

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage) { result._storage.Add(component.Key, factor * component.Value ); }

            return result;
        }

        /// <inheritdoc/>
        protected override SparseStorage<T> DivisionOperator(T divisor)
        {
            DictionaryOfKeys<T> result = new DictionaryOfKeys<T>(RowCount, ColumnCount, Count);

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage) { result._storage.Add(component.Key, component.Value / divisor); }

            return result;
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                if(vector.Contains(component.Key.ColumnIndex)) 
                {
                    array[component.Key.RowIndex] += component.Value * vector[component.Key.ColumnIndex];
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[component.Key.RowIndex] += component.Value * vector[component.Key.ColumnIndex];
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


            T[] array = new T[this.ColumnCount];
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                if (vector.Contains(component.Key.RowIndex))
                {
                    array[component.Key.ColumnIndex] += component.Value * vector[component.Key.RowIndex];
                }
            }


            Vect.SparseVector<T> result = new Vect.SparseVector<T>(this.ColumnCount);
            for (int i = 0; i < result.Size; i++)
            {
                if (array[i] != T.Zero) { result.Add(i, array[i]); }
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
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), T> component in _storage)
            {
                result[component.Key.ColumnIndex] += component.Value * vector[component.Key.RowIndex];
            }


            return result;
        }


        #endregion
    }
}
