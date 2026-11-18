using System;
using System.Collections.Generic;
using System.Numerics;

namespace BRIDGES.Numerics.LinearAlgebra.Vectors.Operations
{
    internal static partial class VectorOperations
    {
        #region Subtraction

        /// <summary>
        /// Computes the subtraction of two vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Vector{T}"/> to subtract with. </param>
        /// <returns> The <see cref="Vector{T}"/> resulting from the subtraction. </returns>
        internal static Vector<T> Subtract<T>(Vector<T> left, Vector<T> right)
            where T : INumberBase<T>
        {
            return (left, right) switch
            {
                (DenseVector<T> denseLeft, DenseVector<T> denseRight) => Subtract(denseLeft, denseRight),
                (DenseVector<T> denseLeft, SparseVector<T> sparseRight) => Subtract(denseLeft, sparseRight),
                (SparseVector<T> sparseLeft, DenseVector<T> denseRight) => Subtract(sparseLeft, denseRight),
                (SparseVector<T> sparseLeft, SparseVector<T> sparseRight) => Subtract(sparseLeft, sparseRight),
                // Global fallback (to update with a default subtraction)
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the subtraction of two dense vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> to subtract with. </param>
        /// <returns> The <see cref="DenseVector{T}"/> resulting from the subtraction. </returns>
        internal static DenseVector<T> Subtract<T>(DenseVector<T> left, DenseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseVector<T> result = new DenseVector<T>(left.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                result[i] = left[i] - right[i];
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a dense vector with a sparse vector.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the subtraction. </returns>
        internal static DenseVector<T> Subtract<T>(DenseVector<T> left, SparseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseVector<T> result = new DenseVector<T>(left);
            foreach ((int index, T value) in right)
            {
                result[index] -= value;
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a sparse vector with a dense vector.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the subtraction. </returns>
        internal static DenseVector<T> Subtract<T>(SparseVector<T> left, DenseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseVector<T> result = -right;
            foreach ((int index, T leftValue) in left)
            {
                result[index] = leftValue - right[index];
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of two sparse vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> to subtract with. </param>
        /// <returns> The <see cref="SparseVector{T}"/> resulting from the subtraction. </returns>
        internal static SparseVector<T> Subtract<T>(SparseVector<T> left, SparseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            int capacity = left.NonZeroCount + right.NonZeroCount < left.RowCount ? left.NonZeroCount + right.NonZeroCount : left.RowCount;
            Dictionary<int, T> components = new Dictionary<int, T>(capacity);

            foreach ((int index, T leftValue) in left) { components.Add(index, leftValue); }

            foreach ((int index, T rightValue) in right)
            {
                if (components.TryGetValue(index, out T? leftValue))
                {
                    if (leftValue == rightValue) { components.Remove(index); }
                    else { components[index] = leftValue! - rightValue; }
                }
                else { components.Add(index, -rightValue); }
            }

            return new SparseVector<T>(left.RowCount, components); ;
        }

        #endregion
    }
}
