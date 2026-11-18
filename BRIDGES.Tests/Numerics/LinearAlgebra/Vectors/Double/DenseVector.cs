using System;
using System.Collections.Generic;

using Xunit;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Vectors.Double
{
    /// <summary>
    /// Tests the members of the <see cref=Vect.DenseVector{T}"/> class.
    /// </summary>
    public class DenseVector
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Vect.Vector{T}.RowCount"/>.
        /// </summary>
        /// <param name="vector"> Vector to evaluate. </param>
        /// <param name="expected"> Expected size of the vector. </param>
        [Theory(DisplayName = "Prop. Size")]
        [ClassData(typeof(ClassDatas.Size))]
        public void Property_Size(Vect.DenseVector<double> vector, int expected)
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
        public void Property_This_Int(Vect.DenseVector<double> vector, double[] components)
        {
            // Arrange

            //Act

            // Assert
            Assert.Equal(components.Length, vector.RowCount);
            for (int i = 0; i < vector.RowCount; i++)
            {
                Assert.Equal(components[i], vector[i]);
            }
        }

        #endregion

        #region Tests : Constructors

        /// <summary>
        /// Tests the property <see cref="Vect.DenseVector{T}.DenseVector(int)"/>.
        /// </summary>
        /// <param name="size"> Expected size of the vector. </param>
        [Theory(DisplayName = "DenseVector<T>(int)")]
        [ClassData(typeof(ClassDatas.Constructor__Int))]
        public void Constructor__Int(int size)
        {
            // Arrange

            //Act
            Vect.DenseVector<double> result = new Vect.DenseVector<double>(size);

            // Assert
            Assert.Equal(size, result.RowCount);
            for (int i = 0; i < size; i++)
            {
                Assert.Equal(0.0, result[0]);
            }
        }

        /// <summary>
        /// Tests the property <see cref="Vect.DenseVector{T}.DenseVector(IList{T})"/>.
        /// </summary>
        /// <param name="components"> Components of the vector. </param>
        [Theory(DisplayName = "DenseVector<T>(IList<T>)")]
        [ClassData(typeof(ClassDatas.Constructor__IListOfT))]
        public void Constructor__IListOfT(double[] components)
        {
            // Arrange

            //Act
            Vect.DenseVector<double> result = new Vect.DenseVector<double>(components);

            // Assert
            Assert.Equal(components.Length, result.RowCount);
            for (int i = 0; i < components.Length; i++)
            {
                Assert.Equal(components[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the property <see cref="Vect.DenseVector{T}.DenseVector(Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="expected"> Expected dense vector to operate from. </param>
        [Theory(DisplayName = "DenseVector<T>(DenseVector<T>)")]
        [ClassData(typeof(ClassDatas.Constructor__DenseVectorOfT))]
        public void Constructor__DenseVectorOfT(Vect.DenseVector<double> expected)
        {
            // Arrange

            //Act
            Vect.DenseVector<double> result = new Vect.DenseVector<double>(expected);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < expected.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
            Assert.NotSame(expected, result);
        }

        /// <summary>
        /// Tests the property <see cref="Vect.DenseVector{T}.DenseVector(Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="sparse"> Sparse vector to copy. </param>
        /// <param name="expected"> Expected dense vector. </param>
        [Theory(DisplayName = "DenseVector<T>(SparseVector<T>)")]
        [ClassData(typeof(ClassDatas.Constructor__SparseVectorOfT))]
        public void Constructor__SparseVectorOfT(Vect.SparseVector<double> sparse, Vect.DenseVector<double> expected)
        {
            // Arrange

            //Act
            Vect.DenseVector<double> result = new Vect.DenseVector<double>(sparse);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < expected.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        #endregion

        #region Tests : Public Static Methods

        //     -----     -----     Additive Abelian Group : DenseVector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.Add(Vect.DenseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(DenseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.Addition__DenseVector_DenseVector))]
        public void Static__Add__DenseVector_DenseVector(Vect.DenseVector<double> left, Vect.DenseVector<double> right, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.DenseVector<double>.Add(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.Subtract(Vect.DenseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(DenseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.Subtraction__DenseVector_DenseVector))]
        public void Static__Subtract__DenseVector_DenseVector(Vect.DenseVector<double> left, Vect.DenseVector<double> right, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.DenseVector<double>.Subtract(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.Opposite(Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.DenseVector{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Static Opposite(DenseVector)")]
        [ClassData(typeof(ClassDatas.UnaryNegation__DenseVector))]
        public void Static__Opposite__DenseVector(Vect.DenseVector<double> operand, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.DenseVector<double>.Opposite(operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }


        //     -----     Other Operations : DenseVector<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.TransposeMultiply(Vect.DenseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(DenseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.TransposeMultiplication__DenseVector_DenseVector))]
        public void Static__TransposeMultiply__DenseVector_DenseVector(Vect.DenseVector<double> left, Vect.DenseVector<double> right, double expected)
        {
            // Arrange

            // Act 
            double result = Vect.DenseVector<double>.TransposeMultiply(left, right);

            // Assert
            Assert.Equal(expected, result);
        }



        //     -----     -----     Right Embedding : SparseVector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.Add(Vect.DenseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(DenseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.Addition__DenseVector_SparseVector))]
        public void Static__Add__DenseVector_SparseVector(Vect.DenseVector<double> left, Vect.SparseVector<double> right, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.DenseVector<double>.Add(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.Subtract(Vect.DenseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(DenseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.Subtraction__DenseVector_SparseVector))]
        public void Static__Subtract__DenseVector_SparseVector(Vect.DenseVector<double> left, Vect.SparseVector<double> right, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.DenseVector<double>.Subtract(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }



        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.Multiply(Vect.DenseVector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.DenseVector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(DenseVector,T)")]
        [ClassData(typeof(ClassDatas.Multiplication__DenseVector_T))]
        public void Static__Multiply__DenseVector_T(Vect.DenseVector<double> operand, double factor, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.DenseVector<double>.Multiply(operand, factor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.Multiply(T,Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Vect.DenseVector{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(T,DenseVector)")]
        [ClassData(typeof(ClassDatas.Multiplication__T_DenseVector))]
        public void Static__Multiply__T_DenseVector(Vect.DenseVector<double> operand, double factor, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.DenseVector<double>.Multiply(factor, operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.Divide(Vect.DenseVector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.DenseVector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the division. </param>
        [Theory(DisplayName = "Static Divide(DenseVector,T)")]
        [ClassData(typeof(ClassDatas.Division__DenseVector_T))]
        public void Static__Divide__DenseVector_T(Vect.DenseVector<double> operand, double divisor, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = Vect.DenseVector<double>.Divide(operand, divisor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        #endregion

        #region Tests : Operators

        //     -----     -----     Additive Abelian Group : DenseVector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.operator+(Vect.DenseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (DenseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.Addition__DenseVector_DenseVector))]
        public void Operator__Addition__DenseVector_DenseVector(Vect.DenseVector<double> left, Vect.DenseVector<double> right, Vect.DenseVector<double> expected)
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
        /// Tests the method <see cref="Vect.DenseVector{T}.operator-(Vect.DenseVector{T},Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.DenseVector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (DenseVector,DenseVector)")]
        [ClassData(typeof(ClassDatas.Subtraction__DenseVector_DenseVector))]
        public void Operator__Subtraction__DenseVector_DenseVector(Vect.DenseVector<double> left, Vect.DenseVector<double> right, Vect.DenseVector<double> expected)
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


        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.operator-(Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.DenseVector{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Op - (DenseVector)")]
        [ClassData(typeof(ClassDatas.UnaryNegation__DenseVector))]
        public void Operator__UnaryNegation__DenseVector(Vect.DenseVector<double> operand, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = -operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }



        //     -----     -----     Right Embedding : SparseVector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.operator+(Vect.DenseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (DenseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.Addition__DenseVector_SparseVector))]
        public void Operator__Addition__DenseVector_SparseVector(Vect.DenseVector<double> left, Vect.SparseVector<double> right, Vect.DenseVector<double> expected)
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
        /// Tests the method <see cref="Vect.DenseVector{T}.operator-(Vect.DenseVector{T},Vect.SparseVector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.DenseVector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.SparseVector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (DenseVector,SparseVector)")]
        [ClassData(typeof(ClassDatas.Subtraction__DenseVector_SparseVector))]
        public void Operator__Subtraction__DenseVector_SparseVector(Vect.DenseVector<double> left, Vect.SparseVector<double> right, Vect.DenseVector<double> expected)
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



        //     -----      Group Action : T     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.operator*(Vect.DenseVector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.DenseVector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Op * (DenseVector,T)")]
        [ClassData(typeof(ClassDatas.Multiplication__DenseVector_T))]
        public void Operator__Multiplication__DenseVector_T(Vect.DenseVector<double> operand, double factor, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = operand * factor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.operator*(T,Vect.DenseVector{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Vect.DenseVector{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Op * (T,DenseVector)")]
        [ClassData(typeof(ClassDatas.Multiplication__T_DenseVector))]
        public void Operator__Multiplication__T_DenseVector(Vect.DenseVector<double> operand, double factor, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = factor * operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.operator/(Vect.DenseVector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.DenseVector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.DenseVector{T}"/> of the division. </param>
        [Theory(DisplayName = "Op / (DenseVector,T)")]
        [ClassData(typeof(ClassDatas.Division__DenseVector_T))]
        public void Operator__Division__DenseVector_T(Vect.DenseVector<double> operand, double divisor, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act 
            Vect.DenseVector<double> result = operand / divisor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

        #endregion

        #region Tests : Public Methods

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.GetValue(int)"/>.
        /// </summary>
        /// <param name="vector"> Vector to operate from. </param>
        /// <param name="components"> Components of the vector. </param>
        [Theory(DisplayName = "GetValue(int)")]
        [ClassData(typeof(ClassDatas.Components))]
        public void GetValue__Int(Vect.DenseVector<double> vector, double[] components)
        {
            // Arrange

            // Act & Assert
            for (int i = 0; i < vector.RowCount; i++)
            {
                Assert.Equal(components[i], vector.Get(i));
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.SetValue(int,T)"/>.
        /// </summary>
        /// <param name="vector"> Vector to operate on. </param>
        /// <param name="index"> Index of the component to set. </param>
        /// <param name="value"> Value of the component to set. </param>
        /// <param name="expected"> Expected vector a</param>
        [Theory(DisplayName = "SetValue(int,T)")]
        [ClassData(typeof(ClassDatas.SetValue__Int_T))]
        public void SetValue__Int_T(Vect.DenseVector<double> vector, int index, double value, Vect.DenseVector<double> expected)
        {
            // Arrange

            // Act
            vector[index] = value;

            // Assert
            Assert.Equal(expected.RowCount, vector.RowCount);
            for (int i = 0; i < vector.RowCount; i++)
            {
                Assert.Equal(expected[i], vector[i]);
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.DenseVector{T}.ToArray()"/>.
        /// </summary>
        /// <param name="vector"> Vector to operate from. </param>
        /// <param name="components"> Components of the vector. </param>
        [Theory(DisplayName = "ToArray(int)")]
        [ClassData(typeof(ClassDatas.Components))]
        public void ToArray(Vect.DenseVector<double> vector, double[] components)
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
            /// Computes and stores general data related to general <see cref="Vect.DenseVector{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class V1
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored vector <see cref="V1"/>.
                /// </summary>
                private static readonly Vect.DenseVector<double> _v1 = CreateV1();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Vect.DenseVector{T}"/> of type <see cref="V1"/>. </description>
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
                ///         <description> A newly computed <see cref="Vect.DenseVector{T}"/> of type <see cref="V1"/>. </description>
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
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Addition_V2() => [new Vect.DenseVector<double>(new double[] { 1.0, 6.5, 5.0 })];

                /// <summary>
                /// Provides the expected result of the subtraction : <c>V1-V2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Subtraction_V2() => [new Vect.DenseVector<double>(new double[] { 1.0, -7.5, 5.0 })];

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-V1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] UnaryNegation() => [new Vect.DenseVector<double>(new double[] { -1.0, 0.5, -5.0 })];


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
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RightMultiplication_T() => [3.0, new Vect.DenseVector<double>(new double[] { 3.0, -1.5, 15 })];

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
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] LeftMultiplication_T() => [5.0, new Vect.DenseVector<double>(new double[] { 5.0, -2.5, 25.0 })];


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
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Division_T() => [4.0, new Vect.DenseVector<double>(new double[] { 0.25, -0.125, 1.25 })];


                //     -----     Public Methods

                /// <summary>
                /// Provides the information the set the value of a component.
                /// </summary>
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
                public static object[] SetValue() => [1, 2.2, new Vect.DenseVector<double>(new double[] { 1.0, 2.2, 5.0 })];

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a vector <see cref="V1"/>.
                /// </summary>
                /// <returns> The <see cref="Vect.DenseVector{T}"/>. </returns>
                private static Vect.DenseVector<double> CreateV1()
                {
                    return new Vect.DenseVector<double>(new double[] { 1.0, -0.5, 5.0 });
                }

                #endregion
            }

            /// <summary>
            /// Computes and stores general data related to general <see cref="Vect.DenseVector{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class V2
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored vector <see cref="V2"/>.
                /// </summary>
                private static readonly Vect.DenseVector<double> _v2 = CreateV2();

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
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the addition. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Addition_V1() => [new Vect.DenseVector<double>(new double[] { 1.0, 6.5, 5.0 })];

                /// <summary>
                /// Provides the expected result of the subtraction : <c>V2-V1</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the subtraction. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Subtraction_V1() => [new Vect.DenseVector<double>(new double[] { -1.0, 7.5, -5.0 })];

                /// <summary>
                /// Provides the expected result of the unary negation : <c>-V2</c>
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the unary negation. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] UnaryNegation() => [new Vect.DenseVector<double>(new double[] { 0.0, -7.0, 0.0 })];


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
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the right scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RightMultiplication_T() => [3.0, new Vect.DenseVector<double>(new double[] { 0.0, 21.0, 0.0 })];

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
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the left scalar multiplication. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] LeftMultiplication_T() => [5.0, new Vect.DenseVector<double>(new double[] { 0.0, 35.0, 0.0 })];

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
                ///         <description> The <see cref="Vect.DenseVector{T}"/> resulting from the scalar division. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Division_T() => [4.0, new Vect.DenseVector<double>(new double[] { 0.0, 1.75, 0.0 })];

                //     -----     Public Methods

                /// <summary>
                /// Provides the information the set the value of a component.
                /// </summary>
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
                public static object[] SetValue() => [0, 1.25, new Vect.DenseVector<double>(new double[] { 1.25, 7.0, 0.0 })];

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a vector <see cref="V2"/>.
                /// </summary>
                /// <returns> The <see cref="Vect.DenseVector{T}"/>. </returns>
                private static Vect.DenseVector<double> CreateV2()
                {
                    return new Vect.DenseVector<double>(new double[] { 0.0, 7.0, 0.0 });
                }

                #endregion
            }

            /// <summary>
            /// Computes and stores general data related to general <see cref="Vect.DenseVector{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            internal static class V3
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored vector <see cref="V3"/>.
                /// </summary>
                private static readonly Vect.DenseVector<double> _v3 = CreateV3();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="Vect.DenseVector{T}"/> of type <see cref="V3"/>. </description>
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
                ///         <description> A newly computed <see cref="Vect.DenseVector{T}"/> of type <see cref="V3"/>. </description>
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
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of elements. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Size() => [6];

                /// <summary>
                /// Provides the expected values of each component of the vector.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item is :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An array of <see cref="double"/> representing the values of the vector. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Components() => [new double[] { 0.0, 5.0, 3.5, 0.0, 6.0, 2.0 }];

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a vector <see cref="V3"/>.
                /// </summary>
                /// <returns> The <see cref="Vect.DenseVector{T}"/>. </returns>
                private static Vect.DenseVector<double> CreateV3()
                {
                    return new Vect.DenseVector<double>(new double[] { 0.0, 5.0, 3.5, 0.0, 6.0, 2.0 });
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
            /// Class data for <see cref="DenseVector.Property_Size(Vect.DenseVector{double}, int)"/>.
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
            /// Class data for <see cref="DenseVector.Property_This_Int(Vect.DenseVector{double}, double[])"/>.
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
            /// Class data for <see cref="DenseVector.Constructor__Int(int)"/>.
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
            /// Class data for <see cref="DenseVector.Constructor__IListOfT(double[])"/>.
            /// </summary>
            internal class Constructor__IListOfT : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Components },
                        { DataStorages.V2.Components },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseVector.Constructor__DenseMatrix(Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Constructor__DenseVectorOfT : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable },
                        { DataStorages.V2.Readable },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseVector.Constructor__DenseMatrix(Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Constructor__SparseVectorOfT : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { SparseVector.DataStorages.V1.Readable, DataStorages.V1.Readable },
                        { SparseVector.DataStorages.V2.Readable, DataStorages.V2.Readable },
                    };
            }


            //     -----     Static

            /// <summary>
            /// Class data for <see cref="DenseVector.Operator__Addition__DenseVector_DenseVector(Vect.DenseVector{double}, Vect.DenseVector{double}, Vect.DenseVector{double})"/> and
            /// <see cref="DenseVector.Static__Add__DenseVector_DenseVector(Vect.DenseVector{double}, Vect.DenseVector{double}, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Addition__DenseVector_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V2.Readable, DataStorages.V1.Addition_V2 },
                        { DataStorages.V2.Readable, DataStorages.V1.Readable, DataStorages.V2.Addition_V1 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseVector.Operator__Subtraction__DenseVector_DenseVector(Vect.DenseVector{double}, Vect.DenseVector{double}, Vect.DenseVector{double})"/> and
            /// <see cref="DenseVector.Static__Add__DenseVector_DenseVector(Vect.DenseVector{double}, Vect.DenseVector{double}, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Subtraction__DenseVector_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V2.Readable, DataStorages.V1.Subtraction_V2 },
                        { DataStorages.V2.Readable, DataStorages.V1.Readable, DataStorages.V2.Subtraction_V1 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseVector.Operator__UnaryNegation__DenseVector(Vect.DenseVector{double}, Vect.DenseVector{double})"/> and
            /// <see cref="DenseVector.Static__Opposite__DenseVector(Vect.DenseVector{double}, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class UnaryNegation__DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.UnaryNegation },
                        { DataStorages.V2.Readable, DataStorages.V2.UnaryNegation },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseVector.Static__TransposeMultiply__DenseVector_DenseVector(Vect.DenseVector{double}, Vect.DenseVector{double}, double)"/>.
            /// </summary>
            internal class TransposeMultiplication__DenseVector_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V2.Readable, DataStorages.V1.TransposeMultiply_V2 },
                        { DataStorages.V2.Readable, DataStorages.V1.Readable, DataStorages.V2.TransposeMultiply_V1 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseVector.Operator__Multiplication__DenseVector_T(Vect.DenseVector{double}, double, Vect.DenseVector{double})"/> and
            /// <see cref="DenseVector.Static__Multiply__DenseVector_T(Vect.DenseVector{double}, double, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Multiplication__DenseVector_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.RightMultiplication_T },
                        { DataStorages.V2.Readable, DataStorages.V2.RightMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseVector.Operator__Multiplication__T_DenseVector(Vect.DenseVector{double}, double, Vect.DenseVector{double})"/> and
            /// <see cref="DenseVector.Static__Multiply__T_DenseVector(Vect.DenseVector{double}, double, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Multiplication__T_DenseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.LeftMultiplication_T },
                        { DataStorages.V2.Readable, DataStorages.V2.LeftMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseVector.Operator__Division__DenseVector_T(Vect.DenseVector{double}, double, Vect.DenseVector{double})"/> and
            /// <see cref="DenseVector.Static__Divide__DenseVector_T(Vect.DenseVector{double}, double, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Division__DenseVector_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, DataStorages.V1.Division_T },
                        { DataStorages.V2.Readable, DataStorages.V2.Division_T },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseVector.Operator__Addition__DenseVector_SparseVector(Vect.DenseVector{double}, Vect.SparseVector{double}, Vect.DenseVector{double})"/> and
            /// <see cref="DenseVector.Static__Add__DenseVector_SparseVector(Vect.DenseVector{double}, Vect.SparseVector{double}, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Addition__DenseVector_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, SparseVector.DataStorages.V2.Readable, DataStorages.V1.Addition_V2 },
                        { DataStorages.V2.Readable, SparseVector.DataStorages.V1.Readable, DataStorages.V2.Addition_V1 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DenseVector.Operator__Subtraction__DenseVector_SparseVector(Vect.DenseVector{double}, Vect.SparseVector{double}, Vect.DenseVector{double})"/> and
            /// <see cref="DenseVector.Static__Add__DenseVector_SparseVector(Vect.DenseVector{double}, Vect.SparseVector{double}, Vect.DenseVector{double})"/>.
            /// </summary>
            internal class Subtraction__DenseVector_SparseVector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Readable, SparseVector.DataStorages.V2.Readable, DataStorages.V1.Subtraction_V2 },
                        { DataStorages.V2.Readable, SparseVector.DataStorages.V1.Readable, DataStorages.V2.Subtraction_V1 },
                    };
            }


            //     -----     Methods

            /// <summary>
            /// Class data for <see cref="DenseVector.SetValue__Int_T(Vect.DenseVector{double}, int, double, Vect.DenseVector{double})"/>
            /// </summary>
            internal class SetValue__Int_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.V1.Writable, DataStorages.V1.SetValue },
                        { DataStorages.V2.Writable, DataStorages.V2.SetValue },
                    };
            }
        }

        #endregion
    }
}
