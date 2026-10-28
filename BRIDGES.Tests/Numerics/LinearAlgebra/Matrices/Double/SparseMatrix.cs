using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

using Mat = BRIDGES.Numerics.LinearAlgebra.Matrices;
using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Matrices.Double
{
    /// <summary>
    /// Tests the members of the <see cref=Mat.SparseMatrix{T}"/> class.
    /// </summary>
    public class SparseMatrix
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Mat.Matrix{T}.RowCount"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to evaluate. </param>
        /// <param name="expected"> Expected number of row. </param>
        [Theory(DisplayName = "Prop. RowCount")]
        [ClassData(typeof(DataClasses.RowCount))]
        public void Property_RowCount(Mat.SparseMatrix<double> matrix, int expected)
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
        public void Property_ColumnCount(Mat.SparseMatrix<double> matrix, int expected)
        {
            // Arrange

            //Act
            int actual = matrix.ColumnCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Tests : Public Static Methods

        //     -----     -----     Algebraic Near Ring : SparseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Add(Mat.SparseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(SparseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Addition__SparseMatrix_SparseMatrix))]
        public void Static__Add__SparseMatrix_SparseMatrix(Mat.SparseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.Add(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (expected.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Subtract(Mat.SparseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(SparseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Subtraction__SparseMatrix_SparseMatrix))]
        public void Static__Subtract__SparseMatrix_SparseMatrix(Mat.SparseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.Subtract(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Opposite(Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.SparseMatrix{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Static Opposite(SparseMatrix)")]
        [ClassData(typeof(DataClasses.UnaryNegation__SparseMatrix))]
        public void Static__Opposite__SparseMatrix(Mat.SparseMatrix<double> operand, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.Opposite(operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (expected.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Multiply(Mat.SparseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(SparseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_SparseMatrix))]
        public void Static__Multiply__SparseMatrix_SparseMatrix(Mat.SparseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.Multiply(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (expected.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }


        //     -----     Other Operations : SparseMatrix<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.TransposeMultiply(Mat.SparseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(SparseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__SparseMatrix_SparseMatrix))]
        public void Static__TransposeMultiply__SparseMatrix_SparseMatrix(Mat.SparseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.TransposeMultiply(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if(expected.Contains(i_Row, i_Column))
                    {
                        if (expected.Contains(i_Row, i_Column))
                        {
                            Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.MultiplyTranspose(Mat.SparseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static MultiplyTranspose(SparseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.MultiplicationTranspose__SparseMatrix_SparseMatrix))]
        public void Static__MultiplyTranspose__SparseMatrix_SparseMatrix(Mat.SparseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.MultiplyTranspose(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (expected.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }


        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Add(Mat.SparseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(SparseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Addition__SparseMatrix_DenseMatrix))]
        public void Static__Add__SparseMatrix_DenseMatrix(Mat.SparseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.Add(left, right);

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Subtract(Mat.SparseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(SparseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Subtraction__SparseMatrix_DenseMatrix))]
        public void Static__Subtract__SparseMatrix_DenseMatrix(Mat.SparseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.Subtract(left, right);

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Multiply(Mat.SparseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(SparseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_DenseMatrix))]
        public void Static__Multiply__SparseMatrix_DenseMatrix(Mat.SparseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.Multiply(left, right);

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


        //     -----     Other Right Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.TransposeMultiply(Mat.SparseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(SparseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__SparseMatrix_DenseMatrix))]
        public void Static__TransposeMultiply__SparseMatrix_DenseMatrix(Mat.SparseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.TransposeMultiply(left, right);

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.MultiplyTranspose(Mat.SparseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static MultiplyTranspose(SparseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.MultiplicationTranspose__SparseMatrix_DenseMatrix))]
        public void Static__MultiplyTranspose__SparseMatrix_DenseMatrix(Mat.SparseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.MultiplyTranspose(left, right);

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


        //     -----     -----     Left Embedding : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Add(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DenseMatrix.DataClasses.Addition__DenseMatrix_SparseMatrix))]
        public void Static__Add__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.AdditionOperator(left, right);

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Subtract(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DenseMatrix.DataClasses.Subtraction__DenseMatrix_SparseMatrix))]
        public void Static__Subtract__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.SubtractionOperator(left, right);

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Multiply(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DenseMatrix.DataClasses.Multiplication__DenseMatrix_SparseMatrix))]
        public void Static__Multiply__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.MultiplyOperator(left, right);

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


        //     -----     Other Left Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.TransposeMultiply(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DenseMatrix.DataClasses.TransposeMultiplication__DenseMatrix_SparseMatrix))]
        public void Static__TransposeMultiply__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.TransposeMultiply(left, right);

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.MultiplyTranspose(Mat.DenseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.DenseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static MultiplyTranspose(DenseMatrix,SparseMatrix)")]
        [ClassData(typeof(DenseMatrix.DataClasses.MultiplicationTranspose__DenseMatrix_SparseMatrix))]
        public void Static__MultiplyTranspose__DenseMatrix_SparseMatrix(Mat.DenseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.DenseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.DenseMatrix<double> result = Mat.SparseMatrix<double>.MultiplyTranspose(left, right);

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Multiply(Mat.SparseMatrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.SparseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(SparseMatrix,T)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_T))]
        public void Static__Multiply__SparseMatrix_T(Mat.SparseMatrix<double> operand, double factor, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.Multiply(operand, factor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (expected.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Multiply(T,Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Mat.SparseMatrix{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(T,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__T_SparseMatrix))]
        public void Static__Multiply__T_SparseMatrix(Mat.SparseMatrix<double> operand, double factor, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.Multiply(factor, operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (expected.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Divide(Mat.SparseMatrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.SparseMatrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the division. </param>
        [Theory(DisplayName = "Static Divide(SparseMatrix,T)")]
        [ClassData(typeof(DataClasses.Division__SparseMatrix_T))]
        public void Static__Divide__SparseMatrix_T(Mat.SparseMatrix<double> operand, double divisor, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = Mat.SparseMatrix<double>.Divide(operand, divisor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (expected.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }



        //     -----      -----     Vectors     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Multiply(Mat.SparseMatrix{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.SparseMatrix{T}"/> to multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(SparseMatrix,SparseVector)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_SparseVector))]
        public void Static__Multiply__SparseMatrix_SparseVector(Mat.SparseMatrix<double> matrix, Vect.SparseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = Mat.SparseMatrix<double>.Multiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != 0.0)
                {
                    Assert.Equal(expected[i], result[i]);
                }
                else { Assert.False(result.Contains(i)); }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.Multiply(Mat.SparseMatrix{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.SparseMatrix{T}"/> to multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(SparseMatrix,DenseVector)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_DenseVector))]
        public void Static__Multiply__SparseMatrix_DenseVector(Mat.SparseMatrix<double> matrix, Vect.DenseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Mat.SparseMatrix<double>.Multiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }



        //     -----      Other Operations : Vectors     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.TransposeMultiply(Mat.SparseMatrix{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(SparseMatrix,SparseVector)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__SparseMatrix_SparseVector))]
        public void Static__TransposeMultiply__SparseMatrix_SparseVector(Mat.SparseMatrix<double> matrix, Vect.SparseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = Mat.SparseMatrix<double>.TransposeMultiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != 0.0)
                {
                    Assert.Equal(expected[i], result[i]);
                }
                else { Assert.False(result.Contains(i)); }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.TransposeMultiply(Mat.SparseMatrix{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(SparseMatrix,DenseVector)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__SparseMatrix_DenseVector))]
        public void Static__TransposeMultiply__SparseMatrix_DenseVector(Mat.SparseMatrix<double> matrix, Vect.DenseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Mat.SparseMatrix<double>.TransposeMultiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }
        
        #endregion

        #region Tests : Operators

        //     -----     -----     Algebraic Near Ring : SparseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator+(Mat.SparseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (SparseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Addition__SparseMatrix_SparseMatrix))]
        public void Operator__Addition__SparseMatrix_SparseMatrix(Mat.SparseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = left + right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if(result.Contains(i_Row,i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator-(Mat.SparseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (SparseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Subtraction__SparseMatrix_SparseMatrix))]
        public void Operator__Subtraction__SparseMatrix_SparseMatrix(Mat.SparseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = left - right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (result.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator-(Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.SparseMatrix{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Op - (SparseMatrix)")]
        [ClassData(typeof(DataClasses.UnaryNegation__SparseMatrix))]
        public void Operator__UnaryNegation__SparseMatrix(Mat.SparseMatrix<double> operand, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = -operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (result.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator*(Mat.SparseMatrix{T},Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Op * (SparseMatrix,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_SparseMatrix))]
        public void Operator__Multiplication__SparseMatrix_SparseMatrix(Mat.SparseMatrix<double> left, Mat.SparseMatrix<double> right, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = left * right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (result.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }



        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator+(Mat.SparseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (SparseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Addition__SparseMatrix_DenseMatrix))]
        public void Operator__Addition__SparseMatrix_DenseMatrix(Mat.SparseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator-(Mat.SparseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (SparseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Subtraction__SparseMatrix_DenseMatrix))]
        public void Operator__Subtraction__SparseMatrix_DenseMatrix(Mat.SparseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator*(Mat.SparseMatrix{T},Mat.DenseMatrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.DenseMatrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Op * (SparseMatrix,DenseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_DenseMatrix))]
        public void Operator__Multiplication__SparseMatrix_DenseMatrix(Mat.SparseMatrix<double> left, Mat.DenseMatrix<double> right, Mat.DenseMatrix<double> expected)
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



        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator*(Mat.SparseMatrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.SparseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Op * (SparseMatrix,T)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_T))]
        public void Operator__Multiplication__SparseMatrix_T(Mat.SparseMatrix<double> operand, double factor, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = operand * factor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (result.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator*(T,Mat.SparseMatrix{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Mat.SparseMatrix{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Op * (T,SparseMatrix)")]
        [ClassData(typeof(DataClasses.Multiplication__T_SparseMatrix))]
        public void Operator__Multiplication__T_SparseMatrix(Mat.SparseMatrix<double> operand, double factor, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = factor * operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (result.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator/(Mat.SparseMatrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.SparseMatrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.SparseMatrix{T}"/> of the division. </param>
        [Theory(DisplayName = "Op / (SparseMatrix,T)")]
        [ClassData(typeof(DataClasses.Division__SparseMatrix_T))]
        public void Operator__Division__SparseMatrix_T(Mat.SparseMatrix<double> operand, double divisor, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.SparseMatrix<double> result = operand / divisor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    if (result.Contains(i_Row, i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], result[i_Row, i_Column]);
                    }
                }
            }
        }



        //     -----      -----     Vectors     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator*(Mat.SparseMatrix{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="vector"> Right <see cref="Vect.SparseVector{T}"/> for the multiplication. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Op * (SparseMatrix,SparseVector)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_SparseVector))]
        public void Operator__Multiplication__SparseMatrix_SparseVector(Mat.SparseMatrix<double> matrix, Vect.SparseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = matrix * vector;

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                if (expected[i] != 0.0)
                {
                    Assert.Equal(expected[i], result[i]);
                }
                else { Assert.False(result.Contains(i)); }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix{T}.operator*(Mat.SparseMatrix{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="vector"> Right <see cref="Vect.DenseVector{T}"/> for the multiplication. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Op * (SparseMatrix,DenseVector)")]
        [ClassData(typeof(DataClasses.Multiplication__SparseMatrix_DenseVector))]
        public void Operator__Multiplication__SparseMatrix_DenseVector(Mat.SparseMatrix<double> matrix, Vect.DenseVector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = matrix * vector;

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.ToArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="expected"> Two-dimensional array containing the values of components. </param>
        [Theory(DisplayName = "GetComponent(int, int)")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void GetComponent__Int_Int(Mat.SparseMatrix<double> matrix, double[,] expected)
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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.SetComponent(int, int, T)"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to set. </param>
        /// <param name="columnIndex"> Column index of the component to set. </param>
        /// <param name="value"> Value of the component to set. </param>
        /// <param name="expected"> Expected sparse storage after the component iis set. </param>
        [Theory(DisplayName = "SetComponent(int, int, T)")]
        [ClassData(typeof(DataClasses.SetComponent))]
        public void SetComponent__Int_Int(Mat.SparseMatrix<double> matrix, int rowIndex, int columnIndex, double value, Mat.SparseMatrix<double> expected)
        {
            // Arrange

            // Act
            matrix.SetComponent(rowIndex, columnIndex, value);

            // Assert
            Assert.Equal(expected.RowCount, matrix.RowCount);
            Assert.Equal(expected.ColumnCount, matrix.ColumnCount);
            Assert.Equal(expected.NonZeroCount, matrix.NonZeroCount);
            for (int i_Column = 0; i_Column < expected.ColumnCount; i_Column++)
            {
                for (int i_Row = 0; i_Row < expected.RowCount; i_Row++)
                {
                    if(expected.Contains(i_Row,i_Column))
                    {
                        Assert.Equal(expected[i_Row, i_Column], matrix[i_Row, i_Column]);
                    }
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix.Contains(int, int)"/>.
        /// </summary>
        /// <param name="matrix"> Sparse storage to operate on. </param>
        /// <param name="isContained"> Two-dimensional array evaluating whether a component at the given row and column has an entry in the sparse storage.</param>
        [Theory(DisplayName = "Contains(int, int)")]
        [ClassData(typeof(DataClasses.Contains))]
        public void Contains__Int_Int(Mat.SparseMatrix<double> matrix, bool[,] isContained)
        {
            // Arrange

            // Act & Assert
            Assert.Equal(isContained.GetLength(0), matrix.RowCount);
            Assert.Equal(isContained.GetLength(1), matrix.ColumnCount);
            for (int i_Row = 0; i_Row < matrix.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < matrix.ColumnCount; i_Column++)
                {
                    Assert.Equal(isContained[i_Row, i_Column], matrix.Contains(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.SparseMatrix.ToArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="expected"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToArray(Mat.SparseMatrix<double> matrix, double[,] expected)
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
        /// Tests the method <see cref="Mat.SparseMatrix.ToRowMajorArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToRowMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToRowMajorArray(Mat.SparseMatrix<double> matrix, double[,] values)
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
        /// Tests the method <see cref="Mat.SparseMatrix.ToColumnMajorArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToColumnMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToColumnMajorArray(Mat.SparseMatrix<double> matrix, double[,] values)
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
        /// Tests the method <see cref="Mat.SparseMatrix.RowVectors()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "RowVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void RowVectors(Mat.SparseMatrix<double> matrix, double[,] values)
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
            Vect.SparseVector<double>[] result = matrix.RowVectors();

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
        /// Tests the method <see cref="Mat.SparseMatrix{T}.ColumnVectors()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ColumnVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ColumnVectors(Mat.SparseMatrix<double> matrix, double[,] values)
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
            Vect.SparseVector<double>[] result = matrix.ColumnVectors();

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
            /// Computes and stores general data related to general <see cref="Mat.SparseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M1 is a [2x3] matrix. </remarks>
            internal static class M1
            {
                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.CompressedColumn{T}"/>
                /// </summary>
                internal static class FromCC
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M1"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m1 = CreateM1();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M1"/>. </description>
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
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M1"/>. </description>
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
                    public static object[] RowCount() => SparseStorages.CompressedColumn.DataStorages.S1.RowCount();

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
                    public static object[] ColumnCount() => SparseStorages.CompressedColumn.DataStorages.S1.ColumnCount();


                    //     -----     Public Static Methods

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M1<sup>t</sup>·M2</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] TransposeMultiply_M2()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S1.TransposeMultiply_S2();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M1·M2<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] MultiplyTranspose_M2()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S1.MultiplyTranspose_S2();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the transposition : <c>M1<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the transposition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Transpose()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S1.Transpose();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    //     -----     Operators

                    /// <summary>
                    /// Provides the expected result of the addition : <c>M1+M2</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the addition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Addition_M2()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S1.Addition_S2();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the subtraction : <c>M1-M2</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the subtraction. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Subtraction_M2()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S1.Subtraction_S2();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the unary negation : <c>-M1</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the unary negation. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] UnaryNegation()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S1.UnaryNegation();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RightMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S1.RightMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] LeftMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S1.LeftMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the scalar division. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Division_T()
                    {
                        (double divisor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S1.Division_T();

                        return [divisor, new Mat.SparseMatrix<double>(result)];
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
                    public static object[] ToArray() => SparseStorages.CompressedColumn.DataStorages.S1.ToArray();

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

                        // Presented in a tranposed way: each lines corresponds to a column 
                        List<double> values = new List<double>
                        ([
                             1.0,     2.0,
                         4.0,    -5.0,
                        -2.0,     3.0
                        ]);

                        List<int> rowIndices = new List<int>
                        ([
                              0,    1,
                          0,    1,
                          0,    1,
                        ]);

                        int[] columnPointers = new int[] { 0, 2, 4, 6 };

                        Mat.SparseStorages.CompressedColumn<double> compressedColumn = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                        Mat.SparseMatrix<double> result = new Mat.SparseMatrix<double>(compressedColumn);

                        return [rowIndex, columnIndex, value, result];
                    }

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M1"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM1()
                    {
                        Mat.SparseStorages.CompressedColumn<double> s1 = SparseStorages.CompressedColumn.DataStorages.S1.CreateS1();

                        return new Mat.SparseMatrix<double>(s1);
                    }

                    #endregion
                }


                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>
                /// </summary>
                internal static class FromDOK
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M1"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m1 = CreateM1();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M1"/>. </description>
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
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M1"/>. </description>
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
                    public static object[] RowCount() => SparseStorages.DictionaryOfKeys.DataStorages.S1.RowCount();

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
                    public static object[] ColumnCount() => SparseStorages.DictionaryOfKeys.DataStorages.S1.ColumnCount();


                    //     -----     Public Static Methods

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M1<sup>t</sup>·M2</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] TransposeMultiply_M2()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S1.TransposeMultiply_S2();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M1·M2<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] MultiplyTranspose_M2()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S1.MultiplyTranspose_S2();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the transposition : <c>M1<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the transposition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Transpose()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S1.Transpose();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    //     -----     Operators

                    /// <summary>
                    /// Provides the expected result of the addition : <c>M1+M2</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the addition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Addition_M2()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S1.Addition_S2();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the subtraction : <c>M1-M2</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the subtraction. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Subtraction_M2()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S1.Subtraction_S2();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the unary negation : <c>-M1</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the unary negation. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] UnaryNegation()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S1.UnaryNegation();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RightMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S1.RightMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] LeftMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S1.LeftMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the scalar division. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Division_T()
                    {
                        (double divisor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S1.Division_T();

                        return [divisor, new Mat.SparseMatrix<double>(result)];
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
                    public static object[] ToArray() => SparseStorages.DictionaryOfKeys.DataStorages.S1.ToArray();

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

                        Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 5);

                        dok.Add(0, 0, 1.0); dok.Add(0, 1,  4.0); dok.Add(0, 2, -2.0);
                        dok.Add(1, 0, 2.0); dok.Add(1, 1, -5.0); dok.Add(1, 2,  3.0);

                        Mat.SparseMatrix<double> result = new Mat.SparseMatrix<double>(dok);

                        return [rowIndex, columnIndex, value, result];
                    }

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M1"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM1()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> s1 = SparseStorages.DictionaryOfKeys.DataStorages.S1.CreateS1();

                        return new Mat.SparseMatrix<double>(s1);
                    }

                    #endregion
                }
            }

            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M2 is a [2x3] matrix. </remarks>
            internal static class M2
            {
                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.CompressedColumn{T}"/>
                /// </summary>
                internal static class FromCC
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M2"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m2 = CreateM2();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M2"/>. </description>
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
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M2"/>. </description>
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
                    public static object[] RowCount() => SparseStorages.CompressedColumn.DataStorages.S2.RowCount();

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
                    public static object[] ColumnCount() => SparseStorages.CompressedColumn.DataStorages.S2.ColumnCount();


                    //     -----     Public Static Methods

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M2<sup>t</sup>·M1</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] TransposeMultiply_M1()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S2.TransposeMultiply_S1();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M2·M1<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] MultiplyTranspose_M1()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S2.MultiplyTranspose_S1();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the transposition : <c>M2<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the transposition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Transpose()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S2.Transpose();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    //     -----     Operators

                    /// <summary>
                    /// Provides the expected result of the addition : <c>M2+M1</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the addition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Addition_M1()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S2.Addition_S1();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the subtraction : <c>M2-M1</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the subtraction. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Subtraction_M1()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S2.Subtraction_S1();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the unary negation : <c>-M2</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the unary negation. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] UnaryNegation()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S2.UnaryNegation();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RightMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S2.RightMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] LeftMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S2.LeftMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the scalar division. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Division_T()
                    {
                        (double divisor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S2.Division_T();

                        return [divisor, new Mat.SparseMatrix<double>(result)];
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
                    public static object[] ToArray() => SparseStorages.CompressedColumn.DataStorages.S2.ToArray();

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

                        // Presented in a tranposed way: each lines corresponds to a column 
                        List<double> values = new List<double>
                        ([
                             4,    -2,
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


                        Mat.SparseStorages.CompressedColumn<double> compressedColumn = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                        Mat.SparseMatrix<double> result = new Mat.SparseMatrix<double>(compressedColumn);

                        return [rowIndex, columnIndex, value, result];
                    }

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M2"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM2()
                    {
                        Mat.SparseStorages.CompressedColumn<double> s2 = SparseStorages.CompressedColumn.DataStorages.S2.CreateS2();

                        return new Mat.SparseMatrix<double>(s2);
                    }

                    #endregion
                }


                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>
                /// </summary>
                internal static class FromDOK
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M2"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m2 = CreateM2();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M2"/>. </description>
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
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M2"/>. </description>
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
                    public static object[] RowCount() => SparseStorages.DictionaryOfKeys.DataStorages.S2.RowCount();

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
                    public static object[] ColumnCount() => SparseStorages.DictionaryOfKeys.DataStorages.S2.ColumnCount();


                    //     -----     Public Static Methods

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M2<sup>t</sup>·M1</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] TransposeMultiply_M1()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S2.TransposeMultiply_S1();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M2·M1<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] MultiplyTranspose_M1()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S2.MultiplyTranspose_S1();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the transposition : <c>M2<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the transposition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Transpose()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S2.Transpose();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    //     -----     Operators

                    /// <summary>
                    /// Provides the expected result of the addition : <c>M2+M1</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the addition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Addition_M1()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S2.Addition_S1();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the subtraction : <c>M2-M1</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the subtraction. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Subtraction_M1()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S2.Subtraction_S1();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the unary negation : <c>-M2</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the unary negation. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] UnaryNegation()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S2.UnaryNegation();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RightMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S2.RightMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] LeftMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S2.LeftMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the scalar division. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Division_T()
                    {
                        (double divisor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S2.Division_T();

                        return [divisor, new Mat.SparseMatrix<double>(result)];
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
                    public static object[] ToArray() => SparseStorages.DictionaryOfKeys.DataStorages.S2.ToArray();

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

                        Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 5);

                        dok.Add(0, 0,  4.0); dok.Add(0, 1, -1.0); dok.Add(0, 2,  6.0);
                        dok.Add(1, 0, -2.0); dok.Add(1, 1,  1.0); dok.Add(1, 2, -3.0);

                        Mat.SparseMatrix<double> result = new Mat.SparseMatrix<double>(dok);

                        return [rowIndex, columnIndex, value, result];
                    }

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M2"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM2()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> s2 = SparseStorages.DictionaryOfKeys.DataStorages.S2.CreateS2();

                        return new Mat.SparseMatrix<double>(s2);
                    }

                    #endregion
                }
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M3 is a [6x6] matrix. </remarks>
            internal static class M3
            {
                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.CompressedColumn{T}"/>
                /// </summary>
                internal static class FromCC
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M3"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m3 = CreateM3();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M3"/>. </description>
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
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M3"/>. </description>
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
                    public static object[] RowCount() => SparseStorages.CompressedColumn.DataStorages.S3.RowCount();

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
                    public static object[] ColumnCount() => SparseStorages.CompressedColumn.DataStorages.S3.ColumnCount();


                    //     -----     Public Static Methods

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M3<sup>t</sup>·M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] TransposeMultiply_M4()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S3.TransposeMultiply_S4();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M3·M4<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] MultiplyTranspose_M4()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S3.MultiplyTranspose_S4();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    public static object[] TransposeMultiply_V3()
                    {
                        double[] result = SparseStorages.CompressedColumn.DataStorages.S3.TransposeMultiply_V3();

                        return [result];
                    }


                    /// <summary>
                    /// Provides the expected result of the transposition : <c>M3<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the transposition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Transpose()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S3.Transpose();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    //     -----     Operators

                    /// <summary>
                    /// Provides the expected result of the addition : <c>M3+M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the addition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Addition_M4()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S3.Addition_S4();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the subtraction : <c>M3-M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the subtraction. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Subtraction_M4()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S3.Subtraction_S4();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the unary negation : <c>-M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the unary negation. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] UnaryNegation()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S3.UnaryNegation();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M3·M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Multiplication_M4()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S3.Multiplication_S4();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RightMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S3.RightMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] LeftMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S3.LeftMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the scalar division. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Division_T()
                    {
                        (double divisor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S3.Division_T();

                        return [divisor, new Mat.SparseMatrix<double>(result)];
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
                    public static object[] Multiplication_V3()
                    {
                        double[] result = SparseStorages.CompressedColumn.DataStorages.S3.Multiplication_V3();

                        return [result];
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
                    public static object[] ToArray() => SparseStorages.CompressedColumn.DataStorages.S3.ToArray();

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

                        // Presented in a tranposed way: each lines corresponds to a column 
                        List<double> values = new List<double>
                        ([
                            10,     7,    /*0,*/    6,    1,     2,
                        15,    14,     -1,      4,    1,     4,
                        20,    21,      2,      2,    2,     8,
                        25,    28,     -3,     -2,    3,    16,
                        30,    35,      4,      8,    5,    32,
                        35,    42,     -5,     -6,    8,    64
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


                        Mat.SparseStorages.CompressedColumn<double> compressedColumn = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                        Mat.SparseMatrix<double> result = new Mat.SparseMatrix<double>(compressedColumn);

                        return [rowIndex, columnIndex, value, result];
                    }

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M3"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM3()
                    {
                        Mat.SparseStorages.CompressedColumn<double> s3 = SparseStorages.CompressedColumn.DataStorages.S3.CreateS3();

                        return new Mat.SparseMatrix<double>(s3);
                    }

                    #endregion
                }

                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>
                /// </summary>
                internal static class FromDOK
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M3"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m3 = CreateM3();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M3"/>. </description>
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
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M3"/>. </description>
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
                    public static object[] RowCount() => SparseStorages.DictionaryOfKeys.DataStorages.S3.RowCount();

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
                    public static object[] ColumnCount() => SparseStorages.DictionaryOfKeys.DataStorages.S3.ColumnCount();


                    //     -----     Public Static Methods

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M3<sup>t</sup>·M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] TransposeMultiply_M4()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S3.TransposeMultiply_S4();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M3·M4<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] MultiplyTranspose_M4()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S3.MultiplyTranspose_S4();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    public static object[] TransposeMultiply_V3()
                    {
                        double[] result = SparseStorages.DictionaryOfKeys.DataStorages.S3.TransposeMultiply_V3();

                        return [result];
                    }


                    /// <summary>
                    /// Provides the expected result of the transposition : <c>M3<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the transposition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Transpose()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S3.Transpose();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    //     -----     Operators

                    /// <summary>
                    /// Provides the expected result of the addition : <c>M3+M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the addition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Addition_M4()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S3.Addition_S4();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the subtraction : <c>M3-M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the subtraction. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Subtraction_M4()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S3.Subtraction_S4();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the unary negation : <c>-M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the unary negation. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] UnaryNegation()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S3.UnaryNegation();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M3·M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Multiplication_M4()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S3.Multiplication_S4();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RightMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S3.RightMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] LeftMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S3.LeftMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
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
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the scalar division. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Division_T()
                    {
                        (double divisor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S3.Division_T();

                        return [divisor, new Mat.SparseMatrix<double>(result)];
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
                    public static object[] Multiplication_V3()
                    {
                        double[] result = SparseStorages.DictionaryOfKeys.DataStorages.S3.Multiplication_V3();

                        return [result];
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
                    public static object[] ToArray() => SparseStorages.DictionaryOfKeys.DataStorages.S3.ToArray();

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

                        Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 35);

                          dok.Add(0, 0, 10.0);   dok.Add(0, 1, 15.0); dok.Add(0, 2, 20.0); dok.Add(0, 3, 25.0); dok.Add(0, 4, 30.0); dok.Add(0, 5, 35.0);
                          dok.Add(1, 0,  7.0);   dok.Add(1, 1, 14.0); dok.Add(1, 2, 21.0); dok.Add(1, 3, 28.0); dok.Add(1, 4, 35.0); dok.Add(1, 5, 42.0);
                        /*dok.Add(2, 0,  0.0);*/ dok.Add(2, 1, -1.0); dok.Add(2, 2,  2.0); dok.Add(2, 3, -3.0); dok.Add(2, 4,  4.0); dok.Add(2, 5, -5.0);
                          dok.Add(3, 0,  6.0);   dok.Add(3, 1,  4.0); dok.Add(3, 2,  2.0); dok.Add(3, 3, -2.0); dok.Add(3, 4,  8.0); dok.Add(3, 5, -6.0);
                          dok.Add(4, 0,  1.0);   dok.Add(4, 1,  1.0); dok.Add(4, 2,  2.0); dok.Add(4, 3,  3.0); dok.Add(4, 4,  5.0); dok.Add(4, 5,  8.0);
                          dok.Add(5, 0,  2.0);   dok.Add(5, 1,  4.0); dok.Add(5, 2,  8.0); dok.Add(5, 3, 16.0); dok.Add(5, 4, 32.0); dok.Add(5, 5, 64.0);

                        Mat.SparseMatrix<double> result = new Mat.SparseMatrix<double>(dok);

                        return [rowIndex, columnIndex, value, result];
                    }

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M3"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM3()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> s3 = SparseStorages.DictionaryOfKeys.DataStorages.S3.CreateS3();

                        return new Mat.SparseMatrix<double>(s3);
                    }

                    #endregion
                }
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M4 is a [6x6] matrix. </remarks>
            internal static class M4
            {
                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.CompressedColumn{T}"/>
                /// </summary>
                internal static class FromCC
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M4"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m4 = CreateM4();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M4"/>. </description>
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
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M4"/>. </description>
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
                    public static object[] RowCount() => SparseStorages.CompressedColumn.DataStorages.S4.RowCount();

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
                    public static object[] ColumnCount() => SparseStorages.CompressedColumn.DataStorages.S4.ColumnCount();


                    //     -----     Public Static Methods

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M4<sup>t</sup>·M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] TransposeMultiply_M3()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S4.TransposeMultiply_S3();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M4·M3<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] MultiplyTranspose_M3()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S4.MultiplyTranspose_S3();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    public static object[] TransposeMultiply_V3()
                    {
                        double[] result = SparseStorages.CompressedColumn.DataStorages.S4.TransposeMultiply_V3();

                        return [result];
                    }


                    /// <summary>
                    /// Provides the expected result of the transposition : <c>M4<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the transposition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Transpose()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S4.Transpose();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    //     -----     Operators

                    /// <summary>
                    /// Provides the expected result of the addition : <c>M4+M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the addition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Addition_M3()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S4.Addition_S3();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the subtraction : <c>M4-M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the subtraction. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Subtraction_M3()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S4.Subtraction_S3();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the unary negation : <c>-M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the unary negation. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] UnaryNegation()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S4.UnaryNegation();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M4·M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Multiplication_M3()
                    {
                        Mat.SparseStorages.CompressedColumn<double> result = SparseStorages.CompressedColumn.DataStorages.S4.Multiplication_S3();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the right scalar multiplication by 3.0 : <c>M4·3.0</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                    ///         <term> 1 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RightMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S4.RightMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                    ///         <term> 1 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] LeftMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S4.LeftMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the scalar division by 4.0 : <c>M4/4.0</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                    ///         <term> 1 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the scalar division. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Division_T()
                    {
                        (double divisor, Mat.SparseStorages.CompressedColumn<double> result) = SparseStorages.CompressedColumn.DataStorages.S4.Division_T();

                        return [divisor, new Mat.SparseMatrix<double>(result)];
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
                    public static object[] Multiplication_V3()
                    {
                        double[] result = SparseStorages.CompressedColumn.DataStorages.S4.Multiplication_V3();

                        return [result];
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
                    public static object[] ToArray() => SparseStorages.CompressedColumn.DataStorages.S4.ToArray();

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

                        // Presented in a tranposed way: each lines corresponds to a column 
                        List<double> values = new List<double>
                        ([
                            11,    21,    31,    41,    51,    61,
                        12,    22,    32,    42,    52,    62,
                        13,    23,    33,    43,    53,    63,
                        14,    24,    34,    44,    54,    64,
                        15,    25,    35,    45,    55,    65,
                        16,    26,    36,    46,    56,   /*0*/
                        ]);

                        List<int> rowIndices = new List<int>
                        ([
                            0,    1,    2,    3,    4,     5,
                        0,    1,    2,    3,    4,     5,
                        0,    1,    2,    3,    4,     5,
                        0,    1,    2,    3,    4,     5,
                        0,    1,    2,    3,    4,     5,
                        0,    1,    2,    3,    4,   /*5*/
                        ]);

                        int[] columnPointers = new int[] { 0, 6, 12, 18, 24, 30, 35 };


                        Mat.SparseStorages.CompressedColumn<double> compressedColumn = new Mat.SparseStorages.CompressedColumn<double>(rowCount, columnCount, ref values, ref rowIndices, ref columnPointers);
                        Mat.SparseMatrix<double> result = new Mat.SparseMatrix<double>(compressedColumn);

                        return [rowIndex, columnIndex, value, result];
                    }

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M4"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM4()
                    {
                        Mat.SparseStorages.CompressedColumn<double> s4 = SparseStorages.CompressedColumn.DataStorages.S4.CreateS4();

                        return new Mat.SparseMatrix<double>(s4);
                    }

                    #endregion
                }

                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>
                /// </summary>
                internal static class FromDOK
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M4"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m4 = CreateM4();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M4"/>. </description>
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
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M4"/>. </description>
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
                    public static object[] RowCount() => SparseStorages.DictionaryOfKeys.DataStorages.S4.RowCount();

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
                    public static object[] ColumnCount() => SparseStorages.DictionaryOfKeys.DataStorages.S4.ColumnCount();


                    //     -----     Public Static Methods

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M4<sup>t</sup>·M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] TransposeMultiply_M3()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S4.TransposeMultiply_S3();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M4·M3<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] MultiplyTranspose_M3()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S4.MultiplyTranspose_S3();

                        return [new Mat.SparseMatrix<double>(result)];
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
                    public static object[] TransposeMultiply_V3()
                    {
                        double[] result = SparseStorages.DictionaryOfKeys.DataStorages.S4.TransposeMultiply_V3();

                        return [result];
                    }


                    /// <summary>
                    /// Provides the expected result of the transposition : <c>M4<sup>t</sup></c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the transposition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Transpose()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S4.Transpose();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    //     -----     Operators

                    /// <summary>
                    /// Provides the expected result of the addition : <c>M4+M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the addition. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Addition_M3()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S4.Addition_S3();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the subtraction : <c>M4-M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the subtraction. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Subtraction_M3()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S4.Subtraction_S3();

                        return [new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the unary negation : <c>-M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the unary negation. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] UnaryNegation()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S4.UnaryNegation();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the multiplication : <c>M4·M3</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Multiplication_M3()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> result = SparseStorages.DictionaryOfKeys.DataStorages.S4.Multiplication_S3();

                        return [new Mat.SparseMatrix<double>(result)];
                    }


                    /// <summary>
                    /// Provides the expected result of the right scalar multiplication by 3.0 : <c>M4·3.0</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                    ///         <term> 1 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the right scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RightMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S4.RightMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·M4</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                    ///         <term> 1 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the left scalar multiplication. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] LeftMultiplication_T()
                    {
                        (double factor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S4.LeftMultiplication_T();

                        return [factor, new Mat.SparseMatrix<double>(result)];
                    }

                    /// <summary>
                    /// Provides the expected result of the scalar division by 4.0 : <c>M4/4.0</c>
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="double"/> representing the scalar factor. </description>
                    ///         <term> 1 </term>
                    ///         <description> An <see cref="Mat.SparseMatrix{T}"/> representing the result of the scalar division. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Division_T()
                    {
                        (double divisor, Mat.SparseStorages.DictionaryOfKeys<double> result) = SparseStorages.DictionaryOfKeys.DataStorages.S4.Division_T();

                        return [divisor, new Mat.SparseMatrix<double>(result)];
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
                    public static object[] Multiplication_V3()
                    {
                        double[] result = SparseStorages.DictionaryOfKeys.DataStorages.S4.Multiplication_V3();

                        return [result];
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
                    public static object[] ToArray() => SparseStorages.DictionaryOfKeys.DataStorages.S4.ToArray();

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

                        Mat.SparseStorages.DictionaryOfKeys<double> dok = new Mat.SparseStorages.DictionaryOfKeys<double>(rowCount, columnCount, 35);

                        dok.Add(0, 0, 11); dok.Add(0, 1, 12); dok.Add(0, 2, 13); dok.Add(0, 3, 14); dok.Add(0, 4, 15);   dok.Add(0, 5, 16);
                        dok.Add(1, 0, 21); dok.Add(1, 1, 22); dok.Add(1, 2, 23); dok.Add(1, 3, 24); dok.Add(1, 4, 25);   dok.Add(1, 5, 26);
                        dok.Add(2, 0, 31); dok.Add(2, 1, 32); dok.Add(2, 2, 33); dok.Add(2, 3, 34); dok.Add(2, 4, 35);   dok.Add(2, 5, 36);
                        dok.Add(3, 0, 41); dok.Add(3, 1, 42); dok.Add(3, 2, 43); dok.Add(3, 3, 44); dok.Add(3, 4, 45);   dok.Add(3, 5, 46);
                        dok.Add(4, 0, 51); dok.Add(4, 1, 52); dok.Add(4, 2, 53); dok.Add(4, 3, 54); dok.Add(4, 4, 55);   dok.Add(4, 5, 56);
                        dok.Add(5, 0, 61); dok.Add(5, 1, 62); dok.Add(5, 2, 63); dok.Add(5, 3, 64); dok.Add(5, 4, 65); /*dok.Add(5, 5,  0);*/

                        Mat.SparseMatrix<double> result = new Mat.SparseMatrix<double>(dok);

                        return [rowIndex, columnIndex, value, result];
                    }

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M4"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM4()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> s4 = SparseStorages.DictionaryOfKeys.DataStorages.S4.CreateS4();

                        return new Mat.SparseMatrix<double>(s4);
                    }

                    #endregion
                }
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="Mat.SparseMatrix{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The matrix M5 is a [3x5] matrix. </remarks>
            internal static class M5
            {
                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.CompressedColumn{T}"/>
                /// </summary>
                internal static class FromCC
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M4"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m5 = CreateM5();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M5"/>. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Readable()
                    {
                        return [_m5];
                    }

                    /// <summary>
                    /// Provides a writable version of the data that can be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M5"/>. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Writable()
                    {
                        return [CreateM5()];
                    }


                    //     -----     Properties

                    /// <summary>
                    /// Provides the expected number of row of the matrix <see cref="M5"/>.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="int"/> representing the row count. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RowCount() => SparseStorages.CompressedColumn.DataStorages.S5.RowCount();

                    /// <summary>
                    /// Provides the expected number of columns of the matrix <see cref="M5"/>.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="int"/> representing the column count. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] ColumnCount() => SparseStorages.CompressedColumn.DataStorages.S5.ColumnCount();


                    //     -----     Public Methods

                    /// <summary>
                    /// Provides the information to evaluate whether a component is contained in the sparse matrix.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> Two-dimensional array of <see cref="bool"/> evaluating whether a component at the given row and column has an entry in the sparse matrix. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Contains() => SparseStorages.CompressedColumn.DataStorages.S5.Contains();


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
                    public static object[] ToArray() => SparseStorages.CompressedColumn.DataStorages.S5.ToArray();

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M5"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM5()
                    {
                        Mat.SparseStorages.CompressedColumn<double> s5 = SparseStorages.CompressedColumn.DataStorages.S5.CreateS5();

                        return new Mat.SparseMatrix<double>(s5);
                    }

                    #endregion
                }

                /// <summary>
                /// The storage of the <see cref="Mat.SparseMatrix{T}"/> is a <see cref="Mat.SparseStorages.DictionaryOfKeys{T}"/>
                /// </summary>
                internal static class FromDOK
                {
                    #region Static Fields

                    /// <summary>
                    /// Staticly stored matrix <see cref="M4"/>.
                    /// </summary>
                    private static readonly Mat.SparseMatrix<double> _m5 = CreateM5();

                    #endregion

                    #region Public Static Methods

                    /// <summary>
                    /// Provides a readable version of the data that must not be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> The staticly stored <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M5"/>. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Readable()
                    {
                        return [_m5];
                    }

                    /// <summary>
                    /// Provides a writable version of the data that can be altered during the parametrised tests.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> A newly computed <see cref="Mat.SparseMatrix{T}"/> of type <see cref="M5"/>. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Writable()
                    {
                        return [CreateM5()];
                    }


                    //     -----     Properties

                    /// <summary>
                    /// Provides the expected number of row of the matrix <see cref="M5"/>.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="int"/> representing the row count. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] RowCount() => SparseStorages.DictionaryOfKeys.DataStorages.S5.RowCount();

                    /// <summary>
                    /// Provides the expected number of columns of the matrix <see cref="M5"/>.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> An <see cref="int"/> representing the column count. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] ColumnCount() => SparseStorages.DictionaryOfKeys.DataStorages.S5.ColumnCount();


                    //     -----     Public Methods

                    /// <summary>
                    /// Provides the information to evaluate whether a component is contained in the sparse matrix.
                    /// </summary>
                    /// <returns> An <see cref="T:object[]"/> whose array item are :
                    /// <list type="table">
                    ///     <item>
                    ///         <term> 0 </term>
                    ///         <description> Two-dimensional array of <see cref="bool"/> evaluating whether a component at the given row and column has an entry in the sparse matrix. </description>
                    ///     </item>
                    /// </list>
                    /// </returns>
                    public static object[] Contains() => SparseStorages.DictionaryOfKeys.DataStorages.S5.Contains();


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
                    public static object[] ToArray() => SparseStorages.DictionaryOfKeys.DataStorages.S5.ToArray();

                    #endregion

                    #region Other Static Methods

                    /// <summary>
                    /// Creates a matrix <see cref="M5"/>.
                    /// </summary>
                    /// <returns> The <see cref="Mat.SparseMatrix{T}"/>. </returns>
                    private static Mat.SparseMatrix<double> CreateM5()
                    {
                        Mat.SparseStorages.DictionaryOfKeys<double> s5 = SparseStorages.DictionaryOfKeys.DataStorages.S5.CreateS5();

                        return new Mat.SparseMatrix<double>(s5);
                    }

                    #endregion
                }
            }
        }

        #endregion

        #region Data Classes for Parametrised Tests

        internal static class DataClasses
        {
            #region For Properties

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Property_RowCount(Mat.SparseMatrix{double}, int)"/>.
            /// </summary>
            internal class RowCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M1.FromCC.RowCount },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M2.FromCC.RowCount },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M3.FromCC.RowCount },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M4.FromCC.RowCount },
                        // For DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M1.FromDOK.RowCount },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M2.FromDOK.RowCount },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M3.FromDOK.RowCount },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M4.FromDOK.RowCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Property_ColumnCount(Mat.SparseMatrix{double}, int)"/>.
            /// </summary>
            internal class ColumnCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M1.FromCC.ColumnCount },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M2.FromCC.ColumnCount },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M3.FromCC.ColumnCount },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M4.FromCC.ColumnCount },
                        // For DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M1.FromDOK.ColumnCount },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M2.FromDOK.ColumnCount },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M3.FromDOK.ColumnCount },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M4.FromDOK.ColumnCount },
                    };
            }

            #endregion

            #region For Operations

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Addition__SparseMatrix_SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Add__SparseMatrix_SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class Addition__SparseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M2.FromCC.Readable, DataStorages.M1.FromCC.Addition_M2 },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M1.FromCC.Readable, DataStorages.M2.FromCC.Addition_M1 },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.Addition_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.Addition_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromDOK.Addition_M2 },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromDOK.Addition_M1 },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.Addition_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.Addition_M3 },

                        // For CompressedColumn with DictionaryOfKeys
                        { DataStorages.M1.FromCC.Readable, DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromCC.Addition_M2 },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromCC.Addition_M1 },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.Addition_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.Addition_M3 },
                        // For DictionaryOfKeys with CompressedColumn
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromCC.Readable, DataStorages.M1.FromDOK.Addition_M2 },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromCC.Readable, DataStorages.M2.FromDOK.Addition_M1 },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.Addition_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.Addition_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Subtraction__SparseMatrix_SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Add__SparseMatrix_SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class Subtraction__SparseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M2.FromCC.Readable, DataStorages.M1.FromCC.Subtraction_M2 },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M1.FromCC.Readable, DataStorages.M2.FromCC.Subtraction_M1 },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.Subtraction_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.Subtraction_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromDOK.Subtraction_M2 },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromDOK.Subtraction_M1 },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.Subtraction_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.Subtraction_M3 },
                        
                        // For CompressedColumn with DictionaryOfKeys
                        { DataStorages.M1.FromCC.Readable, DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromCC.Subtraction_M2 },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromCC.Subtraction_M1 },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.Subtraction_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.Subtraction_M3 },
                        // For DictionaryOfKeys with CompressedColumn
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromCC.Readable, DataStorages.M1.FromDOK.Subtraction_M2 },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromCC.Readable, DataStorages.M2.FromDOK.Subtraction_M1 },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.Subtraction_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.Subtraction_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__UnaryNegation__SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Opposite__SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class UnaryNegation__SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M1.FromCC.UnaryNegation },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M2.FromCC.UnaryNegation },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M3.FromCC.UnaryNegation },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M4.FromCC.UnaryNegation },
                        // For DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M1.FromDOK.UnaryNegation },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M2.FromDOK.UnaryNegation },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M3.FromDOK.UnaryNegation },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M4.FromDOK.UnaryNegation },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Multiplication__SparseMatrix_SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Multiply__SparseMatrix_SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class Multiplication__SparseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with CompressedColumn
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.Multiplication_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.Multiplication_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.Multiplication_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.Multiplication_M3 },
                        
                        // For CompressedColumn with DictionaryOfKeys
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.Multiplication_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.Multiplication_M3 },
                        // For DictionaryOfKeys with CompressedColumn
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.Multiplication_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.Multiplication_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="SparseMatrix.Static__TransposeMultiply__SparseMatrix_SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class TransposeMultiplication__SparseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M2.FromCC.Readable, DataStorages.M1.FromCC.TransposeMultiply_M2 },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M1.FromCC.Readable, DataStorages.M2.FromCC.TransposeMultiply_M1 },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.TransposeMultiply_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.TransposeMultiply_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromDOK.TransposeMultiply_M2 },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromDOK.TransposeMultiply_M1 },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.TransposeMultiply_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.TransposeMultiply_M3 },
                        
                        // For CompressedColumn with CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromCC.TransposeMultiply_M2 },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromCC.TransposeMultiply_M1 },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.TransposeMultiply_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.TransposeMultiply_M3 },
                        // For DictionaryOfKeys with CompressedColumn
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromCC.Readable, DataStorages.M1.FromDOK.TransposeMultiply_M2 },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromCC.Readable, DataStorages.M2.FromDOK.TransposeMultiply_M1 },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.TransposeMultiply_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.TransposeMultiply_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Static__MultiplyTranspose__SparseMatrix_SparseMatrix(Mat.SparseMatrix{double}, Mat.SparseMatrix{double}, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class MultiplicationTranspose__SparseMatrix_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M2.FromCC.Readable, DataStorages.M1.FromCC.MultiplyTranspose_M2 },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M1.FromCC.Readable, DataStorages.M2.FromCC.MultiplyTranspose_M1 },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.MultiplyTranspose_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromCC.MultiplyTranspose_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromDOK.MultiplyTranspose_M2 },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromDOK.MultiplyTranspose_M1 },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.MultiplyTranspose_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromDOK.MultiplyTranspose_M3 },
                        
                        // For CompressedColumn with DictionaryOfKeys
                        { DataStorages.M1.FromCC.Readable, DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromCC.MultiplyTranspose_M2 },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromCC.MultiplyTranspose_M1 },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.MultiplyTranspose_M4 },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.MultiplyTranspose_M3 },
                        // For DictionaryOfKeys with CompressedColumn
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M2.FromCC.Readable, DataStorages.M1.FromDOK.MultiplyTranspose_M2 },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M1.FromCC.Readable, DataStorages.M2.FromDOK.MultiplyTranspose_M1 },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M4.FromCC.Readable, DataStorages.M3.FromDOK.MultiplyTranspose_M4 },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M3.FromCC.Readable, DataStorages.M4.FromDOK.MultiplyTranspose_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Multiplication__SparseMatrix_T(Mat.SparseMatrix{double}, double, Mat.SparseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Multiply__SparseMatrix_T(Mat.SparseMatrix{double}, double, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class Multiplication__SparseMatrix_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M1.FromCC.RightMultiplication_T },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M2.FromCC.RightMultiplication_T },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M3.FromCC.RightMultiplication_T },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M4.FromCC.RightMultiplication_T },
                        // For DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M1.FromDOK.RightMultiplication_T },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M2.FromDOK.RightMultiplication_T },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M3.FromDOK.RightMultiplication_T },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M4.FromDOK.RightMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Multiplication__T_SparseMatrix(Mat.SparseMatrix{double}, double, Mat.SparseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Multiply__T_SparseMatrix(Mat.SparseMatrix{double}, double, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class Multiplication__T_SparseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M1.FromCC.LeftMultiplication_T },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M2.FromCC.LeftMultiplication_T },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M3.FromCC.LeftMultiplication_T },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M4.FromCC.LeftMultiplication_T },
                        // For DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M1.FromDOK.LeftMultiplication_T },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M2.FromDOK.LeftMultiplication_T },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M3.FromDOK.LeftMultiplication_T },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M4.FromDOK.LeftMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Division__SparseMatrix_T(Mat.SparseMatrix{double}, double, Mat.SparseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Divide__SparseMatrix_T(Mat.SparseMatrix{double}, double, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class Division__SparseMatrix_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M1.FromCC.Division_T },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M2.FromCC.Division_T },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M3.FromCC.Division_T },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M4.FromCC.Division_T },
                        // For DictionaryOfKeys
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M1.FromDOK.Division_T },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M2.FromDOK.Division_T },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M3.FromDOK.Division_T },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M4.FromDOK.Division_T },
                    };
            }



            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Addition__SparseMatrix_DenseMatrix(Mat.SparseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Add__SparseMatrix_DenseMatrix(Mat.SparseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Addition__SparseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with DenseMatrix
                        { DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Addition_M2 },
                        { DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Addition_M1 },
                        { DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Addition_M4 },
                        { DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Addition_M3 },
                        // For DictionaryOfKeys with DenseMatrix
                        { DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Addition_M2 },
                        { DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Addition_M1 },
                        { DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Addition_M4 },
                        { DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Addition_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Subtraction__SparseMatrix_DenseMatrix(Mat.SparseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Add__SparseMatrix_DenseMatrix(Mat.SparseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Subtraction__SparseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with DenseMatrix
                        { DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Subtraction_M2 },
                        { DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Subtraction_M1 },
                        { DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Subtraction_M4 },
                        { DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Subtraction_M3 },
                        // For DictionaryOfKeys with DenseMatrix
                        { DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Subtraction_M2 },
                        { DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Subtraction_M1 },
                        { DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Subtraction_M4 },
                        { DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Subtraction_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Multiplication__SparseMatrix_DenseMatrix(Mat.SparseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/> and
            /// <see cref="SparseMatrix.Static__Multiply__SparseMatrix_DenseMatrix(Mat.SparseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class Multiplication__SparseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with DenseMatrix
                        { DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Multiplication_M4 },
                        { DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Multiplication_M3 },
                        // For DictionaryOfKeys with DenseMatrix
                        { DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Multiplication_M4 },
                        { DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Multiplication_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="SparseMatrix.Static__TransposeMultiply__SparseMatrix_DenseMatrix(Mat.SparseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class TransposeMultiplication__SparseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with DenseMatrix
                        { DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.TransposeMultiply_M2 },
                        { DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.TransposeMultiply_M1 },
                        { DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_M4 },
                        { DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_M3 },
                        // For DictionaryOfKeys with DenseMatrix
                        { DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.TransposeMultiply_M2 },
                        { DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.TransposeMultiply_M1 },
                        { DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_M4 },
                        { DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Static__MultiplyTranspose__SparseMatrix_DenseMatrix(Mat.SparseMatrix{double}, Mat.DenseMatrix{double}, Mat.DenseMatrix{double})"/>.
            /// </summary>
            internal class MultiplicationTranspose__SparseMatrix_DenseMatrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with DenseMatrix
                        { DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.MultiplyTranspose_M2 },
                        { DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.MultiplyTranspose_M1 },
                        { DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.MultiplyTranspose_M4 },
                        { DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.MultiplyTranspose_M3 },
                        // For DictionaryOfKeys with DenseMatrix
                        { DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.MultiplyTranspose_M2 },
                        { DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.MultiplyTranspose_M1 },
                        { DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.MultiplyTranspose_M4 },
                        { DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.MultiplyTranspose_M3 },
                    };
            }



            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Multiplication__SparseMatrix_SparseVector(Mat.SparseMatrix{double}, Vect.SparseVector{double}, double[])"/> and
            /// <see cref="SparseMatrix.Static__Multiply__SparseMatrix_SparseVector(Mat.SparseMatrix{double}, Vect.SparseVector{double}, double[]))"/> and
            /// </summary>
            internal class Multiplication__SparseMatrix_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with SparseVector
                        { DataStorages.M3.FromCC.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M3.FromCC.Multiplication_V3 },
                        { DataStorages.M4.FromCC.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M4.FromCC.Multiplication_V3 },
                        // For DictionaryOfKeys with SparseVector
                        { DataStorages.M3.FromDOK.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M3.FromDOK.Multiplication_V3 },
                        { DataStorages.M4.FromDOK.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M4.FromDOK.Multiplication_V3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Operator__Multiplication__SparseMatrix_DenseVector(Mat.SparseMatrix{double}, Vect.DenseVector{double}, double[]))"/> and
            /// <see cref="SparseMatrix.Static__Multiply__SparseMatrix_DenseVector(Mat.SparseMatrix{double}, Vect.DenseVector{double}, double[]))"/> and
            /// </summary>
            internal class Multiplication__SparseMatrix_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with DenseVector
                        { DataStorages.M3.FromCC.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M3.FromCC.Multiplication_V3 },
                        { DataStorages.M4.FromCC.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M4.FromCC.Multiplication_V3 },
                        // For DictionaryOfKeys with DenseVector
                        { DataStorages.M3.FromDOK.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M3.FromDOK.Multiplication_V3 },
                        { DataStorages.M4.FromDOK.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M4.FromDOK.Multiplication_V3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="SparseMatrix.Static__TransposeMultiply__SparseMatrix_SparseVector(Mat.SparseMatrix{double}, Vect.SparseVector{double}, double[]))"/>.
            /// </summary>
            internal class TransposeMultiplication__SparseMatrix_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with SparseVector
                        { DataStorages.M3.FromCC.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M3.FromCC.TransposeMultiply_V3 },
                        { DataStorages.M4.FromCC.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M4.FromCC.TransposeMultiply_V3 },
                        // For DictionaryOfKeys with SparseVector
                        { DataStorages.M3.FromDOK.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M3.FromDOK.TransposeMultiply_V3 },
                        { DataStorages.M4.FromDOK.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DataStorages.M4.FromDOK.TransposeMultiply_V3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Static__TransposeMultiply__SparseMatrix_DenseVector(Mat.SparseMatrix{double}, Vect.DenseVector{double}, double[]))"/>.
            /// </summary>
            internal class TransposeMultiplication__SparseMatrix_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn with DenseVector
                        { DataStorages.M3.FromCC.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M3.FromCC.TransposeMultiply_V3 },
                        { DataStorages.M4.FromCC.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M4.FromCC.TransposeMultiply_V3 },
                        // For DictionaryOfKeys with DenseVector
                        { DataStorages.M3.FromDOK.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M3.FromDOK.TransposeMultiply_V3 },
                        { DataStorages.M4.FromDOK.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DataStorages.M4.FromDOK.TransposeMultiply_V3 },
                    };
            }

            #endregion

            #region For Methods

            /// <summary>
            /// Class data for <see cref="SparseMatrix.GetComponent__Int_Int(Mat.SparseMatrix{double}, double[,])"/>,
            /// <see cref="SparseMatrix.ToArray(Mat.SparseMatrix{double}, double[,])"/>,
            /// <see cref="SparseMatrix.ToRowMajorArray(Mat.SparseMatrix{double}, double[,])"/>, 
            /// <see cref="SparseMatrix.ToColumnMajorArray(Mat.SparseMatrix{double}, double[,])"/>,
            /// <see cref="SparseMatrix.RowVectors(Mat.SparseMatrix{double}, double[,])"/> and
            /// <see cref="SparseMatrix.ColumnVectors(Mat.SparseMatrix{double}, double[,])"/>.
            /// </summary>
            internal class ToArray : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M1.FromCC.Readable, DataStorages.M1.FromCC.ToArray },
                        { DataStorages.M2.FromCC.Readable, DataStorages.M2.FromCC.ToArray },
                        { DataStorages.M3.FromCC.Readable, DataStorages.M3.FromCC.ToArray },
                        { DataStorages.M4.FromCC.Readable, DataStorages.M4.FromCC.ToArray },
                        { DataStorages.M5.FromCC.Readable, DataStorages.M5.FromCC.ToArray },
                        // For DictionaryOfKeys 
                        { DataStorages.M1.FromDOK.Readable, DataStorages.M1.FromDOK.ToArray },
                        { DataStorages.M2.FromDOK.Readable, DataStorages.M2.FromDOK.ToArray },
                        { DataStorages.M3.FromDOK.Readable, DataStorages.M3.FromDOK.ToArray },
                        { DataStorages.M4.FromDOK.Readable, DataStorages.M4.FromDOK.ToArray },
                        { DataStorages.M5.FromDOK.Readable, DataStorages.M5.FromDOK.ToArray },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.SetComponent__Int_Int(Mat.SparseMatrix{double}, int, int, double, Mat.SparseMatrix{double})"/>.
            /// </summary>
            internal class SetComponent : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M1.FromCC.Writable, DataStorages.M1.FromCC.SetComponent },
                        { DataStorages.M2.FromCC.Writable, DataStorages.M2.FromCC.SetComponent },
                        { DataStorages.M3.FromCC.Writable, DataStorages.M3.FromCC.SetComponent },
                        { DataStorages.M4.FromCC.Writable, DataStorages.M4.FromCC.SetComponent },
                        // For DictionaryOfKeys 
                        { DataStorages.M1.FromDOK.Writable, DataStorages.M1.FromDOK.SetComponent },
                        { DataStorages.M2.FromDOK.Writable, DataStorages.M2.FromDOK.SetComponent },
                        { DataStorages.M3.FromDOK.Writable, DataStorages.M3.FromDOK.SetComponent },
                        { DataStorages.M4.FromDOK.Writable, DataStorages.M4.FromDOK.SetComponent },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseMatrix.Contains__Int_Int(Mat.SparseMatrix{double}, bool[,])"/>.
            /// </summary>
            internal class Contains : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For CompressedColumn
                        { DataStorages.M5.FromCC.Readable,  DataStorages.M5.FromCC.Contains },
                        // For DictionaryOfKeys 
                        { DataStorages.M5.FromDOK.Readable,  DataStorages.M5.FromDOK.Contains },
                    };
            }

            #endregion
        }

        #endregion
    }
}
