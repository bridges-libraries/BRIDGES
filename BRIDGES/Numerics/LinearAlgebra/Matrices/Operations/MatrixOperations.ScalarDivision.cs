using System;
using System.Numerics;
using System.Collections.Generic;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Right Scalar Division : Main

        /// <summary>
        /// Computes the scalar division of a matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the scalar division. </returns>
        internal static Matrix<T> Divide<T>(Matrix<T> operand, T divisor)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseMatrix<T> denseOperand) => Divide(denseOperand, divisor),
                (SparseMatrix<T> sparseOperand) => Divide(sparseOperand, divisor),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the scalar division of a dense matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the scalar division. </returns>
        internal static DenseMatrix<T> Divide<T>(DenseMatrix<T> operand, T divisor)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.RowCount, operand.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = operand[i_Row, i_Column] / divisor;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the scalar division of a sparse matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to divide. </param>
        /// <param name="divisor"> <typeparamref name="T"/> number to divide with. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the scalar division. </returns>
        internal static SparseMatrix<T> Divide<T>(SparseMatrix<T> operand, T divisor)
            where T : INumberBase<T>
        {
            return operand.Storage switch
            {
                (SStor.CompressedColumn<T> operandCCS) => new SparseMatrix<T>(Divide(operandCCS, divisor)),
                // Global fallback (to update with a default multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Right Scalar Division : SparseStorages

        private static SStor.CompressedColumn<T> Divide<T>(SStor.CompressedColumn<T> operand, T divisor)
            where T : INumberBase<T>
        {
            List<T> resultValues = new List<T>(operand.Values.Count);
            for (int i = 0; i < operand.Values.Count; i++) { resultValues.Add(operand.Values[i] / divisor); }

            List<int> resultRowIndices = new List<int>(operand.RowIndices);

            int[] resultColumnPointers = new int[operand.ColumnPointers.Count];
            for (int i = 0; i < operand.ColumnPointers.Count; i++) { resultColumnPointers[i] = operand.ColumnPointers[i]; }

            return new SStor.CompressedColumn<T>(operand.RowCount, operand.ColumnCount, resultColumnPointers, resultRowIndices.ToArray(), resultValues.ToArray());
        }

        #endregion
    }
}
