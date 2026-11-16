using System;
using System.Numerics;
using System.Collections.Generic;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Multiplication : Main

        /// <summary>
        /// Computes the multiplication of two matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the multiplication. </returns>
        internal static Matrix<T> Multiply<T>(Matrix<T> left, Matrix<T> right)
            where T : INumberBase<T>
        {
            return (left, right) switch
            {
                (DenseMatrix<T> denseLeft, DenseMatrix<T> denseRight) => Multiply(denseLeft,denseRight),
                (DenseMatrix<T> denseLeft, SparseMatrix<T> sparseRight) => Multiply(denseLeft, sparseRight),
                (SparseMatrix<T> sparseLeft, DenseMatrix<T> denseRight) => Multiply(sparseLeft, denseRight),
                (SparseMatrix<T> sparseLeft, SparseMatrix<T> sparseRight) => Multiply(sparseLeft, sparseRight),
                // Global fallback (to update with default multiplication)
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the multiplication of two dense matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        internal static DenseMatrix<T> Multiply<T>(DenseMatrix<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationCompatibility(left, right);

            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, right.ColumnCount);
            for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
            {
                for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                {
                    T component = default!;
                    for (int i_InitialColumn = 0; i_InitialColumn < left.ColumnCount; i_InitialColumn++)
                    {
                        component += left[i_ResultRow, i_InitialColumn] * right[i_InitialColumn, i_ResultColumn];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a dense matrix with a sparse matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        internal static DenseMatrix<T> Multiply<T>(DenseMatrix<T> left, SparseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationCompatibility(left, right);

            return (right.Storage) switch
            {
                (SStor.CompressedColumn<T> rightCCS) => Multiply(left, rightCCS),
                // Global fallback (to update with a default multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the right multiplication of a sparse matrix with a dense matrix.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the multiplication. </returns>
        internal static DenseMatrix<T> Multiply<T>(SparseMatrix<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationCompatibility(left, right);

            return (left.Storage) switch
            {
                (SStor.CompressedColumn<T> leftCCS) => Multiply(leftCCS, right),
                // Global fallback (to update with a default multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the multiplication of two sparse matrices.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> for the multiplication. </param>
        /// <returns> The <see cref="SparseMatrix{T}"/> resulting from the multiplication. </returns>
        internal static SparseMatrix<T> Multiply<T>(SparseMatrix<T> left, SparseMatrix<T> right)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationCompatibility(left, right);

            return (left.Storage, right.Storage) switch
            {
                (SStor.CompressedColumn<T> leftCCS, SStor.CompressedColumn<T> rightCCS) => new SparseMatrix<T>(Multiply(leftCCS, rightCCS)),
                // CompressedColumn fallback
                // (SStor.CompressedColumn<T> leftCCS, _) => new SparseMatrix<T>(Multiply(leftCCS, right)),
                // Global fallback (to update with a default multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Multiplication : SparseStorages

        private static DenseMatrix<T> Multiply<T>(DenseMatrix<T> left, SStor.CompressedColumn<T> right)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, right.ColumnCount);
            for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
            {
                for (int i_ResultRow = 0; i_ResultRow < result.RowCount; i_ResultRow++)
                {
                    T component = T.Zero;
                    for (int i_Right = right.ColumnPointers[i_ResultColumn]; i_Right < right.ColumnPointers[i_ResultColumn + 1]; i_Right++)
                    {
                        component += left[i_ResultRow, right.RowIndices[i_Right]] * right.Values[i_Right];
                    }
                    result[i_ResultRow, i_ResultColumn] = component;
                }
            }

            return result;
        }

        private static DenseMatrix<T> Multiply<T>(SStor.CompressedColumn<T> left, DenseMatrix<T> right)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(left.RowCount, right.ColumnCount);
            for (int i_LeftColumn = 0; i_LeftColumn < left.ColumnCount; i_LeftColumn++)
            {
                for (int i_Left = left.ColumnPointers[i_LeftColumn]; i_Left < left.ColumnPointers[i_LeftColumn + 1]; i_Left++)
                {
                    T leftComponent = left.Values[i_Left];
                    int i_ResultRow = left.RowIndices[i_Left];

                    for (int i_ResultColumn = 0; i_ResultColumn < result.ColumnCount; i_ResultColumn++)
                    {
                        result[i_ResultRow, i_ResultColumn] += leftComponent * right[i_LeftColumn, i_ResultColumn];
                    }
                }
            }

            return result;
        }

        private static SStor.CompressedColumn<T> Multiply<T>(SStor.CompressedColumn<T> left, SStor.CompressedColumn<T> right)
            where T : INumberBase<T>
        {
            int resultRowCount = left.RowCount;
            int resultColumnCount = right.ColumnCount;
            List<T> resultValues = new List<T>();
            List<int> resultRowIndices = new List<int>();
            int[] resultColumnPointers = new int[resultColumnCount + 1];


            T[] tmp_ResultColumn = new T[resultRowCount];
            bool[] tmp_IsUsed = new bool[resultRowCount];

            for (int i_ResultColumn = 0; i_ResultColumn < resultColumnCount; i_ResultColumn++)
            {
                Array.Clear(tmp_ResultColumn, 0, resultRowCount);
                Array.Clear(tmp_IsUsed, 0, resultRowCount);

                for (int i_Right = right.ColumnPointers[i_ResultColumn]; i_Right < right.ColumnPointers[i_ResultColumn + 1]; i_Right++)
                {
                    int rightRow = right.RowIndices[i_Right];
                    T rightComponent = right.Values[i_Right];

                    for (int i_Left = left.ColumnPointers[rightRow]; i_Left < left.ColumnPointers[rightRow + 1]; i_Left++)
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

            return new SStor.CompressedColumn<T>(resultRowCount, resultColumnCount, resultColumnPointers, resultRowIndices.ToArray(), resultValues.ToArray());
        }
        
        #endregion
    }
}
