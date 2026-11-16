using System;
using System.Numerics;
using System.Collections.Generic;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Right Scalar Multiplication : Main

        /// <summary>
        /// Computes the right scalar multiplication of a matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the right scalar multiplication. </returns>
        internal static Matrix<T> Multiply<T>(Matrix<T> operand, T factor)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseMatrix<T> denseOperand) => Multiply(denseOperand, factor),
                (SparseMatrix<T> sparseOperand) => Multiply(sparseOperand, factor),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the right scalar multiplication of a dense matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the right scalar multiplication. </returns>
        internal static DenseMatrix<T> Multiply<T>(DenseMatrix<T> operand, T factor)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.RowCount, operand.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = operand[i_Row, i_Column] * factor;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the right scalar multiplication of a sparse matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the right scalar multiplication. </returns>
        internal static SparseMatrix<T> Multiply<T>(SparseMatrix<T> operand, T factor)
            where T : INumberBase<T>
        {
            return operand.Storage switch
            {
                (SStor.CompressedColumn<T> operandCCS) => new SparseMatrix<T>(Multiply(operandCCS, factor)),
                // Global fallback (to update with a default multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Right Scalar Multiplication : SparseStorages

        private static SStor.CompressedColumn<T> Multiply<T>(SStor.CompressedColumn<T> operand, T factor)
            where T : INumberBase<T>
        {
            List<T> resultValues = new List<T>(operand.Values.Count);
            for (int i = 0; i < operand.Values.Count; i++) { resultValues.Add(operand.Values[i] * factor); }

            List<int> resultRowIndices = new List<int>(operand.RowIndices);

            int[] resultColumnPointers = new int[operand.ColumnPointers.Count];
            for (int i = 0; i < operand.ColumnPointers.Count; i++) { resultColumnPointers[i] = operand.ColumnPointers[i]; }

            return new SStor.CompressedColumn<T>(operand.RowCount, operand.ColumnCount, resultColumnPointers, resultRowIndices.ToArray(), resultValues.ToArray());
        }

        #endregion


        #region Left Scalar Multiplication : Main

        /// <summary>
        /// Computes the left scalar multiplication of a matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix{T}"/> to multiply on the left. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="Matrix{T}"/> resulting from the left scalar multiplication. </returns>
        internal static Matrix<T> Multiply<T>(T factor, Matrix<T> operand)
            where T : INumberBase<T>
        {
            return operand switch
            {
                (DenseMatrix<T> denseOperand) => Multiply(factor, denseOperand),
                (SparseMatrix<T> sparseOperand) => Multiply(factor, sparseOperand),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the left scalar multiplication of a dense matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <param name="operand"> <see cref="DenseMatrix{T}"/> to multiply on the left. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the left scalar multiplication. </returns>
        internal static DenseMatrix<T> Multiply<T>(T factor, DenseMatrix<T> operand)
            where T : INumberBase<T>
        {
            DenseMatrix<T> result = new DenseMatrix<T>(operand.RowCount, operand.ColumnCount);
            for (int i_Row = 0; i_Row < result.RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < result.ColumnCount; i_Column++)
                {
                    result[i_Row, i_Column] = factor * operand[i_Row, i_Column];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the left scalar multiplication of a sparse matrix with a <typeparamref name="T"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix{T}"/> to multiply on the left. </param>
        /// <param name="factor"> <typeparamref name="T"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the left scalar multiplication. </returns>
        internal static SparseMatrix<T> Multiply<T>(T factor, SparseMatrix<T> operand)
            where T : INumberBase<T>
        {
            return operand.Storage switch
            {
                (SStor.CompressedColumn<T> operandCCS) => new SparseMatrix<T>(Multiply(factor, operandCCS)),
                // Global fallback (to update with a default multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Left Scalar Multiplication : SparseStorages

        private static SStor.CompressedColumn<T> Multiply<T>(T factor, SStor.CompressedColumn<T> operand)
            where T : INumberBase<T>
        {
            List<T> resultValues = new List<T>(operand.Values.Count);
            for (int i = 0; i < operand.Values.Count; i++) { resultValues.Add(factor * operand.Values[i]); }

            List<int> resultRowIndices = new List<int>(operand.RowIndices);

            int[] resultColumnPointers = new int[operand.ColumnPointers.Count];
            for (int i = 0; i < operand.ColumnPointers.Count; i++) { resultColumnPointers[i] = operand.ColumnPointers[i]; }

            return new SStor.CompressedColumn<T>(operand.RowCount, operand.ColumnCount, resultColumnPointers, resultRowIndices.ToArray(), resultValues.ToArray());
        }

        #endregion
    }
}
