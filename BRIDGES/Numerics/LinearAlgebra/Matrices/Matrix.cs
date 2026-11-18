using System;
using System.Numerics;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices
{
    /// <summary>
    /// Represents a generic matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix component values. </typeparam>
    public abstract class Matrix<T>
        where T : INumberBase<T>
    {
        #region Properties

        /// <summary>
        /// Gets the number of rows of this matrix.
        /// </summary>
        public int RowCount { get; init; }

        /// <summary>
        /// Gets the number of columns of this matrix.
        /// </summary>
        public int ColumnCount { get; init; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Matrix{T}"/> class from its size.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the <see cref="Matrix{T}"/>. </param>
        /// <param name="columnCount"> Number of columns for the <see cref="Matrix{T}"/>. </param>
        /// <remarks> The constructor is set to internal to prevent any external derived class. </remarks>
        internal Matrix(int rowCount, int columnCount)
        {
            //     -----     Verifications

            if (rowCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowCount), "The number of row must be strictly larger than 0.");
            }
            if (columnCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(columnCount), "The number of columns must be strictly larger than 0.");
            }

            // Initialise properties
            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        #endregion

        #region Public Static Methods

        //     -----     -----     Algebraic Near Ring : Matrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> Add(Matrix<T> left, Matrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> Subtract(Matrix<T> left, Matrix<T> right) => left - right;


        /// <inheritdoc cref="operator -(Matrix{T})"/>
        public static Matrix<T> Opposite(Matrix<T> operand) => -operand;


        /// <inheritdoc cref="operator *(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> Multiply(Matrix<T> left, Matrix<T> right) => left * right;


        //     -----     Other Operations : DenseMatrix<T>     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.TransposeMultiply{T}(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> TransposeMultiply(Matrix<T> left, Matrix<T> right) => Operations.MatrixOperations.TransposeMultiply(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.MultiplyTranspose{T}(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> MultiplyTranspose(Matrix<T> left, Matrix<T> right) => Operations.MatrixOperations.MultiplyTranspose(left, right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(Matrix{T}, T)"/>
        public static Matrix<T> Multiply(Matrix<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, Matrix{T})"/>
        public static Matrix<T> Multiply(T factor, Matrix<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(Matrix{T}, T)"/>
        public static Matrix<T> Divide(Matrix<T> operand, T divisor) => operand / divisor;


        //     -----     -----     Vectors     -----     -----     //

        /// <inheritdoc cref="operator *(Matrix{T}, Vect.Vector{T})"/>
        public static Vect.Vector<T> Multiply(Matrix<T> matrix, Vect.Vector<T> vector) => matrix * vector;

        /// <inheritdoc cref="Operations.MatrixOperations.TransposeMultiply{T}(Matrix{T}, Vect.Vector{T})"/>
        public static Vect.Vector<T> TransposeMultiply(Matrix<T> matrix, Vect.Vector<T> vector) => Operations.MatrixOperations.TransposeMultiply(matrix, vector);

        #endregion

        #region Operators

        //     -----     -----     Algebraic Near Ring : Matrix<T>     -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Add{T}(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right) => Operations.MatrixOperations.Add(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.Subtract{T}(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> operator -(Matrix<T> left, Matrix<T> right) => Operations.MatrixOperations.Subtract(left, right);


        /// <inheritdoc cref="Operations.MatrixOperations.Negate{T}(Matrix{T})"/>
        public static Matrix<T> operator -(Matrix<T> operand) => Operations.MatrixOperations.Negate(operand);


        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(Matrix{T}, Matrix{T})"/>
        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right) => Operations.MatrixOperations.Multiply(left, right);


        //     -----     Group Action : T     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(Matrix{T}, T)"/>
        public static Matrix<T> operator *(Matrix<T> operand, T factor) => Operations.MatrixOperations.Multiply(operand, factor);

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(T, Matrix{T})"/>
        public static Matrix<T> operator *(T factor, Matrix<T> operand) => Operations.MatrixOperations.Multiply(factor, operand);


        /// <inheritdoc cref="Operations.MatrixOperations.Divide{T}(Matrix{T}, T)"/>
        public static Matrix<T> operator /(Matrix<T> operand, T divisor) => Operations.MatrixOperations.Divide(operand, divisor);


        //     -----     Vectors     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(Matrix{T}, Vect.Vector{T})"/>
        public static Vect.Vector<T> operator *(Matrix<T> matrix, Vect.Vector<T> vector) => Operations.MatrixOperations.Multiply(matrix, vector);

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the value of the component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to get. </param>
        /// <param name="column"> Column index of the component to get. </param>
        /// <returns> The value of the component. </returns>
        public abstract T GetComponent(int row, int column);


        /// <summary>
        /// Provides an array representation of this matrix.
        /// </summary>
        /// <returns> The two-dimensional array. </returns>
        public abstract T[,] ToArray();

        /// <summary>
        /// Provides an array representation of this matrix, with the values arranged row by row.
        /// </summary>
        /// <returns> The one-dimensional array, resulting from the concatenation of the rows. </returns>
        public abstract T[] ToRowMajorArray();

        /// <summary>
        /// Provides an array representation of this matrix, with the values arranged column by column.
        /// </summary>
        /// <returns> The one-dimensional array, resulting from the concatenation of the columns. </returns>
        public abstract T[] ToColumnMajorArray();

        /// <summary>
        /// Arranges each matrix row into a vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.Vector{T}"/>. </returns>
        public abstract Vect.Vector<T>[] RowVectors();

        /// <summary>
        /// Arranges each matrix column into a vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.Vector{T}"/> array.</returns>
        public abstract Vect.Vector<T>[] ColumnVectors();

        #endregion


        #region Override : Object

        /// <inheritdoc/>
        public override string? ToString()
        {
            return $"Matrix [{RowCount};{ColumnCount}]";
        }

        #endregion
    }
}