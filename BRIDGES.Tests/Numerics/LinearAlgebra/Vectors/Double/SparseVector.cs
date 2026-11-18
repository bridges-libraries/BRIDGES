using System;
using System.Collections.Generic;

using Xunit;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Vectors.Double
{
    /// <summary>
    /// Tests the members of the <see cref=Vect.SparseVector{T}"/> class.
    /// </summary>
    public class SparseVector
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Vect.Vector{T}.RowCount"/>.
        /// </summary>
        /// <param name="vector"> Vector to evaluate. </param>
        /// <param name="expected"> Expected size of the vector. </param>
        [Theory(DisplayName = "Prop. Size")]
        [ClassData(typeof(ClassDatas.Size))]
        public void Property_Size(Vect.SparseVector<double> vector, int expected)
        {
            // Arrange

            //Act
            int size = vector.RowCount;

            // Assert
            Assert.Equal(expected, size);
        }

        /// <summary>
        /// Tests the property <see cref="Vect.Vector{T}.this(int)"/>.
        /// </summary>
        /// <param name="vector"> Vector to evaluate. </param>
        /// <param name="components"> Expected components of the vector. </param>
        [Theory(DisplayName = "Prop. this[int]")]
        [ClassData(typeof(ClassDatas.Components))]
        public void Property_This_Int(Vect.SparseVector<double> vector,  double[] components)
        {
            // Arrange

            //Act

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(components.Length, vector.RowCount);
            for (int i = 0; i < vector.RowCount; i++)
            {
                if (vector.Contains(i))
                {
                    Assert.Equal(components[i], vector[i]);
                    nonZeroCount++;
                }
                else
                {
                    Assert.Equal(0.0, components[i]);
                }
            }
            Assert.Equal(nonZeroCount, vector.NonZeroCount);
        }

        #endregion

        #region Tests : Constructors

        /// <summary>
        /// Tests the property <see cref="Vect.SparseVector{T}.SparseVector(int)"/>.
        /// </summary>
        /// <param name="size"> Expected size of the vector. </param>
        [Theory(DisplayName = "SparseVector<T>(int)")]
        [ClassData(typeof(ClassDatas.Constructor__Int))]
        public void Constructor__Int(int size)
        {
            // Arrange

            //Act
            Vect.SparseVector<double> result = new Vect.SparseVector<double>(size);

            // Assert
            Assert.Equal(size, result.RowCount);
            Assert.Equal(0, result.NonZeroCount);
        }

        /// <summary>
        /// Tests the property <see cref="Vect.SparseVector{T}.SparseVector(int, IList{int}, IList{T})"/>.
        /// </summary>
        /// <param name="size"> Expected size of the vector. </param>
        /// <param name="indices"> Indices of the non-zero values for the vector. </param>
        /// <param name="values"> Non-zero values for the vector. </param>
        /// <param name="expected"> Expected vector. </param>
        [Theory(DisplayName = "SparseVector<T>(int,IList<int>,IList<T>) - Check")]
        [ClassData(typeof(ClassDatas.Constructor__Int_IListOfInt_IListOfT))]
        public void Constructor__Int_IListOfInt_IListOfT(int size, IList<int> indices, IList<double> values, Vect.SparseVector<double> expected)
        {
            // Arrange

            //Act
            Vect.SparseVector<double> result = new Vect.SparseVector<double>(size, indices, values);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        /// <summary>
        /// Tests the property <see cref="Vect.SparseVector{T}.SparseVector(int, IList{int}, IList{T}, bool)"/>.
        /// </summary>
        /// <param name="size"> Expected size of the vector. </param>
        /// <param name="indices"> Indices of the non-zero values for the vector. </param>
        /// <param name="values"> Non-zero values for the vector. </param>
        /// <param name="expected"> Expected vector. </param>
        [Theory(DisplayName = "SparseVector<T>(int,IList<int>,IList<T>,Bool) - No Check")]
        [ClassData(typeof(ClassDatas.Constructor__Int_IListOfInt_IListOfT))]
        public void Constructor__Int_IListOfInt_IListOfT_Bool__Check(int size, IList<int> indices, IList<double> values, Vect.SparseVector<double> expected)
        {
            // Arrange
            if (values is double[]) 
            {
                indices = new List<int>(values.Count);
                for (int i = 0; i < values.Count; i++) { indices.Add(i); }
            }

            //Act
            Vect.SparseVector<double> result = new Vect.SparseVector<double>(size, indices, values, true);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        /// <summary>
        /// Tests the property <see cref="Vect.SparseVector{T}.SparseVector(int, IList{int}, IList{T}, bool)"/>.
        /// </summary>
        /// <param name="size"> Expected size of the vector. </param>
        /// <param name="indices"> Indices of the non-zero values for the vector. </param>
        /// <param name="values"> Non-zero values for the vector. </param>
        /// <param name="expected"> Expected vector. </param>
        [Theory(DisplayName = "SparseVector<T>(int,IList<int>,IList<T>,Bool)")]
        [ClassData(typeof(ClassDatas.Constructor__Int_IListOfInt_IListOfT))]
        public void Constructor__Int_IListOfInt_IListOfT_Bool__NoCheck(int size, IList<int> indices, IList<double> values, Vect.SparseVector<double> expected)
        {
            // Arrange
            if (values is double[])
            {
                indices = new List<int>(values.Count);
                for (int i = 0; i < values.Count; i++) { indices.Add(i); }
            }

            //Act
            Vect.SparseVector<double> result = new Vect.SparseVector<double>(size, indices, values, false);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(values[nonZeroCount], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        /// <summary>
        /// Tests the property <see cref="Vect.SparseVector{T}.SparseVector(Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="expected"> Expected sparse vector to operate from. </param>
        [Theory(DisplayName = "SparseVector<T>(SparseVector<T>)")]
        [ClassData(typeof(ClassDatas.Constructor__SparseVectorOfT))]
        public void Constructor__SparseVectorOfT(Vect.SparseVector<double> expected)
        {
            // Arrange

            //Act
            Vect.SparseVector<double> result = new Vect.SparseVector<double>(expected);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);

            Assert.NotSame(expected, result);
        }

        #endregion

        #region Tests : Public Static Methods

        //     -----     -----     Additive Abelian Group : SparseVector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.Add(Vect.SparseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(SparseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.Addition__SparseVector_SparseVector))]
        public void Static__Add__SparseVector_SparseVector(Vect.SparseVector<double> left, Vect.SparseVector<double> right, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = Vect.SparseVector<double>.Add(left, right);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.Subtract(Vect.SparseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(SparseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.Subtraction__SparseVector_SparseVector))]
        public void Static__Subtract__SparseVector_SparseVector(Vect.SparseVector<double> left, Vect.SparseVector<double> right, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = Vect.SparseVector<double>.Subtract(left, right);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }


        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.Opposite(Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.SparseVector{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Static Opposite(SparseVector)")]
        [ClassData(typeof(ClassDatas.UnaryNegation__SparseVector))]
        public void Static__Opposite__SparseVector(Vect.SparseVector<double> operand, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = Vect.SparseVector<double>.Opposite(operand);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }


        //     -----     Other Operations : SparseVector<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.TransposeMultiply(Vect.SparseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(SparseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.TransposeMultiplication__SparseVector_SparseVector))]
        public void Static__TransposeMultiply__SparseVector_SparseVector(Vect.SparseVector<double> left, Vect.SparseVector<double> right, double expected)
        {
            // Arrange

            // Act 
            double result = Vect.SparseVector<double>.TransposeMultiply(left, right);

            // Assert
            Assert.Equal(expected, result);
        }



        //     -----     -----     Right Embedding : DenseVector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.Add(Vect.SparseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(SparseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.Addition__SparseVector_DenseVector))]
        public void Static__Add__SparseVector_DenseVector(Vect.SparseVector<double> left, Vect.DenseVector<double> right, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.SparseVector<double>.Add(left, right);
            
            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.Subtract(Vect.SparseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(SparseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.Subtraction__SparseVector_DenseVector))]
        public void Static__Subtract__SparseVector_DenseVector(Vect.SparseVector<double> left, Vect.DenseVector<double> right, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.SparseVector<double>.Subtract(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }



        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.Multiply(Vect.SparseVector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.SparseVector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(SparseVector,T)")]
        [ClassData(typeof(ClassDatas.Multiplication__SparseVector_T))]
        public void Static__Multiply__SparseVector_T(Vect.SparseVector<double> operand, double factor, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = Vect.SparseVector<double>.Multiply(operand, factor);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.Multiply(T,Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Vect.SparseVector{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(T,SparseVector)")]
        [ClassData(typeof(ClassDatas.Multiplication__T_SparseVector))]
        public void Static__Multiply__T_SparseVector(Vect.SparseVector<double> operand, double factor, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = Vect.SparseVector<double>.Multiply(factor, operand);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }


        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.Divide(Vect.SparseVector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.SparseVector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the division. </param>
        [Theory(DisplayName = "Static Divide(SparseVector,T)")]
        [ClassData(typeof(ClassDatas.Division__SparseVector_T))]
        public void Static__Divide__SparseVector_T(Vect.SparseVector<double> operand, double divisor, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = Vect.SparseVector<double>.Divide(operand, divisor);

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        #endregion

        #region Tests : Operators

        //     -----     -----     Additive Abelian Group : SparseVector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.operator+(Vect.SparseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (SparseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.Addition__SparseVector_SparseVector))]
        public void Operator__Addition__SparseVector_SparseVector(Vect.SparseVector<double> left, Vect.SparseVector<double> right, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = left + right;

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.operator-(Vect.SparseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (SparseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.Subtraction__SparseVector_SparseVector))]
        public void Operator__Subtraction__SparseVector_SparseVector(Vect.SparseVector<double> left, Vect.SparseVector<double> right, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = left - right;

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }


        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.operator-(Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.SparseVector{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Op - (SparseVector)")]
        [ClassData(typeof(ClassDatas.UnaryNegation__SparseVector))]
        public void Operator__UnaryNegation__SparseVector(Vect.SparseVector<double> operand, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = -operand;

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }



        //     -----     -----     Right Embedding : DenseVector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.operator+(Vect.SparseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (SparseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.Addition__SparseVector_DenseVector))]
        public void Operator__Addition__SparseVector_DenseVector(Vect.SparseVector<double> left, Vect.DenseVector<double> right, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = left + right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.operator-(Vect.SparseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.SparseVector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (SparseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.Subtraction__SparseVector_DenseVector))]
        public void Operator__Subtraction__SparseVector_DenseVector(Vect.SparseVector<double> left, Vect.DenseVector<double> right, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = left - right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }



        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.operator*(Vect.SparseVector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.SparseVector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Op * (SparseVector,T)")]
        [ClassData(typeof(ClassDatas.Multiplication__SparseVector_T))]
        public void Operator__Multiplication__SparseVector_T(Vect.SparseVector<double> operand, double factor, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = operand * factor;

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.operator*(T,Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Vect.SparseVector{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Op * (T,SparseVector)")]
        [ClassData(typeof(ClassDatas.Multiplication__T_SparseVector))]
        public void Operator__Multiplication__T_SparseVector(Vect.SparseVector<double> operand, double factor, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = factor * operand;

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }


        /// <summary>
        /// Tests the method <see cref="Vect.SparseVector{T}.operator/(Vect.SparseVector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.SparseVector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.SparseVector{T}"/> of the division. </param>
        [Theory(DisplayName = "Op / (SparseVector,T)")]
        [ClassData(typeof(ClassDatas.Division__SparseVector_T))]
        public void Operator__Division__SparseVector_T(Vect.SparseVector<double> operand, double divisor, Vect.SparseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.SparseVector<double> result = operand / divisor;

            // Assert
            int nonZeroCount = 0;

            Assert.Equal(expected.RowCount, result.RowCount);
            Assert.Equal(expected.NonZeroCount, result.NonZeroCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                if (result.Contains(i))
                {
                    Assert.Equal(expected[i], result[i]);
                    nonZeroCount++;
                }
            }
            Assert.Equal(nonZeroCount, result.NonZeroCount);
        }

        #endregion

        #region Tests : Public Methods

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.GetValue(int)"/>.
        /// </summary>
        /// <param name="vector"> Vector to operate from. </param>
        /// <param name="components"> Components of the vector. </param>
        [Theory(DisplayName = "GetComponent(int)")]
        [ClassData(typeof(ClassDatas.Components))]
        public void GetComponent__Int(Vect.SparseVector<double> vector, double[] components)
        {
            // Arrange

            // Act & Assert
            for (int i = 0; i < vector.RowCount; i++)
            {
                Assert.Equal(components[i], vector.Get(i));
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.ToArray()"/>.
        /// </summary>
        /// <param name="vector"> Vector to operate from. </param>
        /// <param name="components"> Components of the vector. </param>
        [Theory(DisplayName = "ToArray(int)")]
        [ClassData(typeof(ClassDatas.Components))]
        public void ToArray(Vect.SparseVector<double> vector, double[] components)
        {
            // Arrange

            // Act
            double[] result = vector.ToArray();

            // Assert
            Assert.Equal(components.Length, result.Length);
            for (int i = 0; i < components.Length; i++)
            {
                Assert.Equal(components[i], result[i]);
            }
        }

        #endregion


        #region Storage Classes for Data Classes

        internal static class DataStorages
        {
            /// <summary>
            /// Computes and stores general data related to general <see cref="Vect.SparseVector{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class V1
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored vector <see cref="V1"/>.
                /// </summary>
                private static readonly Vect.SparseVector<double> _v1 = CreateV1();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Vect.SparseVector{T}"/> of type <see cref="V1"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_v1];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Vect.SparseVector{T}"/> of type <see cref="V1"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateV1()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of element in the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The number of element in the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Size() => [3];

                /// <summary>
                /// Provides the expected number of non-zero element in the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The number of non-zero element in the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Capacity() => [3];

                /// <summary>
                /// Provides the expected indices of each component of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the indices of the non-zero components of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Indices() => [new List<int> { 0, 1, 2 }];

                /// <summary>
                /// Provides the expected non-zero values of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the non-zero values of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Values() => [new List<double> { 1.0, -0.5, 5.0 }];
                

                /// <summary>
                /// Provides the expected values of each component of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the values of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Components() => [new double[] { 1.0, -0.5, 5.0 }];

                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>V1<sup>t</sup>·V2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="double"/> number resulting from the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] TransposeMultiply_V2() => [-3.5];


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>V1+V2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Addition_V2() => [new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { 1.0, 6.5, 5.0 })];

                /// <summary>
                /// Provides the expected result of the subtraction : <c>V1-V2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Subtraction_V2() => [new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { 1.0, -7.5, 5.0 })];

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-V1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] UnaryNegation() => [new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { -1.0, 0.5, -5.0 })];


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 3.0 : <c>V1·3.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The value of the multiplication factor. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RightMultiplication_T() => [3.0, new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { 3.0, -1.5, 15 })];

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·V1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The value of the multiplication factor. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] LeftMultiplication_T() => [5.0, new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { 5.0, -2.5, 25.0 })];


                /// <summary>
                /// Provides the expected result of the scalar division by 4.0 : <c>V1/4.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The value of the divisor. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Division_T() => [4.0, new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { 0.25, -0.125, 1.25 })];


                //     -----     Public Methods

                /// <summary>
                /// Provides the information the set the value of a component.
                /// </summary>
                /// <remarks>
                /// Case : The component exists in the sparse storage and the value in non-zero.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> after the value is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent__Case_01() => [0, 2.2, new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { 2.2, -0.5, 5.0 })];

                /// <summary>
                /// Provides the information the set the value of a component.
                /// </summary>
                /// <remarks>
                /// Case : The component exists in the sparse storage and the value in zero.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> after the value is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent__Case_02() => [1, 0.0, new Vect.SparseVector<double>(3, new int[] { 0, 2 }, new double[] { 1.0, 5.0 })];

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a vector <see cref="V1"/>.
                /// </summary>
                /// <returns> The <see cref="Vect.SparseVector{T}"/>. </returns>
                private static Vect.SparseVector<double> CreateV1()
                {
                    return new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { 1.0, -0.5, 5.0 });
                }

                #endregion
            }

            /// <summary>
            /// Computes and stores general data related to general <see cref="Vect.SparseVector{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class V2
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored vector <see cref="V2"/>.
                /// </summary>
                private static readonly Vect.SparseVector<double> _v2 = CreateV2();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Vect.DenseVector{T}"/> of type <see cref="V2"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_v2];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Vect.DenseVector{T}"/> of type <see cref="V2"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateV2()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of element in the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The number of element in the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Size() => [3];

                /// <summary>
                /// Provides the expected number of non-zero element in the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The number of non-zero element in the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Capacity() => [1];

                /// <summary>
                /// Provides the expected indices of each component of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the indices of the non-zero components of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Indices() => [new List<int> { 1 }];

                /// <summary>
                /// Provides the expected non-zero values of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the non-zero values of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Values() => [new List<double> { 7.0 }];
                


                /// <summary>
                /// Provides the expected values of each component of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the values of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Components() => [new double[] { 0.0, 7.0, 0.0 }];


                //     -----     Public Static Methods

                /// <summary>
                /// Provides the expected result of the multiplication : <c>V2<sup>t</sup>·V1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="double"/> number resulting from the multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] TransposeMultiply_V1() => [-3.5];


                //     -----     Operators

                /// <summary>
                /// Provides the expected result of the addition : <c>V2+V1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Addition_V1() => [new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { 1.0, 6.5, 5.0 })];

                /// <summary>
                /// Provides the expected result of the subtraction : <c>V2-V1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Subtraction_V1() => [new Vect.SparseVector<double>(3, new int[] { 0, 1, 2 }, new double[] { -1.0, 7.5, -5.0 })];

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-V2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] UnaryNegation() => [new Vect.SparseVector<double>(3, new int[] { 1 }, new double[] { -7.0 })];


                /// <summary>
                /// Provides the expected result of the right scalar multiplication by 3.0 : <c>V2·3.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The value of the multiplication factor. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RightMultiplication_T() => [3.0, new Vect.SparseVector<double>(3, new int[] { 1 }, new double[] { 21.0 })];

                /// <summary>
                /// Provides the expected result of the left scalar multiplication by 5.0 : <c>5.0·V2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The value of the multiplication factor. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] LeftMultiplication_T() => [5.0, new Vect.SparseVector<double>(3, new int[] { 1 }, new double[] { 35.0 })];

                /// <summary>
                /// Provides the expected result of the scalar division by 4.0 : <c>V2/4.0</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The value of the divisor. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> The <see cref="Vect.SparseVector{T}"/> resulting from the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Division_T() => [4.0, new Vect.SparseVector<double>(3, new int[] { 1 }, new double[] { 1.75 })];


                //     -----     Public Methods

                /// <summary>
                /// Provides the information the set the value of a component.
                /// </summary>
                /// <remarks>
                /// Case : The component exists in the sparse storage and the value in non-zero.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> after the value is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent__Case_01() => [1, 2.2, new Vect.SparseVector<double>(3, new int[] { 1 }, new double[] { 2.2 })];

                /// <summary>
                /// Provides the information the set the value of a component.
                /// </summary>
                /// <remarks>
                /// Case : The component exists in the sparse storage and the value in zero.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> after the value is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent__Case_02() => [1, 0.0, new Vect.SparseVector<double>(3, new int[] { }, new double[] { })];

                /// <summary>
                /// Provides the information the set the value of a component.
                /// </summary>
                /// <remarks>
                /// Case : The component doesn't exist in the sparse storage and the value in non-zero.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> after the value is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent__Case_03() => [0, 3.4, new Vect.SparseVector<double>(3, new int[] { 0, 1 }, new double[] { 3.4, 7.0 })];

                /// <summary>
                /// Provides the information the set the value of a component.
                /// </summary>
                /// <remarks>
                /// Case : The component doesn't exist in the sparse storage and the value in zero.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Index of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Value of the component to set. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> after the value is set. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] SetComponent__Case_04() => [2, 0.0, new Vect.SparseVector<double>(3, new int[] { 1 }, new double[] { 7.0 })];

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a vector <see cref="V2"/>.
                /// </summary>
                /// <returns> The <see cref="Vect.SparseVector{T}"/>. </returns>
                private static Vect.SparseVector<double> CreateV2()
                {
                    return new Vect.SparseVector<double>(3, new int[] { 1 }, new double[] { 7.0 });
                }

                #endregion
            }

            /// <summary>
            /// Computes and stores general data related to general <see cref="Vect.SparseVector{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class V3
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored vector <see cref="V3"/>.
                /// </summary>
                private static readonly Vect.SparseVector<double> _v3 = CreateV3();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Vect.SparseVector{T}"/> of type <see cref="V3"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_v3];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="Vect.SparseVector{T}"/> of type <see cref="V3"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateV3()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of element in the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The number of element in the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Size() => [6];

                /// <summary>
                /// Provides the expected number of non-zero element in the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The number of non-zero element in the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Capacity() => [4];

                /// <summary>
                /// Provides the expected indices of each component of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the indices of the non-zero components of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Indices() => [new List<int> { 1, 2, 4, 5 }];

                /// <summary>
                /// Provides the expected non-zero values of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the non-zero values of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Values() => [new List<double> { 5.0, 3.5, 6.0, 2.0 }];



                /// <summary>
                /// Provides the expected values of each component of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array containing the values of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Components() => [new double[] { 0.0, 5.0, 3.5, 0.0, 6.0, 2.0 }];

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a vector <see cref="V3"/>.
                /// </summary>
                /// <returns> The <see cref="Vect.SparseVector{T}"/>. </returns>
                private static Vect.SparseVector<double> CreateV3()
                {
                    return new Vect.SparseVector<double>(6, new int[] { 1, 2, 4, 5 }, new double[] { 5.0, 3.5, 6.0, 2.0 });
                }

                #endregion
            }
        }

        #endregion

        #region Data Classes for Parametrised Tests

        internal static class ClassDatas
        {
            //     -----     Properties

            /// <summary>
            /// Class data for <see cref="SparseVector.Property_Size(Vect.SparseVector{double}, int)"/>.
            /// </summary>
            internal class Size : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.Size },
                        { DataStorages.V2.Readable, DataStorages.V2.Size },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Property_This_Int(Vect.SparseVector{double}, int)"/>.
            /// </summary>
            internal class Components : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.Components },
                        { DataStorages.V2.Readable, DataStorages.V2.Components },
                    };
            }


            //     -----     Constructors

            /// <summary>
            /// Class data for <see cref="SparseVector.Constructor__Int(int)"/>.
            /// </summary>
            internal class Constructor__Int : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Size },
                        { DataStorages.V2.Size },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Constructor__Int_IListOfInt_IListOfT(int, IList{int}, IList{double}, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class Constructor__Int_IListOfInt_IListOfT : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Size, DataStorages.V1.Indices, DataStorages.V1.Values, DataStorages.V1.Readable },
                        { DataStorages.V2.Size, DataStorages.V2.Indices, DataStorages.V2.Values, DataStorages.V2.Readable },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Constructor__Int_IListOfInt_IListOfT_Bool__Check(int, IList{int}, IList{double}, Vect.SparseVector{double})"/> and
            /// <see cref="SparseVector.Constructor__Int_IListOfInt_IListOfT_Bool__Check(int, IList{int}, IList{double}, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class Constructor__Int_IListOfInt_IListOfT_Bool : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Size, DataStorages.V1.Indices, DataStorages.V1.Values, DataStorages.V1.Readable },
                        { DataStorages.V2.Size, DataStorages.V2.Indices, DataStorages.V2.Values, DataStorages.V2.Readable },

                        { DataStorages.V1.Size, DataStorages.V1.Indices, DataStorages.V1.Components, DataStorages.V1.Readable },
                        { DataStorages.V2.Size, DataStorages.V2.Indices, DataStorages.V2.Components, DataStorages.V2.Readable },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Constructor__SparseVectorOfT(Vect.SparseVector{double})"/>.
            /// </summary>
            internal class Constructor__SparseVectorOfT : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable },
                        { DataStorages.V2.Readable },
                    };
            }


            //     -----     Static

            /// <summary>
            /// Class data for <see cref="SparseVector.Operator__Addition__SparseVector_SparseVector(Vect.SparseVector{double}, Vect.SparseVector{double}, Vect.SparseVector{double})"/> and
            /// <see cref="SparseVector.Static__Add__SparseVector_SparseVector(Vect.SparseVector{double}, Vect.SparseVector{double}, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class Addition__SparseVector_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V2.Readable, DataStorages.V1.Addition_V2 },
                        { DataStorages.V2.Readable, DataStorages.V1.Readable, DataStorages.V2.Addition_V1 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Operator__Subtraction__SparseVector_SparseVector(Vect.SparseVector{double}, Vect.SparseVector{double}, Vect.SparseVector{double})"/> and
            /// <see cref="SparseVector.Static__Add__SparseVector_SparseVector(Vect.SparseVector{double}, Vect.SparseVector{double}, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class Subtraction__SparseVector_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V2.Readable, DataStorages.V1.Subtraction_V2 },
                        { DataStorages.V2.Readable, DataStorages.V1.Readable, DataStorages.V2.Subtraction_V1 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Operator__UnaryNegation__SparseVector(Vect.SparseVector{double}, Vect.SparseVector{double})"/> and
            /// <see cref="SparseVector.Static__Opposite__SparseVector(Vect.SparseVector{double}, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class UnaryNegation__SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.UnaryNegation },
                        { DataStorages.V2.Readable, DataStorages.V2.UnaryNegation },
                    };
            }


            /// <summary>
            /// Class data for <see cref="SparseVector.Static__TransposeMultiply__SparseVector_SparseVector(Vect.SparseVector{double}, Vect.SparseVector{double}, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class TransposeMultiplication__SparseVector_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V2.Readable, DataStorages.V1.TransposeMultiply_V2 },
                        { DataStorages.V2.Readable, DataStorages.V1.Readable, DataStorages.V2.TransposeMultiply_V1 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="SparseVector.Operator__Multiplication__SparseVector_T(Vect.SparseVector{double}, double, Vect.SparseVector{double})"/> and
            /// <see cref="SparseVector.Static__Multiply__SparseVector_T(Vect.SparseVector{double}, double, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class Multiplication__SparseVector_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.RightMultiplication_T },
                        { DataStorages.V2.Readable, DataStorages.V2.RightMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Operator__Multiplication__T_SparseVector(Vect.SparseVector{double}, double, Vect.SparseVector{double})"/> and
            /// <see cref="SparseVector.Static__Multiply__T_SparseVector(Vect.SparseVector{double}, double, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class Multiplication__T_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.LeftMultiplication_T },
                        { DataStorages.V2.Readable, DataStorages.V2.LeftMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Operator__Division__SparseVector_T(Vect.SparseVector{double}, double, Vect.SparseVector{double})"/> and
            /// <see cref="SparseVector.Static__Divide__SparseVector_T(Vect.SparseVector{double}, double, Vect.SparseVector{double})"/>.
            /// </summary>
            internal class Division__SparseVector_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.Division_T },
                        { DataStorages.V2.Readable, DataStorages.V2.Division_T },
                    };
            }


            /// <summary>
            /// Class data for <see cref="SparseVector.Operator__Addition__SparseVector_DenseVector(Vect.SparseVector{double}, Vect.DenseVector{double}, Vect.DenseVector{double})"/> and
            /// <see cref="SparseVector.Static__Add__SparseVector_DenseVector(Vect.SparseVector{double}, Vect.DenseVector{double}, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Addition__SparseVector_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Addition_V2 },
                        { DataStorages.V2.Readable, DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Addition_V1 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="SparseVector.Operator__Subtraction__SparseVector_DenseVector(Vect.SparseVector{double}, Vect.DenseVector{double}, Vect.DenseVector{double})"/> and
            /// <see cref="SparseVector.Static__Add__SparseVector_DenseVector(Vect.SparseVector{double}, Vect.DenseVector{double}, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Subtraction__SparseVector_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Subtraction_V2 },
                        { DataStorages.V2.Readable, DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Subtraction_V1 },
                    };
            }


            //     -----     Methods

            /// <summary>
            /// Class data for <see cref="DenseVector.SetValue__Int_T(Vect.DenseVector{double}, int, double, Vect.DenseVector{double})"/>
            /// </summary>
            internal class SetComponent__Int_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Writable, DataStorages.V1.SetComponent__Case_01 },
                        { DataStorages.V1.Writable, DataStorages.V1.SetComponent__Case_02 },

                        { DataStorages.V2.Writable, DataStorages.V2.SetComponent__Case_01 },
                        { DataStorages.V2.Writable, DataStorages.V2.SetComponent__Case_02 },
                        { DataStorages.V2.Writable, DataStorages.V2.SetComponent__Case_03 },
                        { DataStorages.V2.Writable, DataStorages.V2.SetComponent__Case_04 },
                    };
            }
        }

        #endregion
    }
}
