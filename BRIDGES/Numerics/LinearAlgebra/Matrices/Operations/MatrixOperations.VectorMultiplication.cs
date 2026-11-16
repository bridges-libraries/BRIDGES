using System;
using System.Numerics;
using System.Collections.Generic;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;
using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Operations
{
    internal static partial class MatrixOperations
    {
        #region Right Vector Multiplication : Main

        /// <summary>
        /// Computes the right multiplication of a matrix with a vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.Vector<T> Multiply<T>(Matrix<T> matrix, Vect.Vector<T> vector)
            where T : INumberBase<T>
        {
            return (matrix, vector) switch
            {
                (DenseMatrix<T> denseMatrix, Vect.DenseVector<T> denseVector) => Multiply(denseMatrix, denseVector),
                (DenseMatrix<T> denseMatrix, Vect.SparseVector<T> sparseVector) => Multiply(denseMatrix, sparseVector),
                // DenseMatrix<T> fallback
                //(DenseMatrix<T> denseMatrix, _) => Multiply(denseMatrix, vector),
                (SparseMatrix<T> sparseMatrix, Vect.DenseVector<T> denseVector) => Multiply(sparseMatrix, denseVector),
                (SparseMatrix<T> sparseMatrix, Vect.SparseVector<T> sparseVector) => Multiply(sparseMatrix, sparseVector),
                // SparseMatrix<T> fallback
                //(SparseMatrix<T> sparseMatrix, _) => Multiply(sparseMatrix, vector),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the right multiplication of a dense matrix with a dense vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.DenseVector<T> Multiply<T>(DenseMatrix<T> matrix, Vect.DenseVector<T> vector)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationCompatibility(matrix, vector);

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(matrix.RowCount);
            for (int i_Result = 0; i_Result < result.Size; i_Result++)
            {
                T component = default!;
                for (int i = 0; i < matrix.ColumnCount; i++)
                {
                    component += matrix[i_Result, i] * vector[i];
                }
                result[i_Result] = component;
            }

            return result;
        }

        /// <summary>
        /// Computes the right multiplication of a dense matrix with a sparse vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.DenseVector<T> Multiply<T>(DenseMatrix<T> matrix, Vect.SparseVector<T> vector)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationCompatibility(matrix, vector);

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(matrix.RowCount);
            for (int i_Result = 0; i_Result < result.Size; i_Result++)
            {
                T component = default!;
                foreach ((int index, T vectorComponent) in vector.NonZeros())
                {
                    component += matrix[i_Result, index] * vectorComponent;
                }
                result[i_Result] = component;
            }

            return result;
        }


        /// <summary>
        /// Computes the right multiplication of a sparse matrix with a dense vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.DenseVector<T> Multiply<T>(SparseMatrix<T> matrix, Vect.DenseVector<T> vector)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationCompatibility(matrix, vector);

            return matrix.Storage switch
            {
                (SStor.CompressedColumn<T> matrixCCS) => Multiply(matrixCCS, vector),
                // Global fallback (to update with a default multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the right multiplication of a sparse matrix with a sparse vector : <c>M·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.SparseVector<T> Multiply<T>(SparseMatrix<T> matrix, Vect.SparseVector<T> vector)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureMultiplicationCompatibility(matrix, vector);

            return matrix.Storage switch
            {
                (SStor.CompressedColumn<T> matrixCCS) => Multiply(matrixCCS, vector),
                // Global fallback (to update with a default multiplication)
                _ => throw new NotImplementedException(),
            };

        }

        #endregion

        #region Right Vector Multiplication : SparseStorages

        internal static Vect.DenseVector<T> Multiply<T>(SStor.CompressedColumn<T> storage, Vect.DenseVector<T> vector)
            where T : INumberBase<T>
        {
            Vect.DenseVector<T> result = new Vect.DenseVector<T>(storage.RowCount);
            for (int i_StorageColumn = 0; i_StorageColumn < storage.ColumnCount; i_StorageColumn++)
            {
                T vectorValue = vector[i_StorageColumn];
                for (int i = storage.ColumnPointers[i_StorageColumn]; i < storage.ColumnPointers[i_StorageColumn + 1]; i++)
                {
                    result[storage.RowIndices[i]] += storage.Values[i] * vectorValue;
                }
            }

            return result;
        }

        internal static Vect.SparseVector<T> Multiply<T>(SStor.CompressedColumn<T> storage, Vect.SparseVector<T> vector)
            where T : INumberBase<T>
        {
            T[] array = new T[storage.RowCount];
            for (int i_StorageColumn = 0; i_StorageColumn < storage.ColumnCount; i_StorageColumn++)
            {
                if (!vector.Contains(i_StorageColumn)) { continue; }

                T vectorValue = vector[i_StorageColumn];
                for (int i = storage.ColumnPointers[i_StorageColumn]; i < storage.ColumnPointers[i_StorageColumn + 1]; i++)
                {
                    array[storage.RowIndices[i]] += storage.Values[i] * vectorValue;
                }
            }


            Vect.SparseVector<T> result = new Vect.SparseVector<T>(storage.RowCount);
            for (int i = 0; i < result.Size; i++)
            {
                if (array[i] != T.Zero) { result.Add(i, array[i]); }
            }

            return result;
        }

        #endregion


        #region Right Vector Transpose Multiplication : Main

        /// <summary>
        /// Computes the multiplication of a transposed matrix with a vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.Vector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.Vector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.Vector<T> TransposeMultiply<T>(Matrix<T> matrix, Vect.Vector<T> vector)
            where T : INumberBase<T>
        {
            return (matrix, vector) switch
            {
                (DenseMatrix<T> denseMatrix, Vect.DenseVector<T> denseVector) => TransposeMultiply(denseMatrix, denseVector),
                (DenseMatrix<T> denseMatrix, Vect.SparseVector<T> sparseVector) => TransposeMultiply(denseMatrix, sparseVector),
                // DenseMatrix<T> fallback
                //(DenseMatrix<T> denseMatrix, _) => TransposeMultiply(denseMatrix, vector),
                (SparseMatrix<T> sparseMatrix, Vect.DenseVector<T> denseVector) => TransposeMultiply(sparseMatrix, denseVector),
                (SparseMatrix<T> sparseMatrix, Vect.SparseVector<T> sparseVector) => TransposeMultiply(sparseMatrix, sparseVector),
                // SparseMatrix<T> fallback
                //(SparseMatrix<T> sparseMatrix, _) => TransposeMultiply(sparseMatrix, vector),
                // Global fallback
                _ => throw new NotImplementedException(),
            };
        }


        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with a dense vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.DenseVector<T> TransposeMultiply<T>(DenseMatrix<T> matrix, Vect.DenseVector<T> vector)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureTransposeMultiplicationCompatibility(matrix, vector);

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(matrix.ColumnCount);
            for (int i_Result = 0; i_Result < result.Size; i_Result++)
            {
                T component = default!;
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    component += matrix[i, i_Result] * vector[i];
                }
                result[i_Result] = component;
            }

            return result;
        }

        /// <summary>
        /// Computes the multiplication of a transposed dense matrix with a sparse vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.DenseVector<T> TransposeMultiply<T>(DenseMatrix<T> matrix, Vect.SparseVector<T> vector)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureTransposeMultiplicationCompatibility(matrix, vector);

            Vect.DenseVector<T> result = new Vect.DenseVector<T>(matrix.ColumnCount);
            for (int i_Result = 0; i_Result < result.Size; i_Result++)
            {
                T component = default!;
                foreach ((int index, T vectorComponent) in vector.NonZeros())
                {
                    component += matrix[index, i_Result] * vectorComponent;
                }
                result[i_Result] = component;
            }

            return result;
        }


        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a dense vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.DenseVector<T> TransposeMultiply<T>(SparseMatrix<T> matrix, Vect.DenseVector<T> vector)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureTransposeMultiplicationCompatibility(matrix, vector);

            return matrix.Storage switch
            {
                (SStor.CompressedColumn<T> matrixCCS) => TransposeMultiply(matrixCCS, vector),
                // Global fallback (to update with a default transpose multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a sparse vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        internal static Vect.SparseVector<T> TransposeMultiply<T>(SparseMatrix<T> matrix, Vect.SparseVector<T> vector)
            where T : INumberBase<T>
        {
            //     -----     Verifications
            EnsureTransposeMultiplicationCompatibility(matrix, vector);

            return matrix.Storage switch
            {
                (SStor.CompressedColumn<T> matrixCCS) => TransposeMultiply(matrixCCS, vector),
                // Global fallback (to update with a default transpose multiplication)
                _ => throw new NotImplementedException(),
            };
        }

        #endregion

        #region Right Vector Transpose Multiplication : SparseStorages

        internal static Vect.DenseVector<T> TransposeMultiply<T>(SStor.CompressedColumn<T> storage, Vect.DenseVector<T> vector)
            where T : INumberBase<T>
        {
            Vect.DenseVector<T> result = new Vect.DenseVector<T>(storage.ColumnCount);
            for (int i_StorageColumn = 0; i_StorageColumn < storage.ColumnCount; i_StorageColumn++)
            {
                T component = default!;
                for (int i = storage.ColumnPointers[i_StorageColumn]; i < storage.ColumnPointers[i_StorageColumn + 1]; i++)
                {
                    component += storage.Values[i] * vector[storage.RowIndices[i]];
                }
                result[i_StorageColumn] = component;
            }

            return result;
        }

        internal static Vect.SparseVector<T> TransposeMultiply<T>(SStor.CompressedColumn<T> storage, Vect.SparseVector<T> vector)
            where T : INumberBase<T>
        {
            Vect.SparseVector<T> result = new Vect.SparseVector<T>(storage.ColumnCount);
            for (int i_StorageColumn = 0; i_StorageColumn < storage.ColumnCount; i_StorageColumn++)
            {
                T component = default!;
                for (int i = storage.ColumnPointers[i_StorageColumn]; i < storage.ColumnPointers[i_StorageColumn + 1]; i++)
                {
                    if (vector.TryGet(storage.RowIndices[i], out T? value))
                    {
                        component += storage.Values[i] * value!;
                    }
                }

                if (component != T.Zero) { result.Add(i_StorageColumn, component); }
            }

            return result;
        }

        #endregion
    }
}
