using System;
using System.Collections.Generic;

using MNet_LinAlg = MathNet.Numerics.LinearAlgebra;

using Alg_Sets = BRIDGES.Algebra.Sets;

using Vect = BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a dense matrix.
    /// </summary>
    public sealed class DenseMatrix : Matrix,
        Alg_Sets.IGroupAction<DenseMatrix, double>
    {
        #region Fields

        /// <summary>
        /// Dense matrix from Math.Net library.
        /// </summary>
        private readonly MNet_LinAlg.Double.DenseMatrix _storedMatrix;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int RowCount => _storedMatrix.RowCount; 

        /// <inheritdoc/>
        public override int ColumnCount => _storedMatrix.ColumnCount;

        /// <summary>
        /// Gets the value of this matrix at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the value to get. </param>
        /// <param name="column"> Column index of the value to get. </param>
        /// <returns> The value at the given row and column index. </returns>
        public double this[int row, int column]
        {
            get { return _storedMatrix[row, column]; }
            set { _storedMatrix[row, column] = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="DenseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="DenseMatrix"/>. </param>
        public DenseMatrix(int rowCount, int columnCount)
            : base(rowCount, columnCount)
        {
            _storedMatrix = MNet_LinAlg.Double.DenseMatrix.Create(rowCount, columnCount, 0.0);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class by defining its size and values row by row.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="DenseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="DenseMatrix"/>. </param>
        /// <param name="components"> Component values given row by row. </param>
        /// <exception cref="ArgumentException"> The number of components provided doesn't match the given number of rows and columns. </exception>
        public DenseMatrix(int rowCount, int columnCount, double[] components)
            : base(rowCount, columnCount)
        {
            // Verifications
            if (components.Length != rowCount * columnCount)
            {
                throw new ArgumentException("The number of values provided doesn't match the given number of rows and columns.");
            }

            // Instanciate fields
            _storedMatrix =  MNet_LinAlg.Double.DenseMatrix.Create(rowCount, columnCount, (int i_R, int i_C) => components[(i_R * columnCount) + i_C]);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class by defining its values.
        /// </summary>
        /// <param name="components">  Values of the matrix. </param>
        /// <exception cref="ArgumentException"> The dimensions of the array must be at least 1. </exception>
        public DenseMatrix(double[,] components)
            : base(components.GetLength(0), components.GetLength(1))
        {
            // Verifications
            if (!(components?.GetLength(0) > 0 && components?.GetLength(1) > 0))
            {
                throw new ArgumentException("The dimensions of the array must be strictly larger than 0.");
            }

            // Instanciate fields
            _storedMatrix = MNet_LinAlg.Double.DenseMatrix.Create(components.GetLength(0), components.GetLength(1), (int i_R, int i_C) => components[i_R, i_C]);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> class from another <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to deep copy. </param>
        public DenseMatrix(DenseMatrix matrix)
            : base(matrix.RowCount, matrix.ColumnCount)
        {
            _storedMatrix = MNet_LinAlg.Double.DenseMatrix.Create(matrix.RowCount, matrix.ColumnCount, (int i_R, int i_C) => matrix[i_R, i_C]);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="DenseMatrix"/> from a <see cref="MNet_LinAlg.Double.DenseMatrix"/>.
        /// </summary>
        /// <param name="matrix"> Storage of the matrix. </param>
        /// <exception cref="ArgumentException"> The dimensions of the array must be at least 1. </exception>
        internal DenseMatrix(ref MNet_LinAlg.Double.DenseMatrix matrix)
            : base(matrix.RowCount, matrix.ColumnCount)
        {
            _storedMatrix = matrix;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="DenseMatrix"/> for the addition. 
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="DenseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="DenseMatrix"/>. </param>
        /// <returns> The <see cref="DenseMatrix"/> of the given size, with zeros on every coordinates. </returns>
        public static DenseMatrix Zero(int rowCount, int columnCount) => new DenseMatrix(rowCount, columnCount);

        /// <summary>
        /// Returns the neutral <see cref="Matrix"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="Matrix"/>. </param>
        /// <returns> The <see cref="DenseMatrix"/> of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static DenseMatrix Identity(int size)
        {
            DenseMatrix result = new DenseMatrix(size, size);

            for (int i = 0; i < size; i++) { result._storedMatrix[i, i] = 1d; }

            return result;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Transposes the <see cref="DenseMatrix"/>
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to tranpose. </param>
        /// <returns> The new <see cref="DenseMatrix"/>, transposed of the initial one. </returns>
        public static DenseMatrix Transpose(DenseMatrix matrix)
        {
            MNet_LinAlg.Double.DenseMatrix result = MNet_LinAlg.Double.DenseMatrix.Create(matrix.ColumnCount, matrix.RowCount, (int i_R, int i_C) => matrix[i_C, i_R]);

            return new DenseMatrix(ref result);
        }


        /******************** Algebraic Near Ring ********************/

        /// <inheritdoc cref="operator +(DenseMatrix, DenseMatrix)"/>
        public static DenseMatrix Add(DenseMatrix left, DenseMatrix right) => left + right;

        /// <inheritdoc cref="operator -(DenseMatrix, DenseMatrix)"/>
        public static DenseMatrix Subtract(DenseMatrix left, DenseMatrix right) => left - right;


        /// <inheritdoc cref="operator -(DenseMatrix)"/>
        public static DenseMatrix Opposite(DenseMatrix operand) => -operand;


        /// <inheritdoc cref="operator *(DenseMatrix, DenseMatrix)"/>
        public static DenseMatrix Multiply(DenseMatrix left, DenseMatrix right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="DenseMatrix"/> with its transposition : <c>A<sup>t</sup>·A</c>.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix"/> for the operation. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the operation. </returns>
        public static DenseMatrix TransposeMultiplySelf(DenseMatrix operand)
        {
            int count = operand.RowCount;
            int rowCount = operand.ColumnCount, columnCount = operand.ColumnCount;

            double[,] values = new double[rowCount, columnCount];
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    double value = 0d;
                    for (int i = 0; i < count; i++) { value += operand[i, i_R] * operand[i, i_C]; }

                    values[i_R, i_C] = value;
                }
            }

            return new DenseMatrix(values);
        }


        /******************** Embedding : CompressedColumn  ********************/

        /// <inheritdoc cref="operator +(DenseMatrix, Sparse.CompressedColumn)"/>
        public static DenseMatrix Add(DenseMatrix left, Sparse.CompressedColumn right) => left + right;

        /// <summary>
        /// Computes the left addition of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Sparse.CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(Sparse.CompressedColumn left, DenseMatrix right) => Sparse.CompressedColumn.Add(left, right);


        /// <inheritdoc cref="operator -(DenseMatrix, Sparse.CompressedColumn)"/>
        public static DenseMatrix Subtract(DenseMatrix left, Sparse.CompressedColumn right) => left - right;

        /// <summary>
        /// Computes the left subtraction of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Sparse.CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(Sparse.CompressedColumn left, DenseMatrix right) => Sparse.CompressedColumn.Subtract(left, right);


        /// <inheritdoc cref="operator *(DenseMatrix, Sparse.CompressedColumn)"/>
        public static DenseMatrix Multiply(DenseMatrix left, Sparse.CompressedColumn right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Sparse.CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(Sparse.CompressedColumn left, DenseMatrix right) => Sparse.CompressedColumn.Multiply(left, right);


        /******************** Embedding : CompressedRow ********************/

        /// <inheritdoc cref="operator +(DenseMatrix, Sparse.CompressedRow)"/>
        public static DenseMatrix Add(DenseMatrix left, Sparse.CompressedRow right) => left + right;

        /// <summary>
        /// Computes the right addition of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Sparse.CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix Add(Sparse.CompressedRow left, DenseMatrix right) => Sparse.CompressedRow.Add(left, right);


        /// <inheritdoc cref="operator -(DenseMatrix, Sparse.CompressedRow)"/>
        public static DenseMatrix Subtract(DenseMatrix left, Sparse.CompressedRow right) => left - right;

        /// <summary>
        /// Computes the right subtraction of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Sparse.CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix Subtract(Sparse.CompressedRow left, DenseMatrix right) => Sparse.CompressedRow.Subtract(left, right);


        /// <inheritdoc cref="operator *(DenseMatrix, Sparse.CompressedRow)"/>
        public static DenseMatrix Multiply(DenseMatrix left, Sparse.CompressedRow right) => left * right;

        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Sparse.CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix Multiply(Sparse.CompressedRow left, DenseMatrix right) => Sparse.CompressedRow.Multiply(left, right);


        /******************** Embedding : Sparse Matrix ********************/

        /// <inheritdoc cref="operator +(DenseMatrix, SparseMatrix)"/>
        public static DenseMatrix Add(DenseMatrix left, SparseMatrix right) => left + right;

        /// <summary>
        /// Computes the left addition of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/> on the right.
        /// </summary>
        /// <param name="right"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="left"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The left addition of a <see cref="DenseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static DenseMatrix Add(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft + right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft + right; }
            else { throw new NotImplementedException($"The left addition of a {right.GetType()} with a {left.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <inheritdoc cref="operator -(DenseMatrix, SparseMatrix)"/>
        public static DenseMatrix Subtract(DenseMatrix left, SparseMatrix right) => left - right;

        /// <summary>
        /// Computes the left subtraction of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/> on the right.
        /// </summary>
        /// <param name="right"> Left <see cref="SparseMatrix"/> to subtract with from the left. </param>
        /// <param name="left"> Right <see cref="DenseMatrix"/> to subtract on the left. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The left subtraction of a <see cref="DenseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static DenseMatrix Subtract(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft - right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft - right; }
            else { throw new NotImplementedException($"The left addition of a {right.GetType()} with a {left.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <inheritdoc cref="operator *(DenseMatrix, SparseMatrix)"/>
        public static DenseMatrix Multiply(DenseMatrix left, SparseMatrix right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/> on the right.
        /// </summary>
        /// <param name="right"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="left"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The left multiplication of a <see cref="DenseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static DenseMatrix Multiply(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft * right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft * right; }
            else { throw new NotImplementedException($"The left multiplication of a {right.GetType()} with a {left.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /******************** Embedding : Matrix ********************/

        /// <inheritdoc cref="operator +(DenseMatrix, Matrix)"/>
        public static DenseMatrix Add(DenseMatrix left, Matrix right) => left + right;

        /// <summary>
        /// Computes the left addition of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/> on the right.
        /// </summary>
        /// <param name="right"> Left <see cref="Matrix"/> for the addition. </param>
        /// <param name="left"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The left addition of a <see cref="DenseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static DenseMatrix Add(Matrix left, DenseMatrix right)
        {
            if (left is DenseMatrix denseLeft) { return denseLeft + right; }
            else if (left is SparseMatrix sparseLeft) { return sparseLeft + right; }
            else { throw new NotImplementedException($"The left addition of a {right.GetType()} with a {left.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <inheritdoc cref="operator -(DenseMatrix, Matrix)"/>
        public static DenseMatrix Subtract(DenseMatrix left, Matrix right) => left - right;

        /// <summary>
        /// Computes the left subtraction of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/> on the right.
        /// </summary>
        /// <param name="right"> Left <see cref="Matrix"/> to subtract with from the left. </param>
        /// <param name="left"> Right <see cref="DenseMatrix"/> to subtract on the left. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The left subtraction of a <see cref="DenseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static DenseMatrix Subtract(Matrix left, DenseMatrix right)
        {
            if (left is DenseMatrix denseLeft) { return denseLeft - right; }
            else if (left is SparseMatrix sparseLeft) { return sparseLeft - right; }
            else { throw new NotImplementedException($"The left subtraction of a {right.GetType()} with a {left.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <inheritdoc cref="operator *(DenseMatrix, Matrix)"/>
        public static DenseMatrix Multiply(DenseMatrix left, Matrix right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/> on the right.
        /// </summary>
        /// <param name="right"> Left <see cref="Matrix"/> for the multiplication. </param>
        /// <param name="left"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The left multiplication of a <see cref="DenseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static DenseMatrix Multiply(Matrix left, DenseMatrix right)
        {
            if (left is DenseMatrix denseLeft) { return denseLeft * right; }
            else if (left is SparseMatrix sparseLeft) { return sparseLeft * right; }
            else { throw new NotImplementedException($"The left multiplication of a {right.GetType()} with a {left.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(DenseMatrix, double)"/>
        public static DenseMatrix Multiply(DenseMatrix operand, double factor) => operand * factor;

        /// <inheritdoc cref="operator *(double, DenseMatrix)"/>
        public static DenseMatrix Multiply(double factor, DenseMatrix operand) => factor * operand;


        /// <inheritdoc cref="operator *(DenseMatrix, double)"/>
        public static DenseMatrix Divide(DenseMatrix operand, double divisor) => operand / divisor;


        /******************** Other Operations ********************/

        /// <inheritdoc cref="operator *(DenseMatrix, Vect.Vector)"/>
        public static Vect.Vector Multiply(DenseMatrix matrix, Vect.Vector vector) => matrix * vector;

        /// <inheritdoc cref="operator *(DenseMatrix, Vect.DenseVector)"/>
        public static Vect.DenseVector Multiply(DenseMatrix matrix, Vect.DenseVector vector) => matrix * vector;

        /// <inheritdoc cref="operator *(DenseMatrix, Vect.SparseVector)"/>
        public static Vect.DenseVector Multiply(DenseMatrix matrix, Vect.SparseVector vector) => matrix * vector;


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="DenseMatrix"/> with a <see cref="Vect.Vector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the transposed <see cref="DenseMatrix"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector TransposeMultiply(DenseMatrix matrix, Vect.Vector vector)
        {
            if (vector is Vect.DenseVector denseVector) { return DenseMatrix.TransposeMultiply(matrix, denseVector); }
            else if (vector is Vect.SparseVector sparseVector) { return DenseMatrix.TransposeMultiply(matrix, sparseVector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} with a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="DenseMatrix"/> with a <see cref="Vect.DenseVector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector TransposeMultiply(DenseMatrix matrix, Vect.DenseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            for (int i_R = 0; i_R < components.Length; i_R++)
            {
                double component = 0d;
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    component += matrix[i, i_R] * vector[i];
                }
                components[i_R] = component;
            }

            return new Vect.DenseVector(components);
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="DenseMatrix"/> with a <see cref="Vect.SparseVector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector TransposeMultiply(DenseMatrix matrix, Vect.SparseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            foreach (var (rowIndex, value) in vector.NonZeros())
            {
                for (int i_R = 0; i_R < components.Length; i_R++)
                {
                    components[i_R] += matrix[rowIndex, i_R] * value;
                }
            }

            return new Vect.DenseVector(components);
        }

        #endregion

        #region Operators

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix operator +(DenseMatrix left, DenseMatrix right)
        {
            MNet_LinAlg.Double.DenseMatrix result = (MNet_LinAlg.Double.DenseMatrix)left._storedMatrix.Add(right._storedMatrix);

            return new DenseMatrix(ref result);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix operator -(DenseMatrix left, DenseMatrix right)
        {
            MNet_LinAlg.Double.DenseMatrix result = (MNet_LinAlg.Double.DenseMatrix) left._storedMatrix.Subtract(right._storedMatrix) ;

            return new DenseMatrix(ref result);
        }


        /// <summary>
        /// Computes the opposite of the <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix"/> whose opposite is to be computed. </param>
        /// <returns> The <see cref="DenseMatrix"/>, opposite of the initial one. </returns>
        public static DenseMatrix operator -(DenseMatrix operand)
        {
            MNet_LinAlg.Double.DenseMatrix result = MNet_LinAlg.Double.DenseMatrix.Create(operand.RowCount, operand.ColumnCount, (int i_R, int i_C) => -operand[i_R, i_C]);

            return new DenseMatrix(ref result);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of left and the number of rows of right must be equal. </exception>
        public static DenseMatrix operator *(DenseMatrix left, DenseMatrix right)
        {
            MNet_LinAlg.Double.DenseMatrix result = (MNet_LinAlg.Double.DenseMatrix) left._storedMatrix.Multiply(right._storedMatrix);

            return new DenseMatrix(ref result);
        }


        /******************** Embedding : CompressedColumn ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the matrices must be the same. </exception>
        public static DenseMatrix operator +(DenseMatrix left, Sparse.CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The size of the matrices must be the same.");
            }

            MNet_LinAlg.Double.DenseMatrix result = (MNet_LinAlg.Double.DenseMatrix)left._storedMatrix.Clone();

            // Iterate on the columns of right
            for (int i_C = 0; i_C < right.ColumnCount; i_C++)
            {
                // Iterate on the non-zero values of the current right column
                for (int i = right.ColumnPointer(i_C); i < right.ColumnPointer(i_C + 1); i++)
                {
                    result[right.RowIndex(i), i_C] += right[i];
                }
            }

            return new DenseMatrix(ref result);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the matrices must be the same. </exception>
        public static DenseMatrix operator -(DenseMatrix left, Sparse.CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The size of the matrices must be the same.");
            }

            MNet_LinAlg.Double.DenseMatrix result = (MNet_LinAlg.Double.DenseMatrix)left._storedMatrix.Clone();

            // Iterate on the columns of right
            for (int i_C = 0; i_C < right.ColumnCount; i_C++)
            {
                // Iterate on the non-zero values of the current right column
                for (int i = right.ColumnPointer(i_C); i < right.ColumnPointer(i_C + 1); i++)
                {
                    result[right.RowIndex(i), i_C] -= right[i];
                }
            }

            return new DenseMatrix(ref result);
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of left and the number of rows of right must be equal. </exception>
        public static DenseMatrix operator *(DenseMatrix left, Sparse.CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount) { throw new ArgumentException("The number of columns of left and the number of rows of right must be equal."); }

            double[,] result = new double[left.RowCount, right.ColumnCount];

            // Iterate on the columns of right 
            for (int i_C = 0; i_C < right.ColumnCount; i_C++)
            {
                // Iterate on the rows of left
                for (int i_R = 0; i_R < left.RowCount; i_R++)
                {
                    double sum = 0d;

                    // Iterate on the non-zero values of the current right column
                    for (int i_NZ = right.ColumnPointer(i_C); i_NZ < right.ColumnPointer(i_C + 1); i_NZ++)
                    {
                        sum += left[i_R, right.RowIndex(i_NZ)] * right[i_NZ];
                    }

                    result[i_R, i_C] = sum;
                }
            }

            return new DenseMatrix(result);
        }


        /******************** Embedding :  CompressedRow ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the matrices must be the same. </exception>
        public static DenseMatrix operator +(DenseMatrix left, Sparse.CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            MNet_LinAlg.Double.DenseMatrix result = (MNet_LinAlg.Double.DenseMatrix)left._storedMatrix.Clone();

            // Iterate on the rows of right
            for (int i_R = 0; i_R < right.RowCount; i_R++)
            {
                // Iterate on the non-zero values of the current right row
                for (int i_NZ = right.RowPointer(i_R); i_NZ < right.RowPointer(i_R + 1); i_NZ++)
                {
                    result[i_R, right.ColumnIndex(i_NZ)] += right[i_NZ];
                }
            }

            return new DenseMatrix(ref result);
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the matrices must be the same. </exception>
        public static DenseMatrix operator -(DenseMatrix left, Sparse.CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            MNet_LinAlg.Double.DenseMatrix result = (MNet_LinAlg.Double.DenseMatrix)left._storedMatrix.Clone();

            // Iterate on the rows of right
            for (int i_R = 0; i_R < right.RowCount; i_R++)
            {
                // Iterate on the non-zero values of the current right row
                for (int i_NZ = right.RowPointer(i_R); i_NZ < right.RowPointer(i_R + 1); i_NZ++)
                {
                    result[i_R, right.ColumnIndex(i_NZ)] -= right[i_NZ];
                }
            }

            return new DenseMatrix(ref result);
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="Sparse.CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Sparse.CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of left and the number of rows of right must be equal. </exception>
        public static DenseMatrix operator *(DenseMatrix left, Sparse.CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount) { throw new ArgumentException("The number of columns of left and the number of rows of right must be equal."); }

            int rowCount = left.RowCount;
            int columnCount = right.ColumnCount;
            double[,] result = new double[rowCount, columnCount];

            // Iterate on the rows of right
            int i_NZ = right.RowPointer(0);
            for (int i_RR = 0; i_RR < right.RowCount; i_RR++)
            {
                // Iterate on the non-zero values of current right row
                for (; i_NZ < right.RowPointer(i_RR + 1); i_NZ++)
                {
                    int i_C = right.ColumnIndex(i_NZ);
                    double val = right[i_NZ];

                    // Iterate on the rows of left
                    for (int i_R = 0; i_R < rowCount; i_R++)
                    {
                        result[i_R, i_C] += left[i_R, i_RR] * val;
                    }
                }
            }

            return new DenseMatrix(result);
        }


        /******************** Embedding : Sparse Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The right addition of a <see cref="DenseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static DenseMatrix operator +(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return left + ccsRight; }
            else if (right is Sparse.CompressedRow crsRight) { return left + crsRight; }
            else { throw new NotImplementedException($"The right addition of a {left.GetType()} with a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of a <see cref="DenseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static DenseMatrix operator -(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return left - ccsRight; }
            else if (right is Sparse.CompressedRow crsRight) { return left - crsRight; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} with a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of a <see cref="DenseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. 
        /// </exception>
        public static DenseMatrix operator *(DenseMatrix left, SparseMatrix right)
        {
            if (right is Sparse.CompressedColumn ccsRight) { return left * ccsRight; }
            else if (right is Sparse.CompressedRow crsRight) { return left * crsRight; }
            else { throw new NotImplementedException($"The right multiplication of a {left.GetType()} with a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /******************** Embedding : Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/> on the right.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The right addition of a <see cref="DenseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static DenseMatrix operator +(DenseMatrix left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left + denseRight; }
            else if (right is SparseMatrix sparseRight) { return left + sparseRight; }
            else { throw new NotImplementedException($"The right addition of a {left.GetType()} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of a <see cref="DenseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static DenseMatrix operator -(DenseMatrix left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left - denseRight; }
            else if (right is SparseMatrix sparseRight) { return left - sparseRight; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a <see cref="DenseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static DenseMatrix operator *(DenseMatrix left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left * denseRight; }
            else if (right is SparseMatrix sparseRight) { return left * sparseRight; }
            else { throw new NotImplementedException($"The right multiplication of a {left.GetType()} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="DenseMatrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the right scalar multiplication. </returns>
        public static DenseMatrix operator *(DenseMatrix operand, double factor)
        {
            int rowCount = operand.RowCount, columnCount = operand.ColumnCount;

            double[,] values = new double[rowCount, columnCount];
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    values[i_R, i_C] = operand[i_R, i_C] * factor;
                }
            }

            return new DenseMatrix(values);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="DenseMatrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix"/> to multiply on the left. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the left scalar multiplication. </returns>
        public static DenseMatrix operator *(double factor, DenseMatrix operand)
        {
            int rowCount = operand.RowCount, columnCount = operand.ColumnCount;

            double[,] values = new double[rowCount, columnCount];
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    values[i_R, i_C] = factor * operand[i_R, i_C];
                }
            }

            return new DenseMatrix(values);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="DenseMatrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="DenseMatrix"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the scalar division. </returns>
        public static DenseMatrix operator /(DenseMatrix operand, double divisor)
        {
            int rowCount = operand.RowCount, columnCount = operand.ColumnCount;

            double[,] values = new double[rowCount, columnCount];
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (int i_C = 0; i_C < columnCount; i_C++)
                {
                    values[i_R, i_C] = operand[i_R, i_C] / divisor;
                }
            }

            return new DenseMatrix(values);
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="Vect.Vector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a <see cref="DenseMatrix"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector operator *(DenseMatrix matrix, Vect.Vector vector)
        {
            if (vector is Vect.DenseVector denseVector) { return matrix * denseVector; }
            else if (vector is Vect.SparseVector sparseVector) { return matrix * sparseVector; }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} with a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="Vect.DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector operator *(DenseMatrix matrix, Vect.DenseVector vector)
        {
            // Verifications
            if (matrix.ColumnCount != vector.Size) { throw new ArgumentException("The number of columns of the matrix and the number of rows of the vector must be equal."); }

            double[] components = new double[matrix.RowCount];

            for (int i_R = 0; i_R < components.Length; i_R++)
            {
                double component = 0d;
                for (int i = 0; i < matrix.ColumnCount; i++)
                {
                    component += matrix[i_R, i] * vector[i];
                }
                components[i_R] = component;
            }

            return new Vect.DenseVector(components);
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="DenseMatrix"/> with a <see cref="Vect.SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="DenseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector operator *(DenseMatrix matrix, Vect.SparseVector vector)
        {
            // Verifications
            if (matrix.ColumnCount != vector.Size) { throw new ArgumentException("The number of columns of the matrix and the number of rows of the vector must be equal."); }

            double[] components = new double[matrix.RowCount];

            foreach ((int rowIndex, double value) in vector.NonZeros())
            {
                for (int i_R = 0; i_R < components.Length; i_R++)
                {
                    components[i_R] += matrix[i_R, rowIndex] * value;
                }
            }

            return new Vect.DenseVector(components);
        }

        #endregion


        #region Overrides

        /******************** Matrix ********************/

        /// <inheritdoc cref="Matrix.ToArray()"/>
        public override double[,] ToArray() => _storedMatrix.ToArray();

        /// <inheritdoc cref="Matrix.At(int, int)"/>
        public override double At(int row, int column) => _storedMatrix[row, column];

        #endregion

        #region Explicit Implementations

        /******************** IGroupAction<DenseMatrix, double> ********************/

        /// <inheritdoc/>
        DenseMatrix Alg_Sets.IGroupAction<DenseMatrix, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        DenseMatrix Alg_Sets.IGroupAction<DenseMatrix, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}
