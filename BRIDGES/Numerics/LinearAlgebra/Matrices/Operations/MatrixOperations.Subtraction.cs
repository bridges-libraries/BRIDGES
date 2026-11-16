using System;
using System.Numerics;
using System.Collections.Generic;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Subtraction : Main

        /// <summary>
        /// Computes the subtraction of two matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> to subtract with. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the subtraction. </returns>
        internal static Matrix<T> Subtract<T>(Matrix<T> left, Matrix<T> right)
            where T : INumberBase<T>
        {
            return (left, right) switch
            {
                (DenseMatrix<T> denseLeft, DenseMatrix<T> denseRight) => Subtract(denseLeft,denseRight),
                (DenseMatrix<T> denseLeft, SparseMatrix<T> sparseRight) => Subtract(denseLeft, sparseRight),
                (SparseMatrix<T> sparseLeft, DenseMatrix<T> denseRight) => Subtract(sparseLeft, denseRight),
                (SparseMatrix<T> sparseLeft, SparseMatrix<T> sparseRight) => Subtract(sparseLeft, sparseRight),
                // Global fallback (to update with a default subtraction)
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the subtraction of two dense matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to subtract with. </param>
        /// <returns> The <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        internal static DenseMatrix<T> Subtract<T>(DenseMatrix<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, left.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = left[i_Row, i_Column] - right[i_Row, i_Column];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a dense matrix with a sparse matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        internal static DenseMatrix<T> Subtract<T>(DenseMatrix<T> left, SparseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseMatrix<T> result = new DenseMatrix<T>(left);

            foreach ((int row, int column, T value) in right.Storage)
            {
                result[row, column] -= value;
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of a sparse matrix with a dense matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the subtraction. </returns>
        internal static DenseMatrix<T> Subtract<T>(SparseMatrix<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            DenseMatrix<T> result = -right;
            foreach ((int row, int column, T value) in left.Storage)
            {
                result[row, column] = value - right[row, column];
            }

            return result;
        }

        /// <summary>
        /// Computes the subtraction of two sparse matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to subtract with. </param>
        /// <returns> The <see cref="SparseMatrix{T}"/> resulting from the subtraction. </returns>
        internal static SparseMatrix<T> Subtract<T>(SparseMatrix<T> left, SparseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureSameDimensions(left, right);

            return (left.Storage, right.Storage) switch
            {
                (SStor.CompressedColumn<T> leftCCS, SStor.CompressedColumn<T> rightCCS) => new SparseMatrix<T>(Subtract(leftCCS, rightCCS)),
                // CompressedColumn fallback
                // (SStor.CompressedColumn<T> leftCCS, _) => new SparseMatrix<T>(Subtract(leftCCS, right)),
                // Global fallback (to update with a default addition)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Subtraction : SparseStorages

        private static SStor.CompressedColumn<T> Subtract<T>(SStor.CompressedColumn<T> left, SStor.CompressedColumn<T> right)
            where T : INumberBase<T>
        {
            int maxCapacity = left.Values.Count + right.Values.Count;

            List<T> resultValues = new List<T>(maxCapacity);
            List<int> resultRowIndices = new List<int>(maxCapacity);
            int[] resultColumnPointers = new int[left.ColumnCount + 1];

            for (int i_Column = 0; i_Column < left.ColumnCount; i_Column++)
            {
                int i_Left = left.ColumnPointers[i_Column];
                int i_Right = right.ColumnPointers[i_Column];

                while (i_Left < left.ColumnPointers[i_Column + 1] && i_Right < right.ColumnPointers[i_Column + 1])
                {
                    int leftRow = left.RowIndices[i_Left];
                    int rightRow = right.RowIndices[i_Right];

                    if (leftRow == rightRow)
                    {
                        T sum = left.Values[i_Left] - right.Values[i_Right];
                        if (sum != T.Zero)
                        {
                            resultValues.Add(sum);
                            resultRowIndices.Add(leftRow);
                        }
                        i_Left++;
                        i_Right++;
                    }
                    else if (leftRow < rightRow)
                    {
                        resultValues.Add(left.Values[i_Left]);
                        resultRowIndices.Add(leftRow);
                        i_Left++;
                    }
                    else
                    {
                        resultValues.Add(-right.Values[i_Right]);
                        resultRowIndices.Add(rightRow);
                        i_Right++;
                    }
                }

                while (i_Left < left.ColumnPointers[i_Column + 1])
                {
                    resultValues.Add(left.Values[i_Left]);
                    resultRowIndices.Add(left.RowIndices[i_Left]);
                    i_Left++;
                }

                while (i_Right < right.ColumnPointers[i_Column + 1])
                {
                    resultValues.Add(right.Values[i_Right]);
                    resultRowIndices.Add(right.RowIndices[i_Right]);
                    i_Right++;
                }

                resultColumnPointers[i_Column + 1] = resultValues.Count;
            }

            return new SStor.CompressedColumn<T>(left.RowCount, left.ColumnCount, resultColumnPointers, resultRowIndices.ToArray(), resultValues.ToArray());
        }

        #endregion
    }
}
