using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;
using MathNet.Numerics;


namespace BRIDGES.Tests.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class testing the members of the <see cref="DenseMatrix"/> class.
    /// </summary>
    [TestClass]
    public class DenseMatrixTest
    {
        #region Test Fields

        /// <summary>
        /// A dense matrix.
        /// </summary>
        internal static DenseMatrix MatrixA = new DenseMatrix(2, 3, new double[] { 1d, 2d, 3d, 5d, 6d, 7d });

        /// <summary>
        /// A dense matrix.
        /// </summary>
        internal static DenseMatrix MatrixB = new DenseMatrix(2, 3, new double[] { 4d, 3d, 2d, 5d, 4d, 3d });


        /// <summary>
        /// A dense matrix.
        /// </summary>
        internal static DenseMatrix MatrixC = new DenseMatrix(6, 5, new double[] { 
            /* Row 0 */ 0d, 1.5, 0d, 1.25, 0d, 0d, /* Row 1 */ 0d, 0d, 6.75, 0d, /* Row 2 */ 0d, 0d, 2d, 0d, 0d,
            /* Row 3 */ 0d, 0d, 0d, 5.5, 0d, /* Row 4 */ 0d, 4d, 3.5, 2.25, 0d, /* Row 5 */ 0d, 0d, 0d, 0d, 7.25 });

        /// <summary>
        /// A dense matrix.
        /// </summary>
        internal static DenseMatrix MatrixD =  new DenseMatrix(5, 3, new double[] { /* Row 0 */ 3.5, 0d, 0d,
            /* Row 1 */ 0d, 1.5, 0d, /* Row 2 */ 5d, 0d, 0d, /* Row 3 */ 2d, 3d, 4d, /* Row 4 */ 0.5, 2.5, 0d });


        /// <summary>
        /// A dense matrix.
        /// </summary>
        internal static DenseMatrix MatrixE = new DenseMatrix(3, 2, new double[] { 4.0, 3.0, 2.0, -5.0, -4.0, 1.0 });

        #endregion


        #region Behavior

        /// <summary>
    /// Tests that <see cref="DenseMatrix"/> is reference type.
    /// </summary>
        [TestMethod("Behavior IsReference")]
        public void DenseMatrix_IsReference()
        {
            // Arrange
            DenseMatrix matrix = new DenseMatrix(2, 2, new double[] { 1.0, 2.0, 3.0, 4.0 });
            DenseMatrix otherMatrix = new DenseMatrix(1, 2, new double[] { 2.0, 4.0 });
            //Act
            otherMatrix = matrix;
            // Assert
            Assert.AreEqual(matrix, otherMatrix);
            Assert.AreSame(matrix, otherMatrix);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="DenseMatrix.RowCount"/> property.
        /// </summary>
        [TestMethod("Property ColumnCount")]
        public void RowCount()
        {
            // Arrange
            DenseMatrix matrix = MatrixA;
            int expected = 2;
            // Act
            int actual = matrix.RowCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the <see cref="DenseMatrix.ColumnCount"/> property.
        /// </summary>
        [TestMethod("Property RowCount")]
        public void ColumnCount()
        {
            // Arrange
            DenseMatrix matrix = MatrixA;
            int expected = 3;
            // Act
            int actual = matrix.ColumnCount;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the <see cref="DenseMatrix"/> indexer property.
        /// </summary>
        [TestMethod("Property this[int,int]")]
        public void Index_Int_Int()
        {
            // Arrange
            DenseMatrix matrix = MatrixA;
            double[] expected = new double[] { 1d, 2d, 3d, 5d, 6d, 7d };
            //Act

            // Assert
            int rowCount = matrix.RowCount, columnCount = matrix.ColumnCount;
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[(i_R * columnCount) + i_C], matrix[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Tests the initialisation of the <see cref="DenseMatrix"/> from its number of row and column.
        /// </summary>
        [TestMethod("Constructor(Int,Int)")]
        public void Constructor_Int_Int()
        {
            // Arrange

            // Act
            DenseMatrix matrix = new DenseMatrix(2, 2);
            // Assert
            int rowCount = matrix.RowCount, columnCount = matrix.ColumnCount;
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(0d, matrix[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the initialisation of the <see cref="DenseMatrix"/> from its size and components.
        /// </summary>
        [TestMethod("Constructor(Int,Int,double)")]
        public void Constructor_Int_Int_double()
        {
            // Arrange
            double[] expected = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
            // Act
            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });
            // Assert
            int rowCount = matrix.RowCount, columnCount = matrix.ColumnCount;
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[(i_R * columnCount) + i_C], matrix[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Tests the static property <see cref="DenseMatrix.Zero"/>.
        /// </summary>
        /// <param name="rowCount"> The number of row for the <see cref="DenseMatrix"/>. </param>
        /// <param name="columnCount"> The number of column for the <see cref="DenseMatrix"/>. </param>
        [TestMethod("Static Zero")]
        [DataRow(2, 3, DisplayName = "MatrixA Dimensions")]
        [DataRow(3, 2, DisplayName = "MatrixE Dimensions")]
        public void Static_Zero(int rowCount, int columnCount)
        {
            // Arrange

            // Act
            DenseMatrix actual = DenseMatrix.Zero(rowCount, columnCount);
            // Assert
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(0d, actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static property <see cref="DenseMatrix.Identity"/>.
        /// </summary>
        /// <param name="size"> Size for the <see cref="DenseMatrix"/> (i.e. its row and column count). </param>
        [TestMethod("Static Identity")]
        [DataRow(3, DisplayName = "Size 3")]
        [DataRow(10, DisplayName = "Size 10")]
        [DataRow(25, DisplayName = "Size 25")]
        public void Static_Identity(int size)
        {
            // Arrange

            // Act
            DenseMatrix actual = DenseMatrix.Identity(size);
            // Assert
            for (int i_R = 0; i_R < size; i_R++)
            {
                for (int i_C = 0; i_C < size; i_C++)
                {
                    if (i_R == i_C) { Assert.AreEqual(1d, actual[i_R, i_C], Settings.AbsolutePrecision); }
                    else { Assert.AreEqual(0d, actual[i_R, i_C], Settings.AbsolutePrecision); }
                }
            }
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,DenseMatrix)")]
        public void Static_Add_DenseMatrix_DenseMatrix()
        {
            // Arrange
            DenseMatrix left = MatrixA;
            DenseMatrix right = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Add(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
            
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,DenseMatrix)")]
        public void Static_Subtract_DenseMatrix_DenseMatrix()
        {
            // Arrange
            DenseMatrix left = MatrixA;
            DenseMatrix right = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Subtract(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,DenseMatrix)")]
        public void Static_Multiply_DenseMatrix_DenseMatrix()
        {
            // Arrange
            DenseMatrix left = MatrixC;
            DenseMatrix right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actual = DenseMatrix.Multiply(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Embedding : CompressedColumn ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,CompressedColumn)")]
        public void Static_Add_DenseMatrix_CompressedColumn()
        {
            // Arrange
            DenseMatrix left = MatrixA;
            CompressedColumn right = Sparse.CompressedColumnTest.MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Add(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(CompressedColumn, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedColumn,DenseMatrix)")]
        public void Static_Add_CompressedColumn_DenseMatrix()
        {
            // Arrange
            CompressedColumn left = Sparse.CompressedColumnTest.MatrixA;
            DenseMatrix right = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Add(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,CompressedColumn)")]
        public void Static_Subtract_DenseMatrix_CompressedColumn()
        {
            // Arrange
            DenseMatrix left = MatrixA;
            CompressedColumn right = Sparse.CompressedColumnTest.MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Subtract(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(CompressedColumn, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedColumn,DenseMatrix)")]
        public void Static_Subtract_CompressedColumn_DenseMatrix()
        {
            // Arrange
            CompressedColumn left = Sparse.CompressedColumnTest.MatrixA;
            DenseMatrix right = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Subtract(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, CompressedColumn)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,CompressedColumn)")]
        public void Static_Multiply_DenseMatrix_CompressedColumn()
        {
            // Arrange
            DenseMatrix left = MatrixC;
            CompressedColumn right = Sparse.CompressedColumnTest.MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actual = DenseMatrix.Multiply(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(CompressedColumn, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedColumn,DenseMatrix)")]
        public void Static_Multiply_CompressedColumn_DenseMatrix()
        {
            // Arrange

            CompressedColumn left = Sparse.CompressedColumnTest.MatrixC;
            DenseMatrix right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actual = DenseMatrix.Multiply(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Embedding : CompressedRow ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,CompressedRow)")]
        public void Static_Add_DenseMatrix_CompressedRow()
        {
            // Arrange
            DenseMatrix left = MatrixA;
            CompressedRow right = Sparse.CompressedRowTest.MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Add(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(CompressedRow, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(CompressedRow,DenseMatrix)")]
        public void Static_Add_CompressedRow_DenseMatrix()
        {
            // Arrange
            CompressedRow left = Sparse.CompressedRowTest.MatrixA;
            DenseMatrix right = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Add(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,CompressedRow)")]
        public void Static_Subtract_DenseMatrix_CompressedRow()
        {
            // Arrange
            DenseMatrix left = MatrixA;
            CompressedRow right = Sparse.CompressedRowTest.MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Subtract(left, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(CompressedRow, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(CompressedRow,DenseMatrix)")]
        public void Static_Subtract_CompressedRow_DenseMatrix()
        {
            // Arrange
            CompressedRow crsLeft = Sparse.CompressedRowTest.MatrixA;
            DenseMatrix right = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });
            //Act
            DenseMatrix actual = DenseMatrix.Subtract(crsLeft, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, CompressedRow)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,CompressedRow)")]
        public void Static_Multiply_DenseMatrix_CompressedRow()
        {
            // Arrange
            DenseMatrix left = MatrixC;
            CompressedRow right = Sparse.CompressedRowTest.MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actual = DenseMatrix.Multiply(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(CompressedRow, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(CompressedRow,DenseMatrix)")]
        public void Static_Multiply_CompressedRow_DenseMatrix()
        {
            // Arrange
            CompressedRow left = Sparse.CompressedRowTest.MatrixC;
            DenseMatrix right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actual = DenseMatrix.Multiply(left, right);
            // Assert
            Assert.AreEqual(rowCount, actual.RowCount);
            Assert.AreEqual(columnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Embedding : Sparse Matrix ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,SparseMatrix)")]
        public void Static_Add_DenseMatrix_SparseMatrix()
        {
            // Arrange
            DenseMatrix left = MatrixA;

            SparseMatrix crsRight = Sparse.CompressedRowTest.MatrixB;
            SparseMatrix ccsRight = Sparse.CompressedColumnTest.MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Add(left, crsRight);
            DenseMatrix actualFromCcs = DenseMatrix.Add(left, ccsRight);
            // Assert
            Assert.AreEqual(expected.RowCount, actualFromCrs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromCcs.RowCount);

            Assert.AreEqual(expected.ColumnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(SparseMatrix,DenseMatrix)")]
        public void Static_Add_SparseMatrix_DenseMatrix()
        {
            // Arrange

            SparseMatrix crsLeft = Sparse.CompressedRowTest.MatrixA;
            SparseMatrix ccsLeft = Sparse.CompressedColumnTest.MatrixA;

            DenseMatrix right = MatrixB;

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Add(crsLeft, right);
            DenseMatrix actualFromCcs = DenseMatrix.Add(ccsLeft, right);
            // Assert
            Assert.AreEqual(matrix.RowCount, actualFromCrs.RowCount);
            Assert.AreEqual(matrix.RowCount, actualFromCcs.RowCount);

            Assert.AreEqual(matrix.ColumnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.AreEqual(matrix[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(matrix[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,SparseMatrix)")]
        public void Static_Subtract_DenseMatrix_SparseMatrix()
        {
            // Arrange
            DenseMatrix left = MatrixA;

            SparseMatrix crsRight = Sparse.CompressedRowTest.MatrixB;
            SparseMatrix ccsRight = Sparse.CompressedColumnTest.MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Subtract(left, crsRight);
            DenseMatrix actualFromCcs = DenseMatrix.Subtract(left, ccsRight);

            // Assert
            Assert.AreEqual(expected.RowCount, actualFromCrs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromCcs.RowCount);

            Assert.AreEqual(expected.ColumnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(SparseMatrix,DenseMatrix)")]
        public void Static_Subtract_SparseMatrix_DenseMatrix()
        {
            // Arrange
            SparseMatrix crsLeft = Sparse.CompressedRowTest.MatrixA;
            SparseMatrix ccsLeft = Sparse.CompressedColumnTest.MatrixA;

            DenseMatrix right = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Subtract(crsLeft, right);
            DenseMatrix actualFromCcs = DenseMatrix.Subtract(ccsLeft, right);
            // Assert
            Assert.AreEqual(expected.RowCount, actualFromCrs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromCcs.RowCount);

            Assert.AreEqual(expected.ColumnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,SparseMatrix)")]
        public void Static_Multiply_DenseMatrix_SparseMatrix()
        {
            // Arrange
            DenseMatrix left = MatrixC;

            SparseMatrix crsRight = Sparse.CompressedRowTest.MatrixD;
            SparseMatrix ccsRight = Sparse.CompressedColumnTest.MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Multiply(left, crsRight);
            DenseMatrix actualFromCcs = DenseMatrix.Multiply(left, ccsRight);
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,DenseMatrix)")]
        public void Static_Multiply_SparseMatrix_DenseMatrix()
        {
            // Arrange
            SparseMatrix crsLeft = Sparse.CompressedRowTest.MatrixC;
            SparseMatrix ccsLeft = Sparse.CompressedColumnTest.MatrixC;

            DenseMatrix right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Multiply(crsLeft, right);
            DenseMatrix actualFromCcs = DenseMatrix.Multiply(ccsLeft, right);
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Embedding : Matrix ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(DenseMatrix, BRIDGES.LinearAlgebra.Matrices.Matrix)"/>.
        /// </summary>
        [TestMethod("Static Add(DenseMatrix,Matrix)")]
        public void Static_Add_DenseMatrix_Matrix()
        {
            // Arrange
            DenseMatrix left = MatrixA;

            Matrix crsRight = Sparse.CompressedRowTest.MatrixB;
            Matrix ccsright = Sparse.CompressedColumnTest.MatrixB;
            Matrix denseRight = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Add(left, crsRight);
            DenseMatrix actualFromCcs = DenseMatrix.Add(left, ccsright);
            DenseMatrix actualFromDense = DenseMatrix.Add(left, denseRight);

            // Assert
            Assert.AreEqual(expected.RowCount, actualFromCrs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromCcs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromDense.RowCount);

            Assert.AreEqual(expected.ColumnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromCcs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromDense.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Add(Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(Matrix,DenseMatrix)")]
        public void Static_Add_Matrix_DenseMatrix()
        {
            // Arrange
            Matrix crsLeft = Sparse.CompressedRowTest.MatrixA;
            Matrix ccsLeft = Sparse.CompressedColumnTest.MatrixA;
            Matrix denseLeft = MatrixA;

            DenseMatrix right = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Add(crsLeft, right);
            DenseMatrix actualFromCcs = DenseMatrix.Add(ccsLeft, right);
            DenseMatrix actualFromDense = DenseMatrix.Add(denseLeft, right);

            // Assert
            Assert.AreEqual(expected.RowCount, actualFromCrs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromCcs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromDense.RowCount);

            Assert.AreEqual(expected.ColumnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromCcs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromDense.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense[i_R, i_C], Settings.AbsolutePrecision);
                }
            }

        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(DenseMatrix, BRIDGES.LinearAlgebra.Matrices.Matrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(DenseMatrix,Matrix)")]
        public void Static_Subtract_DenseMatrix_Matrix()
        {
            // Arrange
            DenseMatrix left = MatrixA;

            Matrix crsRight = Sparse.CompressedRowTest.MatrixB;
            Matrix ccsright = Sparse.CompressedColumnTest.MatrixB;
            Matrix denseRight = MatrixB;

            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });

            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Subtract(left, crsRight);
            DenseMatrix actualFromCcs = DenseMatrix.Subtract(left, ccsright);
            DenseMatrix actualFromDense = DenseMatrix.Subtract(left, denseRight);

            // Assert
            Assert.AreEqual(expected.RowCount, actualFromCrs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromCcs.RowCount);
            Assert.AreEqual(expected.RowCount, actualFromDense.RowCount);

            Assert.AreEqual(expected.ColumnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromCcs.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, actualFromDense.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Subtract(BRIDGES.LinearAlgebra.Matrices.Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Matrix,DenseMatrix)")]
        public void Static_Subtract_Matrix_DenseMatrix()
        {
            // Arrange
            Matrix crsLeft = Sparse.CompressedRowTest.MatrixA;
            Matrix ccsLeft = Sparse.CompressedColumnTest.MatrixA;
            Matrix denseLeft = MatrixA;

            DenseMatrix right = MatrixB;

            DenseMatrix matrix = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Subtract(crsLeft, right);
            DenseMatrix actualFromCcs = DenseMatrix.Subtract(ccsLeft, right);
            DenseMatrix actualFromDense = DenseMatrix.Subtract(denseLeft, right);

            // Assert
            Assert.AreEqual(matrix.RowCount, actualFromCrs.RowCount);
            Assert.AreEqual(matrix.RowCount, actualFromCcs.RowCount);
            Assert.AreEqual(matrix.RowCount, actualFromDense.RowCount);

            Assert.AreEqual(matrix.ColumnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, actualFromCcs.ColumnCount);
            Assert.AreEqual(matrix.ColumnCount, actualFromDense.ColumnCount);

            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < matrix.ColumnCount; i_C++)
                {
                    Assert.AreEqual(matrix[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(matrix[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(matrix[i_R, i_C], actualFromDense[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, BRIDGES.LinearAlgebra.Matrices.Matrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,Matrix)")]
        public void Static_Multiply_DenseMatrix_Matrix()
        {
            // Arrange
            DenseMatrix left = MatrixC;

            Matrix crsRight = Sparse.CompressedRowTest.MatrixD;
            Matrix ccsRight = Sparse.CompressedColumnTest.MatrixD;
            Matrix denseRight = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Multiply(left, crsRight);
            DenseMatrix actualFromCcs = DenseMatrix.Multiply(left, ccsRight);
            DenseMatrix actualFromDense = DenseMatrix.Multiply(left, denseRight);

            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);
            Assert.AreEqual(rowCount, actualFromDense.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromDense.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(BRIDGES.LinearAlgebra.Matrices.Matrix, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,DenseMatrix)")]
        public void Static_Multiply_Matrix_DenseMatrix()
        {
            // Arrange
            Matrix crsLeft = Sparse.CompressedRowTest.MatrixC;
            Matrix ccsLeft = Sparse.CompressedColumnTest.MatrixC;
            Matrix denseLeft = MatrixC;

            DenseMatrix right = MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            DenseMatrix actualFromCrs = DenseMatrix.Multiply(crsLeft, right);
            DenseMatrix actualFromCcs = DenseMatrix.Multiply(ccsLeft, right);
            DenseMatrix actualFromDense = DenseMatrix.Multiply(denseLeft, right);

            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);
            Assert.AreEqual(rowCount, actualFromDense.RowCount);

            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromDense.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,double)")]
        public void Static_Multiply_DenseMatrix_double()
        {
            // Arrange
            DenseMatrix operand = MatrixE;
            double factor = -2.5;

            DenseMatrix expected = new DenseMatrix(3, 2, new double[] { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });

            //Act
            DenseMatrix actual = DenseMatrix.Multiply(operand, factor);

            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(double, DenseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,DenseMatrix)")]
        public void Static_Multiply_double_DenseMatrix()
        {
            // Arrange
            double factor = -2.5;
            DenseMatrix operand = MatrixE;

            DenseMatrix expected = new DenseMatrix(3, 2, new double[] { -10.0, -7.5, -5.0, 12.5, 10.0, -2.5 });
            //Act
            DenseMatrix actual = DenseMatrix.Multiply(factor, operand);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actual[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Divide(DenseMatrix, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(DenseMatrix,double)")]
        public void Static_Divide_DenseMatrix_double()
        {
            // Arrange
            DenseMatrix operand = new DenseMatrix(2, 1, new double[] { 4.0, 3.0 });
            double divisor = -2.0;

            DenseMatrix expected = new DenseMatrix(2, 1, new double[] { -2.0, -1.5 });
            //Act
            DenseMatrix actual = DenseMatrix.Divide(operand, divisor);
            // Assert
            Assert.AreEqual(expected.RowCount, actual.RowCount);
            Assert.AreEqual(expected.ColumnCount, actual.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected.At(i_R, i_C), actual.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,Vector)")]
        public void Static_Multiply_DenseMatrix_Vector()
        {
            // Arrange
            DenseMatrix matrix = MatrixE;
            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            Vector expected = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });
            //Act
            Vector actualFromDense = DenseMatrix.Multiply(matrix, denseVector);
            Vector actualFromSparse = DenseMatrix.Multiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(expected.Size, actualFromDense.Size);
            Assert.AreEqual(expected.Size, actualFromSparse.Size);

            for (int i_R = 0; i_R < expected.Size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,DenseVector)")]
        public void Static_Multiply_DenseMatrix_DenseVector()
        {
            // Arrange
            DenseMatrix matrix = MatrixE;
            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            DenseVector expected = new DenseVector(new double[3] { 10.0, -34.0, 14.0 });
            //Act
            DenseVector actual = DenseMatrix.Multiply(matrix, denseVector);
            // Assert
            Assert.AreEqual(expected.Size, actual.Size);

            for (int i_R = 0; i_R < expected.Size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.Multiply(DenseMatrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(DenseMatrix,SparseVector)")]
        public void Static_Multiply_DenseMatrix_SparseVector()
        {
            // Arrange
            DenseMatrix matrix = MatrixE;
            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            SparseVector expected = new SparseVector(3, new int[3] { 0, 1, 2 }, new double[3] { 10.0, -34.0, 14.0 });
            // Act
            DenseVector actual = DenseMatrix.Multiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(expected.Size, actual.Size);

            for (int i_R = 0; i_R < expected.Size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.TransposeMultiply(DenseMatrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(DenseMatrix,Vector)")]
        public void Static_TransposeMultiply_DenseMatrix_Vector()
        {
            // Arrange
            DenseMatrix matrix = MatrixE;
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector expected = new DenseVector(new double[2] { -32.0, 0.0 });
            //Act
            DenseVector actualFromDense = DenseMatrix.TransposeMultiply(matrix, denseVector);
            DenseVector actualFromSparse = DenseMatrix.TransposeMultiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(expected.Size, actualFromDense.Size);
            Assert.AreEqual(expected.Size, actualFromSparse.Size);

            for (int i_R = 0; i_R < expected.Size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.TransposeMultiply(DenseMatrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(DenseMatrix,DenseVector)")]
        public void Static_TransposeMultiply_DenseMatrix_DenseVector()
        {
            // Arrange
            DenseMatrix matrix = MatrixE;
            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            DenseVector expected = new DenseVector(new double[2] { -32.0, 0.0 });
            //Act
            DenseVector actual = DenseMatrix.TransposeMultiply(matrix, denseVector);
            // Assert
            Assert.AreEqual(expected.Size, actual.Size);

            for (int i_R = 0; i_R < expected.Size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="DenseMatrix.TransposeMultiply(DenseMatrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(DenseMatrix,SparseVector)")]
        public void Static_TransposeMultiply_DenseMatrix_SparseVector()
        {
            // Arrange
            DenseMatrix matrix = MatrixE;
            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            DenseVector expected = new DenseVector(new double[2] { -32.0, 0.0 });
            //Act
            DenseVector actual = DenseMatrix.TransposeMultiply(matrix, sparseVector);
            // Assert
            Assert.AreEqual(expected.Size, actual.Size);

            for (int i_R = 0; i_R < expected.Size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actual[i_R], Settings.AbsolutePrecision);
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
            DenseMatrix matrix = MatrixE;

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
        /// Tests the method <see cref="DenseMatrix.At(int, int)"/>
        /// </summary>
        [TestMethod("Property At(int,int)")]
        public void At_Int_Int()
        {
            // Arrange
            DenseMatrix matrix = MatrixA;

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
