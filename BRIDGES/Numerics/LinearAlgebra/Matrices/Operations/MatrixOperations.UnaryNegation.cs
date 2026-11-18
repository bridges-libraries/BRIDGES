using System;
using System.Numerics;
using System.Collections.Generic;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;

namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region UnaryNegation : Main

        /// <summary>
        /// Computes the unary negation of a matrix.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="Matrix{T}"/> resulting from the unary negation. </returns>
        internal static Matrix<T> Negate<T>(Matrix<T> operand)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseMatrix<T> denseOperand) => Negate(denseOperand),
                (SparseMatrix<T> sparseOperand) => Negate(sparseOperand),
                // Global fallback (to update with a default unary negation)
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the unary negation of a dense matrix.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="DenseMatrix{T}"/> resulting from the unary negation. </returns>
        internal static DenseMatrix<T> Negate<T>(DenseMatrix<T> operand)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.RowCount, operand.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = -operand[i_Row, i_Column];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the unary negation of a sparse matrix.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to operate from. </param>
        /// <returns> The <see cref="SparseMatrix{T}"/> resulting from the unary negation. </returns>
        internal static SparseMatrix<T> Negate<T>(SparseMatrix<T> operand)
            where T : INumberBase<T>
        {
            return operand.Storage switch
            {
                (SStor.CompressedColumn<T> operandCCS) => new SparseMatrix<T>(Negate(operandCCS)),
                // Global fallback (to update with a default addition)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region UnaryNegation : SparseStorages

        private static SStor.CompressedColumn<T> Negate<T>(SStor.CompressedColumn<T> operand)
            where T : INumberBase<T>
        {
            int[] resultColumnPointers = new int[operand.ColumnPointers.Count];
            for (int i = 0; i < operand.ColumnPointers.Count; i++) { resultColumnPointers[i] = operand.ColumnPointers[i]; }

            int[] resultRowIndices = new int[operand.RowIndices.Count];
            for (int i = 0; i < operand.RowIndices.Count; i++) { resultRowIndices[i] = operand.RowIndices[i]; }

            T[] resultValues = new T[operand.Values.Count];
            for (int i = 0; i < operand.Values.Count; i++) { resultValues[i] = -operand.Values[i]; }

            return new SStor.CompressedColumn<T>(operand.RowCount, operand.ColumnCount, resultColumnPointers, resultRowIndices, resultValues);
        }

        #endregion
    }
}
