using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;
using System.Drawing;


namespace BRIDGES.Tests.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class testing the members of the <see cref="CompressedRow"/> class.
    /// </summary>
    [TestClass]
    public class CompressedRowTest
    {
        #region Test Fields

        /// <summary>
        /// A sparse matrix with compressed row storage.
        /// </summary>
        internal static CompressedRow MatrixA = new CompressedRow(2, 3, new Stor.DictionaryOfKeys(
            new double[] { 1d, 2d, 3d, 5d, 6d, 7d }, new int[] { 0, 0, 0, 1, 1, 1 }, new int[] { 0, 1, 2, 0, 1, 2 }));

        /// <summary>
        /// A sparse matrix with compressed row storage.
        /// </summary>
        internal static CompressedRow MatrixB = new CompressedRow(2, 3, new Stor.DictionaryOfKeys(
            new double[] { 4d, 3d, 2d, 5d, 4d, 3d }, new int[] { 0, 0, 0, 1, 1, 1 }, new int[] { 0, 1, 2, 0, 1, 2 }));


        /// <summary>
        /// A sparse matrix with compressed row storage.
        /// </summary>
        internal static CompressedRow MatrixC = new CompressedRow(6, 5, new Stor.DictionaryOfKeys(
            new double[] { 1.5, 1.25, 6.75, 2d, 5.5, 4d, 3.5, 2.25, 7.25 },
            new int[] { 0, 0, 1, 2, 3, 4, 4, 4, 5 }, new int[] { 1, 3, 3, 2, 3, 1, 2, 3, 4 }));

        /// <summary>
        /// A sparse matrix with compressed row storage.
        /// </summary>
        internal static CompressedRow MatrixD = new CompressedRow(5, 3, new Stor.DictionaryOfKeys(
            new double[] { 3.5, 1.5, 5d, 2d, 3d, 4d, 0.5, 2.5 },
            new int[] { 0, 1, 2, 3, 3, 3, 4, 4 }, new int[] { 0, 1, 0, 0, 1, 2, 0, 1 }));


        /// <summary>
        /// A sparse matrix with compressed row storage.
        /// </summary>
        internal static CompressedRow MatrixE = new CompressedRow(3, 2, new Stor.DictionaryOfKeys(
            new double[] { 4d, 3d, 2d, -5d, -4d, 1d }, new int[] { 0, 0, 1, 1, 2, 2 }, new int[] { 0, 1, 0, 1, 0, 1 }));

        #endregion


        #region Behavior

        /// <summary>
        /// Tests that <see cref="CompressedRow"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void CompressedRow_IsReference()
        {
            // Arrange

            int[] crsRowPointers = new int[3] { 0, 2, 4 };
            List<int> crsColumnIndices = new List<int> { 0, 1, 0, 1 };
            List<double> crsValues = new List<double> { 1.0, 2.0, 3.0, 4.0 };
            CompressedRow expected = new CompressedRow(2, 2, ref crsRowPointers, ref crsColumnIndices, ref crsValues);

            int[] otherCrsRowPointers = new int[2] { 0, 2 };
            List<int> otherCrsColumnIndices = new List<int> { 0, 1 };
            List<double> otherCrsValues = new List<double> { 2.0, 4.0 };
            CompressedRow actual = new CompressedRow(1, 2, ref otherCrsRowPointers, ref otherCrsColumnIndices, ref otherCrsValues );
            //Act
            actual = expected;
            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreSame(expected, actual);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="CompressedRow.RowCount"/> property.
        /// </summary>
        [TestMethod("Property RowCount")]
        public void RowCount()
        {
            // Arrange
            CompressedRow matrix = MatrixA;
            int expected = 2;
            // Act
            int actual = matrix.RowCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the  <see cref="CompressedRow.ColumnCount"/> property.
        /// </summary>
        [TestMethod("Property ColumnCount")]
        public void ColumnCount()
        {
            // Arrange
            CompressedRow matrix = MatrixA;
            int expected = 3;
            // Act
            int actual = matrix.ColumnCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the <see cref="CompressedRow"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int]")]
        public void Index_Int()
        {
            // Arrange
            CompressedRow matrix = MatrixA;
            double[] expected = new double[] { 1d, 2d, 3d, 5d, 6d, 7d };
            //Act

            // Assert
            for (int i = 0; i < matrix.NonZerosCount; i++)
            {
                Assert.AreEqual(expected[i], matrix[i], Settings.AbsolutePrecision);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="CompressedRow"/> from its size and values.
        /// </summary>
        [TestMethod("Constructor(Int,Int,DictionaryOfKeys)")]
        public void Constructor_Int_Int_DictionaryOfKeys()
        {
            // Arrange
            Stor.DictionaryOfKeys dok = new Stor.DictionaryOfKeys(2);
            dok.Add(1.0, 0, 1);
            dok.Add(2.0, 1, 0);
            // Act
            CompressedRow actual = new CompressedRow(2, 2, dok);
            // Assert
            Assert.AreEqual(0d, actual.At(0, 0), Settings.AbsolutePrecision);
            Assert.AreEqual(1.0, actual.At(0, 1), Settings.AbsolutePrecision);
            Assert.AreEqual(2.0, actual.At(1, 0), Settings.AbsolutePrecision);
            Assert.AreEqual(0d, actual.At(1, 1), Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="CompressedRow"/> from its size and components.
        /// </summary>
        [TestMethod("Constructor(Int,Int,double)")]
        public void Constructor_Int_Int_IntArray_IntList_doubleList()
        {
            // Arrange
            int[] rowPointers = new int[3] { 0, 3, 6 };
            List<int> columnIndices = new List<int> { 0, 1, 2, 0, 1, 2 };
            List<double> values = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };

            // Act
            CompressedRow actual = new CompressedRow(2, 3, ref rowPointers, ref columnIndices, ref values);

            // Assert
            Assert.AreEqual(1.0, actual.At(0, 0), Settings.AbsolutePrecision);
            Assert.AreEqual(2.0, actual.At(0, 1), Settings.AbsolutePrecision);
            Assert.AreEqual(3.0, actual.At(0, 2), Settings.AbsolutePrecision);
            Assert.AreEqual(4.0, actual.At(1, 0), Settings.AbsolutePrecision);
            Assert.AreEqual(5.0 ,actual.At(1, 1), Settings.AbsolutePrecision);
            Assert.AreEqual(6.0, actual.At(1, 2), Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="CompressedRow.Zero"/>.
        /// </summary>
        /// <param name="rowCount"> The number of row for the <see cref="CompressedRow"/>. </param>
        /// <param name="columnCount"> The number of column for the <see cref="CompressedRow"/>. </param>
        [TestMethod("Static Zero")]
        [DataRow(2, 3, DisplayName = "MatrixA Dimensions")]
        [DataRow(3, 2, DisplayName = "MatrixE Dimensions")]
        public void Static_Zero(int rowCount, int columnCount)
        {
            // Arrange

            // Act
            CompressedRow actual = CompressedRow.Zero(rowCount, columnCount);
            // Assert
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(0d, actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static property <see cref="CompressedRow.Identity"/>.
        /// </summary>
        /// <param name="size"> Size for the <see cref="CompressedRow"/> (i.e. its row and column count). </param>
        [TestMethod("Static Identity")]
        [DataRow(3, DisplayName = "Size 3")]
        [DataRow(10, DisplayName = "Size 10")]
        [DataRow(25, DisplayName = "Size 25")]
        public void Static_Identity(int size)
        {
            // Arrange

            // Act
            CompressedRow actual = CompressedRow.Identity(size);
            // Assert
            for (int i_R = 0; i_R < size; i_R++)
            {
                for (int i_C = 0; i_C < size; i_C++)
                {
                    if (i_R == i_C) { Assert.AreEqual(1d, actual.At(i_R, i_C), Settings.AbsolutePrecision); }
                    else { Assert.AreEqual(0d, actual.At(i_R, i_C), Settings.AbsolutePrecision); }
                }
            }
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Add(CompressedRow, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,CompressedRow)")]
        public void Static_Add_CompressedRow_CompressedRow()
        {
            // Arrange
            CompressedRow left = MatrixA;
            CompressedRow right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedRow actual = CompressedRow.Add(left, right);

            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Subtract(CompressedRow, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,CompressedRow)")]
        public void Static_Subtract_CompressedRow_CompressedRow()
        {
            // Arrange
            CompressedRow left = MatrixA;
            CompressedRow right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedRow actual = CompressedRow.Subtract(left, right);

            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,CompressedRow)")]
        public void Static_Multiply_CompressedRow_CompressedRow()
        {
            // Arrange
            CompressedRow left = MatrixC;
            CompressedRow right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedRow actual = CompressedRow.Multiply(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Embedding : CompressedColumn ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Add(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,CompressedColumn)")]
        public void Static_Add_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow left = MatrixA;
            CompressedColumn right = CompressedColumnTest.MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedRow actual = CompressedRow.Add(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Add(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,CompressedRow)")]
        public void Static_Add_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn ccsLeft = CompressedColumnTest.MatrixA;
            CompressedRow right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedRow actual = CompressedRow.Add(ccsLeft, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Subtract(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,CompressedColumn)")]
        public void Static_Subtract_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow left = MatrixA;
            CompressedColumn right = CompressedColumnTest.MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedRow actual = CompressedRow.Subtract(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Subtract(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedColumn,CompressedRow)")]
        public void Static_Subtract_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn left = CompressedColumnTest.MatrixA;
            CompressedRow right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedRow actual = CompressedRow.Subtract(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,CompressedColumn)")]
        public void Static_Multiply_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow left = MatrixC;
            CompressedColumn right = CompressedColumnTest.MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedRow actual = CompressedRow.Multiply(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,CompressedRow)")]
        public void Static_Multiply_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn left = CompressedColumnTest.MatrixC;
            CompressedRow right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedRow actual = CompressedRow.Multiply(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Embedding : SparseMatrix ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Add(CompressedRow, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,SparseMatrix)")]
        public void Static_Add_CompressedRow_SparseMatrix()
        {
            // Arrange
            CompressedRow left = MatrixA;

            SparseMatrix crsRight = MatrixB;
            SparseMatrix ccsRight = CompressedColumnTest.MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedRow actualFromCrs = CompressedRow.Add(left, crsRight);
            CompressedRow actualFromCcs = CompressedRow.Add(left, ccsRight);
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Add(SparseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(SparseMatrix,CompressedRow)")]
        public void Static_Add_SparseMatrix_CompressedRow()
        {
            // Arrange
            SparseMatrix crsLeft = MatrixA;
            SparseMatrix ccsLeft = CompressedColumnTest.MatrixA;

            CompressedRow right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedRow actualFromCrs = CompressedRow.Add(crsLeft, right);
            CompressedRow actualFromCcs = CompressedRow.Add(ccsLeft, right);
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Subtract(CompressedRow, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,SparseMatrix)")]
        public void Static_Subtract_CompressedRow_SparseMatrix()
        {
            // Arrange
            CompressedRow left = MatrixA;

            SparseMatrix crsRight = MatrixB;
            SparseMatrix ccsright = CompressedColumnTest.MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedRow actualFromCrs = CompressedRow.Subtract(left, crsRight);
            CompressedRow actualFromCcs = CompressedRow.Subtract(left, ccsright);
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Subtract(SparseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(SparseMatrix,CompressedRow)")]
        public void Static_Subtract_SparseMatrix_CompressedRow()
        {
            // Arrange
            SparseMatrix crsLeft = MatrixA;
            SparseMatrix ccsLeft = CompressedColumnTest.MatrixA;

            CompressedRow right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedRow actualFromCrs = CompressedRow.Subtract(crsLeft, right);
            CompressedRow actualFromCcs = CompressedRow.Subtract(ccsLeft, right);
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,SparseMatrix)")]
        public void Static_Multiply_CompressedRow_SparseMatrix()
        {
            // Arrange
            CompressedRow left = MatrixC;

            SparseMatrix crsRight = MatrixD;
            SparseMatrix ccsRight = CompressedColumnTest.MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedRow actualFromCrs = CompressedRow.Multiply(left, crsRight);
            CompressedRow actualFromCcs = CompressedRow.Multiply(left, ccsRight);
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(SparseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,CompressedRow)")]
        public void Static_Multiply_SparseMatrix_CompressedRow()
        {
            // Arrange
            SparseMatrix crsLeft = MatrixC;
            SparseMatrix ccsLeft = CompressedColumnTest.MatrixC;

            CompressedRow right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedRow actualFromCrs = CompressedRow.Multiply(crsLeft, right);
            CompressedRow actualFromCcs = CompressedRow.Multiply(ccsLeft, right);
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,double)")]
        public void Static_Multiply_CompressedRow_double()
        {
            // Arrange
            CompressedRow operand = MatrixE;
            double factor = -2.5;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { -10d, -7.5 }, { -5d, 12.5 }, { 10d, -2.5 } };
            //Act
            CompressedRow actual = CompressedRow.Multiply(operand, factor);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(double, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,CompressedRow)")]
        public void Static_Multiply_double_CompressedRow()
        {
            // Arrange
            double factor = -2.5;
            CompressedRow operand = MatrixE;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { -10d, -7.5 }, { -5d, 12.5 }, { 10d, -2.5 } };
            //Act
            CompressedRow actual = CompressedRow.Multiply(factor, operand);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Divide(CompressedRow, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(CompressedRow,double)")]
        public void Static_Divide_CompressedRow_double()
        {
            // Arrange
            CompressedRow operand = new CompressedRow(2, 1, new Stor.DictionaryOfKeys(
                new double[] { 4d, 3d }, new int[] { 0, 1 }, new int[] { 0, 0 }));
            double divisor = -2.0;

            int rowCount = 2, columnCount = 1;
            double[,] expected = new double[,] { { -2d }, { -1.5 } };
            //Act
            CompressedRow actual = CompressedRow.Divide(operand, divisor);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,Vector)")]
        public void Static_Multiply_CompressedRow_Vector()
        {
            // Arrange
            CompressedRow matrix = MatrixE;

            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            //Act
            Vector actualFromDense = CompressedRow.Multiply(matrix, denseVector);
            Vector actualFromSparse = CompressedRow.Multiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(size, actualFromDense.Size);
            Assert.AreEqual(size, actualFromSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,DenseVector)")]
        public void Static_Multiply_CompressedRow_DenseVector()
        {
            // Arrange
            CompressedRow matrix = MatrixE;
            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[3] { 10.0, -34.0, 14.0 };
            //Act
            DenseVector actual = CompressedRow.Multiply(matrix, denseVector);
            // Assert
            Assert.AreEqual(size, actual.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.Multiply(CompressedRow, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,SparseVector)")]
        public void Static_Multiply_CompressedRow_SparseVector()
        {
            // Arrange
            CompressedRow matrix = MatrixE;
            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[3] { 10.0, -34.0, 14.0 };
            // Act
            SparseVector actual = CompressedRow.Multiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(size, actual.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="CompressedRow.TransposeMultiply(CompressedRow, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedRow,Vector)")]
        public void Static_TransposeMultiply_CompressedRow_Vector()
        {
            // Arrange
            CompressedRow matrix = MatrixE;

            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            DenseVector actualFromDense = CompressedRow.TransposeMultiply(matrix, denseVector);
            SparseVector actualFromSparse = CompressedRow.TransposeMultiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(size, actualFromDense.Size);
            Assert.AreEqual(size, actualFromSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.TransposeMultiply(CompressedRow, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedRow,DenseVector)")]
        public void Static_TransposeMultiply_CompressedRow_DenseVector()
        {
            // Arrange
            CompressedRow matrix = MatrixE;
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            DenseVector actual = CompressedRow.TransposeMultiply(matrix, denseVector);
            // Assert
            Assert.AreEqual(size, actual.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedRow.TransposeMultiply(CompressedRow, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedRow,SparseVector)")]
        public void Static_TransposeMultiply_CompressedRow_SparseVector()
        {
            // Arrange
            CompressedRow matrix = MatrixE;
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            SparseVector actual = CompressedRow.TransposeMultiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(size, actual.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="CompressedRow.RowPointer(int)"/>.
        /// </summary>
        [TestMethod("Property RowPointer(int)")]
        public void RowPointer_Int()
        {
            // Arrange
            int[] crsRowPointers = new int[3] { 0, 3, 6 };
            List<int> crsColumnIndices = new List<int> { 0, 1, 2, 0, 1, 2 };
            List<double> crsValues = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
            CompressedRow matrix = new CompressedRow(2, 3, ref crsRowPointers, ref crsColumnIndices, ref crsValues);
            //Act

            // Assert
            Assert.AreEqual(0, matrix.RowPointer(0));
            Assert.AreEqual(3, matrix.RowPointer(1));
            Assert.AreEqual(6, matrix.RowPointer(2));
        }

        /// <summary>
        /// Tests the method <see cref="CompressedRow.ColumnIndex(int)"/>.
        /// </summary>
        [TestMethod("Property ColumnIndex(int)")]
        public void ColumnIndex_Int()
        {
            // Arrange
            int[] crsRowPointers = new int[3] { 0, 3, 6 };
            List<int> crsColumnIndices = new List<int> { 0, 1, 2, 0, 1, 2 };
            List<double> crsValues = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
            CompressedRow matrix = new CompressedRow(2, 3, ref crsRowPointers, ref crsColumnIndices, ref crsValues);
            //Act

            // Assert
            Assert.AreEqual(0, matrix.ColumnIndex(0));
            Assert.AreEqual(1, matrix.ColumnIndex(1));
            Assert.AreEqual(2, matrix.ColumnIndex(2));
            Assert.AreEqual(0, matrix.ColumnIndex(3));
            Assert.AreEqual(1, matrix.ColumnIndex(4));
            Assert.AreEqual(2, matrix.ColumnIndex(5));
        }

        /// <summary>
        /// Tests the method <see cref="CompressedRow.NonZero(int)"/>.
        /// </summary>
        [TestMethod("Property NonZero(int)")]
        public void NonZero_Int()
        {
            // Arrange
            int[] crsRowPointers = new int[3] { 0, 3, 6 };
            List<int> crsColumnIndices = new List<int> { 0, 1, 2, 0, 1, 2 };
            List<double> crsValues = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
            CompressedRow matrix = new CompressedRow(2, 3, ref crsRowPointers, ref crsColumnIndices, ref crsValues);
            //Act

            // Assert
            Assert.AreEqual(1.0, matrix.NonZero(0), Settings.AbsolutePrecision);
            Assert.AreEqual(2.0, matrix.NonZero(1), Settings.AbsolutePrecision);
            Assert.AreEqual(3.0, matrix.NonZero(2), Settings.AbsolutePrecision);
            Assert.AreEqual(4.0, matrix.NonZero(3), Settings.AbsolutePrecision);
            Assert.AreEqual(5.0, matrix.NonZero(4), Settings.AbsolutePrecision);
            Assert.AreEqual(6.0, matrix.NonZero(5), Settings.AbsolutePrecision);
        }

        #endregion


        #region Overrides

        /******************** Matrix ********************/

        /// <summary>
        /// Tests the method <see cref="Matrix.ToArray()"/>.
        /// </summary>
        [TestMethod("Method ToArray()")]
        public void ToArray()
        {
            // Arrange
            CompressedRow matrix = MatrixE;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { 4.0, 3.0 }, { 2.0, -5.0 }, { -4.0, 1.0 } };
            // Act 
            double[,] actual = matrix.ToArray();
            // Assert
            Assert.AreEqual(rowCount, actual.GetLength(0));
            Assert.AreEqual(columnCount, actual.GetLength(1));

            for (int i_R = 0; i_R < expected.GetLength(0); i_R++)
            {
                for (int i_C = 0; i_C < expected.GetLength(1); i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="CompressedRow.At(int, int)"/>.
        /// </summary>
        [TestMethod("Property At[int,int]")]
        public void At_Int_Int()
        {
            // Arrange
            CompressedRow matrix = MatrixA;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 1d, 2d, 3d }, { 5d, 6d, 7d } };
            //Act

            // Assert
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], matrix.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        #endregion
    }
}
