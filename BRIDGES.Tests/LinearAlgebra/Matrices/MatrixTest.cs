using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Tests.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class testing the members of the <see cref="Matrix"/> class.
    /// </summary>
    [TestClass]
    public class MatrixTest
    {
        #region Properties

        /// <summary>
        /// Tests the <see cref="DenseMatrix.ColumnCount"/> property.
        /// </summary>
        [TestMethod("Property RowCount")]
        public void RowCount()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });

            int[] crsRowPointers = new int[3] { 0, 3, 6 };
            List<int> crsColumnIndices = new List<int> { 0, 1, 2, 0, 1, 2 };
            List<double> crsValues = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
            Matrix crsMatrix = new CompressedRow(2, 3, ref crsRowPointers, ref crsColumnIndices, ref crsValues);

            Matrix ccsMatrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 });
            // Act

            // Assert
            Assert.AreEqual(2, denseMatrix.RowCount);
            Assert.AreEqual(2, crsMatrix.RowCount);
            Assert.AreEqual(2, ccsMatrix.RowCount);
        }

        /// <summary>
        /// Tests the <see cref="DenseMatrix.ColumnCount"/> property.
        /// </summary>
        [TestMethod("Property ColumnCount")]
        public void ColumnCount()
        {
            // Arrange
            Matrix denseMatrix = new DenseMatrix(2, 3, new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 });

            int[] crsRowPointers = new int[3] { 0, 3, 6 };
            List<int> crsColumnIndices = new List<int> { 0, 1, 2, 0, 1, 2 };
            List<double> crsValues = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
            Matrix crsMatrix = new CompressedRow(2, 3, ref crsRowPointers, ref crsColumnIndices, ref crsValues);

            Matrix ccsMatrix = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 4.0, 2.0, 5.0, 3.0, 6.0 });

            // Act

            // Assert
            Assert.AreEqual(3, denseMatrix.ColumnCount);
            Assert.AreEqual(3, crsMatrix.ColumnCount);
            Assert.AreEqual(3, ccsMatrix.ColumnCount);
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Tests the static method <see cref="Matrix.Add(Matrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Add(Matrix,Matrix)")]
        public void Static_Add_Matrix_Matrix()
        {
            // Arrange
            Matrix denseLeft = DenseMatrixTest.MatrixA;
            Matrix denseRight = DenseMatrixTest.MatrixB;

            Matrix crsLeft = Sparse.CompressedRowTest.MatrixA;
            Matrix crsRight = Sparse.CompressedRowTest.MatrixB;

            Matrix ccsLeft = Sparse.CompressedColumnTest.MatrixA;
            Matrix ccsRight = Sparse.CompressedColumnTest.MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 5d, 5d, 5d }, { 10d, 10d, 10d } };
            //Act
            Matrix actualFromDenseDense = Matrix.Add(denseLeft, denseRight);
            Matrix actualFromDenseCrs = Matrix.Add(denseLeft, crsRight);
            Matrix actualFromDenseCcs = Matrix.Add(denseLeft, ccsRight);

            Matrix actualFromCrsCrs = Matrix.Add(crsLeft, crsRight);
            Matrix actualFromCrsCcs = Matrix.Add(crsLeft, ccsRight);
            Matrix actualFromCrsDense = Matrix.Add(crsLeft, denseRight);

            Matrix actualFromCcsCcs = Matrix.Add(ccsLeft, ccsRight);
            Matrix actualFromCcsDense = Matrix.Add(ccsLeft, denseRight);
            Matrix actualFromCcsCrs = Matrix.Add(ccsLeft, crsRight);
            // Assert
            Assert.AreEqual(rowCount, actualFromDenseDense.RowCount); Assert.AreEqual(columnCount, actualFromDenseDense.ColumnCount);
            Assert.AreEqual(rowCount, actualFromDenseCrs.RowCount); Assert.AreEqual(columnCount, actualFromDenseCrs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromDenseCcs.RowCount); Assert.AreEqual(columnCount, actualFromDenseCcs.ColumnCount);

            Assert.AreEqual(rowCount, actualFromCrsCrs.RowCount); Assert.AreEqual(columnCount, actualFromCrsCrs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCrsCcs.RowCount); Assert.AreEqual(columnCount, actualFromCrsCcs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCrsDense.RowCount); Assert.AreEqual(columnCount, actualFromCrsDense.ColumnCount);

            Assert.AreEqual(rowCount, actualFromCcsCcs.RowCount); Assert.AreEqual(columnCount, actualFromCcsCcs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCcsDense.RowCount); Assert.AreEqual(columnCount, actualFromCcsDense.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCcsCrs.RowCount); Assert.AreEqual(columnCount, actualFromCcsCrs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseCcs.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsDense.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.Subtract(Matrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(Matrix,Matrix)")]
        public void Static_Subtract_Matrix_Matrix()
        {
            // Arrange
            Matrix denseLeft = DenseMatrixTest.MatrixA;
            Matrix denseRight = DenseMatrixTest.MatrixB;

            Matrix crsLeft = Sparse.CompressedRowTest.MatrixA;
            Matrix crsRight = Sparse.CompressedRowTest.MatrixB;

            Matrix ccsLeft = Sparse.CompressedColumnTest.MatrixA;
            Matrix ccsRight = Sparse.CompressedColumnTest.MatrixB;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { -3d, -1d, 1d }, { 0d, 2d, 4d } };
            //Act
            Matrix actualFromDenseDense = Matrix.Subtract(denseLeft, denseRight);
            Matrix actualFromDenseCrs = Matrix.Subtract(denseLeft, crsRight);
            Matrix actualFromDenseCcs = Matrix.Subtract(denseLeft, ccsRight);

            Matrix actualFromCrsCrs = Matrix.Subtract(crsLeft, crsRight);
            Matrix actualFromCrsCcs = Matrix.Subtract(crsLeft, ccsRight);
            Matrix actualFromCrsDense = Matrix.Subtract(crsLeft, denseRight);

            Matrix actualFromCcsCcs = Matrix.Subtract(ccsLeft, ccsRight);
            Matrix actualFromCcsDense = Matrix.Subtract(ccsLeft, denseRight);
            Matrix actualFromCcsCrs = Matrix.Subtract(ccsLeft, crsRight);
            // Assert
            Assert.AreEqual(rowCount, actualFromDenseDense.RowCount); Assert.AreEqual(columnCount, actualFromDenseDense.ColumnCount);
            Assert.AreEqual(rowCount, actualFromDenseCrs.RowCount); Assert.AreEqual(columnCount, actualFromDenseCrs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromDenseCcs.RowCount); Assert.AreEqual(columnCount, actualFromDenseCcs.ColumnCount);

            Assert.AreEqual(rowCount, actualFromCrsCrs.RowCount); Assert.AreEqual(columnCount, actualFromCrsCrs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCrsCcs.RowCount); Assert.AreEqual(columnCount, actualFromCrsCcs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCrsDense.RowCount); Assert.AreEqual(columnCount, actualFromCrsDense.ColumnCount);

            Assert.AreEqual(rowCount, actualFromCcsCcs.RowCount); Assert.AreEqual(columnCount, actualFromCcsCcs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCcsDense.RowCount); Assert.AreEqual(columnCount, actualFromCcsDense.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCcsCrs.RowCount); Assert.AreEqual(columnCount, actualFromCcsCrs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseCcs.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsDense.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,Matrix)")]
        public void Static_Multiply_Matrix_Matrix()
        {
            // Arrange
            Matrix denseLeft = DenseMatrixTest.MatrixC;
            Matrix denseRight = DenseMatrixTest.MatrixD;

            Matrix crsLeft = Sparse.CompressedRowTest.MatrixC;
            Matrix crsRight = Sparse.CompressedRowTest.MatrixD;

            Matrix ccsLeft = Sparse.CompressedColumnTest.MatrixC;
            Matrix ccsRight = Sparse.CompressedColumnTest.MatrixD;

            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            Matrix actualFromDenseDense = Matrix.Multiply(denseLeft, denseRight);
            Matrix actualFromDenseCrs = Matrix.Multiply(denseLeft, crsRight);
            Matrix actualFromDenseCcs = Matrix.Multiply(denseLeft, ccsRight);

            Matrix actualFromCrsCrs = Matrix.Multiply(crsLeft, crsRight);
            Matrix actualFromCrsCcs = Matrix.Multiply(crsLeft, ccsRight);
            Matrix actualFromCrsDense = Matrix.Multiply(crsLeft, denseRight);

            Matrix actualFromCcsCcs = Matrix.Multiply(ccsLeft, ccsRight);
            Matrix actualFromCcsDense = Matrix.Multiply(ccsLeft, denseRight);
            Matrix actualFromCcsCrs = Matrix.Multiply(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(rowCount, actualFromDenseDense.RowCount); Assert.AreEqual(columnCount, actualFromDenseDense.ColumnCount);
            Assert.AreEqual(rowCount, actualFromDenseCrs.RowCount); Assert.AreEqual(columnCount, actualFromDenseCrs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromDenseCcs.RowCount); Assert.AreEqual(columnCount, actualFromDenseCcs.ColumnCount);

            Assert.AreEqual(rowCount, actualFromCrsCrs.RowCount); Assert.AreEqual(columnCount, actualFromCrsCrs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCrsCcs.RowCount); Assert.AreEqual(columnCount, actualFromCrsCcs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCrsDense.RowCount); Assert.AreEqual(columnCount, actualFromCrsDense.ColumnCount);

            Assert.AreEqual(rowCount, actualFromCcsCcs.RowCount); Assert.AreEqual(columnCount, actualFromCcsCcs.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCcsDense.RowCount); Assert.AreEqual(columnCount, actualFromCcsDense.ColumnCount);
            Assert.AreEqual(rowCount, actualFromCcsCrs.RowCount); Assert.AreEqual(columnCount, actualFromCcsCrs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromDenseCcs.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrsDense.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcsCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,double)")]
        public void Static_Multiply_Matrix_double()
        {
            // Arrange
            Matrix denseOperand = DenseMatrixTest.MatrixE;
            Matrix crsOperand = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsOperand = Sparse.CompressedColumnTest.MatrixE;

            double factor = -2.5;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { -10d, -7.5 }, { -5d, 12.5 }, { 10d, -2.5 } };
            // Act
            Matrix actualFromDense = Matrix.Multiply(denseOperand, factor);
            Matrix actualFromCrs = Matrix.Multiply(crsOperand, factor);
            Matrix actualFromCcs = Matrix.Multiply(ccsOperand, factor);

            // Assert
            Assert.AreEqual(rowCount, actualFromDense.RowCount);
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromDense.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(double, Matrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,Matrix)")]
        public void Static_Multiply_double_Matrix()
        {
            // Arrange
            double factor = -2.5;

            Matrix denseOperand = DenseMatrixTest.MatrixE;
            Matrix crsOperand = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsOperand = Sparse.CompressedColumnTest.MatrixE;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { -10d, -7.5 }, { -5d, 12.5 }, { 10d, -2.5 } };
            // Act
            Matrix actualFromDense = Matrix.Multiply(factor, denseOperand);
            Matrix actualFromCrs = Matrix.Multiply(factor, crsOperand);
            Matrix actualFromCcs = Matrix.Multiply(factor, ccsOperand);

            // Assert
            Assert.AreEqual(rowCount, actualFromDense.RowCount);
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromDense.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="Matrix.Divide(Matrix, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(Matrix,double)")]
        public void Static_Divide_Matrix_double()
        {
            // Arrange
            Matrix denseOperand = new DenseMatrix(2, 1, new double[] { 4.0, 3.0 });
            Matrix crsOperand = new CompressedRow(2, 1, new Stor.DictionaryOfKeys(
                new double[] { 4d, 3d }, new int[] { 0, 1 }, new int[] { 0, 0 }));
            Matrix ccsOperand = new CompressedColumn(2, 1, new int[2] { 0, 2 }, new int[2] { 0, 1 }, new double[2] { 4.0, 3.0 });
            double divisor = -2.0;

            int rowCount = 2, columnCount = 1;
            double[,] expected = new double[,] { { -2d }, { -1.5 } };
            // Act
            Matrix actualFromDense = Matrix.Divide(denseOperand, divisor);
            Matrix actualFromCrs = Matrix.Divide(crsOperand, divisor);
            Matrix actualFromCcs = Matrix.Divide(ccsOperand, divisor);

            // Assert
            Assert.AreEqual(rowCount, actualFromDense.RowCount);
            Assert.AreEqual(rowCount, actualFromCrs.RowCount);
            Assert.AreEqual(rowCount, actualFromCcs.RowCount);

            Assert.AreEqual(columnCount, actualFromDense.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualFromCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,Vector)")]
        public void Static_Multiply_Matrix_Vector()
        {
            // Arrange
            Matrix denseMatrix = DenseMatrixTest.MatrixE;
            Matrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            //Act
            Vector actualFromDenseDense = Matrix.Multiply(denseMatrix, denseVector);
            Vector actualFromCrsDense = Matrix.Multiply(crsMatrix, denseVector);
            Vector actualFromCcsDense = Matrix.Multiply(ccsMatrix, denseVector);

            Vector actualFromDenseSparse = Matrix.Multiply(denseMatrix, sparseVector);
            Vector actualFromCrsSparse = Matrix.Multiply(crsMatrix, sparseVector);
            Vector actualFromCcsSparse = Matrix.Multiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(size, actualFromDenseDense.Size);
            Assert.AreEqual(size, actualFromCrsDense.Size);
            Assert.AreEqual(size, actualFromCcsDense.Size);

            Assert.AreEqual(size, actualFromDenseSparse.Size);
            Assert.AreEqual(size, actualFromCrsSparse.Size);
            Assert.AreEqual(size, actualFromCcsSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDenseDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCrsDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCcsDense[i_R], Settings.AbsolutePrecision);

                Assert.AreEqual(expected[i_R], actualFromDenseSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCrsSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCcsSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,DenseVector)")]
        public void Static_Multiply_Matrix_DenseVector()
        {
            // Arrange
            Matrix denseMatrix = DenseMatrixTest.MatrixE;
            Matrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            //Act
            DenseVector actualFromDenseDense = Matrix.Multiply(denseMatrix, denseVector);
            DenseVector actualFromCrsDense = Matrix.Multiply(crsMatrix, denseVector);
            DenseVector actualFromCcsDense = Matrix.Multiply(ccsMatrix, denseVector);

            // Assert
            Assert.AreEqual(size, actualFromDenseDense.Size);
            Assert.AreEqual(size, actualFromCrsDense.Size);
            Assert.AreEqual(size, actualFromCcsDense.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDenseDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCrsDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCcsDense[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.Multiply(Matrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(Matrix,SparseVector)")]
        public void Static_Multiply_Matrix_SparseVector()
        {
            // Arrange
            Matrix denseMatrix = DenseMatrixTest.MatrixE;
            Matrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            // Act
            Vector actualFromDenseSparse = Matrix.Multiply(denseMatrix, sparseVector);
            Vector actualFromCrsSparse= Matrix.Multiply(crsMatrix, sparseVector);
            Vector actualFromCcsSparse = Matrix.Multiply(ccsMatrix, sparseVector);


            // Assert
            Assert.AreEqual(size, actualFromDenseSparse.Size);
            Assert.AreEqual(size, actualFromCrsSparse.Size);
            Assert.AreEqual(size, actualFromCcsSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDenseSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCrsSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCcsSparse[i_R], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="Matrix.TransposeMultiply(Matrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(Matrix,Vector)")]
        public void Static_TransposeMultiply_Matrix_Vector()
        {
            // Arrange
            Matrix denseMatrix = DenseMatrixTest.MatrixE;
            Matrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            Vector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            Vector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            Vector actualFromDenseDense = Matrix.TransposeMultiply(denseMatrix, denseVector);
            Vector actualFromCrsDense = Matrix.TransposeMultiply(crsMatrix, denseVector);
            Vector actualFromCcsDense = Matrix.TransposeMultiply(ccsMatrix, denseVector);

            Vector actualFromDenseSparse = Matrix.TransposeMultiply(denseMatrix, sparseVector);
            Vector actualFromCrsSparse = Matrix.TransposeMultiply(crsMatrix, sparseVector);
            Vector actualFromCcsSparse = Matrix.TransposeMultiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(size, actualFromDenseDense.Size);
            Assert.AreEqual(size, actualFromCrsDense.Size);
            Assert.AreEqual(size, actualFromCcsDense.Size);

            Assert.AreEqual(size, actualFromDenseSparse.Size);
            Assert.AreEqual(size, actualFromCrsSparse.Size);
            Assert.AreEqual(size, actualFromCcsSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDenseDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCrsDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCcsDense[i_R], Settings.AbsolutePrecision);

                Assert.AreEqual(expected[i_R], actualFromDenseSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCrsSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCcsSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.TransposeMultiply(Matrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(Matrix,DenseVector)")]
        public void Static_TransposeMultiply_Matrix_DenseVector()
        {
            // Arrange
            Matrix denseMatrix = DenseMatrixTest.MatrixE;
            Matrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            DenseVector actualFromDenseDense = Matrix.TransposeMultiply(denseMatrix, denseVector);
            DenseVector actualFromCrsDense = Matrix.TransposeMultiply(crsMatrix, denseVector);
            DenseVector actualFromCcsDense = Matrix.TransposeMultiply(ccsMatrix, denseVector);

            // Assert
            Assert.AreEqual(size, actualFromDenseDense.Size);
            Assert.AreEqual(size, actualFromCrsDense.Size);
            Assert.AreEqual(size, actualFromCcsDense.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDenseDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCrsDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCcsDense[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="Matrix.TransposeMultiply(Matrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(Matrix,SparseVector)")]
        public void Static_TransposeMultiply_Matrix_SparseVector()
        {
            // Arrange
            Matrix denseMatrix = DenseMatrixTest.MatrixE;
            Matrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            Vector actualFromDenseSparse = Matrix.TransposeMultiply(denseMatrix, sparseVector);
            Vector actualFromCrsSparse = Matrix.TransposeMultiply(crsMatrix, sparseVector);
            Vector actualFromCcsSparse = Matrix.TransposeMultiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(size, actualFromDenseSparse.Size);
            Assert.AreEqual(size, actualFromCrsSparse.Size);
            Assert.AreEqual(size, actualFromCcsSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualFromDenseSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCrsSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualFromCcsSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the method <see cref="Matrix.ToArray()"/>.
        /// </summary>
        [TestMethod("Method ToArray()")]
        public void ToArray()
        {
            // Arrange
            Matrix denseMatrix = DenseMatrixTest.MatrixE;
            Matrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            Matrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { 4.0, 3.0 }, { 2.0, -5.0 }, { -4.0, 1.0 } };
            // Act 
            double[,] actualFromDense = denseMatrix.ToArray();
            double[,] actualFromCrs = crsMatrix.ToArray();
            double[,] actualFromCCs = ccsMatrix.ToArray();
            // Assert
            Assert.AreEqual(rowCount, actualFromDense.GetLength(0)); Assert.AreEqual(columnCount, actualFromDense.GetLength(1));
            Assert.AreEqual(rowCount, actualFromCrs.GetLength(0)); Assert.AreEqual(columnCount, actualFromCrs.GetLength(1));
            Assert.AreEqual(rowCount, actualFromCCs.GetLength(0)); Assert.AreEqual(columnCount, actualFromCCs.GetLength(1));

            for (int i_R = 0; i_R < expected.GetLength(0); i_R++)
            {
                for (int i_C = 0; i_C < expected.GetLength(1); i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromDense[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCCs[i_R, i_C], Settings.AbsolutePrecision);
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
            Matrix denseMatrix = DenseMatrixTest.MatrixA;
            Matrix crsMatrix = Sparse.CompressedRowTest.MatrixA;
            Matrix ccsMatrix = Sparse.CompressedColumnTest.MatrixA;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 1d, 2d, 3d }, { 5d, 6d, 7d } };
            //Act

            // Assert
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], denseMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], crsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], ccsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        #endregion
    }
}
