using System;
using System.Collections.Generic;
using System.Numerics;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors.Operations
{
    internal static partial class VectorOperations
    {
        #region Addition

        /// <summary>
        /// Computes the addition of two vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Vector{T}"/> for the addition. </param>
        /// <returns> The <see cref="Vector{T}"/> resulting from the addition. </returns>
        internal static Vector<T> Add<T>(Vector<T> left, Vector<T> right)
            where T : INumberBase<T>
        {
            return (left, right) switch
            {
                (DenseVector<T> denseLeft, DenseVector<T> denseRight) => Add(denseLeft, denseRight),
                (DenseVector<T> denseLeft, SparseVector<T> sparseRight) => Add(denseLeft, sparseRight),
                (SparseVector<T> sparseLeft, DenseVector<T> denseRight) => Add(sparseLeft, denseRight),
                (SparseVector<T> sparseLeft, SparseVector<T> sparseRight) => Add(sparseLeft, sparseRight),
                // Global fallback (to update with a default addition)
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the addition of two dense vectors.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> for the addition. </param>
        /// <returns> The <see cref="DenseVector{T}"/> resulting from the addition. </returns>
        internal static DenseVector<T> Add<T>(DenseVector<T> left, DenseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseVector<T> result = new DenseVector<T>(left.RowCount);
            for (int i = 0; i < result.RowCount; i++)
            {
                result[i] = left[i] + right[i];
            }

            return result;
        }

        /// <summary>
        /// Computes the addition of a dense vector with a sparse vector.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the addition. </returns>
        internal static DenseVector<T> Add<T>(DenseVector<T> left, SparseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseVector<T> result = new DenseVector<T>(left);
            foreach ((int index, T value) in right)
            {
                result[index] += value;
            }

            return result;
        }

        /// <summary>
        /// Computes the addition of a sparse vector with a dense vector.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> for the addition. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the addition. </returns>
        internal static DenseVector<T> Add<T>(SparseVector<T> left, DenseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseVector<T> result = new DenseVector<T>(right);
            foreach ((int index, T leftValue) in left)
            {
                result[index] = leftValue + right[index];
            }

            return result;
        }

        /// <summary>
        /// Computes the addition of two sparse vector.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> for the addition. </param>
        /// <returns> The <see cref="SparseVector{T}"/> resulting from the addition. </returns>
        internal static SparseVector<T> Add<T>(SparseVector<T> left, SparseVector<T> right)
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
                    if (leftValue == -rightValue) { components.Remove(index); }
                    else { components[index] = leftValue! + rightValue; }
                }
                else { components.Add(index, rightValue); }
            }

            return new SparseVector<T>(left.RowCount, components); ;
        }

        #endregion
    }
}
