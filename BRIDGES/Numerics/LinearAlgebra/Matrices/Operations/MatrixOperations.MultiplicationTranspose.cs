using System;
using System.Numerics;
using System.Collections.Generic;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Multiplication Transpose : Main

        /// <summary>
        /// Computes the multiplication of a matrix with another transposed matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the operation. </returns>
        internal static Matrix<T> MultiplyTranspose<T>(Matrix<T> left, Matrix<T> right)
            where T : INumberBase<T>
        {
            return (left, right) switch
            {
                (DenseMatrix<T> denseLeft, DenseMatrix<T> denseRight) => MultiplyTranspose(denseLeft, denseRight),
                (DenseMatrix<T> denseLeft, SparseMatrix<T> sparseRight) => MultiplyTranspose(denseLeft, sparseRight),
                (SparseMatrix<T> sparseLeft, DenseMatrix<T> denseRight) => MultiplyTranspose(sparseLeft, denseRight),
                (SparseMatrix<T> sparseLeft, SparseMatrix<T> sparseRight) => MultiplyTranspose(sparseLeft, sparseRight),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the multiplication of a dense matrix with another transposed dense matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> MultiplyTranspose<T>(DenseMatrix<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationTransposeCompatibility(left, right);


            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, right.RowCount);
            for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
            {
                for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                {
                    T component = default!;
                    for (int i_InitialColumn = 0; i_InitialColumn < left.ColumnCount; i_InitialColumn++)
                    {
                        component += left[i_ResultRow, i_InitialColumn] * right[i_ResultColumn, i_InitialColumn];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a dense matrix with a transposed sparse matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> MultiplyTranspose<T>(DenseMatrix<T> left, SparseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationTransposeCompatibility(left, right);

            return (right.Storage) switch
            {
                (SStor.CompressedColumn<T> rightCCS) => MultiplyTranspose(left, rightCCS),
                // Global fallback (to update with a default multiplication transpose)
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the multiplication of a sparse matrix with a transposed dense matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        internal static DenseMatrix<T> MultiplyTranspose<T>(SparseMatrix<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationTransposeCompatibility(left, right);

            return (left.Storage) switch
            {
                (SStor.CompressedColumn<T> leftCCS) => MultiplyTranspose(leftCCS, right),
                // Global fallback (to update with a default multiplication transpose)
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the multiplication of a sparse matrix with another transposed sparse matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the operation. </returns>
        internal static SparseMatrix<T> MultiplyTranspose<T>(SparseMatrix<T> left, SparseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationTransposeCompatibility(left, right);

            return (left.Storage, right.Storage) switch
            {
                (SStor.CompressedColumn<T> leftCCS, SStor.CompressedColumn<T> rightCCS) => new SparseMatrix<T>(MultiplyTranspose(leftCCS, rightCCS)),
                // CompressedColumn fallback
                // (SStor.CompressedColumn<T> leftCCS, _) => new SparseMatrix<T>(MultiplyTranspose(leftCCS, right)),
                // Global fallback (to update with a default multiplication transpose)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Multiplication Transpose : SparseStorages

        private static DenseMatrix<T> MultiplyTranspose<T>(DenseMatrix<T> left, SStor.CompressedColumn<T> right)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, right.RowCount);
            for (int i_RightColumn = 0; i_RightColumn < right.ColumnCount; i_RightColumn++)
            {
                for (int i_Right = right.ColumnPointers[i_RightColumn]; i_Right < right.ColumnPointers[i_RightColumn + 1]; i_Right++)
                {
                    T rightComponent = right.Values[i_Right];
                    int i_ResultColumn = right.RowIndices[i_Right];
                    for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
                    {
                        result[i_ResultRow, i_ResultColumn] += left[i_ResultRow, i_RightColumn] * rightComponent;
                    }
                }
            }

            return result;
        }

        private static DenseMatrix<T> MultiplyTranspose<T>(SStor.CompressedColumn<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, right.RowCount);
            for (int i_LeftColumn = 0; i_LeftColumn < left.ColumnCount; i_LeftColumn++)
            {
                for (int i_Left = left.ColumnPointers[i_LeftColumn]; i_Left < left.ColumnPointers[i_LeftColumn + 1]; i_Left++)
                {
                    T i_LeftComponent = left.Values[i_Left];
                    int i_ResultRow = left.RowIndices[i_Left];

                    for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                    {
                        result[i_ResultRow, i_ResultColumn] += i_LeftComponent * right[i_ResultColumn, i_LeftColumn];
                    }
                }
            }

            return result;
        }

        private static SStor.CompressedColumn<T> MultiplyTranspose<T>(SStor.CompressedColumn<T> left, SStor.CompressedColumn<T> right)
            where T : INumberBase<T>
        {
            int resultRowCount = left.RowCount;
            int resultColumnCount = right.RowCount;

            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];


            bool[] isValid = new bool[right.ColumnCount];
            int[] i_Right_ByColumn = new int[right.ColumnCount];
            for (int i_RightColumn = 0; i_RightColumn < right.ColumnCount; i_RightColumn++)
            {
                isValid[i_RightColumn] = true;
                i_Right_ByColumn[i_RightColumn] = right.ColumnPointers[i_RightColumn];
            }


            T[] tmp_ResultColumn = new T[resultRowCount];
            bool[] tmp_IsUsed = new bool[resultRowCount];


            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++)
            {
                Array.Clear(tmp_ResultColumn, 0, resultRowCount);
                Array.Clear(tmp_IsUsed, 0, resultRowCount);

                for (int i = 0; i < right.ColumnCount; i++)
                {
                    if (isValid[i] && i_ResultColumn != right.RowIndices[i_Right_ByColumn[i]]) { continue; }

                    T rightComponent = right.Values[i_Right_ByColumn[i]];

                    for (int i_Left = left.ColumnPointers[i]; i_Left < left.ColumnPointers[i + 1]; i_Left++)
                    {
                        int leftRow = left.RowIndices[i_Left];
                        T leftComponent = left.Values[i_Left];

                        if (!tmp_IsUsed[leftRow])
                        {
                            tmp_ResultColumn[leftRow] = leftComponent * rightComponent;
                            tmp_IsUsed[leftRow] = true;
                        }
                        else
                        {
                            tmp_ResultColumn[leftRow] += leftComponent * rightComponent;
                        }
                    }
                    i_Right_ByColumn[i]++;
                    if (i_Right_ByColumn[i] == left.ColumnPointers[i + 1]) { isValid[i] = false; }
                }


                for (int i = 0; i < resultRowCount; i++)
                {
                    if (tmp_IsUsed[i] && tmp_ResultColumn[i] != T.Zero)
                    {
                        resultValues.Add(tmp_ResultColumn[i]);
                        resultRowIndices.Add(i);
                    }
                }

                resultColumnPointers[i_ResultColumn + 1] = resultValues.Count;
            }

            return new SStor.CompressedColumn<T>(resultRowCount, resultColumnCount, resultColumnPointers,  resultRowIndices.ToArray(), resultValues.ToArray());
        }

        #endregion
    }
}
