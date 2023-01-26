using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using Stor = BRIDGES.LinearAlgebra.Matrices.Storage;


namespace BRIDGES.Test.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class testing the members of the <see cref="SparseMatrix"/> class.
    /// </summary>
    [TestClass]
    public class SparseMatrixTest
    {
        #region Behavior

        /// <summary>
        /// Tests that <see cref="SparseMatrix"/> is reference type.
        /// </summary>
        [TestMethod("Behavior IsReference")]
        public void SparseMatrix_IsReference()
        {
            // Arrange
            int[] crsRowPointers = new int[3] { 0, 2, 4 };
            List<int> crsColumnIndices = new List<int> { 0, 1, 0, 1 };
            List<double> crsValues = new List<double> { 1.0, 2.0, 3.0, 4.0 };
            Matrix crsMatrix = new CompressedRow(2, 2, ref crsRowPointers, ref crsColumnIndices, ref crsValues);

            int[] otherCrsRowPointers = new int[2] { 0, 2 };
            List<int> otherCrsColumnIndices = new List<int> { 0, 1 };
            List<double> otherCrsValues = new List<double> { 2.0, 4.0 };
            Matrix otherCrsMatrix = new CompressedRow(1, 2, ref otherCrsRowPointers, ref otherCrsColumnIndices, ref otherCrsValues);

            Matrix ccsMatrix = new CompressedColumn(2, 2, new int[3] { 0, 2, 4 }, new int[4] { 0, 1, 0, 1 }, new double[4] { 1.0, 3.0, 2.0, 4.0 });
            Matrix otherCcsMatrix = new CompressedColumn(1, 2, new int[3] { 0, 1, 2 }, new int[2] { 0, 1 }, new double[2] { 2.0, 4.0 });

            //Act
            otherCrsMatrix = crsMatrix;
            otherCcsMatrix = ccsMatrix;

            // Assert
            Assert.AreEqual(crsMatrix, otherCrsMatrix);
            Assert.AreEqual(ccsMatrix, otherCcsMatrix);

            Assert.AreSame(crsMatrix, otherCrsMatrix);
            Assert.AreSame(ccsMatrix, otherCcsMatrix);
        }

        #endregion


        #region Properties

        /// <summary>
        /// Tests the <see cref="Matrix.RowCount"/> property.
        /// </summary>
        [TestMethod("Property RowCount")]
        public void RowCount()
        {
            // Arrange
            SparseMatrix crs= Sparse.CompressedRowTest.MatrixA;
            SparseMatrix ccs = Sparse.CompressedColumnTest.MatrixA;
            int expected = 2;
            // Act
            double actualFromCrs = crs.RowCount;
            double actualFromCcs = ccs.RowCount;
            // Assert
            Assert.AreEqual(expected, actualFromCrs);
            Assert.AreEqual(expected, actualFromCcs);
        }

        /// <summary>
        /// Tests the <see cref="Matrix.ColumnCount"/> property.
        /// </summary>
        [TestMethod("Property ColumnCount")]
        public void ColumnCount()
        {
            // Arrange
            SparseMatrix crs = Sparse.CompressedRowTest.MatrixA;
            SparseMatrix ccs = Sparse.CompressedColumnTest.MatrixA;
            int expected = 3;
            // Act
            double actualFromCrs = crs.ColumnCount;
            double actualFromCcs = ccs.ColumnCount;
            // Assert
            Assert.AreEqual(expected, actualFromCrs);
            Assert.AreEqual(expected, actualFromCcs);
        }

        #endregion

        #region Static Methods

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Add(SparseMatrix, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Add(SparseMatrix,SparseMatrix)")]
        public void Static_Add_SparseMatrix_SparseMatrix()
        {
            // Arrange
            SparseMatrix crsLeft = Sparse.CompressedRowTest.MatrixA;
            SparseMatrix crsRight = Sparse.CompressedRowTest.MatrixB;

            SparseMatrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            SparseMatrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });


            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { 5.0, 5.0, 5.0, 10.0, 10.0, 10.0 });

            //Act
            SparseMatrix CrsCrsMatrix = SparseMatrix.Add(crsLeft, crsRight);
            SparseMatrix CrsCcsMatrix = SparseMatrix.Add(crsLeft, ccsRight);

            SparseMatrix CcsCcsMatrix = SparseMatrix.Add(ccsLeft, ccsRight);
            SparseMatrix CcsCrsMatrix = SparseMatrix.Add(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(expected.RowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(expected.RowCount, CrsCcsMatrix.RowCount);

            Assert.AreEqual(expected.RowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(expected.RowCount, CcsCrsMatrix.RowCount);


            Assert.AreEqual(expected.ColumnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, CrsCcsMatrix.ColumnCount);

            Assert.AreEqual(expected.ColumnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected.At(i_R, i_C), CrsCrsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected.At(i_R, i_C), CrsCcsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected.At(i_R, i_C), CcsCcsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected.At(i_R, i_C), CcsCrsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }

        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Subtract(SparseMatrix, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Subtract(SparseMatrix,SparseMatrix)")]
        public void Static_Subtract_SparseMatrix_SparseMatrix()
        {
            // Arrange
            SparseMatrix crsLeft = Sparse.CompressedRowTest.MatrixA;
            SparseMatrix crsRight = Sparse.CompressedRowTest.MatrixB;

            SparseMatrix ccsLeft = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 1.0, 5.0, 2.0, 6.0, 3.0, 7.0 });
            SparseMatrix ccsRight = new CompressedColumn(2, 3, new int[4] { 0, 2, 4, 6 },
                new int[6] { 0, 1, 0, 1, 0, 1 }, new double[6] { 4.0, 5.0, 3.0, 4.0, 2.0, 3.0 });


            DenseMatrix expected = new DenseMatrix(2, 3, new double[] { -3.0, -1.0, 1.0, 0.0, 2.0, 4.0 });
            
            //Act
            SparseMatrix CrsCrsMatrix = SparseMatrix.Subtract(crsLeft, crsRight);
            SparseMatrix CrsCcsMatrix = SparseMatrix.Subtract(crsLeft, ccsRight);

            SparseMatrix CcsCcsMatrix = SparseMatrix.Subtract(ccsLeft, ccsRight);
            SparseMatrix CcsCrsMatrix = SparseMatrix.Subtract(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(expected.RowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(expected.RowCount, CrsCcsMatrix.RowCount);

            Assert.AreEqual(expected.RowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(expected.RowCount, CcsCrsMatrix.RowCount);

            Assert.AreEqual(expected.ColumnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, CrsCcsMatrix.ColumnCount);

            Assert.AreEqual(expected.ColumnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(expected.ColumnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < expected.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < expected.ColumnCount; i_C++)
                {
                    Assert.AreEqual(expected.At(i_R, i_C), CrsCrsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected.At(i_R, i_C), CrsCcsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected.At(i_R, i_C), CcsCcsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected.At(i_R, i_C), CcsCrsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,SparseMatrix)")]
        public void Static_Multiply_SparseMatrix_SparseMatrix()
        {
            // Arrange
            SparseMatrix crsLeft = Sparse.CompressedRowTest.MatrixC;
            SparseMatrix crsRight = Sparse.CompressedRowTest.MatrixD;
            
            SparseMatrix ccsLeft = Sparse.CompressedColumnTest.MatrixC;
            SparseMatrix ccsRight = Sparse.CompressedColumnTest.MatrixD;


            int rowCount = 6, columnCount = 3;
            double[,] expected = new double[,] { { 2.5, 6.0, 5.0 }, { 13.5, 20.25, 27.0 }, {10.0, 0d, 0d },
                {11.0, 16.5, 22.0 }, { 22.0, 12.75, 9.0 }, {3.625, 18.125, 0d }};
            //Act
            SparseMatrix CrsCrsMatrix = SparseMatrix.Multiply(crsLeft, crsRight);
            SparseMatrix CrsCcsMatrix = SparseMatrix.Multiply(crsLeft, ccsRight);

            SparseMatrix CcsCcsMatrix = SparseMatrix.Multiply(ccsLeft, ccsRight);
            SparseMatrix CcsCrsMatrix = SparseMatrix.Multiply(ccsLeft, crsRight);

            // Assert
            Assert.AreEqual(rowCount, CrsCrsMatrix.RowCount);
            Assert.AreEqual(rowCount, CrsCcsMatrix.RowCount);

            Assert.AreEqual(rowCount, CcsCcsMatrix.RowCount);
            Assert.AreEqual(rowCount, CcsCrsMatrix.RowCount);


            Assert.AreEqual(columnCount, CrsCrsMatrix.ColumnCount);
            Assert.AreEqual(columnCount, CrsCcsMatrix.ColumnCount);

            Assert.AreEqual(columnCount, CcsCcsMatrix.ColumnCount);
            Assert.AreEqual(columnCount, CcsCrsMatrix.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], CrsCrsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], CrsCcsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);

                    Assert.AreEqual(expected[i_R, i_C], CcsCcsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], CcsCrsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(double, SparseMatrix)"/>.
        /// </summary>
        [TestMethod("Static Multiply(double,SparseMatrix)")]
        public void Static_Multiply_double_SparseMatrix()
        {
            // Arrange
            double factor = -2.5;

            SparseMatrix crsOperand = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsOperand = Sparse.CompressedColumnTest.MatrixE;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { -10d, -7.5 }, { -5d, 12.5 }, { 10d, -2.5 } };
            // Act
            SparseMatrix actualCrs = SparseMatrix.Multiply(factor, crsOperand);
            SparseMatrix actualCcs = SparseMatrix.Multiply(factor, ccsOperand);
            // Assert
            Assert.AreEqual(rowCount, actualCrs.RowCount);
            Assert.AreEqual(rowCount, actualCcs.RowCount);

            Assert.AreEqual(columnCount, actualCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, double)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,double)")]
        public void Static_Multiply_SparseMatrix_double()
        {
            // Arrange
            SparseMatrix crsOperand = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsOperand = Sparse.CompressedColumnTest.MatrixE;

            double factor = -2.5;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { -10d, -7.5 }, { -5d, 12.5 }, { 10d, -2.5 } };
            // Act
            SparseMatrix actualCrs = SparseMatrix.Multiply(crsOperand, factor);
            SparseMatrix actualCcs = SparseMatrix.Multiply(ccsOperand, factor);

            // Assert
            Assert.AreEqual(rowCount, actualCrs.RowCount);
            Assert.AreEqual(rowCount, actualCcs.RowCount);

            Assert.AreEqual(columnCount, actualCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Divide(SparseMatrix, double)"/>.
        /// </summary>
        [TestMethod("Static Divide(SparseMatrix,double)")]
        public void Static_Divide_SparseMatrix_double()
        {
            // Arrange
            CompressedRow crsOperand = new CompressedRow(2, 1, new Stor.DictionaryOfKeys(
                new double[] { 4d, 3d }, new int[] { 0, 1 }, new int[] { 0, 0 }));

            SparseMatrix ccsOperand = new CompressedColumn(2, 1, new int[2] { 0, 2 },
                new int[2] { 0, 1 }, new double[2] { 4.0, 3.0 });

            double divisor = -2.0;

            int rowCount = 2, columnCount = 1;
            double[,] expected = new double[,] { { -2d }, { -1.5 } };
            // Act
            SparseMatrix actualCrs = SparseMatrix.Divide(crsOperand, divisor);
            SparseMatrix actualCcs = SparseMatrix.Divide(ccsOperand, divisor);

            // Assert
            Assert.AreEqual(rowCount, actualCrs.RowCount);
            Assert.AreEqual(rowCount, actualCcs.RowCount);

            Assert.AreEqual(columnCount, actualCrs.ColumnCount);
            Assert.AreEqual(columnCount, actualCcs.ColumnCount);

            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualCrs.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualCcs.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,Vector)")]
        public void Static_Multiply_SparseMatrix_Vector()
        {
            // Arrange
            SparseMatrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            Vector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });
            Vector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            //Act
            Vector actualCrsDense = SparseMatrix.Multiply(crsMatrix, denseVector);
            Vector actualCcsDense = SparseMatrix.Multiply(ccsMatrix, denseVector);

            Vector actualCrsSparse = SparseMatrix.Multiply(crsMatrix, sparseVector);
            Vector actualCcsSparse = SparseMatrix.Multiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(size, actualCrsDense.Size);
            Assert.AreEqual(size, actualCcsDense.Size);

            Assert.AreEqual(size, actualCrsSparse.Size);
            Assert.AreEqual(size, actualCcsSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualCrsDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualCcsDense[i_R], Settings.AbsolutePrecision);

                Assert.AreEqual(expected[i_R], actualCrsSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualCcsSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,DenseVector)")]
        public void Static_Multiply_SparseMatrix_DenseVector()
        {
            // Arrange
            SparseMatrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            DenseVector denseVector = new DenseVector(new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            //Act
            DenseVector actualCrsDense = SparseMatrix.Multiply(crsMatrix, denseVector);
            DenseVector actualCcsDense = SparseMatrix.Multiply(ccsMatrix, denseVector);

            // Assert
            Assert.AreEqual(size, actualCrsDense.Size);
            Assert.AreEqual(size, actualCcsDense.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualCrsDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualCcsDense[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.Multiply(SparseMatrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static Multiply(SparseMatrix,SparseVector)")]
        public void Static_Multiply_SparseMatrix_SparseVector()
        {
            // Arrange
            SparseMatrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            SparseVector sparseVector = new SparseVector(2, new int[2] { 0, 1 }, new double[2] { -2.0, 6.0 });

            int size = 3;
            double[] expected = new double[] { 10.0, -34.0, 14.0 };
            // Act
            SparseVector actualCrsSparse = SparseMatrix.Multiply(crsMatrix, sparseVector);
            SparseVector actualCcsSparse = SparseMatrix.Multiply(ccsMatrix, sparseVector);


            // Assert
            Assert.AreEqual(size, actualCrsSparse.Size);
            Assert.AreEqual(size, actualCcsSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualCrsSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualCcsSparse[i_R], Settings.AbsolutePrecision);
            }
        }


        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.TransposeMultiply(SparseMatrix, Vector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(SparseMatrix,Vector)")]
        public void Static_TransposeMultiply_SparseMatrix_Vector()
        {
            // Arrange
            SparseMatrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            Vector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });
            Vector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            Vector actualCrsDense = SparseMatrix.TransposeMultiply(crsMatrix, denseVector);
            Vector actualCcsDense = SparseMatrix.TransposeMultiply(ccsMatrix, denseVector);

            Vector actualCrsSparse = SparseMatrix.TransposeMultiply(crsMatrix, sparseVector);
            Vector actualCcsSparse = SparseMatrix.TransposeMultiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(size, actualCrsDense.Size);
            Assert.AreEqual(size, actualCcsDense.Size);

            Assert.AreEqual(size, actualCrsSparse.Size);
            Assert.AreEqual(size, actualCcsSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualCrsDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualCcsDense[i_R], Settings.AbsolutePrecision);

                Assert.AreEqual(expected[i_R], actualCrsSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualCcsSparse[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.TransposeMultiply(SparseMatrix, DenseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(SparseMatrix,DenseVector)")]
        public void Static_TransposeMultiply_SparseMatrix_DenseVector()
        {
            // Arrange
            SparseMatrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            DenseVector denseVector = new DenseVector(new double[3] { -2.0, 0.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            DenseVector actualCrsDense = SparseMatrix.TransposeMultiply(crsMatrix, denseVector);
            DenseVector actualCcsDense = SparseMatrix.TransposeMultiply(ccsMatrix, denseVector);

            // Assert
            Assert.AreEqual(size, actualCrsDense.Size);
            Assert.AreEqual(size, actualCcsDense.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualCrsDense[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualCcsDense[i_R], Settings.AbsolutePrecision);
            }
        }

        /// <summary>
        /// Tests the static method <see cref="SparseMatrix.TransposeMultiply(SparseMatrix, SparseVector)"/>.
        /// </summary>
        [TestMethod("Static TransposeMultiply(SparseMatrix,SparseVector)")]
        public void Static_TransposeMultiply_SparseMatrix_SparseVector()
        {
            // Arrange
            SparseMatrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            SparseVector sparseVector = new SparseVector(3, new int[2] { 0, 2 }, new double[2] { -2.0, 6.0 });

            int size = 2;
            double[] expected = new double[2] { -32.0, 0.0 };
            //Act
            Vector actualCrsSparse = SparseMatrix.TransposeMultiply(crsMatrix, sparseVector);
            Vector actualCcsSparse = SparseMatrix.TransposeMultiply(ccsMatrix, sparseVector);

            // Assert
            Assert.AreEqual(size, actualCrsSparse.Size);
            Assert.AreEqual(size, actualCcsSparse.Size);

            for (int i_R = 0; i_R < size; i_R++)
            {
                Assert.AreEqual(expected[i_R], actualCrsSparse[i_R], Settings.AbsolutePrecision);
                Assert.AreEqual(expected[i_R], actualCcsSparse[i_R], Settings.AbsolutePrecision);
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
            SparseMatrix crsMatrix = Sparse.CompressedRowTest.MatrixE;
            SparseMatrix ccsMatrix = Sparse.CompressedColumnTest.MatrixE;

            int rowCount = 3, columnCount = 2;
            double[,] expected = new double[,] { { 4.0, 3.0 }, { 2.0, -5.0 }, { -4.0, 1.0 } };
            // Act 
            double[,] actualFromCrs = crsMatrix.ToArray();
            double[,] actualFromCCs = ccsMatrix.ToArray();
            // Assert
            Assert.AreEqual(rowCount, actualFromCrs.GetLength(0)); Assert.AreEqual(columnCount, actualFromCrs.GetLength(1));
            Assert.AreEqual(rowCount, actualFromCCs.GetLength(0)); Assert.AreEqual(columnCount, actualFromCCs.GetLength(1));

            for (int i_R = 0; i_R < expected.GetLength(0); i_R++)
            {
                for (int i_C = 0; i_C < expected.GetLength(1); i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], actualFromCrs[i_R, i_C], Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], actualFromCCs[i_R, i_C], Settings.AbsolutePrecision);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Matrix.At(int,int)"/>.
        /// </summary>
        [TestMethod("Property At(int,int)")]
        public void At_Int_Int()
        {
            // Arrange
            SparseMatrix crsMatrix = Sparse.CompressedRowTest.MatrixA;
            SparseMatrix ccsMatrix = Sparse.CompressedColumnTest.MatrixA;

            int rowCount = 2, columnCount = 3;
            double[,] expected = new double[,] { { 1d, 2d, 3d }, { 5d, 6d, 7d } };
            //Act

            // Assert
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    Assert.AreEqual(expected[i_R, i_C], crsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                    Assert.AreEqual(expected[i_R, i_C], ccsMatrix.At(i_R, i_C), Settings.AbsolutePrecision);
                }
            }
        }

        #endregion
    }
}
