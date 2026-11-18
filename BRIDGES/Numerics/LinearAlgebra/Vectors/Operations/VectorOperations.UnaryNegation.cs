using System;
using System.Collections.Generic;
using System.Numerics;

namespace BRIDGES.Numerics.LinearAlgebra.Vectors.Operations
{
    internal static partial class VectorOperations
    {
        #region UnaryNegation

        /// <summary>
        /// Computes the unary negation of a matrix.
        /// </summary>
        /// <param name="operand"> <see cref="Vector{T}"/> to operate from. </param>
        /// <returns> The <see cref="Vector{T}"/> resulting from the unary negation. </returns>
        internal static Vector<T> Negate<T>(Vector<T> operand)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseVector<T> denseOperand) => Negate(denseOperand),
                (SparseVector<T> sparseOperand) => Negate(sparseOperand),
                // Global fallback (to update with a default unary negation)
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the unary negation of a dense matrix.
        /// </summary>
        /// <param name="operand"> <see cref="DenseVector{T}"/> to operate from. </param>
        /// <returns> The <see cref="DenseVector{T}"/> resulting from the unary negation. </returns>
        internal static DenseVector<T> Negate<T>(DenseVector<T> operand)
            where T : INumberBase<T>
        {
            DenseVector<T> result = new DenseVector<T>(operand.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                result[i] = -operand[i];
            }

            return result;
        }

        /// <summary>
        /// Computes the unary negation of a sparse matrix.
        /// </summary>
        /// <param name="operand"> <see cref="SparseVector{T}"/> to operate from. </param>
        /// <returns> The <see cref="SparseVector{T}"/> resulting from the unary negation. </returns>
        internal static SparseVector<T> Negate<T>(SparseVector<T> operand)
            where T : INumberBase<T>
        {
            Dictionary<int,T> components = new Dictionary<int,T>(operand.NonZeroCount);
            foreach ((int index, T value) in operand)
            {
                components.Add(index, -value);
            }

            return new SparseVector<T>(operand.RowCount, components); ;
        }

        #endregion
    }
}
