using System;
using System.Numerics;


namespace BRIDGES.Numerics.LinearAlgebra.Vectors.Operations
{
    internal static partial class VectorOperations
    {
        #region Transpose Multiplication

        /// <summary>
        /// Computes the multiplication of a transposed vector with another vector : <c>V<sup>t</sup>·W</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="Vector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Vector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vector{T}"/> resulting from the operation. </returns>
        internal static T TransposeMultiply<T>(Vector<T> left, Vector<T> right)
            where T : INumberBase<T>
        {
            return (left, right) switch
            {
                (DenseVector<T> denseLeft, DenseVector<T> denseRight) => TransposeMultiply(denseLeft, denseRight),
                (DenseVector<T> denseLeft, SparseVector<T> sparseRight) => TransposeMultiply(denseLeft, sparseRight),
                (SparseVector<T> sparseLeft, DenseVector<T> denseRight) => TransposeMultiply(sparseLeft, denseRight),
                (SparseVector<T> sparseLeft, SparseVector<T> sparseRight) => TransposeMultiply(sparseLeft, sparseRight),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }

    
        /// <summary>
        /// Computes the multiplication of a transposed dense vector with another dense vector : <c>V<sup>t</sup>·W</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the operation. </returns>
        internal static T TransposeMultiply<T>(DenseVector<T> left, DenseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            int size = left.RowCount;

            T result = T.Zero;
            for (int i = 0; i < size; i++)
            {
                result += left[i] * right[i];
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a transposed dense vector with a sparse vector : <c>V<sup>t</sup>·W</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the operation. </returns>
        internal static T TransposeMultiply<T>(DenseVector<T> left, SparseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            T result = T.Zero;
            foreach ((int index, T value) in right)
            {
                result += left[index] * value;
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a transposed sparse vector with a dense vector : <c>V<sup>t</sup>·W</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseVector{T}"/> resulting from the operation. </returns>
        internal static T TransposeMultiply<T>(SparseVector<T> left, DenseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            T result = T.Zero;
            foreach ((int index, T leftValue) in left)
            {
                result += leftValue * right[index];
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a transposed sparse vector with another sparse vector : <c>V<sup>t</sup>·W</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseVector{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="SparseVector{T}"/> resulting from the operation. </returns>
        internal static T TransposeMultiply<T>(SparseVector<T> left, SparseVector<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            T result = T.Zero;
            foreach ((int index, T leftValue) in left)
            {
                if (right.TryGet(index, out T? rightValue)) { result += leftValue * rightValue!; }
            }

            return result;
        }

        #endregion
    }
}
