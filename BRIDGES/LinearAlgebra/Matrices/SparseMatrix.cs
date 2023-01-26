using System;
using System.Collections.Generic;

using Alg_Sets = BRIDGES.Algebra.Sets;

using Vect = BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a sparse matrix.
    /// </summary>
    public abstract class SparseMatrix : Matrix,
        Alg_Sets.IGroupAction<SparseMatrix, double>
    {
        #region Properties

        /// <summary>
        /// Gets the number of non-zero values in this sparse matrix.
        /// </summary>
        public abstract int NonZerosCount { get; }

        /// <summary>
        /// Gets or sets the non-zero value of this sparse matrix at a given index.
        /// </summary>
        /// <param name="index"> Index of the non-zero value to get.</param>
        /// <returns> The non-zero value of this sparse matrix at the given index. </returns>
        public abstract double this[int index] { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="SparseMatrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="SparseMatrix"/>. </param>
        protected SparseMatrix(int rowCount, int columnCount)
            : base (rowCount, columnCount)
        {
            /* Do Nothing */
        }

        #endregion

        #region Static Methods


        /// <summary>
        /// Transposes the <see cref="SparseMatrix"/>
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to tranpose. </param>
        /// <returns> The new <see cref="SparseMatrix"/>, transposed of the initial one. </returns>
        /// <exception cref="NotImplementedException"> The transposition of the matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix Transpose(SparseMatrix matrix)
        {
            if (matrix is Sparse.CompressedColumn ccs) { return Sparse.CompressedColumn.Transpose(ccs); }
            else if (matrix is Sparse.CompressedRow crs) { return Sparse.CompressedRow.Transpose(crs); }
            else { throw new NotImplementedException($"The transposition of a {matrix.GetType()} as a {typeof(SparseMatrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Algebraic Near Ring ********************/

        /// <inheritdoc cref="operator +(SparseMatrix, SparseMatrix)"/>
        public static SparseMatrix Add(SparseMatrix left, SparseMatrix right) => left + right;

        /// <inheritdoc cref="operator -(SparseMatrix, SparseMatrix)"/>
        public static SparseMatrix Subtract(SparseMatrix left, SparseMatrix right) => left - right;


        /// <inheritdoc cref="operator *(SparseMatrix, SparseMatrix)"/>
        public static SparseMatrix Multiply(SparseMatrix left, SparseMatrix right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="SparseMatrix"/> with its transposition : <c>A<sup>t</sup>·A</c>.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix"/> for the operation. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the operation. </returns>
        public static SparseMatrix TransposeMultiplySelf(SparseMatrix operand)
        {
            if (operand is Sparse.CompressedColumn ccs) { return Sparse.CompressedColumn.TransposeMultiplySelf(ccs); }
            else if (operand is Sparse.CompressedRow crs) { return Sparse.CompressedRow.TransposeMultiplySelf(crs); }
            else { throw new NotImplementedException($"The left multiplication of a {operand.GetType()} with it transposition as {nameof(SparseMatrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(SparseMatrix, double)"/>
        public static SparseMatrix Multiply(SparseMatrix operand, double factor) => operand * factor;

        /// <inheritdoc cref="operator *(double, SparseMatrix)"/>
        public static SparseMatrix Multiply(double factor, SparseMatrix operand) => factor * operand;


        /// <inheritdoc cref="operator /(SparseMatrix, double)"/>
        public static SparseMatrix Divide(SparseMatrix operand, double divisor) => operand / divisor;


        /******************** Other Operations ********************/

        /// <inheritdoc cref="operator *(SparseMatrix, Vect.Vector)"/>
        public static Vect.Vector Multiply(SparseMatrix matrix, Vect.Vector vector) => matrix * vector;

        /// <inheritdoc cref="operator *(SparseMatrix, Vect.DenseVector)"/>
        public static Vect.DenseVector Multiply(SparseMatrix matrix, Vect.DenseVector vector) => matrix * vector;

        /// <inheritdoc cref="operator *(SparseMatrix, Vect.SparseVector)"/>
        public static Vect.SparseVector Multiply(SparseMatrix matrix, Vect.SparseVector vector) => matrix * vector;


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="SparseMatrix"/> with a <see cref="Vect.Vector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a transposed matrix as a <see cref="SparseMatrix"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector TransposeMultiply(SparseMatrix matrix, Vect.Vector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.TransposeMultiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.TransposeMultiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(SparseMatrix)} with a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="Vect.DenseVector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a transposed matrix as a <see cref="SparseMatrix"/> with a <see cref="Vect.DenseVector"/> is not implemented. </exception>
        public static Vect.DenseVector TransposeMultiply(SparseMatrix matrix, Vect.DenseVector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.TransposeMultiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.TransposeMultiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(SparseMatrix)} with a {vector.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="Vect.SparseVector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a transposed matrix as a <see cref="SparseMatrix"/> with a <see cref="Vect.SparseVector"/> is not implemented. </exception>
        public static Vect.SparseVector TransposeMultiply(SparseMatrix matrix, Vect.SparseVector vector)
        {
            if (matrix is Sparse.CompressedColumn ccsMatrix) { return Sparse.CompressedColumn.TransposeMultiply(ccsMatrix, vector); }
            else if (matrix is Sparse.CompressedRow crsMatrix) { return Sparse.CompressedRow.TransposeMultiply(crsMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(SparseMatrix)} with a {vector.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        #endregion

        #region Operators

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The <see cref="SparseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix operator +(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft + right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft + right; }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} as a {typeof(SparseMatrix)} with a {right.GetType()} as a {typeof(SparseMatrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The <see cref="SparseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix operator -(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft - right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft - right; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as a {typeof(SparseMatrix)} with a {right.GetType()} as a {typeof(SparseMatrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /// <summary>
        /// Computes the opposite of the <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix"/> whose opposite is to be computed. </param>
        /// <returns> The <see cref="SparseMatrix"/>, opposite of the initial one. </returns>
        /// <exception cref="NotImplementedException"> The unary negation of a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix operator -(SparseMatrix operand)
        {
            if (operand is Sparse.CompressedColumn ccs) { return -ccs; }
            else if (operand is Sparse.CompressedRow crs) { return -crs; }
            else { throw new NotImplementedException($"The unary negation of a {operand.GetType()} as a {typeof(SparseMatrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The <see cref="SparseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The multiplication of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static SparseMatrix operator *(SparseMatrix left, SparseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft * right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft * right; }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} as a {typeof(SparseMatrix)} with a {right.GetType()} as a {typeof(SparseMatrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Embed In : DenseMatrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/> on the right.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The right addition of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="DenseMatrix"/> is not implemented. </exception>
        public static DenseMatrix operator +(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft + right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft + right; }
            else { throw new NotImplementedException($"The right addition of a {left.GetType()} as a {nameof(SparseMatrix)} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="DenseMatrix"/> is not implemented.
        /// </exception>
        public static DenseMatrix operator -(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft - right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft - right; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as a {nameof(SparseMatrix)} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="DenseMatrix"/> is not implemented. </exception>
        public static DenseMatrix operator *(SparseMatrix left, DenseMatrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft * right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft * right; }
            else { throw new NotImplementedException($"The right multiplication of a {left.GetType()} as a {nameof(SparseMatrix)} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /******************** Embed In : Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="SparseMatrix"/> with a <see cref="Matrix"/> on the right.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the addition. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The right addition of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix operator +(SparseMatrix left, Matrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft + right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft + right; }
            else { throw new NotImplementedException($"The right addition of a {left.GetType()} as a {nameof(SparseMatrix)} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix"/> to subtract with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented.
        /// </exception>
        public static Matrix operator -(SparseMatrix left, Matrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft - right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft - right; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as a {nameof(SparseMatrix)} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a matrix as a <see cref="SparseMatrix"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix operator *(SparseMatrix left, Matrix right)
        {
            if (left is Sparse.CompressedColumn ccsLeft) { return ccsLeft * right; }
            else if (left is Sparse.CompressedRow crsLeft) { return crsLeft * right; }
            else { throw new NotImplementedException($"The right multiplication of a {left.GetType()} as a {nameof(SparseMatrix)} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="SparseMatrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the right scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right scalar multiplication of a matrix as a <see cref="SparseMatrix"/> with a <see cref="double"/> is not implemented. </exception>
        public static SparseMatrix operator *(SparseMatrix operand, double factor)
        {
            if (operand is Sparse.CompressedColumn ccs) { return ccs * factor; }
            else if (operand is Sparse.CompressedRow crs) { return crs * factor; }
            else { throw new NotImplementedException($"The right scalar multiplication of a {operand.GetType()} as a {typeof(SparseMatrix)} with a {factor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="SparseMatrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix"/> to multiply on the left. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the left scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The left scalar multiplication of a matrix as a <see cref="SparseMatrix"/> with a <see cref="double"/> is not implemented. </exception>
        public static SparseMatrix operator *(double factor, SparseMatrix operand)
        {
            if (operand is Sparse.CompressedColumn ccs) { return factor * ccs; }
            else if (operand is Sparse.CompressedRow crs) { return factor * crs; }
            else { throw new NotImplementedException($"The left scalar multiplication of a {operand.GetType()} as a {typeof(SparseMatrix)} with a {factor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="SparseMatrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="SparseMatrix"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="SparseMatrix"/> resulting from the scalar division. </returns>
        /// <exception cref="NotImplementedException"> The scalar division of a matrix as a <see cref="SparseMatrix"/> with a <see cref="double"/> is not implemented. </exception>
        public static SparseMatrix operator /(SparseMatrix operand, double divisor)
        {
            if (operand is Sparse.CompressedColumn ccs) { return ccs / divisor; }
            else if (operand is Sparse.CompressedRow crs) { return crs / divisor; }
            else { throw new NotImplementedException($"The scalar division of a {operand.GetType()} as a {typeof(SparseMatrix)} with a {divisor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="Vect.Vector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a matrix as a <see cref="SparseMatrix"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector operator *(SparseMatrix matrix, Vect.Vector vector)
        {
            if (matrix is Sparse.CompressedColumn ccs) { return ccs * vector; }
            else if (matrix is Sparse.CompressedRow crs) { return crs * vector; }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(SparseMatrix)} with a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="Vect.DenseVector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a matrix as a <see cref="SparseMatrix"/> with a <see cref="Vect.DenseVector"/> is not implemented. </exception>
        public static Vect.DenseVector operator *(SparseMatrix matrix, Vect.DenseVector vector)
        {
            if (matrix is Sparse.CompressedColumn ccs) { return ccs * vector; }
            else if (matrix is Sparse.CompressedRow crs) { return crs * vector; }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(SparseMatrix)} with a {vector.GetType()} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="SparseMatrix"/> with a <see cref="Vect.SparseVector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="SparseMatrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a matrix as a <see cref="SparseMatrix"/> with a vector as a <see cref="Vect.SparseVector"/> is not implemented. </exception>
        public static Vect.SparseVector operator *(SparseMatrix matrix, Vect.SparseVector vector)
        {
            if (matrix is Sparse.CompressedColumn ccs) { return ccs * vector; }
            else if (matrix is Sparse.CompressedRow crs) { return crs * vector; }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(SparseMatrix)} with a {vector.GetType()} as a {nameof(Vect.SparseVector)} is not implemented."); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provides an enumerable collection to iterate on the non-zero entries of the <see cref="SparseMatrix"/> non-zero entries.
        /// </summary>
        /// <returns> The enumerable collection containing row index, the column index and the value of the <see cref="SparseMatrix"/> non-zero entries. </returns>
        public abstract IEnumerable<(int rowIndex, int columnIndex, double value)> NonZeros();

        #endregion


        #region Overrides

        /******************** Matrix ********************/

        /// <inheritdoc cref="Matrix.ToArray()"/>
        public override double[,] ToArray()
        {
            double[,] result = new double[RowCount, ColumnCount];
            foreach((int rowIndex, int columnIndex, double value) in NonZeros())
            {
                result[rowIndex, columnIndex] = value;
            }

            return result;
        }

        #endregion

        #region Explicit Implementations

        /******************** IGroupAction<SparseMatrix, double> ********************/

        /// <inheritdoc/>
        SparseMatrix Alg_Sets.IGroupAction<SparseMatrix, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        SparseMatrix Alg_Sets.IGroupAction<SparseMatrix, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}
