using System;
using System.Collections.Generic;

using Xunit;

using Mat = BRIDGES.Numerics.LinearAlgebra.Matrices;
using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Matrices.Double.SparseStorages
{
    public class DictionaryOfKeys
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.SparseStorage{T}.RowCount"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected number of row. </param>
        [Theory(DisplayName = "Prop. RowCount")]
        [ClassData(typeof(DataClasses.RowCount))]
        public void Property__RowCount(Mat.SparseStorages.DictionaryOfKeys<double> storage, int expected)
        {
            // Arrange

            //Act
            int actual = storage.RowCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.SparseStorage{T}.ColumnCount"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected number of column. </param>
        [Theory(DisplayName = "Prop. ColumnCount")]
        [ClassData(typeof(DataClasses.ColumnCount))]
        public void Property__ColumnCount(Mat.SparseStorages.DictionaryOfKeys<double> storage, int expected)
        {
            // Arrange

            //Act
            int result = storage.ColumnCount;

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.SparseStorage{T}.Count"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected number of non zero values. </param>
        [Theory(DisplayName = "Prop. Count")]
        [ClassData(typeof(DataClasses.Count))]
        public void Property__Count(Mat.SparseStorages.DictionaryOfKeys<double> storage, int expected)
        {
            // Arrange

            //Act
            int actual = storage.Count;

            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.SparseStorage{T}.this(int, int)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "Prop. this[int, int]")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void Property__Indexer__Int_Int(Mat.SparseStorages.DictionaryOfKeys<double> storage, double[,] values)
        {
            // Arrange

            // Act & Assert
            Assert.Equal(values.GetLength(0), storage.RowCount);
            Assert.Equal(values.GetLength(1), storage.ColumnCount);
            for (int i_Row = 0; i_Row < storage.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < storage.ColumnCount; i_Column++)
                {
                    if (values[i_Row, i_Column] == 0.0)
                    {
                        Assert.False(storage.Contains(i_Row, i_Column));
                    }
                    else
                    {
                        Assert.Equal(values[i_Row, i_Column], storage[i_Row, i_Column]);
                    }

                }
            }
        }


        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Storage"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected dictionary containing (rowIndex, columnIndex) keys and the associated component values. </param>
        [Theory(DisplayName = "Prop. Storage")]
        [ClassData(typeof(DataClasses.Storage))]
        public void Property__Storage(Mat.SparseStorages.DictionaryOfKeys<double> storage, Dictionary<(int,int), double> expected)
        {
            // Arrange

            //Act
            IReadOnlyDictionary<(int RowIndex, int ColumnIndex), double> result = storage.Storage;

            // Assert
            Assert.Equal(expected.Count, result.Count);
            foreach(KeyValuePair<(int, int), double> kvp in expected)
            {
                Assert.True(result.ContainsKey(kvp.Key));
                Assert.Equal(kvp.Value, result[kvp.Key]);
            }
        }

        #endregion

        #region Tests : Constructors

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.DictionaryOfKeys(int, int, int)"/>.
        /// </summary>
        /// <param name="rowCount"> Expected number of row. </param>
        /// <param name="columnCount"> Expected number of column. </param>
        /// <param name="capacity"> Expected number of non zero values. </param>
        [Theory(DisplayName = "DictionaryOfKeys<T>(int, int, int)")]
        [ClassData(typeof(DataClasses.Constructor__Int_Int_Int))]
        public void Constructor__Int_Int_Int(int rowCount, int columnCount, int capacity)
        {
            // Arrange

            //Act
            Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, capacity);

            // Assert
            Assert.Equal(rowCount, result.RowCount);
            Assert.Equal(columnCount, result.ColumnCount);
            Assert.Equal(0, result.Count);

            Assert.Empty(result.Storage);
        }


        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.DictionaryOfKeys(Mat.SparseStorages.DictionaryOfKeys{T})"/>.
        /// </summary>
        /// <param name="expected"> Sparse storage to deep copy. </param>
        [Theory(DisplayName = "DictionaryOfKeys<T>(DictionaryOfKeys<T>)")]
        [ClassData(typeof(DataClasses.Constructor__DictionaryOfKeys))]
        public void Constructor__CompressedColumn(Mat.SparseStorages.DictionaryOfKeys<double> expected)
        {
            // Arrange

            //Act
            Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(expected);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);

            Assert.Equal(expected.Count, result.Count);
            foreach(KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(result.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, result[kvp.Key.RowIndex, kvp.Key.ColumnIndex]);
            }
        }

        #endregion

        #region Tests : Public Methods

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.TryGet(int, int, out T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "TryGet(int, int)")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void TryGet__Int_Int(Mat.SparseStorages.DictionaryOfKeys<double> storage, double[,] values)
        {
            // Arrange

            // Act & Assert
            Assert.Equal(values.GetLength(0), storage.RowCount);
            Assert.Equal(values.GetLength(1), storage.ColumnCount);
            for (int i_Row = 0; i_Row < storage.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < storage.ColumnCount; i_Column++)
                {
                    if (values[i_Row, i_Column] == 0.0)
                    {
                        Assert.False(storage.TryGet(i_Row, i_Column, out double value));
                    }
                    else
                    {
                        Assert.True(storage.TryGet(i_Row, i_Column, out double value));
                        Assert.Equal(values[i_Row, i_Column], value);
                    }

                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Insert(int, int, T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected">  Expected sparse storage after insertion of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Insert(int, int, T)")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T(Mat.SparseStorages.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldInsert = !storage.Contains(rowIndex, columnIndex);

            // Act
            bool isInserted = storage.Add(rowIndex, columnIndex, value);

            // Assert
            Assert.Equal(shouldInsert, isInserted);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage[kvp.Key.RowIndex, kvp.Key.ColumnIndex]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Insert(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected">  Expected sparse storage after insertion of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Insert(int, int, T, Bool) - Check")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T_Bool__Check(Mat.SparseStorages.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldInsert = !(storage.Contains(rowIndex, columnIndex) || value == 0.0);

            if (!shouldInsert) { expected = new Mat.SparseStorages.DictionaryOfKeys<double>(storage); }

            // Act
            bool isInserted = storage.Add(rowIndex, columnIndex, value, true);

            // Assert
            Assert.Equal(shouldInsert, isInserted);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage[kvp.Key.RowIndex, kvp.Key.ColumnIndex]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Insert(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected">  Expected sparse storage after insertion of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Insert(int, int, T, Bool) - No Check")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T_Bool__NoCheck(Mat.SparseStorages.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldInsert = !storage.Contains(rowIndex, columnIndex);

            // Act
            bool isInserted = storage.Add(rowIndex, columnIndex, value, false);

            // Assert
            Assert.Equal(shouldInsert, isInserted);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage[kvp.Key.RowIndex, kvp.Key.ColumnIndex]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Replace(int, int, T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Replace(int, int, T)")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T(Mat.SparseStorages.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldReplace = storage.Contains(rowIndex, columnIndex);

            // Act
            bool isReplaced = storage.Replace(rowIndex, columnIndex, value);

            // Assert
            Assert.Equal(shouldReplace, isReplaced);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage[kvp.Key.RowIndex, kvp.Key.ColumnIndex]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Replace(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/>. </param>
        [Theory(DisplayName = "Replace(int, int, T, Bool) - Check")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T_Bool__Check(Mat.SparseStorages.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldReplace = storage.Contains(rowIndex, columnIndex) && value != 0.0;

            if (!shouldReplace) { expected = new Mat.SparseStorages.DictionaryOfKeys<double>(storage); }

            // Act
            bool isReplaced = storage.Replace(rowIndex, columnIndex, value, true);

            // Assert
            Assert.Equal(shouldReplace, isReplaced);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage[kvp.Key.RowIndex, kvp.Key.ColumnIndex]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Replace(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/>. </param>
        [Theory(DisplayName = "Replace(int, int, T, Bool) - No Check")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T_Bool__NoCheck(Mat.SparseStorages.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldReplace = storage.Contains(rowIndex, columnIndex);

            if (!shouldReplace) { expected = new Mat.SparseStorages.DictionaryOfKeys<double>(storage); }

            // Act
            bool isReplaced = storage.Replace(rowIndex, columnIndex, value, false);

                        // Assert
            Assert.Equal(shouldReplace, isReplaced);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage[kvp.Key.RowIndex, kvp.Key.ColumnIndex]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Remove(int, int)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to remove. </param>
        /// <param name="columnIndex"> Column index of the component to remove. </param>
        /// <param name="expected">  Expected sparse storage after removal of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Remove(int, int)")]
        [ClassData(typeof(DataClasses.Remove))]
        public void Remove__Int_Int(Mat.SparseStorages.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, Mat.SparseStorages.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldRemove = storage.Contains(rowIndex, columnIndex);

            // Act
            bool isRemoved = storage.Remove(rowIndex, columnIndex);

            // Assert
            Assert.Equal(shouldRemove, isRemoved);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage[kvp.Key.RowIndex, kvp.Key.ColumnIndex]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.Contains(int, int)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="isContained"> Two-dimensional array evaluating whether a component at the given row and column has an entry in the sparse storage.</param>
        [Theory(DisplayName = "Contains(int, int)")]
        [ClassData(typeof(DataClasses.Contains))]
        public void Contains__Int_Int(Mat.SparseStorages.DictionaryOfKeys<double> storage, bool[,] isContained)
        {
            // Arrange

            // Act & Assert
            Assert.Equal(isContained.GetLength(0), storage.RowCount);
            Assert.Equal(isContained.GetLength(1), storage.ColumnCount);
            for (int i_Row = 0; i_Row < storage.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < storage.ColumnCount; i_Column++)
                {
                    Assert.Equal(isContained[i_Row, i_Column], storage.Contains(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.ToArray()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="expected"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToArray(Mat.SparseStorages.DictionaryOfKeys<double> storage, double[,] expected)
        {
            // Arrange

            // Act
            double[,] result = storage.ToArray();

            // Assert
            Assert.Equal(expected.GetLength(0), result.GetLength(0));
            Assert.Equal(expected.GetLength(1), result.GetLength(1));
            for (int i_Row = 0; i_Row < result.GetLength(0); i_Row++)
            {
                for (int i_Column = 0; i_Column < result.GetLength(1); i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.ToRowMajorArray()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToRowMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToRowMajorArray(Mat.SparseStorages.DictionaryOfKeys<double> storage, double[,] values)
        {
            // Arrange
            double[] expected = new double[values.GetLength(0) * values.GetLength(1)];
            for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
            {
                for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
                {
                    expected[i_Column + (i_Row * values.GetLength(1))] = values[i_Row, i_Column];
                }
            }

            // Act
            double[] result = storage.ToRowMajorArray();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.ToColumnMajorArray()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToColumnMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToColumnMajorArray(Mat.SparseStorages.DictionaryOfKeys<double> storage, double[,] values)
        {
            // Arrange
            double[] expected = new double[values.GetLength(0) * values.GetLength(1)];
            for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
            {
                for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
                {
                    expected[i_Row + (i_Column * values.GetLength(0))] = values[i_Row, i_Column];
                }
            }

            // Act
            double[] result = storage.ToColumnMajorArray();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.RowVectors()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "RowVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void RowVectors(Mat.SparseStorages.DictionaryOfKeys<double> storage, double[,] values)
        {
            // Arrange
            Vect.SparseVector<double>[] expected = new Vect.SparseVector<double>[values.GetLength(0)];

            List<int> rowIndices = new List<int>(values.GetLength(1));
            List<double> rowValues = new List<double>(values.GetLength(1));
            for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
            {
                rowIndices.Clear();
                rowValues.Clear();

                for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
                {
                    double rowValue = values[i_Row, i_Column];
                    if (rowValue != 0.0)
                    {
                        rowIndices.Add(i_Column);
                        rowValues.Add(rowValue);
                    }
                }

                expected[i_Row] = new Vect.SparseVector<double>(values.GetLength(1), rowIndices, rowValues);
            }

            // Act
            Vect.SparseVector<double>[] result = storage.RowVectors();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i_Row = 0; i_Row < expected.Length; i_Row++)
            {
                Vect.SparseVector<double> expectedRow = expected[i_Row];
                Vect.SparseVector<double> resultRow = result[i_Row];

                Assert.Equal(expectedRow.NonZeroCount, resultRow.NonZeroCount);

                int count = 0;
                for (int i = 0; i < expectedRow.Size; i++)
                {
                    if (expectedRow.TryGet(i, out double value))
                    {
                        count++;
                        Assert.Equal(value, resultRow[i]);
                    }
                }

                Assert.Equal(expectedRow.NonZeroCount, count);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.DictionaryOfKeys{T}.ColumnVectors()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ColumnVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ColumnVectors(Mat.SparseStorages.DictionaryOfKeys<double> storage, double[,] values)
        {
            // Arrange
            Vect.SparseVector<double>[] expected = new Vect.SparseVector<double>[values.GetLength(1)];

            List<int> columnIndices = new List<int>(values.GetLength(0));
            List<double> columnValues = new List<double>(values.GetLength(0));
            for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
            {
                columnIndices.Clear();
                columnValues.Clear();

                for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
                {
                    double columnValue = values[i_Row, i_Column];
                    if (columnValue != 0.0)
                    {
                        columnIndices.Add(i_Row);
                        columnValues.Add(columnValue);
                    }
                }

                expected[i_Column] = new Vect.SparseVector<double>(values.GetLength(0), columnIndices, columnValues);
            }

            // Act
            Vect.SparseVector<double>[] result = storage.ColumnVectors();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i_Column = 0; i_Column < expected.Length; i_Column++)
            {
                Vect.SparseVector<double> expectedColumn = expected[i_Column];
                Vect.SparseVector<double> resultColumn = result[i_Column];

                Assert.Equal(expectedColumn.NonZeroCount, resultColumn.NonZeroCount);

                int count = 0;
                for (int i = 0; i < expectedColumn.Size; i++)
                {
                    if (expectedColumn.TryGet(i, out double value))
                    {
                        count++;
                        Assert.Equal(value, resultColumn[i]);
                    }
                }

                Assert.Equal(expectedColumn.NonZeroCount, count);
            }
        }
        
        #endregion


        #region Storage Classes for Data Classes

        internal static class DataStorages
        {
            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse storage S1 represents a [2x3] matrix. </remarks>
            internal static class S1
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S1"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.DictionaryOfKeys<double> _s1 = CreateS1();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S1"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s1];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S1"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS1()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the storage <see cref="S1"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [2];

                /// <summary>
                /// Provides the expected number of columns of the storage <see cref="S1"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [3];

                /// <summary>
                /// Provides the expected number of entries of the storage <see cref="S1"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [6];


                //     -----     Public Static Methods


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S1<sup>t</sup>·S2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> TransposeMultiply_S2()
                {
                    int rowCount = 3;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 8);

                    dok.Add(0, 0, 1.0);   dok.Add(0, 1, 1.0);  /*dok.Add(0, 2, 0.0);*/
                    dok.Add(1, 0, 6.0);   dok.Add(1, 1, 3.0);    dok.Add(1, 2, 3.0);
                    dok.Add(2, 0, -16.0); dok.Add(2, 1, 5.0);    dok.Add(2, 2, -21.0);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S1·S2<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> MultiplyTranspose_S2()
                {
                    int rowCount = 2;
                    int columnCount = 2;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                    dok.Add(0, 0, -11.0); dok.Add(0, 1,  8.0);
                    dok.Add(1, 0,  21.0); dok.Add(1, 1, -6.0);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the transposition : <c>S1<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Transpose()
                {
                    int rowCount = 3;
                    int columnCount = 2;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0,  1.0); dok.Add(0, 1, 2.0); 
                    dok.Add(1, 0,  4.0); dok.Add(1, 1, 7.0); 
                    dok.Add(2, 0, -2.0); dok.Add(2, 1, 3.0); 

                    return dok;
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>S1+S2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Addition_S2()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      dok.Add(0, 0, 6.0);   dok.Add(0, 1, 3.0);   dok.Add(0, 2, 4.0);
                    /*dok.Add(1, 0, 0.0);*/ dok.Add(1, 1, 8.0); /*dok.Add(1, 2, 0.0);*/

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>S1-S2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Subtraction_S2()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0, -4.0); dok.Add(0, 1, 5.0); dok.Add(0, 2, -8.0);
                    dok.Add(1, 0,  4.0); dok.Add(1, 1, 6.0); dok.Add(1, 2,  6.0);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-S1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> UnaryNegation()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0, -1.0); dok.Add(0, 1, -4.0); dok.Add(0, 2,  2.0);
                    dok.Add(1, 0, -2.0); dok.Add(1, 1, -7.0); dok.Add(1, 2, -3.0);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 3.0 : <c>S1·3.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.DictionaryOfKeys<double> Result) RightMultiplication_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0, 3.0); dok.Add(0, 1, 12.0); dok.Add(0, 2, -6.0);
                    dok.Add(1, 0, 6.0); dok.Add(1, 1, 21.0); dok.Add(1, 2,  9.0);

                    return (3.0, dok);
                }

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·S1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.DictionaryOfKeys<double> Result) LeftMultiplication_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0, 5.0);  dok.Add(0, 1, 20.0); dok.Add(0, 2, -10.0);
                    dok.Add(1, 0, 10.0); dok.Add(1, 1, 35.0); dok.Add(1, 2, 15.0);

                    return (5.0, dok);
                }

                /// <summary>
                /// Provides the expected result of the scalar division by 4.0 : <c>S1/4.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Divisor, Mat.SparseStorages.DictionaryOfKeys<double> Result) Division_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0, 0.25); dok.Add(0, 1, 1.0);  dok.Add(0, 2, -0.5);
                    dok.Add(1, 0, 0.5);  dok.Add(1, 1, 1.75); dok.Add(1, 2,  0.75);

                    return (4.0, dok);
                }


                //     -----     Methods

                /// <summary>
                /// Provides an array representation of the matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A two dimensional array of <see cref="double"/> representing the matrix. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    double[,] values = new double[,]
                    {
                        {    1.0,    4.0,    -2.0    },
                        {    2.0,    7.0,     3.0    },
                    };

                    return [values];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse storage <see cref="S1"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>. </returns>
                internal static Mat.SparseStorages.DictionaryOfKeys<double> CreateS1()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0, 1.0);  dok.Add(0, 1, 4.0); dok.Add(0, 2, -2.0);
                    dok.Add(1, 0, 2.0);  dok.Add(1, 1, 7.0); dok.Add(1, 2,  3.0);

                    return dok;
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse storage S2 represents a [2x3] matrix. </remarks>
            internal static class S2
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S2"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.DictionaryOfKeys<double> _s2 = CreateS2();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S2"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s2];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S2"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS2()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the storage <see cref="S2"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [2];

                /// <summary>
                /// Provides the expected number of columns of the storage <see cref="S2"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [3];

                /// <summary>
                /// Provides the expected number of entries of the storage <see cref="S2"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [6];


                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S2<sup>t</sup>·S1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> TransposeMultiply_S1()
                {
                    int rowCount = 3;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 8);

                      dok.Add(0, 0, 1.0);   dok.Add(0, 1, 6.0); dok.Add(0, 2, -16.0);
                      dok.Add(1, 0, 1.0);   dok.Add(1, 1, 3.0); dok.Add(1, 2,   5.0);
                    /*dok.Add(2, 0, 0.0);*/ dok.Add(2, 1, 3.0); dok.Add(2, 2, -21.0);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S2·S1<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> MultiplyTranspose_S1()
                {
                    int rowCount = 2;
                    int columnCount = 2;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                    dok.Add(0, 0, -11.0); dok.Add(0, 1, 21.0);
                    dok.Add(1, 0,   8.0); dok.Add(1, 1, -6.0);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the transposition : <c>S2<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Transpose()
                {
                    int rowCount = 3;
                    int columnCount = 2;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0,  5.0); dok.Add(0, 1, -2.0); 
                    dok.Add(1, 0, -1.0); dok.Add(1, 1,  1.0);
                    dok.Add(2, 0,  6.0); dok.Add(2, 1, -3.0);

                    return dok;
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>S2+S1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Addition_S1()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      dok.Add(0, 0, 6.0);   dok.Add(0, 1, 3.0);   dok.Add(0, 2, 4.0);
                    /*dok.Add(1, 0, 0.0);*/ dok.Add(1, 1, 8.0); /*dok.Add(1, 2, 0.0);*/

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>S2-S1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Subtraction_S1()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0,  4.0); dok.Add(0, 1, -5.0); dok.Add(0, 2,  8.0);
                    dok.Add(1, 0, -4.0); dok.Add(1, 1, -6.0); dok.Add(1, 2, -6.0);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-S2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> UnaryNegation()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0, -5.0); dok.Add(0, 1,  1.0); dok.Add(0, 2, -6.0);
                    dok.Add(1, 0,  2.0); dok.Add(1, 1, -1.0); dok.Add(1, 2,  3.0);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 3.0 : <c>S2·3.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.DictionaryOfKeys<double> Result) RightMultiplication_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0, 15.0); dok.Add(0, 1, -3.0); dok.Add(0, 2, 18.0);
                    dok.Add(1, 0, -6.0); dok.Add(1, 1,  3.0); dok.Add(1, 2, -9.0);

                    return (3.0, dok);
                }

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·S2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.DictionaryOfKeys<double> Result) LeftMultiplication_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0,  25.0); dok.Add(0, 1, -5.0); dok.Add(0, 2,  30.0);
                    dok.Add(1, 0, -10.0); dok.Add(1, 1,  5.0); dok.Add(1, 2, -15.0);

                    return (5.0, dok);
                }

                /// <summary>
                /// Provides the expected result of the scalar division by 4.0 : <c>S2/4.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Divisor, Mat.SparseStorages.DictionaryOfKeys<double> Result) Division_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0,  1.25); dok.Add(0, 1, -0.25); dok.Add(0, 2,  1.5);
                    dok.Add(1, 0, -0.5);  dok.Add(1, 1,  0.25); dok.Add(1, 2, -0.75);

                    return ( 4.0, dok);
                }


                //     -----     Methods

                /// <summary>
                /// Provides an array representation of the matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A two dimensional array of <see cref="double"/> representing the matrix. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    double[,] values = new double[,]
                    {
                        {     5,    -1,     6    },
                        {    -2,     1,    -3    },
                    };

                    return [values];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse storage <see cref="S2"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.SparseStorages.CompressedColumn{T}"/>. </returns>
                internal static Mat.SparseStorages.DictionaryOfKeys<double> CreateS2()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 6);

                    dok.Add(0, 0,  5.0); dok.Add(0, 1, -1.0); dok.Add(0, 2,  6.0);
                    dok.Add(1, 0, -2.0); dok.Add(1, 1,  1.0); dok.Add(1, 2, -3.0);

                    return dok;
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse storage S3 represents a [6x6] matrix. </remarks>
            internal static class S3
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S3"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.DictionaryOfKeys<double> _s3 = CreateS3();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S3"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s3];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S3"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS3()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the storage <see cref="S3"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [6];

                /// <summary>
                /// Provides the expected number of columns of the storage <see cref="S3"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [6];

                /// <summary>
                /// Provides the expected number of entries of the storage <see cref="S3"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [35];


                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S3<sup>t</sup>·S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> TransposeMultiply_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0,  676.0); dok.Add(0, 1,  702.0); dok.Add(0, 2,  728.0); dok.Add(0, 3,  754.0); dok.Add(0, 4,  780.0); dok.Add(0, 5,  806.0);
                    dok.Add(1, 0,  887.0); dok.Add(1, 1,  924.0); dok.Add(1, 2,  961.0); dok.Add(1, 3,  998.0); dok.Add(1, 4, 1035.0); dok.Add(1, 5, 1072.0);
                    dok.Add(2, 0, 1395.0); dok.Add(2, 1, 1450.0); dok.Add(2, 2, 1505.0); dok.Add(2, 3, 1560.0); dok.Add(2, 4, 1615.0); dok.Add(2, 5, 1670.0);
                    dok.Add(3, 0, 1817.0); dok.Add(3, 1, 1884.0); dok.Add(3, 2, 1951.0); dok.Add(3, 3, 2018.0); dok.Add(3, 4, 2085.0); dok.Add(3, 5, 2152.0);
                    dok.Add(4, 0, 3232.0); dok.Add(4, 1, 3334.0); dok.Add(4, 2, 3436.0); dok.Add(4, 3, 3538.0); dok.Add(4, 4, 3640.0); dok.Add(4, 5, 3742.0);
                    dok.Add(5, 0, 5178.0); dok.Add(5, 1, 5316.0); dok.Add(5, 2, 5454.0); dok.Add(5, 3, 5592.0); dok.Add(5, 4, 5730.0); dok.Add(5, 5, 5868.0);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S3·S4<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> MultiplyTranspose_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, 1910); dok.Add(0, 1, 3260); dok.Add(0, 2, 4610); dok.Add(0, 3, 5960); dok.Add(0, 4, 7310); dok.Add(0, 5, 8660);
                    dok.Add(1, 0, 2107); dok.Add(1, 1, 3577); dok.Add(1, 2, 5047); dok.Add(1, 3, 6517); dok.Add(1, 4, 7987); dok.Add(1, 5, 9457);
                    dok.Add(2, 0,  -48); dok.Add(2, 1,  -78); dok.Add(2, 2, -108); dok.Add(2, 3, -138); dok.Add(2, 4, -168); dok.Add(2, 5, -198);
                    dok.Add(3, 0,  -44); dok.Add(3, 1,  -44); dok.Add(3, 2,  -44); dok.Add(3, 3,  -44); dok.Add(3, 4,  -44); dok.Add(3, 5,  -44);
                    dok.Add(4, 0,  294); dok.Add(4, 1,  494); dok.Add(4, 2,  694); dok.Add(4, 3,  894); dok.Add(4, 4, 1094); dok.Add(4, 5, 1294);
                    dok.Add(5, 0, 1902); dok.Add(5, 1, 3162); dok.Add(5, 2, 4422); dok.Add(5, 3, 5682); dok.Add(5, 4, 6942); dok.Add(5, 5, 8202);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S3<sup>t</sup>·V3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of <see cref="double"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static double[] TransposeMultiply_V3() => new double[] { 45, 80.5, 140, 179.5, 283, 368.5 };


                /// <summary>
                /// Provides the expected result of the transposition : <c>S3<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Transpose()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 35);

                    dok.Add(0, 0, 10); dok.Add(0, 1,  7); /*dok.Add(0, 2,  0);*/ dok.Add(0, 3,  6); dok.Add(0, 4, 1); dok.Add(0, 5,  2);
                    dok.Add(1, 0, 15); dok.Add(1, 1, 14);   dok.Add(1, 2, -1);   dok.Add(1, 3,  4); dok.Add(1, 4, 1); dok.Add(1, 5,  4);
                    dok.Add(2, 0, 20); dok.Add(2, 1, 21);   dok.Add(2, 2,  2);   dok.Add(2, 3,  2); dok.Add(2, 4, 2); dok.Add(2, 5,  8);
                    dok.Add(3, 0, 25); dok.Add(3, 1, 28);   dok.Add(3, 2, -3);   dok.Add(3, 3, -2); dok.Add(3, 4, 3); dok.Add(3, 5, 16);
                    dok.Add(4, 0, 30); dok.Add(4, 1, 35);   dok.Add(4, 2,  4);   dok.Add(4, 3, -4); dok.Add(4, 4, 5); dok.Add(4, 5, 32);
                    dok.Add(5, 0, 35); dok.Add(5, 1, 42);   dok.Add(5, 2, -5);   dok.Add(5, 3, -6); dok.Add(5, 4, 8); dok.Add(5, 5, 64);

                    return dok;
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>S3+S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Addition_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, 21); dok.Add(0, 1, 27); dok.Add(0, 2, 33); dok.Add(0, 3, 39); dok.Add(0, 4, 45); dok.Add(0, 5,  51);
                    dok.Add(1, 0, 28); dok.Add(1, 1, 36); dok.Add(1, 2, 44); dok.Add(1, 3, 52); dok.Add(1, 4, 60); dok.Add(1, 5,  68);
                    dok.Add(2, 0, 31); dok.Add(2, 1, 31); dok.Add(2, 2, 35); dok.Add(2, 3, 31); dok.Add(2, 4, 39); dok.Add(2, 5,  31);
                    dok.Add(3, 0, 47); dok.Add(3, 1, 46); dok.Add(3, 2, 45); dok.Add(3, 3, 42); dok.Add(3, 4, 41); dok.Add(3, 5,  40);
                    dok.Add(4, 0, 52); dok.Add(4, 1, 53); dok.Add(4, 2, 55); dok.Add(4, 3, 57); dok.Add(4, 4, 60); dok.Add(4, 5,  64);
                    dok.Add(5, 0, 63); dok.Add(5, 1, 66); dok.Add(5, 2, 71); dok.Add(5, 3, 80); dok.Add(5, 4, 97); dok.Add(5, 5, 130);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>S3-S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Subtraction_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0,  -1); dok.Add(0, 1,   3); dok.Add(0, 2,   7); dok.Add(0, 3,  11); dok.Add(0, 4,  15); dok.Add(0, 5,  19);
                    dok.Add(1, 0, -14); dok.Add(1, 1,  -8); dok.Add(1, 2,  -2); dok.Add(1, 3,   4); dok.Add(1, 4,  10); dok.Add(1, 5,  16);
                    dok.Add(2, 0, -31); dok.Add(2, 1, -33); dok.Add(2, 2, -31); dok.Add(2, 3, -37); dok.Add(2, 4, -31); dok.Add(2, 5, -41);
                    dok.Add(3, 0, -35); dok.Add(3, 1, -38); dok.Add(3, 2, -41); dok.Add(3, 3, -46); dok.Add(3, 4, -49); dok.Add(3, 5, -52);
                    dok.Add(4, 0, -50); dok.Add(4, 1, -51); dok.Add(4, 2, -51); dok.Add(4, 3, -51); dok.Add(4, 4, -50); dok.Add(4, 5, -48);
                    dok.Add(5, 0, -59); dok.Add(5, 1, -58); dok.Add(5, 2, -55); dok.Add(5, 3, -48); dok.Add(5, 4, -33); dok.Add(5, 5,  -2);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> UnaryNegation()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 35);

                      dok.Add(0, 0, -10);   dok.Add(0, 1, -15); dok.Add(0, 2, -20); dok.Add(0, 3, -25); dok.Add(0, 4, -30); dok.Add(0, 5, -35);
                      dok.Add(1, 0,  -7);   dok.Add(1, 1, -14); dok.Add(1, 2, -21); dok.Add(1, 3, -28); dok.Add(1, 4, -35); dok.Add(1, 5, -42);
                    /*dok.Add(2, 0,   0);*/ dok.Add(2, 1,   1); dok.Add(2, 2,  -2); dok.Add(2, 3,   3); dok.Add(2, 4,  -4); dok.Add(2, 5,   5);
                      dok.Add(3, 0,  -6);   dok.Add(3, 1,  -4); dok.Add(3, 2,  -2); dok.Add(3, 3,   2); dok.Add(3, 4,   4); dok.Add(3, 5,   6);
                      dok.Add(4, 0,  -1);   dok.Add(4, 1,  -1); dok.Add(4, 2,  -2); dok.Add(4, 3,  -3); dok.Add(4, 4,  -5); dok.Add(4, 5,  -8);
                      dok.Add(5, 0,  -2);   dok.Add(5, 1,  -4); dok.Add(5, 2,  -8); dok.Add(5, 3, -16); dok.Add(5, 4, -32); dok.Add(5, 5, -64);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S3·S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Multiplication_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, 5735); dok.Add(0, 1, 5870); dok.Add(0, 2, 6005); dok.Add(0, 3, 6140); dok.Add(0, 4, 6275); dok.Add(0, 5, 6410);
                    dok.Add(1, 0, 6517); dok.Add(1, 1, 6664); dok.Add(1, 2, 6811); dok.Add(1, 3, 6958); dok.Add(1, 4, 7105); dok.Add(1, 5, 7252);
                    dok.Add(2, 0, -183); dok.Add(2, 1, -186); dok.Add(2, 2, -189); dok.Add(2, 3, -192); dok.Add(2, 4, -195); dok.Add(2, 5, -198);
                    dok.Add(3, 0, -440); dok.Add(3, 1, -440); dok.Add(3, 2, -440); dok.Add(3, 3, -440); dok.Add(3, 4, -440); dok.Add(3, 5, -440);
                    dok.Add(4, 0,  960); dok.Add(4, 1,  980); dok.Add(4, 2, 1000); dok.Add(4, 3, 1020); dok.Add(4, 4, 1040); dok.Add(4, 5, 1060);
                    dok.Add(5, 0, 6546); dok.Add(5, 1, 6672); dok.Add(5, 2, 6798); dok.Add(5, 3, 6924); dok.Add(5, 4, 7050); dok.Add(5, 5, 7176);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 3.0 : <c>S3·3.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.DictionaryOfKeys<double> Result) RightMultiplication_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 35);

                      dok.Add(0, 0, 30);   dok.Add(0, 1, 45); dok.Add(0, 2, 60); dok.Add(0, 3, 75); dok.Add(0, 4,  90); dok.Add(0, 5, 105);
                      dok.Add(1, 0, 21);   dok.Add(1, 1, 42); dok.Add(1, 2, 63); dok.Add(1, 3, 84); dok.Add(1, 4, 105); dok.Add(1, 5, 126);
                    /*dok.Add(2, 0,  0);*/ dok.Add(2, 1, -3); dok.Add(2, 2,  6); dok.Add(2, 3, -9); dok.Add(2, 4,  12); dok.Add(2, 5, -15);
                      dok.Add(3, 0, 18);   dok.Add(3, 1, 12); dok.Add(3, 2,  6); dok.Add(3, 3, -6); dok.Add(3, 4, -12); dok.Add(3, 5, -18);
                      dok.Add(4, 0,  3);   dok.Add(4, 1,  3); dok.Add(4, 2,  6); dok.Add(4, 3,  9); dok.Add(4, 4,  15); dok.Add(4, 5,  24);
                      dok.Add(5, 0,  6);   dok.Add(5, 1, 12); dok.Add(5, 2, 24); dok.Add(5, 3, 48); dok.Add(5, 4,  96); dok.Add(5, 5, 192);


                    return (3.0, dok);
                }

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.DictionaryOfKeys<double> Result) LeftMultiplication_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 35);

                      dok.Add(0, 0, 50);   dok.Add(0, 1, 75); dok.Add(0, 2, 100); dok.Add(0, 3, 125); dok.Add(0, 4, 150); dok.Add(0, 5, 175);
                      dok.Add(1, 0, 35);   dok.Add(1, 1, 70); dok.Add(1, 2, 105); dok.Add(1, 3, 140); dok.Add(1, 4, 175); dok.Add(1, 5, 210);
                    /*dok.Add(2, 0,  0);*/ dok.Add(2, 1, -5); dok.Add(2, 2,  10); dok.Add(2, 3, -15); dok.Add(2, 4,  20); dok.Add(2, 5, -25);
                      dok.Add(3, 0, 30);   dok.Add(3, 1, 20); dok.Add(3, 2,  10); dok.Add(3, 3, -10); dok.Add(3, 4, -20); dok.Add(3, 5, -30);
                      dok.Add(4, 0,  5);   dok.Add(4, 1,  5); dok.Add(4, 2,  10); dok.Add(4, 3,  15); dok.Add(4, 4,  25); dok.Add(4, 5,  40);
                      dok.Add(5, 0, 10);   dok.Add(5, 1, 20); dok.Add(5, 2,  40); dok.Add(5, 3,  80); dok.Add(5, 4, 160); dok.Add(5, 5, 320);

                    return (5.0, dok);
                }

                /// <summary>
                /// Provides the expected result of the scalar division by 4.0 : <c>S3/4.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Divisor, Mat.SparseStorages.DictionaryOfKeys<double> Result) Division_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 35);

                      dok.Add(0, 0, 2.5);    dok.Add(0, 1,  3.75); dok.Add(0, 2, 5.0);  dok.Add(0, 3,  6.25); dok.Add(0, 4,  7.5);  dok.Add(0, 5,  8.75);
                      dok.Add(1, 0, 1.75);   dok.Add(1, 1,  3.5);  dok.Add(1, 2, 5.25); dok.Add(1, 3,  7.0);  dok.Add(1, 4,  8.75); dok.Add(1, 5, 10.5);
                    /*dok.Add(2, 0, 0.0);*/  dok.Add(2, 1, -0.25); dok.Add(2, 2, 0.5);  dok.Add(2, 3, -0.75); dok.Add(2, 4,  1.0);  dok.Add(2, 5, -1.25);
                      dok.Add(3, 0, 1.5);    dok.Add(3, 1,  1.0);  dok.Add(3, 2, 0.5);  dok.Add(3, 3, -0.5);  dok.Add(3, 4, -1.0);  dok.Add(3, 5, -1.5);
                      dok.Add(4, 0, 0.25);   dok.Add(4, 1,  0.25); dok.Add(4, 2, 0.5);  dok.Add(4, 3,  0.75); dok.Add(4, 4,  1.25); dok.Add(4, 5,  2.0);
                      dok.Add(5, 0, 0.5);    dok.Add(5, 1,  1.0);  dok.Add(5, 2, 2.0);  dok.Add(5, 3,  4.0);  dok.Add(5, 4,  8.0);  dok.Add(5, 5, 16.0);


                    return (4.0, dok);
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S3·V3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of  <see cref="double"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static double[] Multiplication_V3() => new double[] { 395, 437.5, 16, -9, 58, 368 };


                //     -----     Methods

                /// <summary>
                /// Provides an array representation of the matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A two dimensional array of <see cref="double"/> representing the matrix. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    double[,] values = new double[,]
                    {
                        {    10,    15,    20,    25,    30,    35    },
                        {     7,    14,    21,    28,    35,    42    },
                        {     0,    -1,     2,    -3,     4,    -5    },
                        {     6,     4,     2,    -2,    -4,    -6    },
                        {     1,     1,     2,     3,     5,     8    },
                        {     2,     4,     8,    16,    32,    64    },
                    };

                    return [values];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse storage <see cref="S3"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.SparseStorages.CompressedColumn{T}"/>. </returns>
                internal static Mat.SparseStorages.DictionaryOfKeys<double> CreateS3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 35);

                      dok.Add(0, 0, 10.0);   dok.Add(0, 1, 15.0); dok.Add(0, 2, 20.0); dok.Add(0, 3, 25.0); dok.Add(0, 4, 30.0); dok.Add(0, 5, 35.0);
                      dok.Add(1, 0,  7.0);   dok.Add(1, 1, 14.0); dok.Add(1, 2, 21.0); dok.Add(1, 3, 28.0); dok.Add(1, 4, 35.0); dok.Add(1, 5, 42.0);
                    /*dok.Add(2, 0,  0.0);*/ dok.Add(2, 1, -1.0); dok.Add(2, 2,  2.0); dok.Add(2, 3, -3.0); dok.Add(2, 4,  4.0); dok.Add(2, 5, -5.0);
                      dok.Add(3, 0,  6.0);   dok.Add(3, 1,  4.0); dok.Add(3, 2,  2.0); dok.Add(3, 3, -2.0); dok.Add(3, 4, -4.0); dok.Add(3, 5, -6.0);
                      dok.Add(4, 0,  1.0);   dok.Add(4, 1,  1.0); dok.Add(4, 2,  2.0); dok.Add(4, 3,  3.0); dok.Add(4, 4,  5.0); dok.Add(4, 5,  8.0);
                      dok.Add(5, 0,  2.0);   dok.Add(5, 1,  4.0); dok.Add(5, 2,  8.0); dok.Add(5, 3, 16.0); dok.Add(5, 4, 32.0); dok.Add(5, 5, 64.0);

                    return dok;
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse storage S4 represents a [6x6] matrix. </remarks>
            internal static class S4
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S4"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.DictionaryOfKeys<double> _s4 = CreateS4();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S4"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s4];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S4"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS4()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the storage <see cref="S4"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [6];

                /// <summary>
                /// Provides the expected number of columns of the storage <see cref="S4"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [6];

                /// <summary>
                /// Provides the expected number of entries of the storage <see cref="S4"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [36];

                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S4<sup>t</sup>·S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> TransposeMultiply_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, 676); dok.Add(0, 1,  887); dok.Add(0, 2, 1395); dok.Add(0, 3, 1817); dok.Add(0, 4, 3232); dok.Add(0, 5, 5178);
                    dok.Add(1, 0, 702); dok.Add(1, 1,  924); dok.Add(1, 2, 1450); dok.Add(1, 3, 1884); dok.Add(1, 4, 3334); dok.Add(1, 5, 5316);
                    dok.Add(2, 0, 728); dok.Add(2, 1,  961); dok.Add(2, 2, 1505); dok.Add(2, 3, 1951); dok.Add(2, 4, 3436); dok.Add(2, 5, 5454);
                    dok.Add(3, 0, 754); dok.Add(3, 1,  998); dok.Add(3, 2, 1560); dok.Add(3, 3, 2018); dok.Add(3, 4, 3538); dok.Add(3, 5, 5592);
                    dok.Add(4, 0, 780); dok.Add(4, 1, 1035); dok.Add(4, 2, 1615); dok.Add(4, 3, 2085); dok.Add(4, 4, 3640); dok.Add(4, 5, 5730);
                    dok.Add(5, 0, 806); dok.Add(5, 1, 1072); dok.Add(5, 2, 1670); dok.Add(5, 3, 2152); dok.Add(5, 4, 3742); dok.Add(5, 5, 5868);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S4·S3<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> MultiplyTranspose_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, 1910); dok.Add(0, 1, 2107); dok.Add(0, 2,  -48); dok.Add(0, 3, -44); dok.Add(0, 4,  294); dok.Add(0, 5, 1902);
                    dok.Add(1, 0, 3260); dok.Add(1, 1, 3577); dok.Add(1, 2,  -78); dok.Add(1, 3, -44); dok.Add(1, 4,  494); dok.Add(1, 5, 3162);
                    dok.Add(2, 0, 4610); dok.Add(2, 1, 5047); dok.Add(2, 2, -108); dok.Add(2, 3, -44); dok.Add(2, 4,  694); dok.Add(2, 5, 4422);
                    dok.Add(3, 0, 5960); dok.Add(3, 1, 6517); dok.Add(3, 2, -138); dok.Add(3, 3, -44); dok.Add(3, 4,  894); dok.Add(3, 5, 5682);
                    dok.Add(4, 0, 7310); dok.Add(4, 1, 7987); dok.Add(4, 2, -168); dok.Add(4, 3, -44); dok.Add(4, 4, 1094); dok.Add(4, 5, 6942);
                    dok.Add(5, 0, 8660); dok.Add(5, 1, 9457); dok.Add(5, 2, -198); dok.Add(5, 3, -44); dok.Add(5, 4, 1294); dok.Add(5, 5, 8202);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S4<sup>t</sup>·V3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of <see cref="double"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static double[] TransposeMultiply_V3() => new double[] { 641.5, 658, 674.5, 691, 707.5, 724 };


                /// <summary>
                /// Provides the expected result of the transposition : <c>S4<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Transpose()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, 11); dok.Add(0, 1, 21); dok.Add(0, 2, 31); dok.Add(0, 3, 41); dok.Add(0, 4, 51); dok.Add(0, 5, 61);
                    dok.Add(1, 0, 12); dok.Add(1, 1, 22); dok.Add(1, 2, 32); dok.Add(1, 3, 42); dok.Add(1, 4, 52); dok.Add(1, 5, 62);
                    dok.Add(2, 0, 13); dok.Add(2, 1, 23); dok.Add(2, 2, 33); dok.Add(2, 3, 43); dok.Add(2, 4, 53); dok.Add(2, 5, 63);
                    dok.Add(3, 0, 14); dok.Add(3, 1, 24); dok.Add(3, 2, 34); dok.Add(3, 3, 44); dok.Add(3, 4, 54); dok.Add(3, 5, 64);
                    dok.Add(4, 0, 15); dok.Add(4, 1, 25); dok.Add(4, 2, 35); dok.Add(4, 3, 45); dok.Add(4, 4, 55); dok.Add(4, 5, 65);
                    dok.Add(5, 0, 16); dok.Add(5, 1, 26); dok.Add(5, 2, 36); dok.Add(5, 3, 46); dok.Add(5, 4, 56); dok.Add(5, 5, 66);

                    return dok;
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>S4+S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Addition_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, 21); dok.Add(0, 1, 27); dok.Add(0, 2, 33); dok.Add(0, 3, 39); dok.Add(0, 4, 45); dok.Add(0, 5,  51);
                    dok.Add(1, 0, 28); dok.Add(1, 1, 36); dok.Add(1, 2, 44); dok.Add(1, 3, 52); dok.Add(1, 4, 60); dok.Add(1, 5,  68);
                    dok.Add(2, 0, 31); dok.Add(2, 1, 31); dok.Add(2, 2, 35); dok.Add(2, 3, 31); dok.Add(2, 4, 39); dok.Add(2, 5,  31);
                    dok.Add(3, 0, 47); dok.Add(3, 1, 46); dok.Add(3, 2, 45); dok.Add(3, 3, 42); dok.Add(3, 4, 41); dok.Add(3, 5,  40);
                    dok.Add(4, 0, 52); dok.Add(4, 1, 53); dok.Add(4, 2, 55); dok.Add(4, 3, 57); dok.Add(4, 4, 60); dok.Add(4, 5,  64);
                    dok.Add(5, 0, 63); dok.Add(5, 1, 66); dok.Add(5, 2, 71); dok.Add(5, 3, 80); dok.Add(5, 4, 97); dok.Add(5, 5, 130);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>S4-S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Subtraction_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0,  1); dok.Add(0, 1, -3); dok.Add(0, 2, -7); dok.Add(0, 3, -11); dok.Add(0, 4, -15); dok.Add(0, 5, -19);
                    dok.Add(1, 0, 14); dok.Add(1, 1,  8); dok.Add(1, 2,  2); dok.Add(1, 3,  -4); dok.Add(1, 4, -10); dok.Add(1, 5, -16);
                    dok.Add(2, 0, 31); dok.Add(2, 1, 33); dok.Add(2, 2, 31); dok.Add(2, 3,  37); dok.Add(2, 4,  31); dok.Add(2, 5,  41);
                    dok.Add(3, 0, 35); dok.Add(3, 1, 38); dok.Add(3, 2, 41); dok.Add(3, 3,  46); dok.Add(3, 4,  49); dok.Add(3, 5,  52);
                    dok.Add(4, 0, 50); dok.Add(4, 1, 51); dok.Add(4, 2, 51); dok.Add(4, 3,  51); dok.Add(4, 4,  50); dok.Add(4, 5,  48);
                    dok.Add(5, 0, 59); dok.Add(5, 1, 58); dok.Add(5, 2, 55); dok.Add(5, 3,  48); dok.Add(5, 4,  33); dok.Add(5, 5,   2);

                    return dok;
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> UnaryNegation()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, -11); dok.Add(0, 1, -12); dok.Add(0, 2, -13); dok.Add(0, 3, -14); dok.Add(0, 4, -15); dok.Add(0, 5, -16);
                    dok.Add(1, 0, -21); dok.Add(1, 1, -22); dok.Add(1, 2, -23); dok.Add(1, 3, -24); dok.Add(1, 4, -25); dok.Add(1, 5, -26);
                    dok.Add(2, 0, -31); dok.Add(2, 1, -32); dok.Add(2, 2, -33); dok.Add(2, 3, -34); dok.Add(2, 4, -35); dok.Add(2, 5, -36);
                    dok.Add(3, 0, -41); dok.Add(3, 1, -42); dok.Add(3, 2, -43); dok.Add(3, 3, -44); dok.Add(3, 4, -45); dok.Add(3, 5, -46);
                    dok.Add(4, 0, -51); dok.Add(4, 1, -52); dok.Add(4, 2, -53); dok.Add(4, 3, -54); dok.Add(4, 4, -55); dok.Add(4, 5, -56);
                    dok.Add(5, 0, -61); dok.Add(5, 1, -62); dok.Add(5, 2, -63); dok.Add(5, 3, -64); dok.Add(5, 4, -65); dok.Add(5, 5, -66);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S4·S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.DictionaryOfKeys<double> Multiplication_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0,  325); dok.Add(0, 1,  455); dok.Add(0, 2,  684); dok.Add(0, 3,  845); dok.Add(0, 4, 1333); dok.Add(0, 5, 1884);
                    dok.Add(1, 0,  585); dok.Add(1, 1,  825); dok.Add(1, 2, 1234); dok.Add(1, 3, 1515); dok.Add(1, 4, 2353); dok.Add(1, 5, 3264);
                    dok.Add(2, 0,  845); dok.Add(2, 1, 1195); dok.Add(2, 2, 1784); dok.Add(2, 3, 2185); dok.Add(2, 4, 3373); dok.Add(2, 5, 4644);
                    dok.Add(3, 0, 1105); dok.Add(3, 1, 1565); dok.Add(3, 2, 2334); dok.Add(3, 3, 2855); dok.Add(3, 4, 4393); dok.Add(3, 5, 6024);
                    dok.Add(4, 0, 1365); dok.Add(4, 1, 1935); dok.Add(4, 2, 2884); dok.Add(4, 3, 3525); dok.Add(4, 4, 5413); dok.Add(4, 5, 7404);
                    dok.Add(5, 0, 1625); dok.Add(5, 1, 2305); dok.Add(5, 2, 3434); dok.Add(5, 3, 4195); dok.Add(5, 4, 6433); dok.Add(5, 5, 8784);

                    return dok;
                }


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 7.0 : <c>S4·7.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.DictionaryOfKeys<double> Result) RightMultiplication_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0,  77); dok.Add(0, 1,  84); dok.Add(0, 2,  91); dok.Add(0, 3,  98); dok.Add(0, 4, 105); dok.Add(0, 5, 112);
                    dok.Add(1, 0, 147); dok.Add(1, 1, 154); dok.Add(1, 2, 161); dok.Add(1, 3, 168); dok.Add(1, 4, 175); dok.Add(1, 5, 182);
                    dok.Add(2, 0, 217); dok.Add(2, 1, 224); dok.Add(2, 2, 231); dok.Add(2, 3, 238); dok.Add(2, 4, 245); dok.Add(2, 5, 252);
                    dok.Add(3, 0, 287); dok.Add(3, 1, 294); dok.Add(3, 2, 301); dok.Add(3, 3, 308); dok.Add(3, 4, 315); dok.Add(3, 5, 322);
                    dok.Add(4, 0, 357); dok.Add(4, 1, 364); dok.Add(4, 2, 371); dok.Add(4, 3, 378); dok.Add(4, 4, 385); dok.Add(4, 5, 392);
                    dok.Add(5, 0, 427); dok.Add(5, 1, 434); dok.Add(5, 2, 441); dok.Add(5, 3, 448); dok.Add(5, 4, 455); dok.Add(5, 5, 462);

                    return (7.0, dok);
                }

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 2.0 : <c>2.0·S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.DictionaryOfKeys<double> Result) LeftMultiplication_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0,  22); dok.Add(0, 1,  24); dok.Add(0, 2,  26); dok.Add(0, 3,  28); dok.Add(0, 4,  30); dok.Add(0, 5,  32);
                    dok.Add(1, 0,  42); dok.Add(1, 1,  44); dok.Add(1, 2,  46); dok.Add(1, 3,  48); dok.Add(1, 4,  50); dok.Add(1, 5,  52);
                    dok.Add(2, 0,  62); dok.Add(2, 1,  64); dok.Add(2, 2,  66); dok.Add(2, 3,  68); dok.Add(2, 4,  70); dok.Add(2, 5,  72);
                    dok.Add(3, 0,  82); dok.Add(3, 1,  84); dok.Add(3, 2,  86); dok.Add(3, 3,  88); dok.Add(3, 4,  90); dok.Add(3, 5,  92);
                    dok.Add(4, 0, 102); dok.Add(4, 1, 104); dok.Add(4, 2, 106); dok.Add(4, 3, 108); dok.Add(4, 4, 110); dok.Add(4, 5, 112);
                    dok.Add(5, 0, 122); dok.Add(5, 1, 124); dok.Add(5, 2, 126); dok.Add(5, 3, 128); dok.Add(5, 4, 130); dok.Add(5, 5, 132);

                    return (2.0, dok);
                }

                /// <summary>
                /// Provides the expected result of the scalar division by 2.0 : <c>S4/2.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Divisor, Mat.SparseStorages.DictionaryOfKeys<double> Result) Division_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0,  5.5); dok.Add(0, 1,  6); dok.Add(0, 2,  6.5); dok.Add(0, 3,  7); dok.Add(0, 4,  7.5); dok.Add(0, 5,  8);
                    dok.Add(1, 0, 10.5); dok.Add(1, 1, 11); dok.Add(1, 2, 11.5); dok.Add(1, 3, 12); dok.Add(1, 4, 12.5); dok.Add(1, 5, 13);
                    dok.Add(2, 0, 15.5); dok.Add(2, 1, 16); dok.Add(2, 2, 16.5); dok.Add(2, 3, 17); dok.Add(2, 4, 17.5); dok.Add(2, 5, 18);
                    dok.Add(3, 0, 20.5); dok.Add(3, 1, 21); dok.Add(3, 2, 21.5); dok.Add(3, 3, 22); dok.Add(3, 4, 22.5); dok.Add(3, 5, 23);
                    dok.Add(4, 0, 25.5); dok.Add(4, 1, 26); dok.Add(4, 2, 26.5); dok.Add(4, 3, 27); dok.Add(4, 4, 27.5); dok.Add(4, 5, 28);
                    dok.Add(5, 0, 30.5); dok.Add(5, 1, 31); dok.Add(5, 2, 31.5); dok.Add(5, 3, 32); dok.Add(5, 4, 32.5); dok.Add(5, 5, 33);

                    return (2.0, dok);
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S4·V3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of <see cref="double"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static double[] Multiplication_V3() => new double[] { 227.5, 392.5, 557.5, 722.5, 887.5, 1052.5 };


                //     -----     Methods

                /// <summary>
                /// Provides an array representation of the matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A two dimensional array of <see cref="double"/> representing the matrix. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    double[,] values = new double[,]
                    {
                        {    11,     12,     13,     14,     15,     16    },
                        {    21,     22,     23,     24,     25,     26    },
                        {    31,     32,     33,     34,     35,     36    },
                        {    41,     42,     43,     44,     45,     46    },
                        {    51,     52,     53,     54,     55,     56    },
                        {    61,     62,     63,     64,     65,     66    },
                    };

                    return [values];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse storage <see cref="S4"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.SparseStorages.CompressedColumn{T}"/>. </returns>
                internal static Mat.SparseStorages.DictionaryOfKeys<double> CreateS4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 36);

                    dok.Add(0, 0, 11); dok.Add(0, 1, 12); dok.Add(0, 2, 13); dok.Add(0, 3, 14); dok.Add(0, 4, 15); dok.Add(0, 5, 16);
                    dok.Add(1, 0, 21); dok.Add(1, 1, 22); dok.Add(1, 2, 23); dok.Add(1, 3, 24); dok.Add(1, 4, 25); dok.Add(1, 5, 26);
                    dok.Add(2, 0, 31); dok.Add(2, 1, 32); dok.Add(2, 2, 33); dok.Add(2, 3, 34); dok.Add(2, 4, 35); dok.Add(2, 5, 36);
                    dok.Add(3, 0, 41); dok.Add(3, 1, 42); dok.Add(3, 2, 43); dok.Add(3, 3, 44); dok.Add(3, 4, 45); dok.Add(3, 5, 46);
                    dok.Add(4, 0, 51); dok.Add(4, 1, 52); dok.Add(4, 2, 53); dok.Add(4, 3, 54); dok.Add(4, 4, 55); dok.Add(4, 5, 56);
                    dok.Add(5, 0, 61); dok.Add(5, 1, 62); dok.Add(5, 2, 63); dok.Add(5, 3, 64); dok.Add(5, 4, 65); dok.Add(5, 5, 66);

                    return dok;
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>, for <see cref="DataClasses"/>.
            /// </summary>
            /// <remarks> The sparse storage S5 represents a [3x5] matrix. </remarks>
            internal static class S5
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S5"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.DictionaryOfKeys<double> _s5 = CreateS5();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s5];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/> of type <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS5()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the storage <see cref="S5"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [3];

                /// <summary>
                /// Provides the expected number of columns of the storage <see cref="S5"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [5];


                /// <summary>
                /// Provides the expected number of entries of the storage <see cref="S5"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [4];


                /// <summary>
                /// Provides the expected list of values of the storage.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> List of <see cref="double"/> values containing the non-zero values, arranged column by column. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Storage()
                {
                    Dictionary<(int, int), double> dictionary = new Dictionary<(int, int), double>
                    {
                        { (0, 0), 1.0 },
                        { (0, 3), -2.0 },
                        { (2, 0), 2.0 },
                        { (2, 4), 5.5 }
                    };

                    return [dictionary];
                }


                //     -----     Public Methods

                /// <summary>
                /// Provides the information to insert a component in the sparse storage and the expected result. 
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted between two existing components.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after insertion of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_01()
                {
                    int rowIndex = 1;
                    int columnIndex = 0;
                    double value = -3.2;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 5);

                    result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    result.Add(1, 0, -3.2);  /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                    result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse storage and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted after an existing component and at the end of the column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after insertion of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_02()
                {
                    int rowIndex = 2;
                    int columnIndex = 3;
                    double value = 0.7;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/   result.Add(2, 3,  0.7);    result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse storage and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted after an existing component and in the middle of the column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after insertion of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_03()
                {
                    int rowIndex = 1;
                    int columnIndex = 3;
                    double value = 0.3;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/   result.Add(1, 3,  0.3);   /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse storage and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted before an existing component and in the start of the column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after insertion of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_04()
                {
                    int rowIndex = 0;
                    int columnIndex = 4;
                    double value = 2.2;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);     result.Add(0, 4, 2.2);
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse storage and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted in an empty column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after insertion of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_05()
                {
                    int rowIndex = 1;
                    int columnIndex = 2;
                    double value = 1.5;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/   result.Add(1, 2, 1.5);   /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse storage and the expected result.
                /// </summary>
                /// <remarks>
                /// The component has a zero is zero.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after insertion of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_06()
                {
                    int rowIndex = 0;
                    int columnIndex = 1;
                    double value = 0.0;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 5);

                      result.Add(0, 0, 1.0);     result.Add(0, 1, 0.0);   /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse storage and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The inserted component already exists.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after insertion of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_07()
                {
                    int rowIndex = 0;
                    int columnIndex = 3;
                    double value = 8.0;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }


                /// <summary>
                /// Provides the information to replace a component in the sparse storage.
                /// </summary>
                /// <remarks>
                /// Case : The value is non-zero and the component already exists.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after replacement of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Replace__Case_01()
                {
                    int rowIndex = 2;
                    int columnIndex = 0;
                    double value = -7.3;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      result.Add(0, 0, 1.0);    /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/  /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, -7.3);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to replace a component in the sparse storage.
                /// </summary>
                /// <remarks>
                /// Case : The value is non-zero and the component does not exist.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after replacement of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Replace__Case_02()
                {
                    int rowIndex = 1;
                    int columnIndex = 1;
                    double value = 5.1;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to replace a component in the sparse storage.
                /// </summary>
                /// <remarks>
                /// Case : The value is zero and the component already exists.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after replacement of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Replace__Case_03()
                {
                    int rowIndex = 0;
                    int columnIndex = 3;
                    double value = 0.0;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, 0.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3, 0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3, 0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to replace a component in the sparse storage.
                /// </summary>
                /// <remarks>
                /// Case : The value is zero and the component does not exist.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse storage after replacement of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Replace__Case_04()
                {
                    int rowIndex = 1;
                    int columnIndex = 2;
                    double value = 0.0;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }


                /// <summary>
                /// Provides the information to remove a component in the sparse storage.
                /// </summary>
                /// <remarks>
                /// Case : The component already exists and is before another component.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Expected sparse storage after removal of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Remove__Case_01()
                {
                    int rowIndex = 0;
                    int columnIndex = 0;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 3);

                    /*result.Add(0, 0, 0.0);*/ /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, result];
                }

                /// <summary>
                /// Provides the information to remove a component in the sparse storage.
                /// </summary>
                /// <remarks>
                /// Case : The component already exists and is after another component.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Expected sparse storage after removal of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Remove__Case_02()
                {
                    int rowIndex = 2;
                    int columnIndex = 0;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                    /*result.Add(2, 0, 0.0);*/ /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, result];
                }

                /// <summary>
                /// Provides the information to remove a component in the sparse storage.
                /// </summary>
                /// <remarks>
                /// Case : The component already exists and is alone in the column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Expected sparse storage after removal of the component in the storage <see cref="S5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Remove__Case_03()
                {
                    int rowIndex = 0;
                    int columnIndex = 3;

                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> result = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/ /*result.Add(0, 3, 0.0);*/ /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3, 0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3, 0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, result];
                }


                /// <summary>
                /// Provides the information to evaluate whether a component is contained in the sparse storage.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Two-dimensional array evaluating whether a component at the given row and column has an entry in the sparse storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Contains()
                {
                    return [new bool[3, 5] { 
                        {    true,    false,    false,     true,    false}, 
                        {   false,    false,    false,    false,    false},
                        {    true,    false,    false,    false,     true}
                    }];
                }


                /// <summary>
                /// Provides the value of each components of the represented matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Two-dimensional array containing the values of zero and non-zero components. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    return [new double[3, 5] {
                        {    1.0,    0.0,    0.0,    -2.0,    0.0},
                        {    0.0,    0.0,    0.0,    0.0,     0.0},
                        {    2.0,    0.0,    0.0,    0.0,     5.5}
                    }];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse storage <see cref="S5"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>. </returns>
                internal static Mat.SparseStorages.DictionaryOfKeys<double> CreateS5()
                {
                    int rowCount = 3;
                    int columnCount = 5;

                    Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 4);

                      dok.Add(0, 0, 1.0);   /*dok.Add(0, 1, 0.0);*/ /*dok.Add(0, 2, 0.0);*/   dok.Add(0, 3, -2.0);   /*dok.Add(0, 4, 0.0);*/
                    /*dok.Add(1, 0, 0.0);*/ /*dok.Add(1, 1, 0.0);*/ /*dok.Add(1, 2, 0.0);*/ /*dok.Add(1, 3,  0.0);*/ /*dok.Add(1, 4, 0.0);*/
                      dok.Add(2, 0, 2.0);   /*dok.Add(2, 1, 0.0);*/ /*dok.Add(2, 2, 0.0);*/ /*dok.Add(2, 3,  0.0);*/   dok.Add(2, 4, 5.5);

                    return dok;
                }

                #endregion
            }
        }

        #endregion

        #region Data Classes for Parametrised Tests

        internal static class DataClasses
        {
            #region For Properties

            /// <summary>
            /// Class data for <see cref="Property__RowCount(Mat.SparseStorages.CompressedColumn{double}, int)"/>.
            /// </summary>
            internal class RowCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S1.Readable, DataStorages.S1.RowCount },
                        { DataStorages.S1.Readable, DataStorages.S2.RowCount },
                        { DataStorages.S3.Readable, DataStorages.S3.RowCount },
                        { DataStorages.S4.Readable, DataStorages.S4.RowCount },
                        { DataStorages.S5.Readable, DataStorages.S5.RowCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Property__ColumnCount(Mat.SparseStorages.CompressedColumn{double}, int)"/>.
            /// </summary>
            internal class ColumnCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S1.Readable, DataStorages.S1.ColumnCount },
                        { DataStorages.S1.Readable, DataStorages.S2.ColumnCount },
                        { DataStorages.S3.Readable, DataStorages.S3.ColumnCount },
                        { DataStorages.S4.Readable, DataStorages.S4.ColumnCount },
                        { DataStorages.S5.Readable, DataStorages.S5.ColumnCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Property__Count(Mat.SparseStorages.CompressedColumn{double}, int)"/>.
            /// </summary>
            internal class Count : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S1.Readable, DataStorages.S1.Count },
                        { DataStorages.S1.Readable, DataStorages.S2.Count },
                        { DataStorages.S3.Readable, DataStorages.S3.Count },
                        { DataStorages.S4.Readable, DataStorages.S4.Count },
                        { DataStorages.S5.Readable, DataStorages.S5.Count },
                    };
            }


            /// <summary>
            /// Class data for <see cref="Property__Values(Mat.SparseStorages.CompressedColumn{double}, List{double})"/>.
            /// </summary>
            internal class Storage : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Readable, DataStorages.S5.Storage },
                    };
            }

            #endregion

            #region For Constructors

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Constructor__Int_Int_Int(int, int, int)"/>.
            /// </summary>
            internal class Constructor__Int_Int_Int : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.RowCount, DataStorages.S5.ColumnCount, DataStorages.S5.Count },
                    };
            }

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Constructor__CompressedColumn(Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Constructor__DictionaryOfKeys : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Readable },
                    };
            }

            #endregion

            #region For Public Methods

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Add__Int_Int_T(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/> and 
            /// <see cref="CompressedColumn.Add__Int_Int_T_Bool__Check(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/> and
            /// <see cref="CompressedColumn.Add__Int_Int_T_Bool__NoCheck(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Add : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Writable,  DataStorages.S5.Add__Case_01 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Add__Case_02 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Add__Case_03 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Add__Case_04 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Add__Case_05 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Add__Case_06 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Add__Case_07 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Replace__Int_Int_T(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/> and 
            /// <see cref="CompressedColumn.Replace__Int_Int_T_Bool__Check(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/> and
            /// <see cref="CompressedColumn.Replace__Int_Int_T_Bool__NoCheck(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Replace : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Writable,  DataStorages.S5.Replace__Case_01 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Replace__Case_02 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Replace__Case_03 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Replace__Case_04 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Remove__Int_Int(Mat.SparseStorages.CompressedColumn{double}, int, int, Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Remove : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Writable,  DataStorages.S5.Remove__Case_01 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Remove__Case_02 },
                        { DataStorages.S5.Writable,  DataStorages.S5.Remove__Case_03 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Contains__Int_Int(Mat.SparseStorages.CompressedColumn{double}, bool[,])"/>.
            /// </summary>
            internal class Contains : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Readable,  DataStorages.S5.Contains },
                    };
            }


            /// <summary>
            /// Class data for <see cref="CompressedColumn.Property__Indexer__Int_Int(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.TryGet__Int_Int(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.ToArray(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.ToRowMajorArray(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.ToColumnMajorArray(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.RowVectors(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.ColumnVectors(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// </summary>
            internal class ToArray : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S1.Readable,  DataStorages.S1.ToArray },
                        { DataStorages.S2.Readable,  DataStorages.S2.ToArray },
                        { DataStorages.S3.Readable,  DataStorages.S3.ToArray },
                        { DataStorages.S4.Readable,  DataStorages.S4.ToArray },
                        { DataStorages.S5.Readable,  DataStorages.S5.ToArray },
                    };
            }

            #endregion
        }

        #endregion
    }
}
