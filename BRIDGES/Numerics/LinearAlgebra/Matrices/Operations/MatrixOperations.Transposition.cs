using System;
using System.Numerics;
using System.Collections.Generic;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;

namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Transposition : Main

        /// <summary>
        /// Computes the transposition of a matrix.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the transposition. </returns>
        internal static Matrix<T> Transposition<T>(Matrix<T> operand)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseMatrix<T> denseOperand) => Transposition(denseOperand),
                (SparseMatrix<T> sparseOperand) => Transposition(sparseOperand),
                // Global fallback (to update with a default tranposition)
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the transposition of a dense matrix.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="DenseMatrix{T}"/> resulting from the transposition. </returns>
        internal static DenseMatrix<T> Transposition<T>(DenseMatrix<T> operand)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.ColumnCount, operand.RowCount);
            for (int i_InitialRow = 0; i_InitialRow < operand.RowCount; i_InitialRow++)
            {
                for (int i_InitialColumn = 0; i_InitialColumn < operand.ColumnCount; i_InitialColumn++)
                {
                    result[i_InitialColumn, i_InitialRow] = operand[i_InitialRow, i_InitialColumn];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the transposition of a sparse matrix.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="SparseMatrix{T}"/> resulting from the transposition. </returns>
        internal static SparseMatrix<T> Transposition<T>(SparseMatrix<T> operand)
            where T : INumberBase<T>
        {
            return operand.Storage switch
            {
                (SStor.CompressedColumn<T> operandCCS) => new SparseMatrix<T>(Transposition(operandCCS)),
                // Global fallback (to update with a default addition)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Transposition : SparseStorages

        private static SStor.CompressedColumn<T> Transposition<T>(SStor.CompressedColumn<T> operand)
            where T : INumberBase<T>
        {
            int[] count = new int[operand.RowCount];
            foreach (int initialRow in operand.RowIndices)
            {
                count[initialRow]++;
            }

            int[] resultColumnPointer = new int[operand.RowCount + 1];
            resultColumnPointer[0] = 0;
            for (int i = 0; i < operand.RowCount; i++) 
            { 
                resultColumnPointer[i + 1] = resultColumnPointer[i] + count[i]; 
                count[i] = 0;
            }


            int[] resultRowIndices = new int[resultColumnPointer[^1]];
            T[] resultValues = new T[resultColumnPointer[^1]];
            for (int i_Column = 0; i_Column < operand.ColumnCount; i_Column++)
            {
                for (int i = operand.ColumnPointers[i_Column]; i < operand.ColumnPointers[i_Column + 1]; i++)
                {
                    int i_ResultColumn = operand.RowIndices[i];
                    resultRowIndices[resultColumnPointer[i_ResultColumn] + count[i_ResultColumn]] = i_Column;
                    resultValues[resultColumnPointer[i_ResultColumn] + count[i_ResultColumn]] = operand.Values[i];

                    count[i_ResultColumn]++;
                }
            }

            return new SStor.CompressedColumn<T>(operand.ColumnCount, operand.RowCount, resultColumnPointer, resultRowIndices, resultValues);
        }

        #endregion
    }
}
