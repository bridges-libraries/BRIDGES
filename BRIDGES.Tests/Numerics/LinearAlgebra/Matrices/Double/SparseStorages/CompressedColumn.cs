using System;
using System.Collections.Generic;

using Xunit;

using Mat = BRIDGES.Numerics.LinearAlgebra.Matrices;
using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Matrices.Double.SparseStorages
{
    public class CompressedColumn
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.SparseStorage{T}.RowCount"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected number of row. </param>
        [Theory(DisplayName = "Prop. RowCount")]
        [ClassData(typeof(DataClasses.RowCount))]
        public void Property__RowCount(Mat.SparseStorages.CompressedColumn<double> storage, int expected)
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
        public void Property__ColumnCount(Mat.SparseStorages.CompressedColumn<double> storage, int expected)
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
        public void Property__Count(Mat.SparseStorages.CompressedColumn<double> storage, int expected)
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
        public void Property__Indexer__Int_Int(Mat.SparseStorages.CompressedColumn<double> storage, double[,] values)
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
        /// Tests the property <see cref="Mat.SparseStorages.CompressedColumn{T}.Values"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected list of values. </param>
        [Theory(DisplayName = "Prop. Values")]
        [ClassData(typeof(DataClasses.Values))]
        public void Property__Values(Mat.SparseStorages.CompressedColumn<double> storage, List<double> expected)
        {
            // Arrange

            //Act
            List<double> result = storage.Values;

            // Assert
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.CompressedColumn{T}.RowIndices"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected list of row indices. </param>
        [Theory(DisplayName = "Prop. RowIndices")]
        [ClassData(typeof(DataClasses.RowIndices))]
        public void Property__RowIndices(Mat.SparseStorages.CompressedColumn<double> storage, List<int> expected)
        {
            // Arrange

            //Act
            List<int> result = storage.RowIndices;

            // Assert
            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.CompressedColumn{T}.ColumnPointers"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected array of column pointers. </param>
        [Theory(DisplayName = "Prop. ColumnPointers")]
        [ClassData(typeof(DataClasses.ColumnPointers))]
        public void Property__ColumnPointers(Mat.SparseStorages.CompressedColumn<double> storage, int[] expected)
        {
            // Arrange

            //Act
            int[] result = storage.ColumnPointers;

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        #endregion

        #region Tests : Constructors

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.CompressedColumn{T}.CompressedColumn(int, int, int)"/>.
        /// </summary>
        /// <param name="rowCount"> Expected number of row. </param>
        /// <param name="columnCount"> Expected number of column. </param>
        /// <param name="capacity"> Expected number of non zero values. </param>
        [Theory(DisplayName = "CompressedColumn<T>(int, int, int)")]
        [ClassData(typeof(DataClasses.Constructor__Int_Int_Int))]
        public void Constructor__Int_Int_Int(int rowCount, int columnCount, int capacity)
        {
            // Arrange

            //Act
            Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, capacity);

            // Assert
            Assert.Equal(rowCount, result.RowCount);
            Assert.Equal(columnCount, result.ColumnCount);
            Assert.Equal(0, result.Count);

            Assert.Empty(result.Values);
            Assert.Empty(result.RowIndices);
            Assert.Equal(columnCount + 1, result.ColumnPointers.Length);
        }

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.CompressedColumn{T}.CompressedColumn(int, int, ref List{T}, ref List{int}, ref int[])"/>.
        /// </summary>
        /// <param name="rowCount"> Expected number of row. </param>
        /// <param name="columnCount"> Expected number of column. </param>
        /// <param name="values"> List of values. </param>
        /// <param name="rowIndices"> List of row indices. </param>
        /// <param name="columnPointers"> Array of column pointers. </param>
        [Theory(DisplayName = "CompressedColumn<T>(int, int, ref List<double>, ref List<int>, ref int[])")]
        [ClassData(typeof(DataClasses.Constructor__Int_Int_ListOfT_ListOfInt_ArrayofInt))]
        public void Constructor__Int_Int_ListOfT_ListOfInt_ArrayofInt(int rowCount, int columnCount, List<double> values, List<int> rowIndices, int[] columnPointers)
        {
            // Arrange

            //Act
            Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

            // Assert
            Assert.Equal(rowCount, result.RowCount);
            Assert.Equal(columnCount, result.ColumnCount);

            Assert.Equal(values.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(values[i], result.Values[i]);
                Assert.Equal(rowIndices[i], result.RowIndices[i]);
            }

            Assert.Equal(columnCount + 1, result.ColumnPointers.Length);
            for (int i = 0; i < result.ColumnPointers.Length; i++)
            {
                Assert.Equal(columnPointers[i], result.ColumnPointers[i]);
            }

            Assert.Same(values, result.Values);
            Assert.Same(rowIndices, result.RowIndices);
            Assert.Same(columnPointers, result.ColumnPointers);
        }

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.CompressedColumn{T}.CompressedColumn(Mat.SparseStorages.CompressedColumn{T})"/>.
        /// </summary>
        /// <param name="expected"> Sparse storage to deep copy. </param>
        [Theory(DisplayName = "CompressedColumn<T>(CompressedColumn<T>)")]
        [ClassData(typeof(DataClasses.Constructor__CompressedColumn))]
        public void Constructor__CompressedColumn(Mat.SparseStorages.CompressedColumn<double> expected)
        {
            // Arrange

            //Act
            Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(expected);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);

            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expected.Values[i], result.Values[i]);
                Assert.Equal(expected.RowIndices[i], result.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Length, result.ColumnPointers.Length);
            for (int i = 0; i < result.ColumnPointers.Length; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], result.ColumnPointers[i]);
            }

            Assert.NotSame(expected.Values, result.Values);
            Assert.NotSame(expected.RowIndices, result.RowIndices);
            Assert.NotSame(expected.ColumnPointers, result.ColumnPointers);
        }

        #endregion

        #region Tests : Public Methods

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.TryGet(int, int, out T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "TryGet(int, int)")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void TryGet__Int_Int(Mat.SparseStorages.CompressedColumn<double> storage, double[,] values)
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
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.Insert(int, int, T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected">  Expected sparse storage after insertion of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Insert(int, int, T)")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T(Mat.SparseStorages.CompressedColumn<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.CompressedColumn<double> expected)
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
            for (int i = 0; i < storage.Count; i++)
            {
                Assert.Equal(expected.Values[i], storage.Values[i]);
                Assert.Equal(expected.RowIndices[i], storage.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Length, storage.ColumnPointers.Length);
            for (int i = 0; i < storage.ColumnPointers.Length; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], storage.ColumnPointers[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.Insert(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected">  Expected sparse storage after insertion of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Insert(int, int, T, Bool) - Check")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T_Bool__Check(Mat.SparseStorages.CompressedColumn<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.CompressedColumn<double> expected)
        {
            // Arrange
            bool shouldInsert = !(storage.Contains(rowIndex, columnIndex) || value == 0.0);

            if (!shouldInsert) { expected = new Mat.SparseStorages.CompressedColumn<double>(storage); }

            // Act
            bool isInserted = storage.Add(rowIndex, columnIndex, value, true);

            // Assert
            Assert.Equal(shouldInsert, isInserted);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            for (int i = 0; i < storage.Count; i++)
            {
                Assert.Equal(expected.Values[i], storage.Values[i]);
                Assert.Equal(expected.RowIndices[i], storage.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Length, storage.ColumnPointers.Length);
            for (int i = 0; i < storage.ColumnPointers.Length; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], storage.ColumnPointers[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.Insert(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected">  Expected sparse storage after insertion of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Insert(int, int, T, Bool) - No Check")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T_Bool__NoCheck(Mat.SparseStorages.CompressedColumn<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.CompressedColumn<double> expected)
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
            for (int i = 0; i < storage.Count; i++)
            {
                Assert.Equal(expected.Values[i], storage.Values[i]);
                Assert.Equal(expected.RowIndices[i], storage.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Length, storage.ColumnPointers.Length);
            for (int i = 0; i < storage.ColumnPointers.Length; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], storage.ColumnPointers[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.Replace(int, int, T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Replace(int, int, T)")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T(Mat.SparseStorages.CompressedColumn<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.CompressedColumn<double> expected)
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
            for (int i = 0; i < storage.Count; i++)
            {
                Assert.Equal(expected.Values[i], storage.Values[i]);
                Assert.Equal(expected.RowIndices[i], storage.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Length, storage.ColumnPointers.Length);
            for (int i = 0; i < storage.ColumnPointers.Length; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], storage.ColumnPointers[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.Replace(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/>. </param>
        [Theory(DisplayName = "Replace(int, int, T, Bool) - Check")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T_Bool__Check(Mat.SparseStorages.CompressedColumn<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.CompressedColumn<double> expected)
        {
            // Arrange
            bool shouldReplace = storage.Contains(rowIndex, columnIndex) && value != 0.0;

            if (!shouldReplace) { expected = new Mat.SparseStorages.CompressedColumn<double>(storage); }

            // Act
            bool isReplaced = storage.Replace(rowIndex, columnIndex, value, true);

            // Assert
            Assert.Equal(shouldReplace, isReplaced);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            for (int i = 0; i < storage.Count; i++)
            {
                Assert.Equal(expected.Values[i], storage.Values[i]);
                Assert.Equal(expected.RowIndices[i], storage.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Length, storage.ColumnPointers.Length);
            for (int i = 0; i < storage.ColumnPointers.Length; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], storage.ColumnPointers[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.Replace(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/>. </param>
        [Theory(DisplayName = "Replace(int, int, T, Bool) - No Check")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T_Bool__NoCheck(Mat.SparseStorages.CompressedColumn<double> storage, int rowIndex, int columnIndex, double value, Mat.SparseStorages.CompressedColumn<double> expected)
        {
            // Arrange
            bool shouldReplace = storage.Contains(rowIndex, columnIndex);

            if (!shouldReplace) { expected = new Mat.SparseStorages.CompressedColumn<double>(storage); }

            // Act
            bool isReplaced = storage.Replace(rowIndex, columnIndex, value, false);

                        // Assert
            Assert.Equal(shouldReplace, isReplaced);

            Assert.Equal(expected.RowCount, storage.RowCount);
            Assert.Equal(expected.ColumnCount, storage.ColumnCount);

            Assert.Equal(expected.Count, storage.Count);
            for (int i = 0; i < storage.Count; i++)
            {
                Assert.Equal(expected.Values[i], storage.Values[i]);
                Assert.Equal(expected.RowIndices[i], storage.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Length, storage.ColumnPointers.Length);
            for (int i = 0; i < storage.ColumnPointers.Length; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], storage.ColumnPointers[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.Remove(int, int)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to remove. </param>
        /// <param name="columnIndex"> Column index of the component to remove. </param>
        /// <param name="expected">  Expected sparse storage after removal of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Remove(int, int)")]
        [ClassData(typeof(DataClasses.Remove))]
        public void Remove__Int_Int(Mat.SparseStorages.CompressedColumn<double> storage, int rowIndex, int columnIndex, Mat.SparseStorages.CompressedColumn<double> expected)
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
            for (int i = 0; i < storage.Count; i++)
            {
                Assert.Equal(expected.Values[i], storage.Values[i]);
                Assert.Equal(expected.RowIndices[i], storage.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Length, storage.ColumnPointers.Length);
            for (int i = 0; i < storage.ColumnPointers.Length; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], storage.ColumnPointers[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.Contains(int, int)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="isContained"> Two-dimensional array evaluating whether a component at the given row and column has an entry in the sparse storage.</param>
        [Theory(DisplayName = "Contains(int, int)")]
        [ClassData(typeof(DataClasses.Contains))]
        public void Contains__Int_Int(Mat.SparseStorages.CompressedColumn<double> storage, bool[,] isContained)
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
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.ToArray()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="expected"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToArray(Mat.SparseStorages.CompressedColumn<double> storage, double[,] expected)
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
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.ToRowMajorArray()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToRowMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToRowMajorArray(Mat.SparseStorages.CompressedColumn<double> storage, double[,] values)
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
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.ToColumnMajorArray()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToColumnMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToColumnMajorArray(Mat.SparseStorages.CompressedColumn<double> storage, double[,] values)
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
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.RowVectors()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "RowVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void RowVectors(Mat.SparseStorages.CompressedColumn<double> storage, double[,] values)
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
        /// Tests the method <see cref="Mat.SparseStorages.CompressedColumn{T}.ColumnVectors()"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ColumnVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ColumnVectors(Mat.SparseStorages.CompressedColumn<double> storage, double[,] values)
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
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.CompressedColumn{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse storage S1 represents a [2x3] matrix. </remarks>
            internal static class S1
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S1"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.CompressedColumn<double> _s1 = CreateS1();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S1"/>. </description>
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
                ///         <description> A newly computed <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S1"/>. </description>
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> TransposeMultiply_S2()
                {
                    int rowCount = 3;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                          1,      6,    -16,
                          1,      3,      5,
                        /*0,*/    3,    -21
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                          0,      1,    2,
                          0,      1,    2,
                        /*0,*/    1,    2
                    ]);

                    int[] columnPointers = new int[] { 0, 3, 6, 8 };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S1·S2<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> MultiplyTranspose_S2()
                {
                    int rowCount = 2;
                    int columnCount = 2;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        -11,    21,
                          8,    -6
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                          0,      1,
                          0,      1,
                    ]);

                    int[] columnPointers = new int[] { 0, 2, 4 };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }


                /// <summary>
                /// Provides the expected result of the transposition : <c>S1<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Transpose()
                {
                    int rowCount = 3;
                    int columnCount = 2;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        1,    4,    -2,
                        2,    7,     3
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                        0,    1,    2,
                        0,    1,    2,
                    ]);

                    int[] columnPointers = new int[] { 0, 3, 6 };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>S1+S2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Addition_S2()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        6,    /*0,*/
                        3,      8,
                        4,    /*0*/
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                        0,    /*1,*/
                        0,      1,
                        0,    /*1*/
                    ]);

                    int[] columnPointers = new int[] { 0, 1, 3, 4 };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>S1-S2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Subtraction_S2()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        -4,    4,
                         5,    6,
                        -8,    6
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-S1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> UnaryNegation()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        -1,    -2,
                        -4,    -7,
                         2,    -3
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.CompressedColumn<double> Result) RightMultiplication_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         3,     6,
                        12,    21,
                        -6,     9
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return ( 3.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.CompressedColumn<double> Result) LeftMultiplication_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                          5,    10,
                         20,    35,
                        -10,    15
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return (5.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Divisor, Mat.SparseStorages.CompressedColumn<double> Result) Division_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        0.25,     0.5,
                           1,    1.75,
                        -0.5,    0.75
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return (4.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                /// <returns> The <see cref="Mat.SparseStorages.CompressedColumn{T}"/>. </returns>
                internal static Mat.SparseStorages.CompressedColumn<double> CreateS1()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         1.0,    2.0,
                         4.0,    7.0,
                        -2.0,    3.0
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                          0,    1,
                          0,    1,
                          0,    1,
                    ]);

                    int[] columnPointers = new int[] { 0, 2, 4, 6 };

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.CompressedColumn{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse storage S2 represents a [2x3] matrix. </remarks>
            internal static class S2
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S2"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.CompressedColumn<double> _s2 = CreateS2();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S2"/>. </description>
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> TransposeMultiply_S1()
                {
                    int rowCount = 3;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                          1,    1,    /*0,*/
                          6,    3,      3,
                        -16,    5,    -21
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                          0,      1,    /*2,*/
                          0,      1,      2,
                          0,      1,      2
                    ]);

                    int[] columnPointers = new int[] { 0, 2, 5, 8 };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S2·S1<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> MultiplyTranspose_S1()
                {
                    int rowCount = 2;
                    int columnCount = 2;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        -11,     8,
                         21,    -6,
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }


                /// <summary>
                /// Provides the expected result of the transposition : <c>S2<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Transpose()
                {
                    int rowCount = 3;
                    int columnCount = 2;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         5,    -1,     6,
                        -2,     1,    -3
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>S2+S1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Addition_S1()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        6,    /*0,*/
                        3,      8,
                        4,    /*0,*/
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                          0,    /*1,*/
                          0,      1,
                          0,    /*1,*/
                    ]);

                    int[] columnPointers = new int[] { 0, 1, 3, 4 };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>S2-S1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Subtraction_S1()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         4,    -4,
                        -5,    -6,
                         8,    -6
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-S2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> UnaryNegation()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        -5,     2,
                         1,    -1,
                        -6,     3
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.CompressedColumn<double> Result) RightMultiplication_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        15,    -6,
                        -3,     3,
                        18,    -9
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return ( 3.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.CompressedColumn<double> Result) LeftMultiplication_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        25,    -10,
                        -5,      5,
                        30,    -15
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return ( 5.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Divisor, Mat.SparseStorages.CompressedColumn<double> Result) Division_T()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         1.25,     -0.5,
                        -0.25,     0.25,
                          1.5,    -0.75
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return ( 4.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                internal static Mat.SparseStorages.CompressedColumn<double> CreateS2()
                {
                    int rowCount = 2;
                    int columnCount = 3;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         5,    -2,
                        -1,     1,
                         6,    -3
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                          0,      1,
                          0,      1,
                          0,      1,
                    ]);

                    int[] columnPointers = new int[] { 0, 2, 4, 6 };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.CompressedColumn{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse storage S3 represents a [6x6] matrix. </remarks>
            internal static class S3
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S3"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.CompressedColumn<double> _s3 = CreateS3();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S3"/>. </description>
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
                ///         <description> A newly computed <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S3"/>. </description>
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> TransposeMultiply_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         676,     887,    1395,    1817,    3232,    5178,
                         702,     924,    1450,    1884,    3334,    5316,
                         728,     961,    1505,    1951,    3436,    5454,
                         754,     998,    1560,    2018,    3538,    5592,
                         780,    1035,    1615,    2085,    3640,    5730,
                         806,    1072,    1670,    2152,    3742,    5868
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S3·S4<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> MultiplyTranspose_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         1910,    2107,    -48,     -44,     294,    1902,
                         3260,    3577,    -78,     -44,     494,    3162,
                         4610,    5047,    -108,    -44,     694,    4422,
                         5960,    6517,    -138,    -44,     894,    5682,
                         7310,    7987,    -168,    -44,    1094,    6942,
                         8660,    9457,    -198,    -44,    1294,    8202
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Transpose()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                          10,    15,    20,    25,    30,    35,
                           7,    14,    21,    28,    35,    42,
                         /*0,*/    -1,     2,    -3,     4,    -5,
                           6,     4,     2,    -2,    -4,    -6,
                           1,     1,     2,     3,     5,     8,
                           2,     4,     8,    16,    32,     64
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                          0,      1,      2,      3,    4,    5,
                          0,      1,      2,      3,    4,    5,
                        /*0,*/    1,      2,      3,    4,    5,
                          0,      1,      2,      3,    4,    5,
                          0,      1,      2,      3,    4,    5,
                          0,      1,      2,      3,    4,    5
                    ]);

                    int[] columnPointers = new int[] { 0, 6, 12, 17, 23, 29, 35 };

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>S3+S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Addition_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        21,    28,    31,    47,    52,    63,
                        27,    36,    31,    46,    53,    66,
                        33,    44,    35,    45,    55,    71,
                        39,    52,    31,    42,    57,    80,
                        45,    60,    39,    41,    60,    97,
                        51,    68,    31,    40,    64,    130
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>S3-S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Subtraction_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        -1,    -14,    -31,    -35,    -50,    -59,
                         3,     -8,    -33,    -38,    -51,    -58,
                         7,     -2,    -31,    -41,    -51,    -55,
                        11,      4,    -37,    -46,    -51,    -48,
                        15,     10,    -31,    -49,    -50,    -33,
                        19,     16,    -41,    -52,    -48,     -2
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> UnaryNegation()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        -10,     -7,    /*0,*/    -6,    -1,     -2,
                        -15,    -14,      1,      -4,    -1,     -4,
                        -20,    -21,     -2,      -2,    -2,     -8,
                        -25,    -28,      3,       2,    -3,    -16,
                        -30,    -35,     -4,       4,    -5,    -32,
                        -35,    -42,      5,       6,    -8,    -64
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    ]);

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S3·S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Multiplication_S4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        5735,    6517,    -183,    -440,      960,     6546,
                        5870,    6664,    -186,    -440,      980,     6672,
                        6005,    6811,    -189,    -440,     1000,     6798,
                        6140,    6958,    -192,    -440,     1020,     6924,
                        6275,    7105,    -195,    -440,     1040,     7050,
                        6410,    7252,    -198,    -440,     1060,     7176
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.CompressedColumn<double> Result) RightMultiplication_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         30,     21,    /*0,*/    18,     3,      6,
                         45,     42,     -3,      12,     3,     12,
                         60,     63,      6,       6,     6,     24,
                         75,     84,     -9,      -6,     9,     48,
                         90,    105,     12,     -12,    15,     96,
                        105,    126,    -15,     -18,    24,    192
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    ]);

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };


                    return ( 3.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.CompressedColumn<double> Result) LeftMultiplication_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                          50,     35,    /*0,*/    30,     5,     10,
                          75,     70,     -5,      20,     5,     20,
                         100,    105,     10,      10,    10,     40,
                         125,    140,    -15,     -10,    15,     80,
                         150,    175,     20,     -20,    25,    160,
                         175,    210,    -25,     -30,    40,    320
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    ]);

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };


                    return ( 5.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Divisor, Mat.SparseStorages.CompressedColumn<double> Result) Division_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         2.5,    1.75,      /*0,*/    1.5,    0.25,    0.5,
                        3.75,     3.5,    -0.25,        1,    0.25,      1,
                           5,    5.25,      0.5,      0.5,     0.5,      2,
                        6.25,       7,    -0.75,     -0.5,    0.75,      4,
                         7.5,    8.75,        1,       -1,    1.25,      8,
                        8.75,    10.5,    -1.25,     -1.5,       2,     16
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    ]);

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };


                    return ( 4.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                internal static Mat.SparseStorages.CompressedColumn<double> CreateS3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        10,     7,    /*0,*/     6,    1,     2,
                        15,    14,     -1,       4,    1,     4,
                        20,    21,      2,       2,    2,     8,
                        25,    28,     -3,      -2,    3,    16,
                        30,    35,      4,      -4,    5,    32,
                        35,    42,     -5,      -6,    8,    64
                    ]);

                    List<int> rowIndices = new List<int>
                    ([
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    ]);

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35};

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.CompressedColumn{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse storage S4 represents a [6x6] matrix. </remarks>
            internal static class S4
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S4"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.CompressedColumn<double> _s4 = CreateS4();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S4"/>. </description>
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
                ///         <description> A newly computed <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S4"/>. </description>
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> TransposeMultiply_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         676,     702,     728,     754,     780,     806,
                         887,     924,     961,     998,    1035,    1072,
                        1395,    1450,    1505,    1560,    1615,    1670,
                        1817,    1884,    1951,    2018,    2085,    2152,
                        3232,    3334,    3436,    3538,    3640,    3742,
                        5178,    5316,    5454,    5592,    5730,    5868
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>S4·S3<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> MultiplyTranspose_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         1910,    3260,    4610,    5960,    7310,    8660,
                         2107,    3577,    5047,    6517,    7987,    9457,
                          -48,     -78,    -108,    -138,    -168,    -198,
                          -44,     -44,     -44,     -44,     -44,     -44,
                          294,     494,     694,     894,    1094,    1294,
                         1902,    3162,    4422,    5682,    6942,    8202
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Transpose()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         11,    12,    13,    14,    15,    16,
                         21,    22,    23,    24,    25,    26,
                         31,    32,    33,    34,    35,    36,
                         41,    42,    43,    44,    45,    46,
                         51,    52,    53,    54,    55,    56,
                         61,    62,    63,    64,    65,    66
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>S4+S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Addition_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        21,    28,    31,    47,    52,    63,
                        27,    36,    31,    46,    53,    66,
                        33,    44,    35,    45,    55,    71,
                        39,    52,    31,    42,    57,    80,
                        45,    60,    39,    41,    60,    97,
                        51,    68,    31,    40,    64,    130
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>S4-S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Subtraction_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                          1,     14,    31,    35,    50,    59,
                         -3,      8,    33,    38,    51,    58,
                         -7,      2,    31,    41,    51,    55,
                        -11,     -4,    37,    46,    51,    48,
                        -15,    -10,    31,    49,    50,    33,
                        -19,    -16,    41,    52,    48,     2
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-S4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> UnaryNegation()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        -11,    -21,    -31,    -41,    -51,    -61,
                        -12,    -22,    -32,    -42,    -52,    -62,
                        -13,    -23,    -33,    -43,    -53,    -63,
                        -14,    -24,    -34,    -44,    -54,    -64,
                        -15,    -25,    -35,    -45,    -55,    -65,
                        -16,    -26,    -36,    -46,    -56,    -66
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>S4·S3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static Mat.SparseStorages.CompressedColumn<double> Multiplication_S3()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                         325,     585,     845,    1105,    1365,    1625,
                         455,     825,    1195,    1565,    1935,    2305,
                         684,    1234,    1784,    2334,    2884,    3434,
                         845,    1515,    2185,    2855,    3525,    4195,
                        1333,    2353,    3373,    4393,    5413,    6433,
                        1884,    3264,    4644,    6024,    7404,    8784
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.CompressedColumn<double> Result) RightMultiplication_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                          77,    147,    217,    287,    357,    427,
                          84,    154,    224,    294,    364,    434,
                          91,    161,    231,    301,    371,    441,
                          98,    168,    238,    308,    378,    448,
                         105,    175,    245,    315,    385,    455,
                         112,    182,    252,    322,    392,    462
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return ( 7.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Factor, Mat.SparseStorages.CompressedColumn<double> Result) LeftMultiplication_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        22,    42,    62,    82,    102,    122,
                        24,    44,    64,    84,    104,    124,
                        26,    46,    66,    86,    106,    126,
                        28,    48,    68,    88,    108,    128,
                        30,    50,    70,    90,    110,    130,
                        32,    52,    72,    92,    112,    132
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return (2.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                ///         <description> An <see cref="Mat.SparseStorages.CompressedColumn{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static (double Divisor, Mat.SparseStorages.CompressedColumn<double> Result) Division_T()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                          5.5,    10.5,    15.5,    20.5,    25.5,    30.5,
                            6,      11,      16,      21,      26,      31,
                          6.5,    11.5,    16.5,    21.5,    26.5,    31.5,
                            7,      12,      17,      22,      27,      32,
                          7.5,    12.5,    17.5,    22.5,    27.5,    32.5,
                            8,      13,      18,      23,      28,      33
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return (2.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers));
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
                internal static Mat.SparseStorages.CompressedColumn<double> CreateS4()
                {
                    int rowCount = 6;
                    int columnCount = 6;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                        11,    21,    31,    41,    51,    61,
                        12,    22,    32,    42,    52,    62,
                        13,    23,    33,    43,    53,    63,
                        14,    24,    34,    44,    54,    64,
                        15,    25,    35,    45,    55,    65,
                        16,    26,    36,    46,    56,    66,
                    ]);

                    List<int> rowIndices = new List<int>(values.Count);
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices.Add(i_Row);
                        }
                        columnPointers[i_Column + 1] = rowIndices.Count;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseStorages.CompressedColumn{T}"/>, for <see cref="DataClasses"/>.
            /// </summary>
            /// <remarks> The sparse storage S5 represents a [3x5] matrix. </remarks>
            internal static class S5
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse storage <see cref="S5"/>.
                /// </summary>
                private static readonly Mat.SparseStorages.CompressedColumn<double> _s5 = CreateS5();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S5"/>. </description>
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
                ///         <description> A newly computed <see cref="Mat.SparseStorages.CompressedColumn{T}"/> of type <see cref="S5"/>. </description>
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
                public static object[] Values() => [new List<double>() { 1.0, 2.0, -2.0, 5.5 }];

                /// <summary>
                /// Provides the expected list of row indices of the storage.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> List of <see cref="int"/> values containing the row indices of each non-zero values. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowIndices() => [new List<int>() { 0, 2, 0, 2 }];

                /// <summary>
                /// Provides the expected array of column pointers of the storage.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Array of <see cref="int"/> values containing the column pointers for the represented matrix. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnPointers() => [new int[] { 0, 2, 2, 2, 3, 4 }];


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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,       -3.2,        2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 1, 2, 0, 2 };
                    int[] columnPointers = new int[] { 0, 3, 3, 3, 4, 5 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/      0.7,
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 2, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 4, 5 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,        0.3,      /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 1, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 4, 5 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                             2.2,      /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 0, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 3, 5 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/      1.5,      /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 1, 0, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 3, 4, 5 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                             0.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 0, 2 };
                    int[] columnPointers = new int[] { 0, 2, 3, 3, 4, 5 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 3, 4 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/     -7.3,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 3, 4 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 3, 4 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                             0.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 3, 4 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 3, 4 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                           /*0.0,*/    /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 2, 0, 2 };
                    int[] columnPointers = new int[] { 0, 1, 1, 1, 2, 3 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 0, 2 };
                    int[] columnPointers = new int[] { 0, 1, 1, 1, 2, 3 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 2 };
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 2, 3 };

                    Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);

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
                /// <returns> The <see cref="Mat.SparseStorages.CompressedColumn{T}"/>. </returns>
                internal static Mat.SparseStorages.CompressedColumn<double> CreateS5()
                {
                    int rowCount = 3;
                    int columnCount = 5;

                    // Presented in a tranposed way: each lines corresponds to a column 
                    List<double> values = new List<double>
                    ([
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    ]);

                    List<int> rowIndices = new List<int>() { 0, 2, 0, 2};
                    int[] columnPointers = new int[] { 0, 2, 2, 2, 3, 4 };

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
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
            internal class Values : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Readable, DataStorages.S5.Values },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Property__RowIndices(Mat.SparseStorages.CompressedColumn{double}, List{int})"/>.
            /// </summary>
            internal class RowIndices : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Readable, DataStorages.S5.RowIndices },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Property__ColumnPointers(Mat.SparseStorages.CompressedColumn{double}, int[])"/>.
            /// </summary>
            internal class ColumnPointers : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.Readable, DataStorages.S5.ColumnPointers },
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
            /// Class data for <see cref="CompressedColumn.Constructor__Int_Int_ListOfT_ListOfInt_ArrayofInt(int, int, List{double}, List{int}, int[])"/>.
            /// </summary>
            internal class Constructor__Int_Int_ListOfT_ListOfInt_ArrayofInt : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.RowCount, DataStorages.S5.ColumnCount, DataStorages.S5.Values, DataStorages.S5.RowIndices, DataStorages.S5.ColumnPointers },
                    };
            }

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Constructor__CompressedColumn(Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Constructor__CompressedColumn : BaseDataClass
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
