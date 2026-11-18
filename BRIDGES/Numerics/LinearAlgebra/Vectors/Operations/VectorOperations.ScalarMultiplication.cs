using System;
using System.Numerics;
using System.Collections.Generic;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors.Operations
{
    internal static partial class VectorOperations
    {
        #region Right Scalar Multiplication

        /// <summary>
        /// Computes the right scalar multiplication of a vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the right scalar multiplication. </returns>
        internal static Vector<T> Multiply<T>(Vector<T> operand, T factor)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseVector<T> denseOperand) => Multiply(denseOperand, factor),
                (SparseVector<T> sparseOperand) => Multiply(sparseOperand, factor),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the right scalar multiplication of a dense vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the right scalar multiplication. </returns>
        internal static DenseVector<T> Multiply<T>(DenseVector<T> operand, T factor)
            where T : INumberBase<T>
        {
            DenseVector<T> result = new DenseVector<T>(operand.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                result[i] = operand[i] * factor;
            }

            return result;
        }

        /// <summary>
        /// Computes the right scalar multiplication of a sparse vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> resulting from the right scalar multiplication. </returns>
        internal static SparseVector<T> Multiply<T>(SparseVector<T> operand, T factor)
            where T : INumberBase<T>
        {
            Dictionary<int, T> components = new Dictionary<int, T>(operand.NonZeroCount);
            if (factor != T.Zero)
            {
                foreach ((int index, T value) in operand)
                {
                    components.Add(index, value * factor);
                }
            }

            return new SparseVector<T>(operand.RowCount, components); ;
        }

        #endregion


        #region Left Scalar Multiplication

        /// <summary>
        /// Computes the left scalar multiplication of a vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector{T}"/> to multiply on the left. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the left scalar multiplication. </returns>
        internal static Vector<T> Multiply<T>(T factor, Vector<T> operand)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseVector<T> denseOperand) => Multiply(factor, denseOperand),
                (SparseVector<T> sparseOperand) => Multiply(factor, sparseOperand),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the left scalar multiplication of a dense vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="DenseVector{T}"/> to multiply on the left. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the left scalar multiplication. </returns>
        internal static DenseVector<T> Multiply<T>(T factor, DenseVector<T> operand)
            where T : INumberBase<T>
        {
            DenseVector<T> result = new DenseVector<T>(operand.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                result[i] = factor * operand[i];
            }

            return result;
        }

        /// <summary>
        /// Computes the left scalar multiplication of a sparse vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector{T}"/> to multiply on the left. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> resulting from the left scalar multiplication. </returns>
        internal static SparseVector<T> Multiply<T>(T factor, SparseVector<T> operand)
            where T : INumberBase<T>
        {
            Dictionary<int, T> components = new Dictionary<int, T>(operand.NonZeroCount);
            if (factor != T.Zero)
            {
                foreach ((int index, T value) in operand)
                {
                    components.Add(index, factor * value);
                }
            }

            return new SparseVector<T>(operand.RowCount, components); ;
        }

        #endregion
    }
}
