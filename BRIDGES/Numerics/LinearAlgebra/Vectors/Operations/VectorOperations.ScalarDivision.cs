using System;
using System.Collections.Generic;
using System.Numerics;

namespace BRIDGES.Numerics.LinearAlgebra.Vectors.Operations
{
    internal static partial class VectorOperations
    {
        #region Right Scalar Division

        /// <summary>
        /// Computes the scalar division of a vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Vector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the scalar division. </returns>
        internal static Vector<T> Divide<T>(Vector<T> operand, T divisor)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseVector<T> denseOperand) => Divide(denseOperand, divisor),
                (SparseVector<T> sparseOperand) => Divide(sparseOperand, divisor),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the scalar division of a dense vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the scalar division. </returns>
        internal static DenseVector<T> Divide<T>(DenseVector<T> operand, T divisor)
            where T : INumberBase<T>
        {
            DenseVector<T> result = new DenseVector<T>(operand.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                result[i] = operand[i] / divisor;
            }

            return result;
        }

        /// <summary>
        /// Computes the scalar division of a sparse vector with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> resulting from the scalar division. </returns>
        internal static SparseVector<T> Divide<T>(SparseVector<T> operand, T divisor)
            where T : INumberBase<T>
        {
            Dictionary<int, T> components = new Dictionary<int, T>(operand.NonZeroCount);
            foreach ((int index, T value) in operand)
            {
                components.Add(index, value / divisor);
            }

            return new SparseVector<T>(operand.RowCount, components); ;
        }

        #endregion
    }
}
