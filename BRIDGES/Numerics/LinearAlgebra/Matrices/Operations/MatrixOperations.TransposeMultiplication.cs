using System;
using System.Numerics;
using System.Collections.Generic;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Transpose Multiplication : Main

        /// <summary>
        /// Computes the multiplication of a transposed matrix with another matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the operation. </returns>
        internal static Matrix<T> TransposeMultiply<T>(Matrix<T> left, Matrix<T> right)
            where T : INumberBase<T>
        {
            return (left, right) switch
            {
                (DenseMatrix<T> denseLeft, DenseMatrix<T> denseRight) => TransposeMultiply(denseLeft, denseRight),
                (DenseMatrix<T> denseLeft, SparseMatrix<T> sparseRight) => TransposeMultiply(denseLeft, sparseRight),
                (SparseMatrix<T> sparseLeft, DenseMatrix<T> denseRight) => TransposeMultiply(sparseLeft, denseRight),
                (SparseMatrix<T> sparseLeft, SparseMatrix<T> sparseRight) => TransposeMultiply(sparseLeft, sparseRight),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with another dense matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> TransposeMultiply<T>(DenseMatrix<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureTransposeMultiplicationCompatibility(left, right);

            DenseMatrix<T> result = new DenseMatrix<T>(left.ColumnCount, right.ColumnCount);
            for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
            {
                for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                {
                    T component = default!;
                    for (int i_InitialRow = 0; i_InitialRow < right.RowCount; i_InitialRow++)
                    {
                        component += left[i_InitialRow, i_ResultRow] * right[i_InitialRow, i_ResultColumn];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with a sparse matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> TransposeMultiply<T>(DenseMatrix<T> left, SparseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureTransposeMultiplicationCompatibility(left, right);

            return (right.Storage) switch
            {
                (SStor.CompressedColumn<T> rightCCS) => TransposeMultiply(left, rightCCS),
                // Global fallback (to update with a default transpose multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a dense matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> TransposeMultiply<T>(SparseMatrix<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureTransposeMultiplicationCompatibility(left, right);

            return (left.Storage) switch
            {
                (SStor.CompressedColumn<T> leftCCS) => TransposeMultiply(leftCCS, right),
                // Global fallback (to update with a default transpose multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with another sparse matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the operation. </returns>
        internal static SparseMatrix<T> TransposeMultiply<T>(SparseMatrix<T> left, SparseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureTransposeMultiplicationCompatibility(left, right);

            return (left.Storage, right.Storage) switch
            {
                (SStor.CompressedColumn<T> leftCCS, SStor.CompressedColumn<T> rightCCS) => new SparseMatrix<T>(TransposeMultiply(leftCCS, rightCCS)),
                // CompressedColumn fallback
                // (SStor.CompressedColumn<T> leftCCS, _) => new SparseMatrix<T>(TransposeMultiply(leftCCS, right)),
                // Global fallback (to update with a default transpose multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Transpose Multiplication : SparseStorages

        private static DenseMatrix<T> TransposeMultiply<T>(DenseMatrix<T> left, SStor.CompressedColumn<T> right)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(left.ColumnCount, right.ColumnCount);
            for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
                {
                    T component = T.Zero;
                    for (int i_Right = right.ColumnPointers[i_ResultColumn]; i_Right < right.ColumnPointers[i_ResultColumn + 1]; i_Right++)
                    {
                        component += left[right.RowIndices[i_Right], i_ResultRow] * right.Values[i_Right];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        private static DenseMatrix<T> TransposeMultiply<T>(SStor.CompressedColumn<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(left.ColumnCount, right.ColumnCount);
            for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
                {
                    T component = T.Zero;
                    for (int i_Left = left.ColumnPointers[i_ResultRow]; i_Left < left.ColumnPointers[i_ResultRow + 1]; i_Left++)
                    {
                        component += left.Values[i_Left] * right[left.RowIndices[i_Left], i_ResultColumn];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        private static SStor.CompressedColumn<T> TransposeMultiply<T>(SStor.CompressedColumn<T> left, SStor.CompressedColumn<T> right)
            where T : INumberBase<T>
        {
            int resultRowCount = left.ColumnCount;
            int resultColumnCount = right.ColumnCount;

            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];

            resultColumnPointers[0] = 0;

            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < resultRowCount; i_ResultRow++)
                {
                    int i_Left = left.ColumnPointers[i_ResultRow];
                    int i_Right = right.ColumnPointers[i_ResultColumn];

                    T resultComponent = T.Zero;
                    while (i_Left < left.ColumnPointers[i_ResultRow + 1] && i_Right < right.ColumnPointers[i_ResultColumn + 1])
                    {
                        int leftRow = left.RowIndices[i_Left];
                        int rightRow = right.RowIndices[i_Right];

                        if (leftRow == rightRow)
                        {
                            resultComponent += left.Values[i_Left] * right.Values[i_Right];
                            i_Left++;
                            i_Right++;
                        }
                        else if (leftRow < rightRow)
                        {
                            i_Left++;
                        }
                        else
                        {
                            i_Right++;
                        }
                    }

                    if (resultComponent != T.Zero)
                    {
                        resultValues.Add(resultComponent);
                        resultRowIndices.Add(i_ResultRow);
                    }
                }

                resultColumnPointers[i_ResultColumn + 1] = resultValues.Count;
            }

            return new SStor.CompressedColumn<T>(resultRowCount, resultColumnCount, resultColumnPointers, resultRowIndices.ToArray(), resultValues.ToArray());
        }

        #endregion

    }
}
