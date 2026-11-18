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
            IReadOnlyList<int> result = storage.ColumnPointers;

            // Assert
            Assert.Equal(expected.Length, result.Count);
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
        public void Property__RowIndices(Mat.SparseStorages.CompressedColumn<double> storage, int[] expected)
        {
            // Arrange

            //Act
            IReadOnlyList<int> result = storage.RowIndices;

            // Assert
            Assert.Equal(expected.Length, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.CompressedColumn{T}.Values"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected list of values. </param>
        [Theory(DisplayName = "Prop. Values")]
        [ClassData(typeof(DataClasses.Values))]
        public void Property__Values(Mat.SparseStorages.CompressedColumn<double> storage, double[] expected)
        {
            // Arrange

            //Act
            IReadOnlyList<double> result = storage.Values;

            // Assert
            Assert.Equal(expected.Length, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }


        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.SparseStorage{T}.this(int, int)"/>.
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
        /// Tests the property <see cref="Mat.SparseStorages.SparseStorage{T}.StorageFormat"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="expected"> The expected sparse storage format. </param>
        [Theory(DisplayName = "Prop. StorageFormat")]
        [ClassData(typeof(DataClasses.StorageFormat))]
        public void Property__StorageFormat(Mat.SparseStorages.CompressedColumn<double> storage, Mat.SparseStorages.SparseStorageFormat expected)
        {
            // Arrange

            // Act
            Mat.SparseStorages.SparseStorageFormat result = storage.StorageFormat;

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region Tests : Constructors

        /// <summary>
        /// Tests the constructor <see cref="Mat.SparseStorages.CompressedColumn{T}.CompressedColumn(int, int, ref List{T}, ref List{int}, ref int[])"/>.
        /// </summary>
        /// <param name="rowCount"> Expected number of row. </param>
        /// <param name="columnCount"> Expected number of column. </param>
        /// <param name="values"> List of values. </param>
        /// <param name="rowIndices"> List of row indices. </param>
        /// <param name="columnPointers"> Array of column pointers. </param>
        [Theory(DisplayName = "CompressedColumn<T>(int, int, ref List<double>, ref List<int>, ref int[])")]
        [ClassData(typeof(DataClasses.Constructor__Int_Int_ListOfT_ListOfInt_ArrayofInt))]
        public void Constructor__Int_Int_ArrayofInt_ArrayofInt_ArrayofT(int rowCount, int columnCount, int[] columnPointers, int[] rowIndices, double[] values)
        {
            // Arrange

            //Act
            Mat.SparseStorages.CompressedColumn<double> result = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);

            // Assert

            Assert.Equal(rowCount, result.RowCount);
            Assert.Equal(columnCount, result.ColumnCount);

            Assert.Equal(columnCount + 1, result.ColumnPointers.Count);
            for (int i = 0; i < result.ColumnPointers.Count; i++)
            {
                Assert.Equal(columnPointers[i], result.ColumnPointers[i]);
            }

            Assert.Equal(values.Length, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(rowIndices[i], result.RowIndices[i]);
                Assert.Equal(values[i], result.Values[i]);
            }

            Assert.Same(columnPointers, result.ColumnPointers);
            Assert.Same(rowIndices, result.RowIndices);
            Assert.Same(values, result.Values);
        }

        /// <summary>
        /// Tests the constructor <see cref="Mat.SparseStorages.CompressedColumn{T}.CompressedColumn(Mat.SparseStorages.CompressedColumn{T})"/>.
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

            Assert.Equal(expected.ColumnPointers.Count, result.ColumnPointers.Count);
            for (int i = 0; i < result.ColumnPointers.Count; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], result.ColumnPointers[i]);
            }

            Assert.NotSame(expected.Values, result.Values);
            Assert.NotSame(expected.RowIndices, result.RowIndices);
            Assert.NotSame(expected.ColumnPointers, result.ColumnPointers);
        }

        #endregion

        #region Tests : Public Static Methods

        /// <summary>
        /// Tests the static method <see cref="Mat.SparseStorages.CompressedColumn{T}.Build(int, int, Mat.SparseBuilders.DictionaryOfKeys{T}, bool)"/>
        /// </summary>
        /// <param name="rowCount"> Expected number of row. </param>
        /// <param name="columnCount"> Expected number of column. </param>
        /// <param name="dok"> Sparse builders containing the unsorted components. </param>
        /// <param name="expected"> The expected compressed column storage resulting from the build. </param>
        [Theory(DisplayName = "Static Build(int, int, DictonaryOfKeys, bool)")]
        [ClassData(typeof(DataClasses.Build__FromDoK))]
        public void Static__Build__Int_Int_DictionaryOfKeys_Bool(int rowCount, int columnCount, Mat.SparseBuilders.DictionaryOfKeys<double> dok, Mat.SparseStorages.CompressedColumn<double> expected)
        {
            // Arrange

            // Act
            Mat.SparseStorages.CompressedColumn<double> result_NoCheck = Mat.SparseStorages.CompressedColumn<double>.Build(rowCount, columnCount, dok, false);
            Mat.SparseStorages.CompressedColumn<double> result_Check = Mat.SparseStorages.CompressedColumn<double>.Build(rowCount, columnCount, dok, true);

            // Assert (NoCheck)
            Assert.Equal(expected.RowCount, result_NoCheck.RowCount);
            Assert.Equal(expected.ColumnCount, result_NoCheck.ColumnCount);

            Assert.Equal(expected.Count, result_NoCheck.Count);
            for (int i = 0; i < result_NoCheck.Count; i++)
            {
                Assert.Equal(expected.Values[i], result_NoCheck.Values[i]);
                Assert.Equal(expected.RowIndices[i], result_NoCheck.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Count, result_NoCheck.ColumnPointers.Count);
            for (int i = 0; i < result_NoCheck.ColumnPointers.Count; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], result_NoCheck.ColumnPointers[i]);
            }

            // Assert (Check)
            Assert.Equal(expected.RowCount, result_Check.RowCount);
            Assert.Equal(expected.ColumnCount, result_Check.ColumnCount);

            Assert.Equal(expected.Count, result_Check.Count);
            for (int i = 0; i < result_Check.Count; i++)
            {
                Assert.Equal(expected.Values[i], result_Check.Values[i]);
                Assert.Equal(expected.RowIndices[i], result_Check.RowIndices[i]);
            }

            Assert.Equal(expected.ColumnPointers.Count, result_Check.ColumnPointers.Count);
            for (int i = 0; i < result_Check.ColumnPointers.Count; i++)
            {
                Assert.Equal(expected.ColumnPointers[i], result_Check.ColumnPointers[i]);
            }
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
                for (int i = 0; i < expectedRow.RowCount; i++)
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
                for (int i = 0; i < expectedColumn.RowCount; i++)
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


                /// <summary>
                /// Provides the expected sparse storage format of the storage <see cref="S1"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Mat.SparseStorages.SparseStorageFormat"/> of the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] StorageFormat() => [Mat.SparseStorages.SparseStorageFormat.CompressedColumn];


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

                    int[] columnPointers = new int[] { 0, 3, 6, 8 };

                    int[] rowIndices = new int[]
                    {
                          0,      1,    2,
                          0,      1,    2,
                        /*0,*/    1,    2
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                          1.0,      6.0,    -16.0,
                          1.0,      3.0,      5.0,
                        /*0.0,*/    3.0,    -21.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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

                    int[] columnPointers = new int[] { 0, 2, 4 };

                    int[] rowIndices = new int[]
                    {
                          0,      1,
                          0,      1,
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                        -11.0,    21.0,
                          8.0,    -6.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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

                    int[] columnPointers = new int[] { 0, 3, 6 };

                    int[] rowIndices = new int[]
                    {
                        0,    1,    2,
                        0,    1,    2,
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                        1.0,    4.0,    -2.0,
                        2.0,    7.0,     3.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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

                    int[] columnPointers = new int[] { 0, 1, 3, 4 };

                    int[] rowIndices = new int[]
                    {
                        0,    /*1,*/
                        0,      1,
                        0,    /*1*/
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                        6.0,    /*0.0,*/
                        3.0,      8.0,
                        4.0,    /*0.0*/
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        -4.0,    4.0,
                         5.0,    6.0,
                        -8.0,    6.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        -1.0,    -2.0,
                        -4.0,    -7.0,
                         2.0,    -3.0
                    };


                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                         3.0,     6.0,
                        12.0,    21.0,
                        -6.0,     9.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return (3.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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
                    double[] values = new double[]
                    {
                          5.0,    10.0,
                         20.0,    35.0,
                        -10.0,    15.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return (5.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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
                    double[] values = new double[]
                    {
                        0.25,    0.5,
                        1.0,     1.75,
                        -0.5,    0.75
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return (4.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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

                    int[] columnPointers = new int[] { 0, 2, 4, 6 };

                    int[] rowIndices = new int[]
                    {
                          0,    1,
                          0,    1,
                          0,    1,
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                         1.0,    2.0,
                         4.0,    7.0,
                        -2.0,    3.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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


                /// <summary>
                /// Provides the expected sparse storage format of the storage <see cref="S2"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Mat.SparseStorages.SparseStorageFormat"/> of the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] StorageFormat() => [Mat.SparseStorages.SparseStorageFormat.CompressedColumn];


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

                    int[] columnPointers = new int[] { 0, 2, 5, 8 };

                    int[] rowIndices = new int[]
                    {
                        0,      1,    /*2,*/
                        0,      1,      2,
                        0,      1,      2
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                          1.0,    1.0,    /*0.0,*/
                          6.0,    3.0,      3.0,
                        -16.0,    5.0,    -21.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        -11.0,     8.0,
                         21.0,    -6.0,
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                         5.0,    -1.0,     6.0,
                        -2.0,     1.0,    -3.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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

                    int[] columnPointers = new int[] { 0, 1, 3, 4 };

                    int[] rowIndices = new int[]
                    {
                          0,    /*1,*/
                          0,      1,
                          0,    /*1,*/
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                        6.0,    /*0.0,*/
                        3.0,      8.0,
                        4.0,    /*0.0,*/
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                         4.0,    -4.0,
                        -5.0,    -6.0,
                         8.0,    -6.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        -5.0,     2.0,
                         1.0,    -1.0,
                        -6.0,     3.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        15.0,    -6.0,
                        -3.0,     3.0,
                        18.0,    -9.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return ( 3.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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
                    double[] values = new double[]
                    {
                        25.0,    -10.0,
                        -5.0,      5.0,
                        30.0,    -15.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return ( 5.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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
                    double[] values = new double[]
                    {
                         1.25,    -0.5,
                        -0.25,     0.25,
                         1.5,     -0.75
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return (4.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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

                    int[] columnPointers = new int[] { 0, 2, 4, 6 };

                    int[] rowIndices = new int[]
                    {
                          0,      1,
                          0,      1,
                          0,      1,
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                         5.0,    -2.0,
                        -1.0,     1.0,
                         6.0,    -3.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers , rowIndices, values);
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


                /// <summary>
                /// Provides the expected sparse storage format of the storage <see cref="S3"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Mat.SparseStorages.SparseStorageFormat"/> of the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] StorageFormat() => [Mat.SparseStorages.SparseStorageFormat.CompressedColumn];


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
                    double[] values = new double[]
                    {
                         676.0,     887.0,    1395.0,    1817.0,    3232.0,    5178.0,
                         702.0,     924.0,    1450.0,    1884.0,    3334.0,    5316.0,
                         728.0,     961.0,    1505.0,    1951.0,    3436.0,    5454.0,
                         754.0,     998.0,    1560.0,    2018.0,    3538.0,    5592.0,
                         780.0,    1035.0,    1615.0,    2085.0,    3640.0,    5730.0,
                         806.0,    1072.0,    1670.0,    2152.0,    3742.0,    5868.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                         1910.0,    2107.0,    -48.0,     -44.0,     294.0,    1902.0,
                         3260.0,    3577.0,    -78.0,     -44.0,     494.0,    3162.0,
                         4610.0,    5047.0,    -108.0,    -44.0,     694.0,    4422.0,
                         5960.0,    6517.0,    -138.0,    -44.0,     894.0,    5682.0,
                         7310.0,    7987.0,    -168.0,    -44.0,    1094.0,    6942.0,
                         8660.0,    9457.0,    -198.0,    -44.0,    1294.0,    8202.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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

                    int[] columnPointers = new int[] { 0, 6, 12, 17, 23, 29, 35 };

                    int[] rowIndices = new int[]
                    {
                          0,      1,      2,      3,    4,    5,
                          0,      1,      2,      3,    4,    5,
                        /*0,*/    1,      2,      3,    4,    5,
                          0,      1,      2,      3,    4,    5,
                          0,      1,      2,      3,    4,    5,
                          0,      1,      2,      3,    4,    5
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                          10.0,    15.0,    20.0,    25.0,    30.0,    35.0,
                           7.0,    14.0,    21.0,    28.0,    35.0,    42.0,
                         /*0.0,*/  -1.0,     2.0,    -3.0,     4.0,    -5.0,
                           6.0,     4.0,     2.0,    -2.0,    -4.0,    -6.0,
                           1.0,     1.0,     2.0,     3.0,     5.0,     8.0,
                           2.0,     4.0,     8.0,    16.0,    32.0,    64.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        21.0,    28.0,    31.0,    47.0,    52.0,     63.0,
                        27.0,    36.0,    31.0,    46.0,    53.0,     66.0,
                        33.0,    44.0,    35.0,    45.0,    55.0,     71.0,
                        39.0,    52.0,    31.0,    42.0,    57.0,     80.0,
                        45.0,    60.0,    39.0,    41.0,    60.0,     97.0,
                        51.0,    68.0,    31.0,    40.0,    64.0,    130.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        -1.0,    -14.0,    -31.0,    -35.0,    -50.0,    -59.0,
                         3.0,     -8.0,    -33.0,    -38.0,    -51.0,    -58.0,
                         7.0,     -2.0,    -31.0,    -41.0,    -51.0,    -55.0,
                        11.0,      4.0,    -37.0,    -46.0,    -51.0,    -48.0,
                        15.0,     10.0,    -31.0,    -49.0,    -50.0,    -33.0,
                        19.0,     16.0,    -41.0,    -52.0,    -48.0,     -2.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };

                    int[] rowIndices = new int[]
                    {
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                        -10.0,     -7.0,    /*0.0,*/    -6.0,    -1.0,     -2.0,
                        -15.0,    -14.0,      1.0,      -4.0,    -1.0,     -4.0,
                        -20.0,    -21.0,     -2.0,      -2.0,    -2.0,     -8.0,
                        -25.0,    -28.0,      3.0,       2.0,    -3.0,    -16.0,
                        -30.0,    -35.0,     -4.0,       4.0,    -5.0,    -32.0,
                        -35.0,    -42.0,      5.0,       6.0,    -8.0,    -64.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        5735.0,    6517.0,    -183.0,    -440.0,      960.0,     6546.0,
                        5870.0,    6664.0,    -186.0,    -440.0,      980.0,     6672.0,
                        6005.0,    6811.0,    -189.0,    -440.0,     1000.0,     6798.0,
                        6140.0,    6958.0,    -192.0,    -440.0,     1020.0,     6924.0,
                        6275.0,    7105.0,    -195.0,    -440.0,     1040.0,     7050.0,
                        6410.0,    7252.0,    -198.0,    -440.0,     1060.0,     7176.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };

                    int[] rowIndices = new int[]
                    {
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                         30.0,     21.0,    /*0.0,*/    18.0,     3.0,      6.0,
                         45.0,     42.0,     -3.0,      12.0,     3.0,     12.0,
                         60.0,     63.0,      6.0,       6.0,     6.0,     24.0,
                         75.0,     84.0,     -9.0,      -6.0,     9.0,     48.0,
                         90.0,    105.0,     12.0,     -12.0,    15.0,     96.0,
                        105.0,    126.0,    -15.0,     -18.0,    24.0,    192.0
                    };


                    return (3.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };

                    int[] rowIndices = new int[]
                    {
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                          50.0,     35.0,    /*0.0,*/    30.0,     5.0,     10.0,
                          75.0,     70.0,     -5.0,      20.0,     5.0,     20.0,
                         100.0,    105.0,     10.0,      10.0,    10.0,     40.0,
                         125.0,    140.0,    -15.0,     -10.0,    15.0,     80.0,
                         150.0,    175.0,     20.0,     -20.0,    25.0,    160.0,
                         175.0,    210.0,    -25.0,     -30.0,    40.0,    320.0
                    };


                    return (5.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };

                    int[] rowIndices = new int[]
                    {
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                         2.5,     1.75,  /*0.0,*/    1.5,    0.25,    0.5,
                         3.75,    3.5,    -0.25,     1.0,    0.25,    1.0,
                         5.0,     5.25,    0.5,      0.5,    0.5,     2.0,
                         6.25,    7.0,    -0.75,    -0.5,    0.75,    4.0,
                         7.5,     8.75,    1.0,     -1.0,    1.25,    8.0,
                         8.75,   10.5,    -1.25,    -1.5,    2.0,     16.0
                    };


                    return (4.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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

                    int[] columnPointers = new int[] { 0, 5, 11, 17, 23, 29, 35 };

                    int[] rowIndices = new int[]
                    {
                        0,    1,    /*2,*/    3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5,
                        0,    1,      2,      3,    4,    5
                    };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                        10.0,     7.0,    /*0.0,*/     6.0,    1.0,     2.0,
                        15.0,    14.0,     -1.0,       4.0,    1.0,     4.0,
                        20.0,    21.0,      2.0,       2.0,    2.0,     8.0,
                        25.0,    28.0,     -3.0,      -2.0,    3.0,    16.0,
                        30.0,    35.0,      4.0,      -4.0,    5.0,    32.0,
                        35.0,    42.0,     -5.0,      -6.0,    8.0,    64.0
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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


                /// <summary>
                /// Provides the expected sparse storage format of the storage <see cref="S4"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Mat.SparseStorages.SparseStorageFormat"/> of the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] StorageFormat() => [Mat.SparseStorages.SparseStorageFormat.CompressedColumn];


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
                    double[] values = new double[]
                    {
                         676.0,     702.0,     728.0,     754.0,     780.0,     806.0,
                         887.0,     924.0,     961.0,     998.0,    1035.0,    1072.0,
                        1395.0,    1450.0,    1505.0,    1560.0,    1615.0,    1670.0,
                        1817.0,    1884.0,    1951.0,    2018.0,    2085.0,    2152.0,
                        3232.0,    3334.0,    3436.0,    3538.0,    3640.0,    3742.0,
                        5178.0,    5316.0,    5454.0,    5592.0,    5730.0,    5868.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                         1910.0,    3260.0,    4610.0,    5960.0,    7310.0,    8660.0,
                         2107.0,    3577.0,    5047.0,    6517.0,    7987.0,    9457.0,
                          -48.0,     -78.0,    -108.0,    -138.0,    -168.0,    -198.0,
                          -44.0,     -44.0,     -44.0,     -44.0,     -44.0,     -44.0,
                          294.0,     494.0,     694.0,     894.0,    1094.0,    1294.0,
                         1902.0,    3162.0,    4422.0,    5682.0,    6942.0,    8202.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                         11.0,    12.0,    13.0,    14.0,    15.0,    16.0,
                         21.0,    22.0,    23.0,    24.0,    25.0,    26.0,
                         31.0,    32.0,    33.0,    34.0,    35.0,    36.0,
                         41.0,    42.0,    43.0,    44.0,    45.0,    46.0,
                         51.0,    52.0,    53.0,    54.0,    55.0,    56.0,
                         61.0,    62.0,    63.0,    64.0,    65.0,    66.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        21.0,    28.0,    31.0,    47.0,    52.0,    63.0,
                        27.0,    36.0,    31.0,    46.0,    53.0,    66.0,
                        33.0,    44.0,    35.0,    45.0,    55.0,    71.0,
                        39.0,    52.0,    31.0,    42.0,    57.0,    80.0,
                        45.0,    60.0,    39.0,    41.0,    60.0,    97.0,
                        51.0,    68.0,    31.0,    40.0,    64.0,   130.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                          1.0,     14.0,    31.0,    35.0,    50.0,    59.0,
                         -3.0,      8.0,    33.0,    38.0,    51.0,    58.0,
                         -7.0,      2.0,    31.0,    41.0,    51.0,    55.0,
                        -11.0,     -4.0,    37.0,    46.0,    51.0,    48.0,
                        -15.0,    -10.0,    31.0,    49.0,    50.0,    33.0,
                        -19.0,    -16.0,    41.0,    52.0,    48.0,     2.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                        -11.0,    -21.0,    -31.0,    -41.0,    -51.0,    -61.0,
                        -12.0,    -22.0,    -32.0,    -42.0,    -52.0,    -62.0,
                        -13.0,    -23.0,    -33.0,    -43.0,    -53.0,    -63.0,
                        -14.0,    -24.0,    -34.0,    -44.0,    -54.0,    -64.0,
                        -15.0,    -25.0,    -35.0,    -45.0,    -55.0,    -65.0,
                        -16.0,    -26.0,    -36.0,    -46.0,    -56.0,    -66.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                         325.0,     585.0,     845.0,    1105.0,    1365.0,    1625.0,
                         455.0,     825.0,    1195.0,    1565.0,    1935.0,    2305.0,
                         684.0,    1234.0,    1784.0,    2334.0,    2884.0,    3434.0,
                         845.0,    1515.0,    2185.0,    2855.0,    3525.0,    4195.0,
                        1333.0,    2353.0,    3373.0,    4393.0,    5413.0,    6433.0,
                        1884.0,    3264.0,    4644.0,    6024.0,    7404.0,    8784.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                    double[] values = new double[]
                    {
                          77.0,    147.0,    217.0,    287.0,    357.0,    427.0,
                          84.0,    154.0,    224.0,    294.0,    364.0,    434.0,
                          91.0,    161.0,    231.0,    301.0,    371.0,    441.0,
                          98.0,    168.0,    238.0,    308.0,    378.0,    448.0,
                         105.0,    175.0,    245.0,    315.0,    385.0,    455.0,
                         112.0,    182.0,    252.0,    322.0,    392.0,    462.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return (7.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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
                    double[] values = new double[]
                    {
                        22.0,    42.0,    62.0,    82.0,    102.0,    122.0,
                        24.0,    44.0,    64.0,    84.0,    104.0,    124.0,
                        26.0,    46.0,    66.0,    86.0,    106.0,    126.0,
                        28.0,    48.0,    68.0,    88.0,    108.0,    128.0,
                        30.0,    50.0,    70.0,    90.0,    110.0,    130.0,
                        32.0,    52.0,    72.0,    92.0,    112.0,    132.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return (2.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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
                    double[] values = new double[]
                    {
                          5.5,    10.5,    15.5,    20.5,    25.5,    30.5,
                          6.0,    11.0,    16.0,    21.0,    26.0,    31.0,
                          6.5,    11.5,    16.5,    21.5,    26.5,    31.5,
                          7.0,    12.0,    17.0,    22.0,    27.0,    32.0,
                          7.5,    12.5,    17.5,    22.5,    27.5,    32.5,
                          8.0,    13.0,    18.0,    23.0,    28.0,    33.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }


                    return (2.0, new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values));
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
                        {    11.0,     12.0,     13.0,     14.0,     15.0,     16.0    },
                        {    21.0,     22.0,     23.0,     24.0,     25.0,     26.0    },
                        {    31.0,     32.0,     33.0,     34.0,     35.0,     36.0    },
                        {    41.0,     42.0,     43.0,     44.0,     45.0,     46.0    },
                        {    51.0,     52.0,     53.0,     54.0,     55.0,     56.0    },
                        {    61.0,     62.0,     63.0,     64.0,     65.0,     66.0    }
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
                    double[] values = new double[]
                    {
                        11.0,    21.0,    31.0,    41.0,    51.0,    61.0,
                        12.0,    22.0,    32.0,    42.0,    52.0,    62.0,
                        13.0,    23.0,    33.0,    43.0,    53.0,    63.0,
                        14.0,    24.0,    34.0,    44.0,    54.0,    64.0,
                        15.0,    25.0,    35.0,    45.0,    55.0,    65.0,
                        16.0,    26.0,    36.0,    46.0,    56.0,    66.0
                    };

                    int[] rowIndices = new int[values.Length];
                    int[] columnPointers = new int[columnCount + 1];

                    columnPointers[0] = 0;
                    for (int i_Column = 0; i_Column < columnCount; i_Column++)
                    {
                        for (int i_Row = 0; i_Row < rowCount; i_Row++)
                        {
                            rowIndices[i_Row + (i_Column * rowCount)] = i_Row;
                        }
                        columnPointers[i_Column + 1] = columnPointers[i_Column] + rowCount;
                    }

                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
                public static object[] Values() => [new double[] { 1.0, 2.0, -2.0, 5.5 }];

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
                public static object[] RowIndices() => [new int[] { 0, 2, 0, 2 }];

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


                /// <summary>
                /// Provides the expected sparse storage format of the storage <see cref="S5"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Mat.SparseStorages.SparseStorageFormat"/> of the storage. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] StorageFormat() => [Mat.SparseStorages.SparseStorageFormat.CompressedColumn];


                //     -----     Public Methods

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

                    int[] columnPointers = new int[] { 0, 2, 2, 2, 3, 4 };
                    int[] rowIndices = new int[] { 0, 2, 0, 2 };

                    // Presented in a tranposed way: each lines corresponds to a column 
                    double[] values = new double[]
                    {
                             1.0,      /*0.0,*/      2.0,
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/    /*0.0,*/
                            -2.0,      /*0.0,*/    /*0.0,*/
                           /*0.0,*/    /*0.0,*/      5.5,
                    };


                    return new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, columnPointers, rowIndices, values);
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
            /// Class data for <see cref="Property__StorageFormat(Mat.SparseStorages.CompressedColumn{double}, Mat.SparseStorages.SparseStorageFormat)"/>.
            /// </summary>
            internal class StorageFormat : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S1.Readable, DataStorages.S1.StorageFormat },
                        { DataStorages.S1.Readable, DataStorages.S2.StorageFormat },
                        { DataStorages.S3.Readable, DataStorages.S3.StorageFormat },
                        { DataStorages.S4.Readable, DataStorages.S4.StorageFormat },
                        { DataStorages.S5.Readable, DataStorages.S5.StorageFormat },
                    };
            }

            #endregion

            #region For Constructors

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Constructor__Int_Int_ListOfT_ListOfInt_ArrayofInt(int, int, List{double}, List{int}, int[])"/>.
            /// </summary>
            internal class Constructor__Int_Int_ListOfT_ListOfInt_ArrayofInt : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.S5.RowCount, DataStorages.S5.ColumnCount, DataStorages.S5.ColumnPointers, DataStorages.S5.RowIndices, DataStorages.S5.Values},
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

            #region For Static Methods

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Static__Build__Int_Int_DictionaryOfKeys_Bool(int, int, Mat.SparseBuilders.DictionaryOfKeys{double}, Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Build__FromDoK : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { SparseBuilders.DictionaryOfKeys.DataStorages.B1.RowCount,  SparseBuilders.DictionaryOfKeys.DataStorages.B1.ColumnCount, SparseBuilders.DictionaryOfKeys.DataStorages.B1.Readable, DataStorages.S1.Readable },
                        { SparseBuilders.DictionaryOfKeys.DataStorages.B2.RowCount,  SparseBuilders.DictionaryOfKeys.DataStorages.B2.ColumnCount, SparseBuilders.DictionaryOfKeys.DataStorages.B2.Readable, DataStorages.S2.Readable },
                        { SparseBuilders.DictionaryOfKeys.DataStorages.B3.RowCount,  SparseBuilders.DictionaryOfKeys.DataStorages.B3.ColumnCount, SparseBuilders.DictionaryOfKeys.DataStorages.B3.Readable, DataStorages.S3.Readable },
                        { SparseBuilders.DictionaryOfKeys.DataStorages.B4.RowCount,  SparseBuilders.DictionaryOfKeys.DataStorages.B4.ColumnCount, SparseBuilders.DictionaryOfKeys.DataStorages.B4.Readable, DataStorages.S4.Readable },
                        { SparseBuilders.DictionaryOfKeys.DataStorages.B5.RowCount,  SparseBuilders.DictionaryOfKeys.DataStorages.B5.ColumnCount, SparseBuilders.DictionaryOfKeys.DataStorages.B5.Readable, DataStorages.S5.Readable },
                    };
            }

            #endregion

            #region For Public Methods

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
