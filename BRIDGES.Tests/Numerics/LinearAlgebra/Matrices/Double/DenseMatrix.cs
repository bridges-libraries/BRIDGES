using System;
using System.Collections.Generic;

using Xunit;

using Mat = BRIDGES.Numerics.LinearAlgebra.Matrices;
using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Matrices.Double
{
    /// <summary>
    /// Tests the members of the <see cref=Mat.DenseMatrix{T}"/> class.
    /// </summary>
    public class DenseMatrix
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Mat.Matrix{T}.RowCount"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to evaluate. </param>
        /// <param name="expected"> Expected number of row. </param>
        [Theory(DisplayName = "Prop. RowCount")]
        [ClassData(typeof(DataClasses.RowCount))]
        public void Property_RowCount(Mat.DenseMatrix<double> matrix, int expected)
        {
            // Arrange

            //Act
            int actual = matrix.RowCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the property <see cref="Mat.Matrix{T}.ColumnCount"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to evaluate. </param>
        /// <param name="expected"> Expected number of columns. </param>
        [Theory(DisplayName = "Prop. ColumnCount")]
        [ClassData(typeof(DataClasses.ColumnCount))]
        public void Property_ColumnCount(Mat.DenseMatrix<double> matrix, int expected)
        {
            // Arrange

            //Act
            int actual = matrix.ColumnCount;

            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.this[int,int]"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate on. </param>
        /// <param name="expected"> Two-dimensional array containing the values of components. </param>
        [Theory(DisplayName = "Prop. this[int,int]")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void Property__Indexer__Int_Int(Mat.DenseMatrix<double> matrix, double[,] expected)
        {
            // Arrange

            // Act

            // Assert
            Assert.Equal(expected.GetLength(0), matrix.RowCount);
            Assert.Equal(expected.GetLength(1), matrix.ColumnCount);
            for (int i_Column = 0; i_Column < expected.GetLength(1); i_Column++)
            {
                for (int i_Row = 0; i_Row < expected.GetLength(0); i_Row++)
                {
                    Assert.Equal(expected[i_Row, i_Column], matrix[i_Row, i_Column]);
                }
            }
        }

        #endregion

        #region Tests : Public Static Methods

        //     -----     -----     Algebraic Near Ring : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Add(Mat.DenseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(DenseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Addition__DenseMatrix_DenseMatrix))]
        public void Static__Add__DenseMatrix_DenseMatrix(Mat.DenseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Add(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Subtract(Mat.DenseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(DenseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Subtraction__DenseMatrix_DenseMatrix))]
        public void Static__Subtract__DenseMatrix_DenseMatrix(Mat.DenseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Subtract(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Opposite(Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Static Opposite(DenseMatrix)")]
        [ClassData(typeof(DataClasses.UnaryNegation__DenseMatrix))]
        public void Static__Opposite__DenseMatrix(Mat.DenseMatrix<double> operand, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Opposite(operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Multiply(Mat.DenseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(DenseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_DenseMatrix))]
        public void Static__Multiply__DenseMatrix_DenseMatrix(Mat.DenseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Multiply(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        //     -----     Other Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.TransposeMultiply(Mat.DenseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(DenseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__DenseMatrix_DenseMatrix))]
        public void Static__TransposeMultiply__DenseMatrix_DenseMatrix(Mat.DenseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.TransposeMultiply(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.MultiplyTranspose(Mat.DenseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static MultiplyTranspose(DenseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.MultiplicationTranspose__DenseMatrix_DenseMatrix))]
        public void Static__MultiplyTranspose__DenseMatrix_DenseMatrix(Mat.DenseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.MultiplyTranspose(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        //     -----     -----     Right Embedding : SparseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Add(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Addition__DenseMatrix_SparseMatrix))]
        public void Static__Add__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Add(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Subtract(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Subtraction__DenseMatrix_SparseMatrix))]
        public void Static__Subtract__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Subtract(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Multiply(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_SparseMatrix))]
        public void Static__Multiply__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Multiply(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        //     -----     Other Right Operations : SparseMatrix<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.TransposeMultiply(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__DenseMatrix_SparseMatrix))]
        public void Static__TransposeMultiply__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.TransposeMultiply(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.MultiplyTranspose(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static MultiplyTranspose(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.MultiplicationTranspose__DenseMatrix_SparseMatrix))]
        public void Static__MultiplyTranspose__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.MultiplyTranspose(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }



        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Multiply(Mat.DenseMatrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(DenseMatrix,T)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_T))]
        public void Static__Multiply__DenseMatrix_T(Mat.DenseMatrix<double> operand, double factor, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Multiply(operand, factor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Multiply(T,Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(T,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__T_DenseMatrix))]
        public void Static__Multiply__T_DenseMatrix(Mat.DenseMatrix<double> operand, double factor, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Multiply(factor, operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Divide(Mat.DenseMatrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the division. </param>
        [Theory(DisplayName = "Static Divide(DenseMatrix,T)")]
        [ClassData(typeof(DataClasses.Division__DenseMatrix_T))]
        public void Static__Divide__DenseMatrix_T(Mat.DenseMatrix<double> operand, double divisor, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Divide(operand, divisor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }



        //     -----     -----     Vectors     -----     -----     //


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Multiply(Mat.DenseMatrix{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(DenseMatrix,DenseVector)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_DenseVector))]
        public void Static__Multiply__DenseMatrix_DenseVector(Mat.DenseMatrix<double> matrix, Vect.DenseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Mat.DenseMatrix<double>.Multiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Multiply(Mat.DenseMatrix{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(DenseMatrix,SparseVector)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_SparseVector))]
        public void Static__Multiply__DenseMatrix_SparseVector(Mat.DenseMatrix<double> matrix, Vect.SparseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Mat.DenseMatrix<double>.Multiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }


        //     -----      Other Operations : Vectors     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.TransposeMultiply(Mat.DenseMatrix{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(DenseMatrix,DenseVector)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__DenseMatrix_DenseVector))]
        public void Static__TransposeMultiply__DenseMatrix_DenseVector(Mat.DenseMatrix<double> matrix, Vect.DenseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Mat.DenseMatrix<double>.TransposeMultiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.TransposeMultiply(Mat.DenseMatrix{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(DenseMatrix,SparseVector)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__DenseMatrix_SparseVector))]
        public void Static__TransposeMultiply__DenseMatrix_SparseVector(Mat.DenseMatrix<double> matrix, Vect.SparseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Mat.DenseMatrix<double>.TransposeMultiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }


        //     -----     -----     Other Operations     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.Transpose(Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the transposition. </param>
        [Theory(DisplayName = "Static Transpose(DenseMatrix)")]
        [ClassData(typeof(DataClasses.Transposition__DenseMatrix))]
        public void Static__Transpose__DenseMatrix(Mat.DenseMatrix<double> operand, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.DenseMatrix<double>.Transpose(operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }

        #endregion

        #region Tests : Operators

        //     -----     -----     Algebraic Near Ring : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator+(Mat.DenseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (DenseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Addition__DenseMatrix_DenseMatrix))]
        public void Operator__Addition__DenseMatrix_DenseMatrix(Mat.DenseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = left + right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator-(Mat.DenseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (DenseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Subtraction__DenseMatrix_DenseMatrix))]
        public void Operator__Subtraction__DenseMatrix_DenseMatrix(Mat.DenseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = left - right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator-(Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Op - (DenseMatrix)")]
        [ClassData(typeof(DataClasses.UnaryNegation__DenseMatrix))]
        public void Operator__UnaryNegation__DenseMatrix(Mat.DenseMatrix<double> operand, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = -operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator*(Mat.DenseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Op * (DenseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_DenseMatrix))]
        public void Operator__Multiplication__DenseMatrix_DenseMatrix(Mat.DenseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = left * right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        //     -----     -----     Right Embedding : SparseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator+(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Addition__DenseMatrix_SparseMatrix))]
        public void Operator__Addition__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = left + right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator-(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Subtraction__DenseMatrix_SparseMatrix))]
        public void Operator__Subtraction__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = left - right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator*(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Op * (DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_SparseMatrix))]
        public void Operator__Multiplication__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = left * right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        //     -----      -----      Group Action : T      -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator*(Mat.DenseMatrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Op * (DenseMatrix,T)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_T))]
        public void Operator__Multiplication__DenseMatrix_T(Mat.DenseMatrix<double> operand, double factor, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = operand * factor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator*(T,Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Op * (T,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__T_DenseMatrix))]
        public void Operator__Multiplication__T_DenseMatrix(Mat.DenseMatrix<double> operand, double factor, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = factor * operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator/(Mat.DenseMatrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.DenseMatrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the division. </param>
        [Theory(DisplayName = "Op / (DenseMatrix,T)")]
        [ClassData(typeof(DataClasses.Division__DenseMatrix_T))]
        public void Operator__Division__DenseMatrix_T(Mat.DenseMatrix<double> operand, double divisor, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = operand / divisor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        //     -----     -----     Vectors     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator*(Mat.DenseMatrix{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Op * (DenseMatrix,DenseVector)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_DenseVector))]
        public void Operator__Multiplication__DenseMatrix_DenseVector(Mat.DenseMatrix<double> matrix, Vect.DenseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Mat.DenseMatrix<double>.Multiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.operator*(Mat.DenseMatrix{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Op * (DenseMatrix,SparseVector)")]
        [ClassData(typeof(DataClasses.Multiplication__DenseMatrix_SparseVector))]
        public void Operator__Multiplication__DenseMatrix_SparseVector(Mat.DenseMatrix<double> matrix, Vect.SparseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Mat.DenseMatrix<double>.Multiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        #endregion

        #region Tests : Public Methods

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.ToArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="expected"> Two-dimensional array containing the values of components. </param>
        [Theory(DisplayName = "GetComponent(int, int)")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void GetComponent__Int_Int(Mat.DenseMatrix<double> matrix, double[,] expected)
        {
            // Arrange

            // Act

            // Assert
            Assert.Equal(expected.GetLength(0), matrix.RowCount);
            Assert.Equal(expected.GetLength(1), matrix.ColumnCount);
            for (int i_Column = 0; i_Column < expected.GetLength(1); i_Column++)
            {
                for (int i_Row = 0; i_Row < expected.GetLength(0); i_Row++)
                {
                    Assert.Equal(expected[i_Row, i_Column], matrix.GetComponent(i_Row, i_Column));
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.SetComponent(int, int, T)"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to set. </param>
        /// <param name="columnIndex"> Column index of the component to set. </param>
        /// <param name="value"> Value of the component to set. </param>
        /// <param name="expected"> Expected sparse storage after the component iis set. </param>
        [Theory(DisplayName = "SetComponent(int, int, T)")]
        [ClassData(typeof(DataClasses.SetComponent))]
        public void SetComponent__Int_Int(Mat.DenseMatrix<double> matrix, int rowIndex, int columnIndex, double value, Mat.DenseMatrix<double> expected)
        {
            // Arrange
            
            // Act
            matrix.SetComponent(rowIndex, columnIndex, value);

            // Assert
            Assert.Equal(expected.RowCount, matrix.RowCount);
            Assert.Equal(expected.ColumnCount, matrix.ColumnCount);
            for (int i_Column = 0; i_Column < expected.ColumnCount; i_Column++)
            {
                for (int i_Row = 0; i_Row < expected.RowCount; i_Row++)
                {
                    Assert.Equal(expected[i_Row, i_Column], matrix[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.ToArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="expected"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToArray(Mat.DenseMatrix<double> matrix, double[,] expected)
        {
            // Arrange

            // Act
            double[,] result = matrix.ToArray();

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
        /// Tests the method <see cref="Mat.DenseMatrix{T}.ToRowMajorArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToRowMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToRowMajorArray(Mat.DenseMatrix<double> matrix, double[,] values)
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
            double[] result = matrix.ToRowMajorArray();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.ToColumnMajorArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToColumnMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToColumnMajorArray(Mat.DenseMatrix<double> matrix, double[,] values)
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
            double[] result = matrix.ToColumnMajorArray();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.RowVectors()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "RowVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void RowVectors(Mat.DenseMatrix<double> matrix, double[,] values)
        {
            // Arrange
            Vect.DenseVector<double>[] expected = new Vect.DenseVector<double>[values.GetLength(0)];

            for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
            {
                expected[i_Row] = new Vect.DenseVector<double>(values.GetLength(1));

                for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
                {
                    expected[i_Row][i_Column] = values[i_Row, i_Column];
                }
            }

            // Act
            Vect.DenseVector<double>[] result = matrix.RowVectors();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i_Row = 0; i_Row < expected.Length; i_Row++)
            {
                Vect.DenseVector<double> expectedRow = expected[i_Row];
                Vect.DenseVector<double> resultRow = result[i_Row];

                Assert.Equal(expectedRow.Size, resultRow.Size);

                for (int i = 0; i < expectedRow.Size; i++)
                {
                    Assert.Equal(expectedRow[i], resultRow[i]);
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.DenseMatrix{T}.ColumnVectors()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ColumnVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ColumnVectors(Mat.DenseMatrix<double> matrix, double[,] values)
        {
            // Arrange
            Vect.DenseVector<double>[] expected = new Vect.DenseVector<double>[values.GetLength(1)];

            for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
            {
                expected[i_Column] = new Vect.DenseVector<double>(values.GetLength(0));

                for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
                {
                    expected[i_Column][i_Row] = values[i_Row, i_Column];
                }
            }

            // Act
            Vect.DenseVector<double>[] result = matrix.ColumnVectors();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i_Column = 0; i_Column < expected.Length; i_Column++)
            {
                Vect.DenseVector<double> expectedColumn = expected[i_Column];
                Vect.DenseVector<double> resultColumn = result[i_Column];

                Assert.Equal(expectedColumn.Size, resultColumn.Size);

                for (int i = 0; i < expectedColumn.Size; i++)
                {
                    Assert.Equal(expectedColumn[i], resultColumn[i]);
                }
            }
        }

        #endregion


        #region Storage Classes for Data Classes

        internal static class DataStorages
        {
            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.DenseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M1 is a [2x3] matrix. </remarks>
            internal static class M1
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored matrix <see cref="M1"/>.
                /// </summary>
                private static readonly Mat.DenseMatrix<double> _m1 = CreateM1();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.DenseMatrix{T}"/> of type <see cref="M1"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_m1];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.DenseMatrix{T}"/> of type <see cref="M1"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateM1()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the matrix <see cref="M1"/>.
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
                /// Provides the expected number of columns of the matrix <see cref="M1"/>.
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


                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>M1<sup>t</sup>·M2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] TransposeMultiply_M2()
                {
                    double[] values = new double[]
                    {
                       1,    1,      0,
                       6,    3,      3,
                     -16,    5,    -21
                    };

                    return [new Mat.DenseMatrix<double>(3, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>M1·M2<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] MultiplyTranspose_M2()
                {
                    double[] values = new double[]
                    {
                    -11,     8,
                     21,    -6
                    };

                    return [new Mat.DenseMatrix<double>(2, 2, values)];
                }


                /// <summary>
                /// Provides the expected result of the transposition : <c>M1<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Transpose()
                {
                    double[] values = new double[]
                    {
                     1,    2,
                     4,    7,
                    -2,    3
                    };

                    return [new Mat.DenseMatrix<double>(3, 2, values)];
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>M1+M2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Addition_M2()
                {
                    double[] values = new double[]
                    {
                        6,    3,    4,
                        0,    8,    0
                    };

                    return [new Mat.DenseMatrix<double>(2, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>M1-M2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Subtraction_M2()
                {
                    double[] values = new double[]
                    {
                    -4,    5,    -8,
                     4,    6,     6
                    };

                    return [new Mat.DenseMatrix<double>(2, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-M1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] UnaryNegation()
                {
                    double[] values = new double[]
                    {
                        -1,    -4,     2,
                        -2,    -7,    -3
                    };

                    return [new Mat.DenseMatrix<double>(2, 3, values)];
                }


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 3.0 : <c>M1·3.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RightMultiplication_T()
                {
                    double[] values = new double[]
                    {
                    3,    12,    -6,
                    6,    21,     9
                    };

                    return [3.0, new Mat.DenseMatrix<double>(2, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·M1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] LeftMultiplication_T()
                {
                    double[] values = new double[]
                    {
                     5,    20,    -10,
                    10,    35,     15
                    };

                    return [5.0, new Mat.DenseMatrix<double>(2, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the scalar division by 4.0 : <c>M1/4.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Division_T()
                {
                    double[] values = new double[]
                    {
                    0.25,       1,    -0.5,
                     0.5,    1.75,    0.75,
                    };

                    return [4.0, new Mat.DenseMatrix<double>(2, 3, values)];
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
                        {    1,    4,    -2    },
                        {    2,    7,     3    },
                    };

                    return [values];
                }

                /// <summary>
                /// Provides the information to set a component in the matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected matrix after the component is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent()
                {
                    int rowIndex = 1;
                    int columnIndex = 1;
                    double value = -5;


                    int rowCount = 2;
                    int columnCount = 3;

                    double[] values = new double[]
                    {
                        1,     4,    -2,
                        2,    -5,     3,
                    };

                    Mat.DenseMatrix<double> result = new Mat.DenseMatrix<double>(rowCount, columnCount, values); 

                    return [rowIndex, columnIndex, value, result];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a matrix <see cref="M1"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.DenseMatrix{T}"/>. </returns>
                private static Mat.DenseMatrix<double> CreateM1()
                {
                    double[] values = new double[]
                    {
                        1,    4,    -2,
                        2,    7,     3
                    };

                    return new Mat.DenseMatrix<double>(2, 3, values);
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.DenseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M2 is a [2x3] matrix. </remarks>
            internal static class M2
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored matrix <see cref="M2"/>.
                /// </summary>
                private static readonly Mat.DenseMatrix<double> _m2 = CreateM2();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.DenseMatrix{T}"/> of type <see cref="M2"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_m2];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.DenseMatrix{T}"/> of type <see cref="M2"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateM2()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the matrix <see cref="M2"/>.
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
                /// Provides the expected number of columns of the matrix <see cref="M2"/>.
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


                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>M2<sup>t</sup>·M1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] TransposeMultiply_M1()
                {
                    double[] values = new double[]
                    {
                     1,    6,    -16,
                     1,    3,      5,
                     0,    3,    -21
                    };

                    return [new Mat.DenseMatrix<double>(3, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>M2·M1<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] MultiplyTranspose_M1()
                {
                    double[] values = new double[]
                    {
                    -11,    21,
                      8,    -6
                    };

                    return [new Mat.DenseMatrix<double>(2, 2, values)];
                }


                /// <summary>
                /// Provides the expected result of the transposition : <c>M2<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Transpose()
                {
                    double[] values = new double[]
                    {
                     5,    -2,
                    -1,     1,
                     6,    -3
                    };

                    return [new Mat.DenseMatrix<double>(3, 2, values)];
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>M2+M1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Addition_M1()
                {
                    double[] values = new double[]
                    {
                    6,    3,    4,
                    0,    8,    0
                    };

                    return [new Mat.DenseMatrix<double>(2, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>M2-M1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Subtraction_M1()
                {
                    double[] values = new double[]
                    {
                     4,    -5,     8,
                    -4,    -6,    -6
                    };

                    return [new Mat.DenseMatrix<double>(2, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-M2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] UnaryNegation()
                {
                    double[] values = new double[]
                    {
                    -5,     1,    -6,
                     2,    -1,     3
                    };

                    return [new Mat.DenseMatrix<double>(2, 3, values)];
                }


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 3.0 : <c>M2·3.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RightMultiplication_T()
                {
                    double[] values = new double[]
                    {
                    15,    -3,     18,
                    -6,     3,     -9
                    };

                    return [3.0, new Mat.DenseMatrix<double>(2, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·M2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] LeftMultiplication_T()
                {
                    double[] values = new double[]
                    {
                     25,    -5,     30,
                    -10,     5,    -15
                    };

                    return [5.0, new Mat.DenseMatrix<double>(2, 3, values)];
                }

                /// <summary>
                /// Provides the expected result of the scalar division by 4.0 : <c>M2/4.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Division_T()
                {
                    double[] values = new double[]
                    {
                    1.25,    -0.25,      1.5,
                    -0.5,     0.25,    -0.75
                    };

                    return [4.0, new Mat.DenseMatrix<double>(2, 3, values)];
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

                /// <summary>
                /// Provides the information to set a component in the matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected matrix after the component is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent()
                {
                    int rowIndex = 0;
                    int columnIndex = 0;
                    double value = 4;


                    int rowCount = 2;
                    int columnCount = 3;

                    double[] values = new double[]
                    {
                         4,    -1,     6,
                        -2,     1,    -3
                    };

                    Mat.DenseMatrix<double> result = new Mat.DenseMatrix<double>(rowCount, columnCount, values);

                    return [rowIndex, columnIndex, value, result];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a matrix <see cref="M2"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.DenseMatrix{T}"/>. </returns>
                private static Mat.DenseMatrix<double> CreateM2()
                {
                    double[] values = new double[]
                    {
                         5,    -1,     6,
                        -2,     1,    -3
                    };

                    return new Mat.DenseMatrix<double>(2, 3, values);
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.DenseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M3 is a [6x6] matrix. </remarks>
            internal static class M3
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored matrix <see cref="M3"/>.
                /// </summary>
                private static readonly Mat.DenseMatrix<double> _m3 = CreateM3();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.DenseMatrix{T}"/> of type <see cref="M3"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_m3];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.DenseMatrix{T}"/> of type <see cref="M3"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateM3()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the matrix <see cref="M3"/>.
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
                /// Provides the expected number of columns of the matrix <see cref="M3"/>.
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


                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>M3<sup>t</sup>·M4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] TransposeMultiply_M4()
                {
                    double[] values = new double[]
                    {
                     676,     702,     728,     754,     780,     806,
                     887,     924,     961,     998,    1035,    1072,
                    1395,    1450,    1505,    1560,    1615,    1670,
                    1817,    1884,    1951,    2018,    2085,    2152,
                    3232,    3334,    3436,    3538,    3640,    3742,
                    5178,    5316,    5454,    5592,    5730,    5868
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>M3·M4<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] MultiplyTranspose_M4()
                {
                    double[] values = new double[]
                    {
                    1910,    3260,    4610,    5960,    7310,    8660,
                    2107,    3577,    5047,    6517,    7987,    9457,
                     -48,     -78,    -108,    -138,    -168,    -198,
                     -44,     -44,     -44,     -44,     -44,     -44,
                     294,     494,     694,     894,    1094,    1294,
                    1902,    3162,    4422,    5682,    6942,    8202
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>M3<sup>t</sup>·V3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of <see cref="double"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] TransposeMultiply_V3() => [new double[] { 45, 80.5, 140, 179.5, 283, 368.5 }];


                /// <summary>
                /// Provides the expected result of the transposition : <c>M3<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Transpose()
                {
                    double[] values = new double[]
                    {
                    10,     7,     0,     6,    1,     2,
                    15,    14,    -1,     4,    1,     4,
                    20,    21,     2,     2,    2,     8,
                    25,    28,    -3,    -2,    3,    16,
                    30,    35,     4,    -4,    5,    32,
                    35,    42,    -5,    -6,    8,    64
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>M3+M4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Addition_M4()
                {
                    double[] values = new double[]
                    {
                    21,    27,    33,    39,    45,     51,
                    28,    36,    44,    52,    60,     68,
                    31,    31,    35,    31,    39,     31,
                    47,    46,    45,    42,    41,     40,
                    52,    53,    55,    57,    60,     64,
                    63,    66,    71,    80,    97,    130
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>M3-M4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Subtraction_M4()
                {
                    double[] values = new double[]
                    {
                     -1,      3,      7,     11,     15,     19,
                    -14,     -8,     -2,      4,     10,     16,
                    -31,    -33,    -31,    -37,    -31,    -41,
                    -35,    -38,    -41,    -46,    -49,    -52,
                    -50,    -51,    -51,    -51,    -50,    -48,
                    -59,    -58,    -55,    -48,    -33,     -2
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-M3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] UnaryNegation()
                {
                    double[] values = new double[]
                    {
                    -10,    -15,    -20,    -25,    -30,    -35,
                     -7,    -14,    -21,    -28,    -35,    -42,
                     -0,      1,     -2,      3,     -4,      5,
                     -6,     -4,     -2,      2,      4,      6,
                     -1,     -1,     -2,     -3,     -5,     -8,
                     -2,     -4,     -8,    -16,    -32,    -64
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>M3·M4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Multiplication_M4()
                {
                    double[] values = new double[]
                    {
                    5735,    5870,    6005,    6140,    6275,    6410,
                    6517,    6664,    6811,    6958,    7105,    7252,
                    -183,    -186,    -189,    -192,    -195,    -198,
                    -440,    -440,    -440,    -440,    -440,    -440,
                     960,     980,    1000,    1020,    1040,    1060,
                    6546,    6672,    6798,    6924,    7050,    7176
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 3.0 : <c>M3·3.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RightMultiplication_T()
                {
                    double[] values = new double[]
                    {
                    30,    45,    60,    75,     90,    105,
                    21,    42,    63,    84,    105,    126,
                     0,    -3,     6,    -9,     12,    -15,
                    18,    12,     6,    -6,    -12,    -18,
                     3,     3,     6,     9,     15,     24,
                     6,    12,    24,    48,     96,    192
                    };

                    return [3.0, new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·M3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] LeftMultiplication_T()
                {
                    double[] values = new double[]
                    {
                    50,    75,    100,    125,    150,    175,
                    35,    70,    105,    140,    175,    210,
                     0,    -5,     10,    -15,     20,    -25,
                    30,    20,     10,    -10,    -20,    -30,
                     5,     5,     10,     15,     25,     40,
                    10,    20,     40,     80,    160,    320
                    };

                    return [5.0, new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the scalar division by 4.0 : <c>M3/4.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Division_T()
                {
                    double[] values = new double[]
                    {
                     2.5,     3.75,       5,     6.25,     7.5,     8.75,
                    1.75,      3.5,    5.25,        7,    8.75,     10.5,
                       0,    -0.25,     0.5,    -0.75,       1,    -1.25,
                     1.5,        1,     0.5,     -0.5,      -1,     -1.5,
                    0.25,     0.25,     0.5,     0.75,    1.25,        2,
                     0.5,        1,       2,        4,       8,       16
                    };

                    return [4.0, new Mat.DenseMatrix<double>(6, 6, values)];
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>M3·V3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of <see cref="double"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Multiplication_V3() => [new double[] { 395, 437.5, 16, -9, 58, 368 }];


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

                /// <summary>
                /// Provides the information to set a component in the matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected matrix after the component is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent()
                {
                    int rowIndex = 3;
                    int columnIndex = 4;
                    double value = 8;


                    int rowCount = 6;
                    int columnCount = 6;

                    double[] values = new double[]
                    {
                        10,    15,    20,    25,    30,    35,
                         7,    14,    21,    28,    35,    42,
                         0,    -1,     2,    -3,     4,    -5,
                         6,     4,     2,    -2,     8,    -6,
                         1,     1,     2,     3,     5,     8,
                         2,     4,     8,    16,    32,    64
                    };

                    Mat.DenseMatrix<double> result = new Mat.DenseMatrix<double>(rowCount, columnCount, values);

                    return [rowIndex, columnIndex, value, result];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a matrix <see cref="M3"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.DenseMatrix{T}"/>. </returns>
                private static Mat.DenseMatrix<double> CreateM3()
                {
                    double[] values = new double[]
                    {
                        10,    15,    20,    25,    30,    35,
                         7,    14,    21,    28,    35,    42,
                         0,    -1,     2,    -3,     4,    -5,
                         6,     4,     2,    -2,    -4,    -6,
                         1,     1,     2,     3,     5,     8,
                         2,     4,     8,    16,    32,    64
                    };

                    return new Mat.DenseMatrix<double>(6, 6, values);
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.DenseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M4 is a [6x6] matrix. </remarks>
            internal static class M4
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored matrix <see cref="M4"/>.
                /// </summary>
                private static readonly Mat.DenseMatrix<double> _m4 = CreateM4();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Mat.DenseMatrix{T}"/> of type <see cref="M4"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_m4];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Mat.DenseMatrix{T}"/> of type <see cref="M4"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateM4()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the matrix <see cref="M4"/>.
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
                /// Provides the expected number of columns of the matrix <see cref="M4"/>.
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


                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>M4<sup>t</sup>·M3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] TransposeMultiply_M3()
                {
                    double[] values = new double[]
                    {
                    676,     887,    1395,    1817,    3232,    5178,
                    702,     924,    1450,    1884,    3334,    5316,
                    728,     961,    1505,    1951,    3436,    5454,
                    754,     998,    1560,    2018,    3538,    5592,
                    780,    1035,    1615,    2085,    3640,    5730,
                    806,    1072,    1670,    2152,    3742,    5868
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the multiplication : <c>M4·M3<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] MultiplyTranspose_M3()
                {
                    double[] values = new double[]
                    {
                    1910,    2107,     -48,    -44,     294,    1902,
                    3260,    3577,     -78,    -44,     494,    3162,
                    4610,    5047,    -108,    -44,     694,    4422,
                    5960,    6517,    -138,    -44,     894,    5682,
                    7310,    7987,    -168,    -44,    1094,    6942,
                    8660,    9457,    -198,    -44,    1294,    8202
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>M4<sup>t</sup>·V3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of <see cref="double"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] TransposeMultiply_V3() => [new double[] { 641.5, 658, 674.5, 691, 707.5, 724 }];


                /// <summary>
                /// Provides the expected result of the transposition : <c>M4<sup>t</sup></c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the transposition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Transpose()
                {
                    double[] values = new double[]
                    {
                    11,    21,    31,    41,    51,    61,
                    12,    22,    32,    42,    52,    62,
                    13,    23,    33,    43,    53,    63,
                    14,    24,    34,    44,    54,    64,
                    15,    25,    35,    45,    55,    65,
                    16,    26,    36,    46,    56,    66
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>M4+M3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Addition_M3()
                {
                    double[] values = new double[]
                    {
                    21,    27,    33,    39,    45,     51,
                    28,    36,    44,    52,    60,     68,
                    31,    31,    35,    31,    39,     31,
                    47,    46,    45,    42,    41,     40,
                    52,    53,    55,    57,    60,     64,
                    63,    66,    71,    80,    97,    130
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the subtraction : <c>M4-M3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Subtraction_M3()
                {
                    double[] values = new double[]
                    {
                     1,    -3,    -7,   -11,    -15,    -19,
                    14,     8,     2,    -4,    -10,    -16,
                    31,    33,    31,    37,     31,     41,
                    35,    38,    41,    46,     49,     52,
                    50,    51,    51,    51,     50,     48,
                    59,    58,    55,    48,     33,      2
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-M4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] UnaryNegation()
                {
                    double[] values = new double[]
                    {
                    -11,    -12,    -13,    -14,    -15,    -16,
                    -21,    -22,    -23,    -24,    -25,    -26,
                    -31,    -32,    -33,    -34,    -35,    -36,
                    -41,    -42,    -43,    -44,    -45,    -46,
                    -51,    -52,    -53,    -54,    -55,    -56,
                    -61,    -62,    -63,    -64,    -65,    -66
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>M4·M3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Multiplication_M3()
                {
                    double[] values = new double[]
                    {
                     325,     455,     684,     845,    1333,    1884,
                     585,     825,    1234,    1515,    2353,    3264,
                     845,    1195,    1784,    2185,    3373,    4644,
                    1105,    1565,    2334,    2855,    4393,    6024,
                    1365,    1935,    2884,    3525,    5413,    7404,
                    1625,    2305,    3434,    4195,    6433,    8784
                    };

                    return [new Mat.DenseMatrix<double>(6, 6, values)];
                }


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 7.0 : <c>M4·7.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RightMultiplication_T()
                {
                    double[] values = new double[]
                    {
                     77,     84,     91,     98,    105,    112,
                    147,    154,    161,    168,    175,    182,
                    217,    224,    231,    238,    245,    252,
                    287,    294,    301,    308,    315,    322,
                    357,    364,    371,    378,    385,    392,
                    427,    434,    441,    448,    455,    462
                    };

                    return [7.0, new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 2.0 : <c>2.0·M4</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] LeftMultiplication_T()
                {
                    double[] values = new double[]
                    {
                     22,     24,     26,     28,     30,     32,
                     42,     44,     46,     48,     50,     52,
                     62,     64,     66,     68,     70,     72,
                     82,     84,     86,     88,     90,     92,
                    102,    104,    106,    108,    110,    112,
                    122,    124,    126,    128,    130,    132
                    };

                    return [2.0, new Mat.DenseMatrix<double>(6, 6, values)];
                }

                /// <summary>
                /// Provides the expected result of the scalar division by 2.0 : <c>M4/2.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                ///         <term> 1 </term>
                ///         <description> An <see cref="Mat.DenseMatrix{T}"/> representing the result of the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Division_T()
                {
                    double[] values = new double[]
                    {
                     5.5,     6,     6.5,     7,     7.5,     8,
                    10.5,    11,    11.5,    12,    12.5,    13,
                    15.5,    16,    16.5,    17,    17.5,    18,
                    20.5,    21,    21.5,    22,    22.5,    23,
                    25.5,    26,    26.5,    27,    27.5,    28,
                    30.5,    31,    31.5,    32,    32.5,    33
                    };

                    return [2.0, new Mat.DenseMatrix<double>(6, 6, values)];
                }


                /// <summary>
                /// Provides the expected result of the multiplication : <c>M4·V3</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of <see cref="double"/> representing the result of the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Multiplication_V3() => [new double[] { 227.5, 392.5, 557.5, 722.5, 887.5, 1052.5 }];


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

                /// <summary>
                /// Provides the information to set a component in the matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected matrix after the component is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent()
                {
                    int rowIndex = 5;
                    int columnIndex = 5;
                    double value = 0;


                    int rowCount = 6;
                    int columnCount = 6;

                    double[] values = new double[]
                    {
                        11,     12,     13,     14,     15,     16,
                        21,     22,     23,     24,     25,     26,
                        31,     32,     33,     34,     35,     36,
                        41,     42,     43,     44,     45,     46,
                        51,     52,     53,     54,     55,     56,
                        61,     62,     63,     64,     65,      0
                    };

                    Mat.DenseMatrix<double> result = new Mat.DenseMatrix<double>(rowCount, columnCount, values);

                    return [rowIndex, columnIndex, value, result];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a matrix <see cref="M4"/>.
                /// </summary>
                /// <returns> The <see cref="Mat.DenseMatrix{T}"/>. </returns>
                private static Mat.DenseMatrix<double> CreateM4()
                {
                    double[] values = new double[]
                    {
                        11,     12,     13,     14,     15,     16,
                        21,     22,     23,     24,     25,     26,
                        31,     32,     33,     34,     35,     36,
                        41,     42,     43,     44,     45,     46,
                        51,     52,     53,     54,     55,     56,
                        61,     62,     63,     64,     65,     66
                    };

                    return new Mat.DenseMatrix<double>(6, 6, values);
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
            /// Class data for <see cref="DenseMatrix.Property_RowCount(Mat.DenseMatrix{double}, int)"/>.
            /// </summary>
            internal class RowCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M1.RowCount },
                        { DataStorages.M2.Readable, DataStorages.M2.RowCount },
                        { DataStorages.M3.Readable, DataStorages.M3.RowCount },
                        { DataStorages.M4.Readable, DataStorages.M4.RowCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Property_ColumnCount(Mat.DenseMatrix{double}, int)"/>.
            /// </summary>
            internal class ColumnCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M1.ColumnCount },
                        { DataStorages.M2.Readable, DataStorages.M2.ColumnCount },
                        { DataStorages.M3.Readable, DataStorages.M3.ColumnCount },
                        { DataStorages.M4.Readable, DataStorages.M4.ColumnCount },
                    };
            }

            #endregion

            #region For Operations

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Addition__DenseMatrix_DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Add__DenseMatrix_DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Addition__DenseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M2.Readable, DataStorages.M1.Addition_M2 },
                        { DataStorages.M2.Readable, DataStorages.M1.Readable, DataStorages.M2.Addition_M1 },
                        { DataStorages.M3.Readable, DataStorages.M4.Readable, DataStorages.M3.Addition_M4 },
                        { DataStorages.M4.Readable, DataStorages.M3.Readable, DataStorages.M4.Addition_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Subtraction__DenseMatrix_DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Add__DenseMatrix_DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Subtraction__DenseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M2.Readable, DataStorages.M1.Subtraction_M2 },
                        { DataStorages.M2.Readable, DataStorages.M1.Readable, DataStorages.M2.Subtraction_M1 },
                        { DataStorages.M3.Readable, DataStorages.M4.Readable, DataStorages.M3.Subtraction_M4 },
                        { DataStorages.M4.Readable, DataStorages.M3.Readable, DataStorages.M4.Subtraction_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__UnaryNegation__DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Opposite__DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class UnaryNegation__DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M1.UnaryNegation },
                        { DataStorages.M2.Readable, DataStorages.M2.UnaryNegation },
                        { DataStorages.M3.Readable, DataStorages.M3.UnaryNegation },
                        { DataStorages.M4.Readable, DataStorages.M4.UnaryNegation },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Multiplication__DenseMatrix_DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Multiply__DenseMatrix_DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Multiplication__DenseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M3.Readable, DataStorages.M4.Readable, DataStorages.M3.Multiplication_M4 },
                        { DataStorages.M4.Readable, DataStorages.M3.Readable, DataStorages.M4.Multiplication_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseMatrix.Static__TransposeMultiply__DenseMatrix_DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class TransposeMultiplication__DenseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M2.Readable, DataStorages.M1.TransposeMultiply_M2 },
                        { DataStorages.M2.Readable, DataStorages.M1.Readable, DataStorages.M2.TransposeMultiply_M1 },
                        { DataStorages.M3.Readable, DataStorages.M4.Readable, DataStorages.M3.TransposeMultiply_M4 },
                        { DataStorages.M4.Readable, DataStorages.M3.Readable, DataStorages.M4.TransposeMultiply_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Static__MultiplyTranspose__DenseMatrix_DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class MultiplicationTranspose__DenseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M2.Readable, DataStorages.M1.MultiplyTranspose_M2 },
                        { DataStorages.M2.Readable, DataStorages.M1.Readable, DataStorages.M2.MultiplyTranspose_M1 },
                        { DataStorages.M3.Readable, DataStorages.M4.Readable, DataStorages.M3.MultiplyTranspose_M4 },
                        { DataStorages.M4.Readable, DataStorages.M3.Readable, DataStorages.M4.MultiplyTranspose_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Multiplication__DenseMatrix_T(Mat.DenseMatrix{double}, double, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Multiply__DenseMatrix_T(Mat.DenseMatrix{double}, double, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Multiplication__DenseMatrix_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M1.RightMultiplication_T },
                        { DataStorages.M2.Readable, DataStorages.M2.RightMultiplication_T },
                        { DataStorages.M3.Readable, DataStorages.M3.RightMultiplication_T },
                        { DataStorages.M4.Readable, DataStorages.M4.RightMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Multiplication__T_DenseMatrix(Mat.DenseMatrix{double}, double, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Multiply__T_DenseMatrix(Mat.DenseMatrix{double}, double, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Multiplication__T_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M1.LeftMultiplication_T },
                        { DataStorages.M2.Readable, DataStorages.M2.LeftMultiplication_T },
                        { DataStorages.M3.Readable, DataStorages.M3.LeftMultiplication_T },
                        { DataStorages.M4.Readable, DataStorages.M4.LeftMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Division__DenseMatrix_T(Mat.DenseMatrix{double}, double, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Divide__DenseMatrix_T(Mat.DenseMatrix{double}, double, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Division__DenseMatrix_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M1.Division_T },
                        { DataStorages.M2.Readable, DataStorages.M2.Division_T },
                        { DataStorages.M3.Readable, DataStorages.M3.Division_T },
                        { DataStorages.M4.Readable, DataStorages.M4.Division_T },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseMatrix.Static__Transpose__DenseMatrix(Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Transposition__DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M1.Transpose },
                        { DataStorages.M2.Readable, DataStorages.M2.Transpose },
                        { DataStorages.M3.Readable, DataStorages.M3.Transpose },
                        { DataStorages.M4.Readable, DataStorages.M4.Transpose },
                    };
            }



            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Addition__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Add__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Add__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Addition__DenseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with CompressedColumn
                        { DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, DataStorages.M1.Addition_M2 },
                        { DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, DataStorages.M2.Addition_M1 },
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DataStorages.M3.Addition_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DataStorages.M4.Addition_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, DataStorages.M1.Addition_M2 },
                        { DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, DataStorages.M2.Addition_M1 },
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DataStorages.M3.Addition_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DataStorages.M4.Addition_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Subtraction__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Add__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Add__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Subtraction__DenseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with CompressedColumn
                        { DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, DataStorages.M1.Subtraction_M2 },
                        { DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, DataStorages.M2.Subtraction_M1 },
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DataStorages.M3.Subtraction_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DataStorages.M4.Subtraction_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, DataStorages.M1.Subtraction_M2 },
                        { DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, DataStorages.M2.Subtraction_M1 },
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DataStorages.M3.Subtraction_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DataStorages.M4.Subtraction_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Multiplication__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="DenseMatrix.Static__Multiply__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Multiply__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Multiplication__DenseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with CompressedColumn
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DataStorages.M3.Multiplication_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DataStorages.M4.Multiplication_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DataStorages.M3.Multiplication_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DataStorages.M4.Multiplication_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseMatrix.Static__TransposeMultiply__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__TransposeMultiply__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class TransposeMultiplication__DenseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with CompressedColumn
                        { DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, DataStorages.M1.TransposeMultiply_M2 },
                        { DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, DataStorages.M2.TransposeMultiply_M1 },
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DataStorages.M3.TransposeMultiply_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DataStorages.M4.TransposeMultiply_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, DataStorages.M1.TransposeMultiply_M2 },
                        { DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, DataStorages.M2.TransposeMultiply_M1 },
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DataStorages.M3.TransposeMultiply_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DataStorages.M4.TransposeMultiply_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Static__MultiplyTranspose__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__MultiplyTranspose__DenseMatrix_SparseMatrix(Mat.DenseMatrix{double}, Mat.SparseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class MultiplicationTranspose__DenseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with CompressedColumn
                        { DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, DataStorages.M1.MultiplyTranspose_M2 },
                        { DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, DataStorages.M2.MultiplyTranspose_M1 },
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DataStorages.M3.MultiplyTranspose_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DataStorages.M4.MultiplyTranspose_M3 },
                        // For DenseMatrix with CompressedColumn
                        { DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, DataStorages.M1.MultiplyTranspose_M2 },
                        { DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, DataStorages.M2.MultiplyTranspose_M1 },
                        { DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DataStorages.M3.MultiplyTranspose_M4 },
                        { DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DataStorages.M4.MultiplyTranspose_M3 },
                    };
            }



            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Multiplication__DenseMatrix_DenseVector(Mat.DenseMatrix{double}, Vect.DenseVector{double}, double[])"/> and
            /// <see cref="DenseMatrix.Static__Multiply__DenseMatrix_DenseVector(Mat.DenseMatrix{double}, Vect.DenseVector{double}, double[])"/> and
            /// </summary>
            internal class Multiplication__DenseMatrix_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M3.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M3.Multiplication_V3 },
                        { DataStorages.M4.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M4.Multiplication_V3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Operator__Multiplication__DenseMatrix_SparseVector(Mat.DenseMatrix{double}, Vect.SparseVector{double}, double[])"/> and
            /// <see cref="DenseMatrix.Static__Multiply__DenseMatrix_SparseVector(Mat.DenseMatrix{double}, Vect.SparseVector{double}, double[])"/> and
            /// </summary>
            internal class Multiplication__DenseMatrix_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M3.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M3.Multiplication_V3 },
                        { DataStorages.M4.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M4.Multiplication_V3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseMatrix.Static__TransposeMultiply__DenseMatrix_DenseVector(Mat.DenseMatrix{double}, Vect.DenseVector{double}, double[])"/>.
            /// </summary>
            internal class TransposeMultiplication__DenseMatrix_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M3.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M3.TransposeMultiply_V3 },
                        { DataStorages.M4.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M4.TransposeMultiply_V3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.Static__TransposeMultiply__DenseMatrix_SparseVector(Mat.DenseMatrix{double}, Vect.SparseVector{double}, double[])"/>.
            /// </summary>
            internal class TransposeMultiplication__DenseMatrix_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M3.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M3.TransposeMultiply_V3 },
                        { DataStorages.M4.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M4.TransposeMultiply_V3 },
                    };
            }

            #endregion

            #region For Methods

            /// <summary>
            /// Class data for <see cref="DenseMatrix.GetComponent__Int_Int(Mat.DenseMatrix{double}, double[,])"/>,
            /// <see cref="DenseMatrix.ToArray(Mat.DenseMatrix{double}, double[,])"/>,
            /// <see cref="DenseMatrix.ToRowMajorArray(Mat.DenseMatrix{double}, double[,])"/>, 
            /// <see cref="DenseMatrix.ToColumnMajorArray(Mat.DenseMatrix{double}, double[,])"/>,
            /// <see cref="DenseMatrix.RowVectors(Mat.DenseMatrix{double}, double[,])"/> and
            /// <see cref="DenseMatrix.ColumnVectors(Mat.DenseMatrix{double}, double[,])"/>.
            /// </summary>
            internal class ToArray : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Readable, DataStorages.M1.ToArray },
                        { DataStorages.M2.Readable, DataStorages.M2.ToArray },
                        { DataStorages.M3.Readable, DataStorages.M3.ToArray },
                        { DataStorages.M4.Readable, DataStorages.M4.ToArray },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseMatrix.SetComponent__Int_Int(Mat.DenseMatrix{double}, int, int, double, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class SetComponent : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.M1.Writable, DataStorages.M1.SetComponent },
                        { DataStorages.M2.Writable, DataStorages.M2.SetComponent },
                        { DataStorages.M3.Writable, DataStorages.M3.SetComponent },
                        { DataStorages.M4.Writable, DataStorages.M4.SetComponent },
                    };
            }

            #endregion
        }


        #endregion
    }
}
