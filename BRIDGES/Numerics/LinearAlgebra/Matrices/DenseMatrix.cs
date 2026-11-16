using System;
using System.Numerics;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices
{
    /// <summary>
    /// Represents a generic dense matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix component values. </typeparam>
    public class DenseMatrix<T> : Matrix<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Storage of this <see cref="DenseMatrix{T}"/>.
        /// </summary>
        private readonly T[,] _storage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the component value of this matrix at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to get or set. </param>
        /// <param name="column"> Column index of the component to get or set. </param>
        /// <returns> The <typeparamref name="T"/> number at the given row and column. </returns>
        public T  this[int row, int column]
        {
            get { return _storage[row, column]; }
            set { _storage[row, column] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix{T}"/> class from its size.
        /// </summary>
        /// <remarks> The matrix is initialised with <see langword="default"/> values. </remarks>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        public DenseMatrix(int rowCount, int columnCount) 
            : base (rowCount, columnCount)
        {
            // Instantiate fields
            _storage = new T[RowCount, ColumnCount];
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix{T}"/> class from its size and values.
        /// </summary>
        /// <remarks> The values must be given rows by rows. </remarks>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="values"> Values to fill the matrix with. </param>
        /// <exception cref="ArgumentOutOfRangeException"> The number of values provided doesn't match the given number of rows and columns.</exception>
        public DenseMatrix(int rowCount, int columnCount, T[] values)
            : base(rowCount, columnCount)
        {
            //     -----     Verifications

            if (values.Length != RowCount * ColumnCount)
            {
                throw new ArgumentOutOfRangeException (nameof(values), "The number of values provided doesn't match the given number of rows and columns.");
            }

            _storage = new T[RowCount, ColumnCount];
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    _storage[r, c] = values[(r * ColumnCount) + c];
                }
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix{T}"/> class by deep copying another <see cref="DenseMatrix{T}"/>.
        /// </summary>
        /// <param name="other"> <see cref="DenseMatrix{T}"/> to deep copy. </param>
        public DenseMatrix(DenseMatrix<T> other)
            : base(other.RowCount, other.ColumnCount)
        {
            // Initialises properties
            _storage = new T[RowCount, ColumnCount];
            Array.Copy(other._storage, _storage, RowCount * ColumnCount);
        }

        #endregion

        #region Public Static Methods

        //     -----     -----     Algebraic Near Ring : DenseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Add(DenseMatrix<T> left, DenseMatrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Subtract(DenseMatrix<T> left, DenseMatrix<T> right) => left - right;


        /// <inheritdoc cref="operator -(DenseMatrix{T})"/>
        public static DenseMatrix<T> Opposite(DenseMatrix<T> operand) => -operand;


        /// <inheritdoc cref="operator *(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Multiply(DenseMatrix<T> left, DenseMatrix<T> right) => left * right;


        //     -----     Other Operations : DenseMatrix<T>     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.TransposeMultiply{T}(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> TransposeMultiply(DenseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.TransposeMultiply(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.MultiplyTranspose{T}(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> MultiplyTranspose(DenseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.MultiplyTranspose(left, right);


        //     -----     -----     Right Embedding : SparseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> Add(DenseMatrix<T> left, SparseMatrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> Subtract(DenseMatrix<T> left, SparseMatrix<T> right) => left - right;


        /// <inheritdoc cref="operator *(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> Multiply(DenseMatrix<T> left, SparseMatrix<T> right) => left * right;


        //     -----     Other Right Operations : SparseMatrix<T>     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.TransposeMultiply{T}(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> TransposeMultiply(DenseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.TransposeMultiply(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.MultiplyTranspose{T}(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> MultiplyTranspose(DenseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.MultiplyTranspose(left, right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(DenseMatrix{T}, T)"/>
        public static DenseMatrix<T> Multiply(DenseMatrix<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, DenseMatrix{T})"/>
        public static DenseMatrix<T> Multiply(T factor, DenseMatrix<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(DenseMatrix{T}, T)"/>
        public static DenseMatrix<T> Divide(DenseMatrix<T> operand, T divisor) => operand / divisor;


        //     -----     -----     Vectors     -----     -----     //

        /// <inheritdoc cref="operator *(DenseMatrix{T}, Vect.DenseVector{T})"/>
        public static Vect.DenseVector<T> Multiply(DenseMatrix<T> matrix, Vect.DenseVector<T> vector) => matrix * vector;

        /// <inheritdoc cref="operator *(DenseMatrix{T}, Vect.SparseVector{T})"/>
        public static Vect.DenseVector<T> Multiply(DenseMatrix<T> matrix, Vect.SparseVector<T> vector) => matrix * vector;


        //     -----      Other Operations : Vectors     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.TransposeMultiply{T}(DenseMatrix{T}, Vect.DenseVector{T})"/>
        public static Vect.DenseVector<T> TransposeMultiply(DenseMatrix<T> matrix, Vect.DenseVector<T> vector) => Operations.MatrixOperations.TransposeMultiply(matrix, vector);

        /// <inheritdoc cref="Operations.MatrixOperations.TransposeMultiply{T}(DenseMatrix{T}, Vect.SparseVector{T})"/>
        public static Vect.DenseVector<T> TransposeMultiply(DenseMatrix<T> matrix, Vect.SparseVector<T> vector) => Operations.MatrixOperations.TransposeMultiply(matrix, vector);


        //     -----     -----     Other Operations     -----     -----     //

        /// <summary>
        /// Computes the transposition of a dense matrix.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix{T}"/> to operate from. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the transposition. </returns>
        public static DenseMatrix<T> Transpose(DenseMatrix<T> matrix)
        {
            DenseMatrix<T> result = new DenseMatrix<T>(matrix.ColumnCount, matrix.RowCount);
            for (int i_InitialRow = 0; i_InitialRow < matrix.RowCount; i_InitialRow++)
            {
                for (int i_InitialColumn = 0; i_InitialColumn < matrix.ColumnCount; i_InitialColumn++)
                {
                    result[i_InitialColumn, i_InitialRow] = matrix[i_InitialRow, i_InitialColumn];
                }
            }

            return result;
        }


        //     -----     -----     Other Static Methods     -----     -----     //

        /// <summary>
        /// Creates the identity matrix of a given size.
        /// </summary>
        /// <remarks> The identity matrix has ones in the diagonal and zeros elsewhere. </remarks>
        /// <param name="size"> Number of rows and columns for the matrix. </param>
        /// <returns> The identity <see cref="DenseMatrix{T}"/>. </returns>
        public static DenseMatrix<T> Identity(int size)
        {
            DenseMatrix<T> result = new DenseMatrix<T>(size, size);
            for (int i_Diagonal = 0; i_Diagonal < size; i_Diagonal++) { result[i_Diagonal, i_Diagonal] = T.One; }

            return result;
        }

        #endregion

        #region Operators

        //     -----     -----     Algebraic Near Ring : DenseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Add{T}(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> operator +(DenseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.Add(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.Subtract{T}(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> operator -(DenseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.Subtract(left, right);


        /// <inheritdoc cref="Operations.MatrixOperations.Negate{T}(DenseMatrix{T})"/>
        public static DenseMatrix<T> operator -(DenseMatrix<T> operand) => Operations.MatrixOperations.Negate(operand);


        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(DenseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> operator *(DenseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.Multiply(left, right);


        //     -----     -----     Right Embedding : SparseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Add{T}(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> operator +(DenseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.Add(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.Subtract{T}(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> operator -(DenseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.Subtract(left, right);


        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(DenseMatrix{T}, SparseMatrix{T})"/>
        public static DenseMatrix<T> operator *(DenseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.Multiply(left, right);


        //     -----      -----      Group Action : T      -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(DenseMatrix{T}, T)"/>
        public static DenseMatrix<T> operator *(DenseMatrix<T> operand, T factor) => Operations.MatrixOperations.Multiply(operand, factor);

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(T, DenseMatrix{T})"/>
        public static DenseMatrix<T> operator *(T factor, DenseMatrix<T> operand) => Operations.MatrixOperations.Multiply(factor, operand);


        /// <inheritdoc cref="Operations.MatrixOperations.Divide{T}(DenseMatrix{T}, T)"/>
        public static DenseMatrix<T> operator /(DenseMatrix<T> operand, T divisor) => Operations.MatrixOperations.Divide(operand, divisor);


        //     -----      -----      Vectors      -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(DenseMatrix{T}, Vect.DenseVector{T})"/>
        public static Vect.DenseVector<T> operator *(DenseMatrix<T> matrix, Vect.DenseVector<T> vector) => Operations.MatrixOperations.Multiply(matrix, vector);

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(DenseMatrix{T}, Vect.SparseVector{T})"/>
        public static Vect.DenseVector<T> operator *(DenseMatrix<T> matrix, Vect.SparseVector<T> vector) => Operations.MatrixOperations.Multiply(matrix, vector);

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override T GetComponent(int row, int column)
        {
            return _storage[row, column];
        }

        
        /// <inheritdoc/>
        public override T[,] ToArray()
        {
            T[,] array = new T[RowCount,ColumnCount];
            Array.Copy(_storage, array, RowCount * ColumnCount);
            
            return array;
        }

        /// <inheritdoc/>
        public override T[] ToRowMajorArray()
        {
            T[] values = new T[RowCount * ColumnCount];
            for (int i_Row = 0; i_Row < RowCount; i_Row++)
            {
                for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
                {
                    values[(i_Row * ColumnCount) + i_Column] = _storage[i_Row, i_Column];
                }
            }
            return values;
        }

        /// <inheritdoc/>
        public override T[] ToColumnMajorArray()
        {
            T[] values = new T[RowCount * ColumnCount];
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                for (int i_Row = 0; i_Row < RowCount; i_Row++)
                {
                    values[i_Row + (i_Column * RowCount)] = _storage[i_Row, i_Column];
                }
            }
            return values;
        }

        /// <summary>
        /// Arranges each dense matrix row into a dense vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.DenseVector{T}"/>. </returns>
        public override Vect.DenseVector<T>[] RowVectors()
        {
            Vect.DenseVector<T>[] rows = new Vect.DenseVector<T>[RowCount];
            for (int i_Row = 0; i_Row < RowCount; i_Row++)
            {
                rows[i_Row] = new Vect.DenseVector<T>(ColumnCount);
                for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
                {
                    rows[i_Row][i_Column] = _storage[i_Row, i_Column];
                }
            }
            return rows;

        }

        /// <summary>
        /// Arranges each dense matrix column into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.DenseVector{T}"/>. </returns>
        public override Vect.DenseVector<T>[] ColumnVectors()
        {
            Vect.DenseVector<T>[] columns = new Vect.DenseVector<T>[ColumnCount];
            for (int i_Column = 0; i_Column < ColumnCount; i_Column++)
            {
                columns[i_Column] = new Vect.DenseVector<T>(RowCount);
                for (int i_Row = 0; i_Row < RowCount; i_Row++)
                {
                    columns[i_Column][i_Row] = _storage[i_Row, i_Column];
                }
            }
            return columns;

        }

        #endregion
    }
}
