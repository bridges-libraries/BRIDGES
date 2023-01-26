using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Test.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class testing the members of the <see cref="CompressedColumn"/> class.
    /// </summary>
    [TestClass]
    public class CompressedColumnTest
    {
        #region Test Fields

        /// <summary>
        /// A sparse matrix with compressed column storage.
        /// </summary>
        internal static CompressedColumn MatrixA = new CompressedColumn(2, 3, new int[] { 0, 2, 4, 6 },
            new int[] { 0, 1, 0, 1, 0, 1 }, new double[] { 1d, 5d, 2d, 6d, 3d, 7d });

        /// <summary>
        /// A sparse matrix with compressed column storage.
        /// </summary>
        internal static CompressedColumn MatrixB = new CompressedColumn(2, 3, new int[] { 0, 2, 4, 6 },
            new int[] { 0, 1, 0, 1, 0, 1 }, new double[] { 4d, 5d, 3d, 4d, 2d, 3d });


        /// <summary>
        /// A sparse matrix with compressed column storage.
        /// </summary>
        internal static CompressedColumn MatrixC = new CompressedColumn(6, 5, new int[] { 0, 0, 2, 4, 8, 9 },
            new int[] { 0, 4, 2, 4, 0, 1, 3, 4, 5 }, new double[] { 1.5, 4.0, 2.0, 3.5, 1.25, 6.75, 5.5, 2.25, 7.25 });

        /// <summary>
        /// A sparse matrix with compressed column storage.
        /// </summary>
        internal static CompressedColumn MatrixD = new CompressedColumn(5, 3, new int[] { 0, 4, 7, 8 },
            new int[] { 0, 2, 3, 4, 1, 3, 4, 3 }, new double[] { 3.5, 5.0, 2.0, 0.5, 1.5, 3.0, 2.5, 4.0 });


        /// <summary>
        /// A sparse matrix with compressed column storage.
        /// </summary>
        internal static CompressedColumn MatrixE = new CompressedColumn(3, 2, new int[] { 0, 3, 6 },
                new int[] { 0, 1, 2, 0, 1, 2 }, new double[] { 4.0, 2.0, -4.0, 3.0, -5.0, 1.0 });

        #endregion


        #region Behavior

        /// <summary>
        /// Tests that <see cref="CompressedColumn"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
                public void CompressedColumn_IsReference()
                {
                    // Arrange
                    CompressedColumn matrix = new CompressedColumn(2, 2, new int[3] { 0, 2, 4 }, new int[4] { 0, 1, 0, 1 }, new double[4] { 1.0, 3.0, 2.0, 4.0 });
                    CompressedColumn otherMatrix = new CompressedColumn(1, 2, new int[3] { 0, 1, 2 }, new int[2] { 0, 1 }, new double[2] { 2.0, 4.0 });
                    //Act
                    otherMatrix = matrix;
                    // Assert
                    Assert.AreEqual(matrix, otherMatrix);
                    Assert.AreSame(matrix, otherMatrix);
                }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="CompressedColumn.RowCount"/> property.
        /// </summary>
        [TestMethod("Property RowCount")]
        public void RowCount()
        {
            // Arrange
            CompressedColumn matrix = MatrixA;
            int expected = 2;
            // Act
            int actual = matrix.RowCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the <see cref="CompressedColumn.ColumnCount"/> property.
        /// </summary>
        [TestMethod("Property ColumnCount")]
        public void ColumnCount()
        {
            // Arrange
            CompressedColumn matrix = MatrixA;
            int expected = 3;
            // Act
            int actual = matrix.ColumnCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the <see cref="CompressedColumn"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int]")]
        public void Index_Int()
        {
            // Arrange
            CompressedColumn matrix = MatrixA;
            double[] expected = new double[] { 1d, 5d, 2d, 6d, 3d, 7d };
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
        /// Tests the initialisation of the <see cref="CompressedColumn"/> from its size and values.
        /// </summary>
        [TestMethod("Constructor(Int,Int,DictionaryOfKeys)")]
        public void Constructor_Int_Int_DictionaryOfKeys()
        {
            // Arrange
            Stor.DictionaryOfKeys dok = new Stor.DictionaryOfKeys(2);
            dok.Add(1.0, 0, 1);
            dok.Add(2.0, 1, 0);

            // Act
            CompressedColumn actual = new CompressedColumn(2, 2, dok);

            // Assert
            Assert.AreEqual(0d, actual.At(0, 0), Settings.AbsolutePrecision);
            Assert.AreEqual(1d, actual.At(0, 1) , Settings.AbsolutePrecision);
            Assert.AreEqual(2d, actual.At(1, 0), Settings.AbsolutePrecision);
            Assert.AreEqual(0d, actual.At(1, 1), Settings.AbsolutePrecision);
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="CompressedColumn"/> from its size and components.
        /// </summary>
        [TestMethod("Constructor(Int,Int,double)")]
        public void Constructor_Int_Int_IntArray_IntList_doubleList()
        {
            // Arrange
            int[] columnPointers = new int[4] { 0, 2, 4, 6 };
            int[] rowIndices = new int[6] { 0, 1, 0, 1, 0, 1 };
            double[] values = new double[6] { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 };

            // Act
            CompressedColumn matrix = new CompressedColumn(2, 3, columnPointers, rowIndices, values);

            // Assert
            Assert.AreEqual(1d, matrix.At(0, 0), Settings.AbsolutePrecision);
            Assert.AreEqual(2d, matrix.At(0, 1), Settings.AbsolutePrecision);
            Assert.AreEqual(3d, matrix.At(0, 2), Settings.AbsolutePrecision);
            Assert.AreEqual(4d, matrix.At(1, 0), Settings.AbsolutePrecision);
            Assert.AreEqual(5d, matrix.At(1, 1), Settings.AbsolutePrecision);
            Assert.AreEqual(6d, matrix.At(1, 2), Settings.AbsolutePrecision);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="CompressedColumn.Zero"/>.
        /// </summary>
        /// <param name="rowCount"> The number of row for the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> The number of column for the <see cref="CompressedColumn"/>. </param>
        [TestMethod("Static Zero")]
        [DataRow(2, 3, DisplayName = "MatrixA Dimensions")]
        [DataRow(3, 2, DisplayName = "MatrixE Dimensions")]
        public void Static_Zero(int rowCount, int columnCount)
        {
            // Arrange

            // Act
            CompressedColumn actual = CompressedColumn.Zero(rowCount, columnCount);
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
        /// Tests the static property <see cref="CompressedColumn.Identity"/>.
        /// </summary>
        /// <param name="size"> Size for the <see cref="CompressedColumn"/> (i.e. its row and column count). </param>
        [TestMethod("Static Identity")]
        [DataRow(3, DisplayName = "Size 3")]
        [DataRow(10, DisplayName = "Size 10")]
        [DataRow(25, DisplayName = "Size 25")]
        public void Static_Identity(int size)
        {
            // Arrange

            // Act
            CompressedColumn actual = CompressedColumn.Identity(size);
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
        /// Tests the static method <see cref="CompressedColumn.Add(CompressedColumn, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,CompressedColumn)")]
        public void Static_Add_CompressedColumn_CompressedColumn()
        {
            // Arrange
            CompressedColumn left = MatrixA;
            CompressedColumn right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedColumn actual = CompressedColumn.Add(left, right);
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
        /// Tests the static method <see cref="CompressedColumn.Subtract(CompressedColumn, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedColumn,CompressedColumn)")]
        public void Static_Subtract_CompressedColumn_CompressedColumn()
        {
            // Arrange
            CompressedColumn left = MatrixA;
            CompressedColumn right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedColumn actual = CompressedColumn.Subtract(left, right);
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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,CompressedColumn)")]
        public void Static_Multiply_CompressedColumn_CompressedColumn()
        {
            // Arrange
            CompressedColumn left = MatrixC;
            CompressedColumn right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedColumn actual = CompressedColumn.Multiply(left, right);
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
        /// Tests the static method <see cref="CompressedColumn.Add(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,CompressedRow)")]
        public void Static_Add_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn left = MatrixA;
            CompressedRow right = CompressedRowTest.MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedColumn actual = CompressedColumn.Add(left, right);
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
        /// Tests the static method <see cref="CompressedColumn.Add(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,CompressedColumn)")]
        public void Static_Add_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow crsLeft = CompressedRowTest.MatrixA;
            CompressedColumn right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedColumn actual = CompressedColumn.Add(crsLeft, right);
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
        /// Tests the static method <see cref="CompressedColumn.Subtract(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedColumn,CompressedRow)")]
        public void Static_Subtract_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn left = MatrixA;
            CompressedRow crsRight = CompressedRowTest.MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedColumn actual = CompressedColumn.Subtract(left, crsRight);
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
        /// Tests the static method <see cref="CompressedColumn.Subtract(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,CompressedColumn)")]
        public void Static_Subtract_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow left = CompressedRowTest.MatrixA;
            CompressedColumn right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedColumn actual = CompressedColumn.Subtract(left, right);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,CompressedRow)")]
        public void Static_Multiply_CompressedColumn_CompressedRow()
        {
            // Arrange
            CompressedColumn left = MatrixC;
            CompressedRow right = CompressedRowTest.MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedColumn actual = CompressedColumn.Multiply(left, right);

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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedRow, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,CompressedColumn)")]
        public void Static_Multiply_CompressedRow_CompressedColumn()
        {
            // Arrange
            CompressedRow left = CompressedRowTest.MatrixC;
            CompressedColumn right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedColumn actual = CompressedColumn.Multiply(left, right);
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


        /******************** Embedding : Sparse Matrix ********************/

        /// <summary>
        /// Tests the static method <see cref="CompressedColumn.Add(CompressedColumn, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,SparseMatrix)")]
        public void Static_Add_CompressedColumn_SparseMatrix()
        {
            // Arrange
            CompressedColumn left = MatrixA;

            SparseMatrix crsRight = CompressedRowTest.MatrixB;
            SparseMatrix ccsRight = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedColumn actualFromCrs = CompressedColumn.Add(left, crsRight);
            CompressedColumn actualFromCcs = CompressedColumn.Add(left, ccsRight);
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
        /// Tests the static method <see cref="CompressedColumn.Add(SparseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(SparseMatrix,CompressedColumn)")]
        public void Static_Add_SparseMatrix_CompressedColumn()
        {
            // Arrange
            SparseMatrix crsLeft = CompressedRowTest.MatrixA;
            SparseMatrix ccsLeft = MatrixA;

            CompressedColumn right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            CompressedColumn actualFromCrs = CompressedColumn.Add(crsLeft, right);
            CompressedColumn actualFromCcs = CompressedColumn.Add(ccsLeft, right);
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
        /// Tests the static method <see cref="CompressedColumn.Subtract(CompressedColumn, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressCompressedColumnedRow,SparseMatrix)")]
        public void Static_Subtract_CompressedColumn_SparseMatrix()
        {
            // Arrange
            CompressedColumn left = MatrixA;
;
            SparseMatrix crsRight = CompressedRowTest.MatrixB;
            SparseMatrix ccsRight = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedColumn actualFromCrs = CompressedColumn.Subtract(left, crsRight);
            CompressedColumn actualFromCcs = CompressedColumn.Subtract(left, ccsRight);
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
        /// Tests the static method <see cref="CompressedColumn.Subtract(SparseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(SparseMatrix,CompressedColumn)")]
        public void Static_Subtract_SparseMatrix_CompressedColumn()
        {
            // Arrange
            SparseMatrix crsLeft = CompressedRowTest.MatrixA;

            SparseMatrix ccsLeft = MatrixA;

            CompressedColumn right = MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            CompressedColumn actualFromCrs = CompressedColumn.Subtract(crsLeft, right);
            CompressedColumn actualFromCcs = CompressedColumn.Subtract(ccsLeft, right);
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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,SparseMatrix)")]
        public void Static_Multiply_CompressedColumn_SparseMatrix()
        {
            // Arrange
            CompressedColumn left = MatrixC;

            SparseMatrix crsRight = CompressedRowTest.MatrixD;
            SparseMatrix ccsRight = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedColumn actualFromCrs = CompressedColumn.Multiply(left, crsRight);
            CompressedColumn actualFromCcs = CompressedColumn.Multiply(left, ccsRight);
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
        /// Tests the static method <see cref="CompressedColumn.Multiply(SparseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,CompressedColumn)")]
        public void Static_Multiply_SparseMatrix_CompressedColumn()
        {
            // Arrange
            SparseMatrix crsLeft = CompressedRowTest.MatrixC;
            SparseMatrix ccsLeft = MatrixC;

            CompressedColumn right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            CompressedColumn actualFromCrs = CompressedColumn.Multiply(crsLeft, right);
            CompressedColumn actualFromCcs = CompressedColumn.Multiply(ccsLeft, right);
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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,double)")]
        public void Static_Multiply_CompressedColumn_double()
        {
            // Arrange
            CompressedColumn operand = MatrixE;
            double factor = -2.5;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { -10d, -7.5 }, { -5d, 12.5 }, { 10d, -2.5 } };
            //Act
            CompressedColumn actual = CompressedColumn.Multiply(operand, factor);
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
        /// Tests the static method <see cref="CompressedColumn.Multiply(double, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,CompressedColumn)")]
        public void Static_Multiply_double_CompressedColumn()
        {
            // Arrange
            double factor = -2.5;
            CompressedColumn operand = MatrixE;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { -10d, -7.5 }, { -5d, 12.5 }, { 10d, -2.5 } };
            //Act
            CompressedColumn actual = CompressedColumn.Multiply(factor, operand);
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
        /// Tests the static method <see cref="CompressedColumn.Divide(CompressedColumn, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(CompressedColumn,double)")]
        public void Static_Divide_CompressedColumn_double()
        {
            // Arrange
            CompressedColumn operand = new CompressedColumn(2, 1, new int[2] { 0, 2 },
                new int[2] { 0, 1 }, new double[2] { 4.0, 3.0 });
            double divisor = -2.0;

            int rowCount = 2, columnCount = 1;
            double[,] expected = new double[,] { { -2d }, { -1.5 } };
            //Act
            CompressedColumn actual = CompressedColumn.Divide(operand, divisor);
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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,Vector)")]
        public void Static_Multiply_CompressedColumn_Vector()
        {
            // Arrange
            CompressedColumn matrix = MatrixE;

            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            //Act
            Vector actualFromDense = CompressedColumn.Multiply(matrix, denseVector);
            Vector actualFromSparse = CompressedColumn.Multiply(matrix, sparseVector);
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
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,DenseVector)")]
        public void Static_Multiply_CompressedColumn_DenseVector()
        {
            // Arrange
            CompressedColumn matrix = MatrixE;
            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            //Act
            DenseVector actual = CompressedColumn.Multiply(matrix, denseVector);
            // Assert
            Assert.AreEqual(size, actual.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedColumn.Multiply(CompressedColumn, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,SparseVector)")]
        public void Static_Multiply_CompressedColumn_SparseVector()
        {
            // Arrange
            CompressedColumn matrix = MatrixE;
            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            // Act
            SparseVector actual = CompressedColumn.Multiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(size, actual.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="CompressedColumn.TransposeMultiply(CompressedColumn, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedColumn,Vector)")]
        public void Static_TransposeMultiply_CompressedColumn_Vector()
        {
            // Arrange
            CompressedColumn matrix = MatrixE;

            Vector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            Vector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            Vector actualFromDense = CompressedColumn.TransposeMultiply(matrix, denseVector);
            Vector actualFromSparse = CompressedColumn.TransposeMultiply(matrix, sparseVector);
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
        /// Tests the static method <see cref="CompressedColumn.TransposeMultiply(CompressedColumn, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedColumn,DenseVector)")]
        public void Static_TransposeMultiply_CompressedColumn_DenseVector()
        {
            // Arrange
            CompressedColumn matrix = MatrixE;
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            DenseVector actual = CompressedColumn.TransposeMultiply(matrix, denseVector);
            // Assert
            Assert.AreEqual(size, actual.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="CompressedColumn.TransposeMultiply(CompressedColumn, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(CompressedColumn,SparseVector)")]
        public void Static_TransposeMultiply_CompressedColumn_SparseVector()
        {
            // Arrange
            CompressedColumn matrix = MatrixE;
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            SparseVector actual = CompressedColumn.TransposeMultiply(matrix, sparseVector);
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
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - Square - A")]
        public void Kernel_Square_A()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(3, 3, new int[] { 0, 3, 6, 9 },
                new int[9] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }, new double[9] { 1.0, 1.0, 1.0, 2.0, 0.0, 0.0, 3.0, 3.0, 3.0 });

            DenseVector[] expected = new DenseVector[1];
            expected[0] = new DenseVector(new double[3] { -3.0, 0.0, 1.0 });

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(expected.Length, kernel.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected expected vectors 
                for (int i_R = 0; i_R < expected.Length; i_R++)
                {
                    DenseVector expectedVector = new DenseVector(expected[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < expectedVector.Size; i++)
                    {
                        numerator += expectedVector[i] * kernelVector[i];
                        denominator += expectedVector[i] * expectedVector[i];
                    }
                    double ratio = numerator / denominator;

                    expectedVector = DenseVector.Multiply(ratio, expectedVector);

                    kernelVector = DenseVector.Subtract(kernelVector, expectedVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.AreEqual(0d, kernelVector[i_C], Settings.AbsolutePrecision, $"Expected {0.0}; Actual {kernelVector[i_C]}. ");
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - Square - B")]
        public void Kernel_Square_B()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(6, 6, new int[7] { 0, 6, 12, 18, 24, 26, 32 },
            new int[32] { 0, 1, 2, 3, 4, 5, /**/ 0, 1, 2, 3, 4, 5, /**/ 0, 1, 2, 3, 4, 5, /**/ 0, 1, 2, 3, 4, 5, /**/ 0, 2, /**/ 0, 1, 2, 3, 4, 5 },
                new double[32]  { 2.0, 1.0, 3.0, 1.0, 1.0, 1.0,/**/ 2.0, 1.0, 3.0, 1.0, 1.0, 1.0,/**/ -4.0, -2.0, -6.0, -2.0, -2.0, -2.0,
                /**/ 3.0, -3.0, -3.0, -3.0, -3.0, -3.0,/**/ -9.0, -6.0, /**/ 1.0, 2.0, 2.0, 2.0, 2.0, 2.0});

            DenseVector[] expected = new DenseVector[3];
            expected[0] = new DenseVector(new double[6] { -1.0, 1.0, 0.0, 0.0, 0.0, 0.0 });
            expected[1] = new DenseVector(new double[6] { 2.0, 0.0, 1.0, 0.0, 0.0, 0.0 });
            expected[2] = new DenseVector(new double[6] { 3.0, 0.0, 0.0, 1.0, 1.0, 0.0 });

            expected = DenseVector.GramSchmidt(expected);

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(expected.Length, kernel.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected expected vectors 
                for (int i_R = 0; i_R < expected.Length; i_R++)
                {
                    DenseVector expectedVector = new DenseVector(expected[i_R].ToArray());

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < expectedVector.Size; i++)
                    {
                        numerator += expectedVector[i] * kernelVector[i];
                        denominator += expectedVector[i] * expectedVector[i];
                    }
                    double ratio = numerator / denominator;

                    expectedVector = DenseVector.Multiply(ratio, expectedVector);

                    kernelVector = DenseVector.Subtract(kernelVector, expectedVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.AreEqual(0d, kernelVector[i_C], Settings.AbsolutePrecision,
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - More Row - A")]
        public void Kernel_HeightRectangular_A()
        {
            CompressedColumn ccs = new CompressedColumn(4, 3, new int[] { 0, 2, 4, 8 },
                new int[8] { 0, 2, 1, 3, 0, 1, 2, 3 }, new double[8] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 });

            DenseVector[] expected = new DenseVector[1];
            expected[0] = new DenseVector(new double[3] { 1.0, 1.0, -1.0 });

            //Act
            var kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(expected.Length, kernel.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected expected vectors 
                for (int i_R = 0; i_R < expected.Length; i_R++)
                {
                    DenseVector expectedVector = new DenseVector(expected[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < expectedVector.Size; i++)
                    {
                        numerator += expectedVector[i] * kernelVector[i];
                        denominator += expectedVector[i] * expectedVector[i];
                    }
                    double ratio = numerator / denominator;

                    expectedVector = DenseVector.Multiply(ratio, expectedVector);

                    kernelVector = DenseVector.Subtract(kernelVector, expectedVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.AreEqual(0d, kernelVector[i_C], Settings.AbsolutePrecision,
                        $"Component {i_C} of vector {i_K} : Expected {0d}; Actual {kernelVector[i_C]}.");
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - More Row - B")]
        public void Kernel_HeightRectangular_B()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(6, 5, new int[] { 0, 5, 10, 13, 16, 21 },
                new int[21]  { 0, 2, 3, 4, 5, /**/ 0, 2, 3, 4, 5, /**/ 1, 3, 5, /**/ 1, 3, 5, /**/ 0, 2, 3, 4, 5 },
                new double[21] { 1.0, 4.0, 1.0, 2.0, 1.0, /**/ 1.0, 2.0, 1.0, 2.0, 1.0, /**/ 1.0, 1.0, 2.0, /**/ -2.0, -2.0, -4.0, /**/ 1.0, 3.0, 1.0, 2.0, 1.0 });

            DenseVector[] expected = new DenseVector[2];
            expected[0] = new DenseVector(new double[5] { 0.0, 0.0, 2.0, 1.0, 0.0 });
            expected[1] = new DenseVector(new double[5] { -0.5, -0.5, 0.0, 0.0, 1.0 });

            expected = DenseVector.GramSchmidt(expected);

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(expected.Length, kernel.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected expected vectors 
                for (int i_R = 0; i_R < expected.Length; i_R++)
                {
                    DenseVector expectedVector = new DenseVector(expected[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < expectedVector.Size; i++)
                    {
                        numerator += expectedVector[i] * kernelVector[i];
                        denominator += expectedVector[i] * expectedVector[i];
                    }
                    double ratio = numerator / denominator;

                    expectedVector = DenseVector.Multiply(ratio, expectedVector);

                    kernelVector = DenseVector.Subtract(kernelVector, expectedVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.AreEqual(0d, kernelVector[i_C], Settings.AbsolutePrecision,
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - More Column - A")]
        public void Kernel_LongRectangular_A()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(2, 4, new int[] { 0, 2, 3, 5, 5 },
                new int[5] { 0, 1, 0, 0, 1 }, new double[5] { 1.0, 2.0, 3.0, 1.0, 2.0 });

            DenseVector[] expected = new DenseVector[2];
            expected[0] = new DenseVector(new double[4] { -1.0, 0.0, 1.0, 0.0 });
            expected[1] = new DenseVector(new double[4] { 0.0, 0.0, 0.0, 1.0 });

            expected = DenseVector.GramSchmidt(expected);

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(expected.Length, kernel.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected expected vectors 
                for (int i_R = 0; i_R < expected.Length; i_R++)
                {
                    DenseVector expectedVector = new DenseVector(expected[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < expectedVector.Size; i++)
                    {
                        numerator += expectedVector[i] * kernelVector[i];
                        denominator += expectedVector[i] * expectedVector[i];
                    }
                    double ratio = numerator / denominator;

                    expectedVector = DenseVector.Multiply(ratio, expectedVector);

                    kernelVector = DenseVector.Subtract(kernelVector, expectedVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.AreEqual(0d, kernelVector[i_C], Settings.AbsolutePrecision, 
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="CompressedColumn.Kernel()"/>.
        /// </summary>
        [TestMethod("Method Kernel() - More Column - B")]
        public void Kernel_LongRectangular_B()
        {
            // Arrange
            CompressedColumn ccs = new CompressedColumn(3, 6, new int[] { 0, 3, 6, 9, 12, 14, 17 },
                new int[17] { 0, 1, 2, /**/ 0, 1, 2, /**/ 0, 1, 2, /**/ 0, 1, 2, /**/ 0, 2, /**/ 0, 1, 2 },
                new double[17] { 2.0, 1.0, 3.0, /**/ 2.0, 1.0, 3.0, /**/ -4.0, -2.0, -6.0, /**/ 3.0, -3.0, -3.0, /**/ -9.0, -6.0, /**/ 1.0, 2.0, 2.0 });

            DenseVector[] expected = new DenseVector[3];
            expected[0] = new DenseVector(new double[6] { -1.0, 1.0, 0.0, 0.0, 0.0, 0.0 });
            expected[1] = new DenseVector(new double[6] { 2.0, 0.0, 1.0, 0.0, 0.0, 0.0 });
            expected[2] = new DenseVector(new double[6] { 3.0, 0.0, 0.0, 1.0, 1.0, 0.0 });

            expected = DenseVector.GramSchmidt(expected);

            // Act
            DenseVector[] kernel = ccs.Kernel();

            // Assert
            Assert.AreEqual(expected.Length, kernel.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].Size, kernel[i].Size);
            }

            // Iterate on each vector of the kernel.
            for (int i_K = 0; i_K < kernel.Length; i_K++)
            {
                DenseVector kernelVector = new DenseVector(kernel[i_K]);

                // Projects the kernel vector on the space generated by the expected expected vectors 
                for (int i_R = 0; i_R < expected.Length; i_R++)
                {
                    DenseVector expectedVector = new DenseVector(expected[i_R]);

                    double numerator = 0.0, denominator = 0.0;
                    for (int i = 0; i < expectedVector.Size; i++)
                    {
                        numerator += expectedVector[i] * kernelVector[i];
                        denominator += expectedVector[i] * expectedVector[i];
                    }
                    double ratio = numerator / denominator;

                    expectedVector = DenseVector.Multiply(ratio, expectedVector);

                    kernelVector = DenseVector.Subtract(kernelVector, expectedVector);
                }

                for (int i_C = 0; i_C < kernelVector.Size; i_C++)
                {
                    Assert.AreEqual(0d, kernelVector[i_C], Settings.AbsolutePrecision,
                        $"Component {i_C} of vector {i_K} : Expected {0.0}; Actual {kernelVector[i_C]}.");
                }
            }
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
            CompressedColumn matrix = MatrixE;

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
        /// Tests the method <see cref="Matrix.At(int, int)"/>.
        /// </summary>
        [TestMethod("Property At(int,int)")]
        public void At_Int_Int()
        {
            // Arrange
            CompressedColumn matrix = MatrixA;

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
