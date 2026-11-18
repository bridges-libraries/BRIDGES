using System;
using System.Numerics;

using BRIDGES.Numerics.LinearAlgebra.Matrices.SparseBuilders;
using BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;



namespace BRIDGES.Numerics.LinearAlgebra.Matrices
{
    /// <summary>
    /// Represents a generic sparse matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix component values. </typeparam>
    public class SparseMatrix<T> : Matrix<T>
        where T : INumberBase<T>
    {
        #region Fields

        private readonly SparseStorage<T> _storage;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of non-zero values in this matrix. 
        /// </summary>
        /// <remarks> It is obtained as the number of entries in the sparse storage. </remarks>
        public int NonZeroCount => _storage.Count;

        /// <summary>
        /// Gets or sets the component value of this sparse matrix at the given row and column. <br/>
        /// The sparse storage must have an existing entry for the component.
        /// </summary>
        /// <remarks>
        /// For efficiency reasons, no checks is performed on the non-zero condition of the <see langword="value"/>.
        /// </remarks>
        /// <param name="row"> Row index of the component to get or set. </param>
        /// <param name="column"> Column index of the component to get or set. </param>
        /// <returns> The <typeparamref name="T"/> number at the given row and column. </returns>
        public T this[int row, int column]
        {
            get { return _storage[row, column]; }
        }


        /// <summary>
        /// Gets the storage format of this sparse matrix.
        /// </summary>
        public SparseStorageFormat StorageFormat => _storage.StorageFormat;

        #endregion

        #region Other Properties

        /// <summary>
        /// Gets the storage of this sparse matrix.
        /// </summary>
        internal SparseStorage<T> Storage => _storage;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseMatrix{T}"/> class from its sparse storage.
        /// </summary>
        /// <param name="storage"> Storage containing the values for the matrix, arranged in an efficient way. </param>
        public SparseMatrix(SparseStorage<T> storage)
            : base(storage.RowCount, storage.ColumnCount)
        {
            _storage = storage;
        }

        #endregion

        #region Public Static Methods

        //     -----     -----     Constructor Pattern     -----     -----     //

        /// <summary>
        /// Builds a <see cref="SparseMatrix{T}"/> storage from the <see cref="DictionaryOfKeys{T}"/> sparse matrix builder.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="dok"> Sparse matrix builder containing the unsorted components for the matrix.</param>
        /// <param name="format"> </param>
        /// <param name="validityChecks"> Evaluates whether the component indices and the component values should be verified.</param>
        /// <returns> The <see cref="CompressedColumn{T}"/> storage.</returns>
        public static SparseMatrix<T> Build(int rowCount, int columnCount, DictionaryOfKeys<T> dok, SparseStorageFormat format = SparseStorageFormat.CompressedColumn, bool validityChecks = true)
        {
            return format switch
            {
                SparseStorageFormat.CompressedColumn => new SparseMatrix<T>(CompressedColumn<T>.Build(rowCount, columnCount, dok, validityChecks)),
                // General fallback
                _ => throw new NotImplementedException()
            };
        }


        //     -----     -----     Algebraic Near Ring : SparseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> Add(SparseMatrix<T> left, SparseMatrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> Subtract(SparseMatrix<T> left, SparseMatrix<T> right) => left - right;


        /// <inheritdoc cref="operator -(SparseMatrix{T})"/>
        public static SparseMatrix<T> Opposite(SparseMatrix<T> operand) => -operand;


        /// <inheritdoc cref="operator *(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> Multiply(SparseMatrix<T> left, SparseMatrix<T> right) => left * right;


        //     -----     Other Operations : SparseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with another sparse matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the operation. </returns>
        public static SparseMatrix<T> TransposeMultiply(SparseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.TransposeMultiply(left, right);

        /// <summary>
        /// Computes the multiplication of a sparse matrix with another transposed sparse matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="SparseMatrix{T}"/> resulting from the operation. </returns>
        public static SparseMatrix<T> MultiplyTranspose(SparseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.MultiplyTranspose(left, right);


        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="operator +(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Add(SparseMatrix<T> left, DenseMatrix<T> right) => left + right;

        /// <inheritdoc cref="operator -(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Subtract(SparseMatrix<T> left, DenseMatrix<T> right) => left - right;


        /// <inheritdoc cref="operator *(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> Multiply(SparseMatrix<T> left, DenseMatrix<T> right) => left * right;


        //     -----     Other Right Operations : DenseMatrix<T>     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a dense matrix : <c>M<sup>t</sup>·N</c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to transpose and multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        public static DenseMatrix<T> TransposeMultiply(SparseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.TransposeMultiply(left, right);

        /// <summary>
        /// Computes the multiplication of a sparse matrix with a transposed dense matrix : <c>M·N<sup>t</sup></c>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix{T}"/> to multiply. </param>
        /// <param name="right"> Right <see cref="DenseMatrix{T}"/> to transpose and multiply. </param>
        /// <returns> The new <see cref="DenseMatrix{T}"/> resulting from the operation. </returns>
        public static DenseMatrix<T> MultiplyTranspose(SparseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.MultiplyTranspose(left, right);


        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="operator *(SparseMatrix{T}, T)"/>
        public static SparseMatrix<T> Multiply(SparseMatrix<T> operand, T factor) => operand * factor;

        /// <inheritdoc cref="operator *(T, SparseMatrix{T})"/>
        public static SparseMatrix<T> Multiply(T factor, SparseMatrix<T> operand) => factor * operand;


        /// <inheritdoc cref="operator /(SparseMatrix{T}, T)"/>
        public static SparseMatrix<T> Divide(SparseMatrix<T> operand, T divisor) => operand / divisor;


        //     -----     -----     Vectors     -----     -----     //

        /// <inheritdoc cref="operator *(SparseMatrix{T}, Vect.DenseVector{T})"/>
        public static Vect.DenseVector<T> Multiply(SparseMatrix<T> matrix, Vect.DenseVector<T> vector) => matrix * vector;

        /// <inheritdoc cref="operator *(SparseMatrix{T}, Vect.SparseVector{T})"/>
        public static Vect.SparseVector<T> Multiply(SparseMatrix<T> matrix, Vect.SparseVector<T> vector) => matrix * vector;


        //     -----      Other Operations : Vectors     -----     //

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a sparse vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.SparseVector{T}"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector<T> TransposeMultiply(SparseMatrix<T> matrix, Vect.SparseVector<T> vector) => Operations.MatrixOperations.TransposeMultiply(matrix, vector);

        /// <summary>
        /// Computes the multiplication of a transposed sparse matrix with a dense vector : <c>M<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix{T}"/> to transpose then multiply. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector{T}"/> to multiply. </param>
        /// <returns> The new <see cref="Vect.DenseVector{T}"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector<T> TransposeMultiply(SparseMatrix<T> matrix, Vect.DenseVector<T> vector) => Operations.MatrixOperations.TransposeMultiply(matrix, vector);

        #endregion

        #region Operators

        //     -----     -----     Algebraic Near Ring : SparseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Add{T}(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> operator +(SparseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.Add(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.Subtract{T}(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> operator -(SparseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.Subtract(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.Negate{T}(SparseMatrix{T})"/>
        public static SparseMatrix<T> operator -(SparseMatrix<T> operand) => Operations.MatrixOperations.Negate(operand);


        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(SparseMatrix{T}, SparseMatrix{T})"/>
        public static SparseMatrix<T> operator *(SparseMatrix<T> left, SparseMatrix<T> right) => Operations.MatrixOperations.Multiply(left, right);


        //     -----     -----     Right Embedding : DenseMatrix<T>     -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Add{T}(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> operator +(SparseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.Add(left, right);

        /// <inheritdoc cref="Operations.MatrixOperations.Subtract{T}(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> operator -(SparseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.Subtract(left, right);


        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(SparseMatrix{T}, DenseMatrix{T})"/>
        public static DenseMatrix<T> operator *(SparseMatrix<T> left, DenseMatrix<T> right) => Operations.MatrixOperations.Multiply(left, right);



        //     -----      -----     Group Action : T     -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(SparseMatrix{T}, T)"/>
        public static SparseMatrix<T> operator *(SparseMatrix<T> operand, T factor) => Operations.MatrixOperations.Multiply(operand, factor);

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(T, SparseMatrix{T})"/>
        public static SparseMatrix<T> operator *(T factor, SparseMatrix<T> operand) => Operations.MatrixOperations.Multiply(factor, operand);


        /// <inheritdoc cref="Operations.MatrixOperations.Divide{T}(SparseMatrix{T}, T)"/>
        public static SparseMatrix<T> operator /(SparseMatrix<T> operand, T divisor) => Operations.MatrixOperations.Divide(operand, divisor);


        //     -----      -----     Vectors     -----     -----     //

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(SparseMatrix{T}, Vect.SparseVector{T})"/>
        public static Vect.SparseVector<T> operator *(SparseMatrix<T> matrix, Vect.SparseVector<T> vector) => Operations.MatrixOperations.Multiply(matrix, vector);

        /// <inheritdoc cref="Operations.MatrixOperations.Multiply{T}(SparseMatrix{T}, Vect.DenseVector{T})"/>
        public static Vect.DenseVector<T> operator *(SparseMatrix<T> matrix, Vect.DenseVector<T> vector) => Operations.MatrixOperations.Multiply(matrix, vector);

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override T GetComponent(int row, int column)
        {
            return _storage.TryGet(row, column, out T? value) ? value! : T.Zero;
        }


        /// <summary>
        /// Evaluates whether the sparse matrix contains an entry for the component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to retrieve. </param>
        /// <param name="column"> Column index of the component to retrieve. </param>
        /// <returns> <see langword="true"/> if the sparse matrix contains an entry, <see langword="false"/> otherwise.  </returns>
        public bool Contains(int row, int column)
        {
            return _storage.Contains(row, column);
        }


        /// <inheritdoc/>
        public override T[,] ToArray() => _storage.ToArray();

        /// <inheritdoc/>
        public override T[] ToRowMajorArray() => _storage.ToRowMajorArray();

        /// <inheritdoc/>
        public override T[] ToColumnMajorArray() => _storage.ToColumnMajorArray();

        /// <summary>
        /// Arranges each sparse matrix row into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.SparseVector{T}"/>.</returns>
        public override Vect.SparseVector<T>[] RowVectors() => _storage.RowVectors();

        /// <summary>
        /// Arranges each sparse matrix column into a sparse vector.
        /// </summary>
        /// <returns> The array of <see cref="Vect.SparseVector{T}"/>.</returns>
        public override Vect.SparseVector<T>[] ColumnVectors() => _storage.ColumnVectors();

        #endregion

        #region Other Methods

        /// <summary>
        /// Gets the sparse storage of this sparse matrix.
        /// </summary>
        internal SparseStorage<T> GetSparseStorage() => _storage;

        #endregion
    }
}
