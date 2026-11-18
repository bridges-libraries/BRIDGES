using System;
using System.Numerics;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Methods

        /// <summary>
        /// Ensures that the two matrices have the same dimensions.
        /// </summary>
        /// <typeparam name="T"> Type of the matrix component values. </typeparam>
        /// <param name="left"> Left operand of the operation. </param>
        /// <param name="right"> Right operand of the operation. </param>
        /// <exception cref="InvalidOperationException"> The two matrices must have the same number of rows and the same number of columns. </exception>
        internal static void EnsureSameDimensions<T>(Matrix<T> left, Matrix<T> right)
            where T : INumberBase<T>
        {
            if (left.RowCount != right.RowCount | left.ColumnCount != right.ColumnCount)
            {
                throw new InvalidOperationException("The two matrices must have the same number of rows and the same number of columns.");
            }
        }

        /// <summary>
        /// Ensures that the two matrices are compatible for the multiplication operation.
        /// </summary>
        /// <typeparam name="T"> Type of the matrix component values. </typeparam>
        /// <param name="left"> Left operand of the operation. </param>
        /// <param name="right"> Right operand of the operation. </param>
        /// <exception cref="InvalidOperationException"> 
        /// The number of columns of <paramref name="left"/> matrix and the number of rows of <paramref name="right"/> matrix must be equal. 
        /// </exception>
        internal static void EnsureMultiplicationCompatibility<T>(Matrix<T> left, Matrix<T> right)
            where T : INumberBase<T>
        {
            if (left.ColumnCount != right.RowCount)
            {
                throw new InvalidOperationException($"The number of columns of {nameof(left)} matrix and the number of rows of {nameof(right)} matrix must be equal.");
            }
        }

        /// <summary>
        /// Ensures that the matrix and the vector are compatible for the multiplication operation.
        /// </summary>
        /// <typeparam name="T"> Type of the matrix and vector component values. </typeparam>
        /// <param name="matrix"> Left matrix operand of the operation. </param>
        /// <param name="vector"> Right vector operand of the operation. </param>
        /// <exception cref="InvalidOperationException">
        /// The number of columns of <paramref name="matrix"/> and the size of <paramref name="vector"/> must be equal.
        /// </exception>
        internal static void EnsureMultiplicationCompatibility<T>(Matrix<T> matrix, Vect.Vector<T> vector)
            where T : INumberBase<T>
        {
            if (matrix.ColumnCount != vector.RowCount)
            {
                throw new InvalidOperationException($"The number of columns of {nameof(matrix)} and the size of {nameof(vector)} must be equal.");
            }
        }


        /// <summary>
        /// Ensures that the two matrices are compatible for the transpose multiplication operation.
        /// </summary>
        /// <typeparam name="T"> Type of the matrix component values. </typeparam>
        /// <param name="left"> Left operand of the operation. </param>
        /// <param name="right"> Right operand of the operation. </param>
        /// <exception cref="InvalidOperationException"> 
        /// The number of rows of <paramref name="left"/> and <paramref name="right"/> matrices must be equal. 
        /// </exception>
        internal static void EnsureTransposeMultiplicationCompatibility<T>(Matrix<T> left, Matrix<T> right)
            where T : INumberBase<T>
        {
            if (left.RowCount != right.RowCount)
            {
                throw new InvalidOperationException($"The number of rows of {nameof(left)} and {nameof(right)} matrices must be equal.");
            }
        }

        /// <summary>
        /// Ensures that the matrix and the vector are compatible for the multiplication operation.
        /// </summary>
        /// <typeparam name="T"> Type of the matrix and vector component values. </typeparam>
        /// <param name="matrix"> Left matrix operand of the operation. </param>
        /// <param name="vector"> Right vector operand of the operation. </param>
        /// <exception cref="InvalidOperationException">
        /// The number of rows of <paramref name="matrix"/> and the size of <paramref name="vector"/> must be equal.
        /// </exception>
        internal static void EnsureTransposeMultiplicationCompatibility<T>(Matrix<T> matrix, Vect.Vector<T> vector)
            where T : INumberBase<T>
        {
            if (matrix.RowCount != vector.RowCount)
            {
                throw new InvalidOperationException($"The number of rows of {nameof(matrix)} and the size of {nameof(vector)} must be equal.");
            }
        }


        /// <summary>
        /// Ensures that the two matrices are compatible for the multiplication transpose operation.
        /// </summary>
        /// <typeparam name="T"> Type of the matrix component values. </typeparam>
        /// <param name="left"> Left operand of the operation. </param>
        /// <param name="right"> Right operand of the operation. </param>
        /// <exception cref="InvalidOperationException"> 
        /// The number of columns of <paramref name="left"/> and <paramref name="right"/> matrices must be equal. 
        /// </exception>
        internal static void EnsureMultiplicationTransposeCompatibility<T>(Matrix<T> left, Matrix<T> right)
            where T : INumberBase<T>
        {
            if (left.ColumnCount != right.ColumnCount)
            {
                throw new InvalidOperationException($"The number of columns of {nameof(left)} and {nameof(right)} matrices must be equal.");
            }
        }
    }

        #endregion
}
