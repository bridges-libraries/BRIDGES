using System;

using Xunit;

using Mat = BRIDGES.Numerics.LinearAlgebra.Matrices;
using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Matrices.Double
{
    /// <summary>
    /// Tests the members of the <see cref=Mat.Matrix{T}"/> class.
    /// </summary>
    public class Matrix
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Mat.Matrix{T}.RowCount"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to evaluate. </param>
        /// <param name="expected"> Expected number of row. </param>
        [Theory(DisplayName = "Prop. RowCount")]
        [ClassData(typeof(DataClasses.RowCount))]
        public void Property_RowCount(Mat.Matrix<double> matrix, int expected)
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
        public void Property_ColumnCount(Mat.Matrix<double> matrix, int expected)
        {
            // Arrange

            //Act
            int actual = matrix.ColumnCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Tests : Public Static Methods

        //     -----     -----     Algebraic Near Ring : Matrix<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.Add(Mat.Matrix{T},Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.Matrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.Matrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(Matrix,Matrix)")]
        [ClassData(typeof(DataClasses.Addition__Matrix_Matrix))]
        public void Static__Add__Matrix_Matrix(Mat.Matrix<double> left, Mat.Matrix<double> right, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.Add(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.Subtract(Mat.Matrix{T},Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.Matrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.Matrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(Matrix,Matrix)")]
        [ClassData(typeof(DataClasses.Subtraction__Matrix_Matrix))]
        public void Static__Subtract__Matrix_Matrix(Mat.Matrix<double> left, Mat.Matrix<double> right, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.Subtract(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.Opposite(Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.Matrix{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Static Opposite(Matrix)")]
        [ClassData(typeof(DataClasses.UnaryNegation__Matrix))]
        public void Static__Opposite__Matrix(Mat.Matrix<double> operand, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.Opposite(operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.Multiply(Mat.Matrix{T},Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.Matrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.Matrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(Matrix,Matrix)")]
        [ClassData(typeof(DataClasses.Multiplication__Matrix_Matrix))]
        public void Static__Multiply__Matrix_Matrix(Mat.Matrix<double> left, Mat.Matrix<double> right, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.Multiply(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        //     -----     Other Operations : Matrix<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.TransposeMultiply(Mat.Matrix{T},Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.Matrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Mat.Matrix{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(Matrix,Matrix)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__Matrix_Matrix))]
        public void Static__TransposeMultiply__Matrix_Matrix(Mat.Matrix<double> left, Mat.Matrix<double> right, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.TransposeMultiply(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.MultiplyTranspose(Mat.Matrix{T},Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.Matrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="Mat.Matrix{T}"/> to transpose and multiply. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static MultiplyTranspose(Matrix,Matrix)")]
        [ClassData(typeof(DataClasses.MultiplicationTranspose__Matrix_Matrix))]
        public void Static__MultiplyTranspose__Matrix_Matrix(Mat.Matrix<double> left, Mat.Matrix<double> right, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.MultiplyTranspose(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }



        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.Multiply(Mat.Matrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.Matrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(Matrix,T)")]
        [ClassData(typeof(DataClasses.Multiplication__Matrix_T))]
        public void Static__Multiply__Matrix_T(Mat.Matrix<double> operand, double factor, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.Multiply(operand, factor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.Multiply(T,Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Mat.Matrix{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(T,Matrix)")]
        [ClassData(typeof(DataClasses.Multiplication__T_Matrix))]
        public void Static__Multiply__T_Matrix(Mat.Matrix<double> operand, double factor, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.Multiply(factor, operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.Divide(Mat.Matrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.Matrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the division. </param>
        [Theory(DisplayName = "Static Divide(Matrix,T)")]
        [ClassData(typeof(DataClasses.Division__Matrix_T))]
        public void Static__Divide__Matrix_T(Mat.Matrix<double> operand, double divisor, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = Mat.Matrix<double>.Divide(operand, divisor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }



        //     -----     -----     Vectors     -----     -----     //


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.Multiply(Mat.Matrix{T},Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.Matrix{T}"/> to multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.Vector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static Multiply(Matrix,Vector)")]
        [ClassData(typeof(DataClasses.Multiplication__Matrix_Vector))]
        public void Static__Multiply__Matrix_Vector(Mat.Matrix<double> matrix, Vect.Vector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = Mat.Matrix<double>.Multiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result.GetComponent(i));
            }
        }


        //     -----      Other Operations : Vectors     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.TransposeMultiply(Mat.Matrix{T},Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.Matrix{T}"/> to transpose and multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.Vector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(Matrix,Vector)")]
        [ClassData(typeof(DataClasses.TransposeMultiplication__Matrix_Vector))]
        public void Static__TransposeMultiply__Matrix_Vector(Mat.Matrix<double> matrix, Vect.Vector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = Mat.Matrix<double>.TransposeMultiply(matrix, vector);

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result.GetComponent(i));
            }
        }

        #endregion

        #region Tests : Operators

        //     -----     Algebraic Near Ring : Matrix<T>    -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.operator+(Mat.Matrix{T},Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.Matrix{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Mat.Matrix{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (Matrix,Matrix)")]
        [ClassData(typeof(DataClasses.Addition__Matrix_Matrix))]
        public void Operator__Addition__Matrix_Matrix(Mat.Matrix<double> left, Mat.Matrix<double> right, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = left + right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.operator-(Mat.Matrix{T},Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.Matrix{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Mat.Matrix{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (Matrix,Matrix)")]
        [ClassData(typeof(DataClasses.Subtraction__Matrix_Matrix))]
        public void Operator__Subtraction__Matrix_Matrix(Mat.Matrix<double> left, Mat.Matrix<double> right, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = left - right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.operator-(Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.Matrix{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Op - (Matrix)")]
        [ClassData(typeof(DataClasses.UnaryNegation__Matrix))]
        public void Operator__UnaryNegation__Matrix(Mat.Matrix<double> operand, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = -operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.operator*(Mat.Matrix{T},Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Mat.Matrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Mat.Matrix{T}"/> for the multiplication. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Op * (Matrix,Matrix)")]
        [ClassData(typeof(DataClasses.Multiplication__Matrix_Matrix))]
        public void Operator__Multiplication__Matrix_Matrix(Mat.Matrix<double> left, Mat.Matrix<double> right, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = left * right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        //     -----      Group Action : T     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.operator*(Mat.Matrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.Matrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Op * (Matrix,T)")]
        [ClassData(typeof(DataClasses.Multiplication__Matrix_T))]
        public void Operator__Multiplication__Matrix_T(Mat.Matrix<double> operand, double factor, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = operand * factor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.operator*(T,Mat.Matrix{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Mat.Matrix{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Op * (T,Matrix)")]
        [ClassData(typeof(DataClasses.Multiplication__T_Matrix))]
        public void Operator__Multiplication__T_Matrix(Mat.Matrix<double> operand, double factor, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = factor * operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.operator/(Mat.Matrix{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Mat.Matrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Mat.Matrix{T}"/> of the division. </param>
        [Theory(DisplayName = "Op / (Matrix,T)")]
        [ClassData(typeof(DataClasses.Division__Matrix_T))]
        public void Operator__Division__Matrix_T(Mat.Matrix<double> operand, double divisor, Mat.Matrix<double> expected)
        {
            // Arrange

            // Act 
            Mat.Matrix<double> result = operand / divisor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.ColumnCount, result.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), result.GetComponent(i_Row, i_Column));
                }
            }
        }



        //     -----     -----     Vectors     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.operator*(Mat.Matrix{T},Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="matrix"> Left <see cref="Mat.Matrix{T}"/> to multiply. </param>
        /// <param name="vector"> Right <see cref="Vect.Vector{T}"/> to multiply. </param>
        /// <param name="expected"> Array of <see cref="double"/> containing the expected component of the result of the multiplication. </param>
        [Theory(DisplayName = "Op * (Matrix,Vector)")]
        [ClassData(typeof(DataClasses.Multiplication__Matrix_Vector))]
        public void Operator__Multiplication__Matrix_Vector(Mat.Matrix<double> matrix, Vect.Vector<double> vector, double[] expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = matrix * vector;

            // Assert
            Assert.Equal(expected.Length, result.Size);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result.GetComponent(i));
            }
        }

        #endregion

        #region Tests : Public Methods

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.ToArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="expected"> Two-dimensional array containing the values of components. </param>
        [Theory(DisplayName = "GetComponent(int, int)")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void GetComponent__Int_Int(Mat.Matrix<double> matrix, double[,] expected)
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
        /// Tests the method <see cref="Mat.Matrix{T}.SetComponent(int, int, T)"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to set. </param>
        /// <param name="columnIndex"> Column index of the component to set. </param>
        /// <param name="value"> Value of the component to set. </param>
        /// <param name="expected"> Expected sparse storage after the component iis set. </param>
        [Theory(DisplayName = "SetComponent(int, int, T)")]
        [ClassData(typeof(DataClasses.SetComponent))]
        public void SetComponent__Int_Int(Mat.Matrix<double> matrix, int rowIndex, int columnIndex, double value, Mat.Matrix<double> expected)
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
                    Assert.Equal(expected.GetComponent(i_Row, i_Column), matrix.GetComponent(i_Row, i_Column));
                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="Mat.Matrix.ToArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="expected"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToArray(Mat.Matrix<double> matrix, double[,] expected)
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
        /// Tests the method <see cref="Mat.Matrix.ToRowMajorArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToRowMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToRowMajorArray(Mat.Matrix<double> matrix, double[,] values)
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
        /// Tests the method <see cref="Mat.Matrix.ToColumnMajorArray()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ToColumnMajorArray()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ToColumnMajorArray(Mat.Matrix<double> matrix, double[,] values)
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
        /// Tests the method <see cref="Mat.Matrix.RowVectors()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "RowVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void RowVectors(Mat.Matrix<double> matrix, double[,] values)
        {
            // Arrange
            Vect.Vector<double>[] expected = new Vect.DenseVector<double>[values.GetLength(0)];

            for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
            {
                expected[i_Row] = new Vect.DenseVector<double>(values.GetLength(1));

                for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
                {
                    expected[i_Row].SetComponent(i_Column, values[i_Row, i_Column]);
                }
            }

            // Act
            Vect.Vector<double>[] result = matrix.RowVectors();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i_Row = 0; i_Row < expected.Length; i_Row++)
            {
                Vect.Vector<double> expectedRow = expected[i_Row];
                Vect.Vector<double> resultRow = result[i_Row];

                Assert.Equal(expectedRow.Size, resultRow.Size);

                for (int i = 0; i < expectedRow.Size; i++)
                {
                    Assert.Equal(expectedRow.GetComponent(i), resultRow.GetComponent(i));
                }
            }
        }

        /// <summary>
        /// Tests the method <see cref="Mat.Matrix{T}.ColumnVectors()"/>.
        /// </summary>
        /// <param name="matrix"> Matrix to operate from. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "ColumnVectors()")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void ColumnVectors(Mat.Matrix<double> matrix, double[,] values)
        {
            // Arrange
            Vect.Vector<double>[] expected = new Vect.DenseVector<double>[values.GetLength(1)];

            for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
            {
                expected[i_Column] = new Vect.DenseVector<double>(values.GetLength(0));

                for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
                {
                    expected[i_Column].SetComponent(i_Row, values[i_Row, i_Column]);
                }
            }

            // Act
            Vect.Vector<double>[] result = matrix.ColumnVectors();

            // Assert
            Assert.Equal(expected.Length, result.Length);
            for (int i_Column = 0; i_Column < expected.Length; i_Column++)
            {
                Vect.Vector<double> expectedColumn = expected[i_Column];
                Vect.Vector<double> resultColumn = result[i_Column];

                Assert.Equal(expectedColumn.Size, resultColumn.Size);

                for (int i = 0; i < expectedColumn.Size; i++)
                {
                    Assert.Equal(expectedColumn.GetComponent(i), resultColumn.GetComponent(i));
                }
            }
        }

        #endregion


        #region Data Classes for Parametrised Tests

        internal static class DataClasses
        {
            #region For Properties

            /// <summary>
            /// Class data for <see cref="Matrix.Property_RowCount(Mat.Matrix{double}, int)"/>.
            /// </summary>
            internal class RowCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M1.RowCount },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M2.RowCount },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M3.RowCount },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M4.RowCount },
                        // For CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.RowCount },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.RowCount },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.RowCount },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.RowCount },
                        // For DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.RowCount },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.RowCount },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.RowCount },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.RowCount },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.Property_ColumnCount(Mat.Matrix{double}, int)"/>.
            /// </summary>
            internal class ColumnCount : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M1.ColumnCount },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M2.ColumnCount },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M3.ColumnCount },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M4.ColumnCount },
                        // For CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.ColumnCount },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.ColumnCount },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.ColumnCount },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.ColumnCount },
                        // For DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.ColumnCount },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.ColumnCount },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.ColumnCount },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.ColumnCount },
                    };
            }

            #endregion

            #region For Operations

            /// <summary>
            /// Class data for <see cref="Matrix.Operator__Addition__Matrix_Matrix(Mat.Matrix{double}, Mat.Matrix{double}, Mat.Matrix{double})"/> and
            /// <see cref="Matrix.Static__Add__Matrix_Matrix(Mat.Matrix{double}, Mat.Matrix{double}, Mat.Matrix{double})"/>.
            /// </summary>
            internal class Addition__Matrix_Matrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Addition_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Addition_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Addition_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Addition_M3 },
                        // For CompressedColumn with CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.Addition_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.Addition_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Addition_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Addition_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.Addition_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.Addition_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Addition_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Addition_M3 },

                        // For DenseMatrix with CompressedColumn
                        { DenseMatrix.DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Addition_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Addition_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Addition_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Addition_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DenseMatrix.DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Addition_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Addition_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Addition_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Addition_M3 },

                        // For CompressedColumn with DenseMatrix
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Addition_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Addition_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Addition_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Addition_M3 },
                        // For CompressedColumn with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromCC.Addition_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromCC.Addition_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.Addition_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.Addition_M3 },

                        // For DictionaryOfKeys with DenseMatrix
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Addition_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Addition_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Addition_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Addition_M3 },
                        // For DictionaryOfKeys with CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromDOK.Addition_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromDOK.Addition_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.Addition_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.Addition_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.Operator__Subtraction__Matrix_Matrix(Mat.Matrix{double}, Mat.Matrix{double}, Mat.Matrix{double})"/> and
            /// <see cref="Matrix.Static__Add__Matrix_Matrix(Mat.Matrix{double}, Mat.Matrix{double}, Mat.Matrix{double})"/>.
            /// </summary>
            internal class Subtraction__Matrix_Matrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Subtraction_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Subtraction_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Subtraction_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Subtraction_M3 },
                        // For CompressedColumn with CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.Subtraction_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.Subtraction_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Subtraction_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Subtraction_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.Subtraction_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.Subtraction_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Subtraction_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Subtraction_M3 },

                        // For DenseMatrix with CompressedColumn
                        { DenseMatrix.DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Subtraction_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Subtraction_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Subtraction_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Subtraction_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DenseMatrix.DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Subtraction_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Subtraction_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Subtraction_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Subtraction_M3 },

                        // For CompressedColumn with DenseMatrix
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Subtraction_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Subtraction_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Subtraction_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Subtraction_M3 },
                        // For CompressedColumn with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromCC.Subtraction_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromCC.Subtraction_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.Subtraction_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.Subtraction_M3 },

                        // For DictionaryOfKeys with DenseMatrix
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Subtraction_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Subtraction_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Subtraction_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Subtraction_M3 },
                        // For DictionaryOfKeys with CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromDOK.Subtraction_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromDOK.Subtraction_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.Subtraction_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.Subtraction_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.Operator__UnaryNegation__Matrix(Mat.Matrix{double}, Mat.Matrix{double})"/> and
            /// <see cref="Matrix.Static__Opposite__Matrix(Mat.Matrix{double}, Mat.Matrix{double})"/>.
            /// </summary>
            internal class UnaryNegation__Matrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M1.UnaryNegation },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M2.UnaryNegation },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M3.UnaryNegation },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M4.UnaryNegation },
                        // For CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.UnaryNegation },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.UnaryNegation },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.UnaryNegation },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.UnaryNegation },
                        // For DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.UnaryNegation },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.UnaryNegation },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.UnaryNegation },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.UnaryNegation },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.Operator__Multiplication__Matrix_Matrix(Mat.Matrix{double}, Mat.Matrix{double}, Mat.Matrix{double})"/> and
            /// <see cref="Matrix.Static__Multiply__Matrix_Matrix(Mat.Matrix{double}, Mat.Matrix{double}, Mat.Matrix{double})"/>.
            /// </summary>
            internal class Multiplication__Matrix_Matrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {       
                        // For DenseMatrix with DenseMatrix
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Multiplication_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Multiplication_M3 },
                        // For CompressedColumn with CompressedColumn
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Multiplication_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Multiplication_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Multiplication_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Multiplication_M3 },
                        
                        // For DenseMatrix with CompressedColumn
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Multiplication_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Multiplication_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Multiplication_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Multiplication_M3 },

                        // For CompressedColumn with DenseMatrix
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Multiplication_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Multiplication_M3 },
                        // For CompressedColumn with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.Multiplication_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.Multiplication_M3 },

                        // For DictionaryOfKeys with DenseMatrix
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Multiplication_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Multiplication_M3 },
                        // For DictionaryOfKeys with CompressedColumn
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.Multiplication_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.Multiplication_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="Matrix.Static__TransposeMultiply__Matrix_Matrix(Mat.Matrix{double}, Mat.Matrix{double}, Mat.Matrix{double})"/>.
            /// </summary>
            internal class TransposeMultiplication__Matrix_Matrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.TransposeMultiply_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.TransposeMultiply_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_M3 },
                        // For CompressedColumn with CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.TransposeMultiply_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.TransposeMultiply_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.TransposeMultiply_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.TransposeMultiply_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.TransposeMultiply_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.TransposeMultiply_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.TransposeMultiply_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.TransposeMultiply_M3 },

                        // For DenseMatrix with CompressedColumn
                        { DenseMatrix.DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.TransposeMultiply_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.TransposeMultiply_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DenseMatrix.DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.TransposeMultiply_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.TransposeMultiply_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_M3 },

                        // For CompressedColumn with DenseMatrix
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.TransposeMultiply_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.TransposeMultiply_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_M3 },
                        // For CompressedColumn with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromCC.TransposeMultiply_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromCC.TransposeMultiply_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.TransposeMultiply_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.TransposeMultiply_M3 },

                        // For DictionaryOfKeys with DenseMatrix
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.TransposeMultiply_M2  },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.TransposeMultiply_M1  },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_M4  },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_M3  },
                        // For DictionaryOfKeys with CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromDOK.TransposeMultiply_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromDOK.TransposeMultiply_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.TransposeMultiply_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.TransposeMultiply_M3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.Static__MultiplyTranspose__Matrix_Matrix(Mat.Matrix{double}, Mat.Matrix{double}, Mat.Matrix{double})"/>.
            /// </summary>
            internal class MultiplicationTranspose__Matrix_Matrix : BaseDataClass
            {
                /// <inheritdoc/>DenseMatrix
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.MultiplyTranspose_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.MultiplyTranspose_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.MultiplyTranspose_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.MultiplyTranspose_M3 },
                        // For CompressedColumn with CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.MultiplyTranspose_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.MultiplyTranspose_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.MultiplyTranspose_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.MultiplyTranspose_M3 },
                        // For DictionaryOfKeys with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.MultiplyTranspose_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.MultiplyTranspose_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.MultiplyTranspose_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.MultiplyTranspose_M3 },

                        // For DenseMatrix with CompressedColumn
                        { DenseMatrix.DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.MultiplyTranspose_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.MultiplyTranspose_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.MultiplyTranspose_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.MultiplyTranspose_M3 },
                        // For DenseMatrix with DictionaryOfKeys
                        { DenseMatrix.DataStorages.M1.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.MultiplyTranspose_M2 },
                        { DenseMatrix.DataStorages.M2.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.MultiplyTranspose_M1 },
                        { DenseMatrix.DataStorages.M3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.MultiplyTranspose_M4 },
                        { DenseMatrix.DataStorages.M4.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.MultiplyTranspose_M3 },

                        // For CompressedColumn with DenseMatrix
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.MultiplyTranspose_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.MultiplyTranspose_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.MultiplyTranspose_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.MultiplyTranspose_M3 },
                        // For CompressedColumn with DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromCC.MultiplyTranspose_M2 },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromCC.MultiplyTranspose_M1 },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.MultiplyTranspose_M4 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.MultiplyTranspose_M3 },

                        // For DictionaryOfKeys with DenseMatrix
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M1.MultiplyTranspose_M2  },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M2.MultiplyTranspose_M1  },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M3.MultiplyTranspose_M4  },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M4.MultiplyTranspose_M3  },
                        // For DictionaryOfKeys with CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M1.FromDOK.MultiplyTranspose_M2 },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M2.FromDOK.MultiplyTranspose_M1 },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M3.FromDOK.MultiplyTranspose_M4 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M4.FromDOK.MultiplyTranspose_M3 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="Matrix.Operator__Multiplication__Matrix_T(Mat.Matrix{double}, double, Mat.Matrix{double})"/> and
            /// <see cref="Matrix.Static__Multiply__Matrix_T(Mat.Matrix{double}, double, Mat.Matrix{double})"/>.
            /// </summary>
            internal class Multiplication__Matrix_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M1.RightMultiplication_T },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M2.RightMultiplication_T },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M3.RightMultiplication_T },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M4.RightMultiplication_T },
                        // For CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.RightMultiplication_T },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.RightMultiplication_T },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.RightMultiplication_T },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.RightMultiplication_T },
                        // For DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.RightMultiplication_T },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.RightMultiplication_T },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.RightMultiplication_T },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.RightMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.Operator__Multiplication__T_Matrix(Mat.Matrix{double}, double, Mat.Matrix{double})"/> and
            /// <see cref="Matrix.Static__Multiply__T_Matrix(Mat.Matrix{double}, double, Mat.Matrix{double})"/>.
            /// </summary>
            internal class Multiplication__T_Matrix : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M1.LeftMultiplication_T },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M2.LeftMultiplication_T },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M3.LeftMultiplication_T },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M4.LeftMultiplication_T },
                        // For CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.LeftMultiplication_T },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.LeftMultiplication_T },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.LeftMultiplication_T },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.LeftMultiplication_T },
                        // For DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.LeftMultiplication_T },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.LeftMultiplication_T },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.LeftMultiplication_T },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.LeftMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.Operator__Division__Matrix_T(Mat.Matrix{double}, double, Mat.Matrix{double})"/> and
            /// <see cref="Matrix.Static__Divide__Matrix_T(Mat.Matrix{double}, double, Mat.Matrix{double})"/>.
            /// </summary>
            internal class Division__Matrix_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M1.Division_T },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M2.Division_T },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M3.Division_T },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M4.Division_T },
                        // For CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.Division_T },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.Division_T },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.Division_T },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.Division_T },
                        // For DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.Division_T },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.Division_T },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.Division_T },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.Division_T },
                    };
            }

            

            /// <summary>
            /// Class data for <see cref="Matrix.Operator__Multiplication__Matrix_Vector(Mat.Matrix{double}, Vect.Vector{double}, double[])"/> and
            /// <see cref="Matrix.Static__Multiply__Matrix_Vector(Mat.Matrix{double}, Vect.Vector{double}, double[])"/> and
            /// </summary>
            internal class Multiplication__Matrix_Vector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with DenseVector
                        { DenseMatrix.DataStorages.M3.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DenseMatrix.DataStorages.M3.Multiplication_V3 },
                        { DenseMatrix.DataStorages.M4.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DenseMatrix.DataStorages.M4.Multiplication_V3 },
                        // For DenseMatrix with SparseVector
                        { DenseMatrix.DataStorages.M3.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DenseMatrix.DataStorages.M3.Multiplication_V3 },
                        { DenseMatrix.DataStorages.M4.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DenseMatrix.DataStorages.M4.Multiplication_V3 },

                        // For CompressedColumn with DenseVector
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M3.FromCC.Multiplication_V3 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M4.FromCC.Multiplication_V3 },
                        // For CompressedColumn with SparseVector
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M3.FromCC.Multiplication_V3 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M4.FromCC.Multiplication_V3 },

                        // For DictionaryOfKeys with DenseVector
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M3.FromDOK.Multiplication_V3 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Multiplication_V3 },
                        // For DictionaryOfKeys with SparseVector
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M3.FromDOK.Multiplication_V3 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M4.FromDOK.Multiplication_V3 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.Static__TransposeMultiply__Matrix_Vector(Mat.Matrix{double}, Vect.Vector{double}, double[])"/> and
            /// </summary>
            internal class TransposeMultiplication__Matrix_Vector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix with DenseVector
                        { DenseMatrix.DataStorages.M3.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_V3 },
                        { DenseMatrix.DataStorages.M4.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_V3 },
                        // For DenseMatrix with SparseVector
                        { DenseMatrix.DataStorages.M3.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DenseMatrix.DataStorages.M3.TransposeMultiply_V3 },
                        { DenseMatrix.DataStorages.M4.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, DenseMatrix.DataStorages.M4.TransposeMultiply_V3 },

                        // For CompressedColumn with DenseVector
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M3.FromCC.TransposeMultiply_V3 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M4.FromCC.TransposeMultiply_V3 },
                        // For CompressedColumn with SparseVector
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M3.FromCC.TransposeMultiply_V3 },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M4.FromCC.TransposeMultiply_V3 },

                        // For DictionaryOfKeys with DenseVector
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M3.FromDOK.TransposeMultiply_V3 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, Vectors.Double.DenseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M4.FromDOK.TransposeMultiply_V3 },
                        // For DictionaryOfKeys with SparseVector
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M3.FromDOK.TransposeMultiply_V3 },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, Vectors.Double.SparseVector.DataStorages.V3.Readable, SparseMatrix.DataStorages.M4.FromDOK.TransposeMultiply_V3 },
                    };
            }

            #endregion

            #region For Methods

            /// <summary>
            /// Class data for <see cref="Matrix.GetComponent__Int_Int(Mat.Matrix{double}, double[,])"/>,
            /// <see cref="Matrix.ToArray(Mat.Matrix{double}, double[,])"/>,
            /// <see cref="Matrix.ToRowMajorArray(Mat.Matrix{double}, double[,])"/>, 
            /// <see cref="Matrix.ToColumnMajorArray(Mat.Matrix{double}, double[,])"/>,
            /// <see cref="Matrix.RowVectors(Mat.Matrix{double}, double[,])"/> and
            /// <see cref="Matrix.ColumnVectors(Mat.Matrix{double}, double[,])"/>.
            /// </summary>
            internal class ToArray : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix
                        { DenseMatrix.DataStorages.M1.Readable, DenseMatrix.DataStorages.M1.ToArray },
                        { DenseMatrix.DataStorages.M2.Readable, DenseMatrix.DataStorages.M2.ToArray },
                        { DenseMatrix.DataStorages.M3.Readable, DenseMatrix.DataStorages.M3.ToArray },
                        { DenseMatrix.DataStorages.M4.Readable, DenseMatrix.DataStorages.M4.ToArray },
                        // For CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Readable, SparseMatrix.DataStorages.M1.FromCC.ToArray },
                        { SparseMatrix.DataStorages.M2.FromCC.Readable, SparseMatrix.DataStorages.M2.FromCC.ToArray },
                        { SparseMatrix.DataStorages.M3.FromCC.Readable, SparseMatrix.DataStorages.M3.FromCC.ToArray },
                        { SparseMatrix.DataStorages.M4.FromCC.Readable, SparseMatrix.DataStorages.M4.FromCC.ToArray },
                        { SparseMatrix.DataStorages.M5.FromCC.Readable, SparseMatrix.DataStorages.M5.FromCC.ToArray },
                        // For DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Readable, SparseMatrix.DataStorages.M1.FromDOK.ToArray },
                        { SparseMatrix.DataStorages.M2.FromDOK.Readable, SparseMatrix.DataStorages.M2.FromDOK.ToArray },
                        { SparseMatrix.DataStorages.M3.FromDOK.Readable, SparseMatrix.DataStorages.M3.FromDOK.ToArray },
                        { SparseMatrix.DataStorages.M4.FromDOK.Readable, SparseMatrix.DataStorages.M4.FromDOK.ToArray },
                        { SparseMatrix.DataStorages.M5.FromDOK.Readable, SparseMatrix.DataStorages.M5.FromDOK.ToArray },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Matrix.SetComponent__Int_Int(Mat.Matrix{double}, int, int, double, Mat.Matrix{double})"/>.
            /// </summary>
            internal class SetComponent : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        // For DenseMatrix
                        { DenseMatrix.DataStorages.M1.Writable, DenseMatrix.DataStorages.M1.SetComponent },
                        { DenseMatrix.DataStorages.M2.Writable, DenseMatrix.DataStorages.M2.SetComponent },
                        { DenseMatrix.DataStorages.M3.Writable, DenseMatrix.DataStorages.M3.SetComponent },
                        { DenseMatrix.DataStorages.M4.Writable, DenseMatrix.DataStorages.M4.SetComponent },
                        // For CompressedColumn
                        { SparseMatrix.DataStorages.M1.FromCC.Writable, SparseMatrix.DataStorages.M1.FromCC.SetComponent },
                        { SparseMatrix.DataStorages.M2.FromCC.Writable, SparseMatrix.DataStorages.M2.FromCC.SetComponent },
                        { SparseMatrix.DataStorages.M3.FromCC.Writable, SparseMatrix.DataStorages.M3.FromCC.SetComponent },
                        { SparseMatrix.DataStorages.M4.FromCC.Writable, SparseMatrix.DataStorages.M4.FromCC.SetComponent },
                        // For DictionaryOfKeys
                        { SparseMatrix.DataStorages.M1.FromDOK.Writable, SparseMatrix.DataStorages.M1.FromDOK.SetComponent },
                        { SparseMatrix.DataStorages.M2.FromDOK.Writable, SparseMatrix.DataStorages.M2.FromDOK.SetComponent },
                        { SparseMatrix.DataStorages.M3.FromDOK.Writable, SparseMatrix.DataStorages.M3.FromDOK.SetComponent },
                        { SparseMatrix.DataStorages.M4.FromDOK.Writable, SparseMatrix.DataStorages.M4.FromDOK.SetComponent },
                    };
            }


            #endregion
        }

        #endregion
    }
}
