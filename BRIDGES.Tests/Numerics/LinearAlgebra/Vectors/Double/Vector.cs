using System;

using Xunit;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Vectors.Double
{
    /// <summary>
    /// Tests the members of the <see cref=Vect.Vector{T}"/> class.
    /// </summary>
    public class Vector
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Vect.Vector{T}.RowCount"/>.
        /// </summary>
        /// <param name="vector"> Vector to evaluate. </param>
        /// <param name="expected"> Expected size of the vector. </param>
        [Theory(DisplayName = "Prop. Size")]
        [ClassData(typeof(ClassDatas.Size))]
        public void Property_Size(Vect.Vector<double> vector, int expected)
        {
            // Arrange

            //Act
            int size = vector.RowCount;

            // Assert
            Assert.Equal(expected, size);
        }

        #endregion

        #region Tests : Public Static Methods

        //     -----     -----     Additive Abelian Group : Vector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.Add(Vect.Vector{T},Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.Vector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.Vector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Static Add(Vector,Vector)")]
        [ClassData(typeof(ClassDatas.Addition__Vector_Vector))]
        public void Static__Add__Vector_Vector(Vect.Vector<double> left, Vect.Vector<double> right, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = Vect.Vector<double>.Add(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.Subtract(Vect.Vector{T},Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.Vector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.Vector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Static Subtract(Vector,Vector)")]
        [ClassData(typeof(ClassDatas.Subtraction__Vector_Vector))]
        public void Static__Subtract__Vector_Vector(Vect.Vector<double> left, Vect.Vector<double> right, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = Vect.Vector<double>.Subtract(left, right);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.Opposite(Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.Vector{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Static Opposite(Vector)")]
        [ClassData(typeof(ClassDatas.UnaryNegation__Vector))]
        public void Static__Opposite__Vector(Vect.Vector<double> operand, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = Vect.Vector<double>.Opposite(operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }


        //     -----     Other Operations : Vector<T>     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.TransposeMultiply(Vect.Vector{T},Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.Vector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Vect.Vector{T}"/> to multiply. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the multiplication. </param>
        [Theory(DisplayName = "Static TransposeMultiply(Vector,Vector)")]
        [ClassData(typeof(ClassDatas.TransposeMultiplication__Vector_Vector))]
        public void Static__TransposeMultiply__Vector_Vector(Vect.Vector<double> left, Vect.Vector<double> right, double expected)
        {
            // Arrange

            // Act 
            double result = Vect.Vector<double>.TransposeMultiply(left, right);

            // Assert
            Assert.Equal(expected, result);
        }



        //     -----      -----     Group Action : T     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.Multiply(Vect.Vector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.Vector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(Vector,T)")]
        [ClassData(typeof(ClassDatas.Multiplication__Vector_T))]
        public void Static__Multiply__Vector_T(Vect.Vector<double> operand, double factor, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = Vect.Vector<double>.Multiply(operand, factor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.Multiply(T,Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Vect.Vector{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Static Multiply(T,Vector)")]
        [ClassData(typeof(ClassDatas.Multiplication__T_Vector))]
        public void Static__Multiply__T_Vector(Vect.Vector<double> operand, double factor, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = Vect.Vector<double>.Multiply(factor, operand);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.Divide(Vect.Vector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.Vector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the division. </param>
        [Theory(DisplayName = "Static Divide(Vector,T)")]
        [ClassData(typeof(ClassDatas.Division__Vector_T))]
        public void Static__Divide__Vector_T(Vect.Vector<double> operand, double divisor, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = Vect.Vector<double>.Divide(operand, divisor);

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }

        #endregion

        #region Tests : Operators

        //     -----     -----     Additive Abelian Group : Vector<T>     -----     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.operator+(Vect.Vector{T},Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.Vector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vect.Vector{T}"/> for the addition. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the addition. </param>
        [Theory(DisplayName = "Op + (Vector,Vector)")]
        [ClassData(typeof(ClassDatas.Addition__Vector_Vector))]
        public void Operator__Addition__Vector_Vector(Vect.Vector<double> left, Vect.Vector<double> right, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = left + right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.operator-(Vect.Vector{T},Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vect.Vector{T}"/> for the subtraction. </param>
        /// <param name="right"> Right <see cref="Vect.Vector{T}"/> for the subtraction. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the subtraction. </param>
        [Theory(DisplayName = "Op - (Vector,Vector)")]
        [ClassData(typeof(ClassDatas.Subtraction__Vector_Vector))]
        public void Operator__Subtraction__Vector_Vector(Vect.Vector<double> left, Vect.Vector<double> right, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = left - right;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.operator-(Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.Vector{T}"/> to operate from. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the unary negation. </param>
        [Theory(DisplayName = "Op - (Vector)")]
        [ClassData(typeof(ClassDatas.UnaryNegation__Vector))]
        public void Operator__UnaryNegation__Vector(Vect.Vector<double> operand, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = -operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }


        //     -----      Group Action : T     -----     //

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.operator*(Vect.Vector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.Vector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the right scalar multiplication. </param>
        [Theory(DisplayName = "Op * (Vector,T)")]
        [ClassData(typeof(ClassDatas.Multiplication__Vector_T))]
        public void Operator__Multiplication__Vector_T(Vect.Vector<double> operand, double factor, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = operand * factor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }

        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.operator*(T,Vect.Vector{T})"/>.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="Vect.Vector{T}"/> to multiply on the left. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the left scalar multiplication. </param>
        [Theory(DisplayName = "Op * (T,Vector)")]
        [ClassData(typeof(ClassDatas.Multiplication__T_Vector))]
        public void Operator__Multiplication__T_Vector(Vect.Vector<double> operand, double factor, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = factor * operand;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }


        /// <summary>
        /// Tests the method <see cref="Vect.Vector{T}.operator/(Vect.Vector{T},T)"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Vect.Vector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <param name="expected"> Expected result <see cref="Vect.Vector{T}"/> of the division. </param>
        [Theory(DisplayName = "Op / (Vector,T)")]
        [ClassData(typeof(ClassDatas.Division__Vector_T))]
        public void Operator__Division__Vector_T(Vect.Vector<double> operand, double divisor, Vect.Vector<double> expected)
        {
            // Arrange

            // Act 
            Vect.Vector<double> result = operand / divisor;

            // Assert
            Assert.Equal(expected.RowCount, result.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                Assert.Equal(expected.GetComponent(i), result.GetComponent(i));
            }
        }

        #endregion


        #region Data Classes for Parametrised Tests

        internal static class ClassDatas
        {
            //     -----     Properties

            /// <summary>
            /// Class data for <see cref="Vector.Property_Size(Vect.Vector{double}, int)"/>.
            /// </summary>
            internal class Size : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V1.Size },
                        { DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V2.Size },

                        { SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V1.Size },
                        { SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V2.Size },
                    };
            }


            //     -----     Methods

            /// <summary>
            /// Class data for <see cref="Vector.Operator__Addition__Vector_Vector(Vect.Vector{double}, Vect.Vector{double}, Vect.Vector{double})"/> and
            /// <see cref="Vector.Static__Add__Vector_Vector(Vect.Vector{double}, Vect.Vector{double}, Vect.Vector{double})"/>.
            /// </summary>
            internal class Addition__Vector_Vector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Addition_V2 },
                        { DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Addition_V1 },

                        { SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.Addition_V2 },
                        { SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.Addition_V1 },

                        { DenseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Addition_V2 },
                        { DenseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Addition_V1 },

                        { SparseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Addition_V2 },
                        { SparseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Addition_V1 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Vector.Operator__Subtraction__Vector_Vector(Vect.Vector{double}, Vect.DenseVector{double}, Vect.Vector{double})"/> and
            /// <see cref="Vector.Static__Add__Vector_Vector(Vect.Vector{double}, Vect.Vector{double}, Vect.Vector{double})"/>.
            /// </summary>
            internal class Subtraction__Vector_Vector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Subtraction_V2 },
                        { DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Subtraction_V1 },

                        { SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.Subtraction_V2 },
                        { SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.Subtraction_V1 },

                        { DenseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Subtraction_V2 },
                        { DenseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Subtraction_V1 },

                        { SparseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Subtraction_V2 },
                        { SparseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Subtraction_V1 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Vector.Operator__UnaryNegation__Vector(Vect.Vector{double}, Vect.Vector{double})"/> and
            /// <see cref="Vector.Static__Opposite__Vector(Vect.Vector{double}, Vect.Vector{double})"/>.
            /// </summary>
            internal class UnaryNegation__Vector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V1.UnaryNegation },
                        { DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V2.UnaryNegation },

                        { SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V1.UnaryNegation },
                        { SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V2.UnaryNegation },
                    };
            }


            /// <summary>
            /// Class data for <see cref="DenseVector.Static__TransposeMultiply__Vector_Vector(Vect.Vector{double}, Vect.Vector{double}, double)"/>.
            /// </summary>
            internal class TransposeMultiplication__Vector_Vector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.TransposeMultiply_V2 },
                        { DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.TransposeMultiply_V1 },

                        { SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.TransposeMultiply_V2 },
                        { SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.TransposeMultiply_V1 },

                        { DenseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.TransposeMultiply_V2 },
                        { DenseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.TransposeMultiply_V1 },

                        { SparseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.TransposeMultiply_V2 },
                        { SparseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V2.TransposeMultiply_V1 },
                    };
            }


            /// <summary>
            /// Class data for <see cref="Vector.Operator__Multiplication__Vector_T(Vect.Vector{double}, double, Vect.Vector{double})"/> and
            /// <see cref="Vector.Static__Multiply__Vector_T(Vect.Vector{double}, double, Vect.Vector{double})"/>.
            /// </summary>
            internal class Multiplication__Vector_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V1.RightMultiplication_T },
                        { DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V2.RightMultiplication_T },

                        { SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V1.RightMultiplication_T },
                        { SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V2.RightMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Vector.Operator__Multiplication__T_Vector(Vect.Vector{double}, double, Vect.Vector{double})"/> and
            /// <see cref="Vector.Static__Multiply__T_Vector(Vect.Vector{double}, double, Vect.Vector{double})"/>.
            /// </summary>
            internal class Multiplication__T_Vector : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V1.LeftMultiplication_T },
                        { DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V2.LeftMultiplication_T },

                        { SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V1.LeftMultiplication_T },
                        { SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V2.LeftMultiplication_T },
                    };
            }

            /// <summary>
            /// Class data for <see cref="Vector.Operator__Division__Vector_T(Vect.Vector{double}, double, Vect.Vector{double})"/> and
            /// <see cref="Vector.Static__Divide__Vector_T(Vect.Vector{double}, double, Vect.Vector{double})"/>.
            /// </summary>
            internal class Division__Vector_T : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DenseVector.DataStorages.V1.Readable, DenseVector.DataStorages.V1.Division_T },
                        { DenseVector.DataStorages.V2.Readable, DenseVector.DataStorages.V2.Division_T },

                        { SparseVector.DataStorages.V1.Readable, SparseVector.DataStorages.V1.Division_T },
                        { SparseVector.DataStorages.V2.Readable, SparseVector.DataStorages.V2.Division_T },
                    };
            }

        }

        #endregion
    }
}
