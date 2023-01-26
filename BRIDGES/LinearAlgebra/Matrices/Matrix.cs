using System;
using System.Collections.Generic;

using Alg_Sets = BRIDGES.Algebra.Sets;

using Vect = BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices
{
    /// <summary>
    /// Class defining a matrix.
    /// </summary>
    public abstract class Matrix :
        Alg_Sets.IGroupAction<Matrix, double>
    {
        #region Properties

        /// <summary>
        /// Gets the number of rows in this matrix.
        /// </summary>
        public abstract int RowCount { get; }

        /// <summary>
        /// Gets the number of columns in this matrix.
        /// </summary>
        public abstract int ColumnCount { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="Matrix"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="Matrix"/>. </param>
        protected Matrix(int rowCount, int columnCount)
        {
            /* Do Nothing */
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Transposes the <see cref="Matrix"/>
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to tranpose. </param>
        /// <returns> The new <see cref="Matrix"/>, transposed of the initial one. </returns>
        /// <exception cref="NotImplementedException"> The transposition of the matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix Transpose(Matrix matrix)
        {
            if (matrix is DenseMatrix dense) { return DenseMatrix.Transpose(dense); }
            else if (matrix is SparseMatrix sparse) { return SparseMatrix.Transpose(sparse); }
            else { throw new NotImplementedException($"The transposition of a {matrix.GetType()} as a {typeof(Matrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Algebraic Near Ring ********************/

        /// <inheritdoc cref="operator +(Matrix, Matrix)"/>
        public static Matrix Add(Matrix left, Matrix right) => left + right;

        /// <inheritdoc cref="operator -(Matrix, Matrix)"/>
        public static Matrix Subtract(Matrix left, Matrix right) => left - right;


        /// <inheritdoc cref="operator -(Matrix)"/>
        public static Matrix Opposite(Matrix operand) => -operand;


        /// <inheritdoc cref="operator *(Matrix, Matrix)"/>
        public static Matrix Multiply(Matrix left, Matrix right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="Matrix"/> with its transposition : <c>A<sup>t</sup>·A</c>.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix"/> for the operation. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the operation. </returns>
        /// <exception cref="NotImplementedException"> The left multiplication a matrix as a <see cref="Matrix"/> with its transposition is not implemented. </exception>
        public static Matrix TransposeMultiplySelf(Matrix operand)
        {
            if (operand is DenseMatrix dense) { return DenseMatrix.TransposeMultiplySelf(dense); }
            else if (operand is SparseMatrix sparse) { return SparseMatrix.TransposeMultiplySelf(sparse); }
            else { throw new NotImplementedException($"The left multiplication of a {operand.GetType()} as {nameof(Matrix)} with it transposition is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(double, Matrix)"/>
        public static Matrix Multiply(double factor, Matrix operand) => factor * operand;

        /// <inheritdoc cref="operator *(Matrix, double)"/>
        public static Matrix Multiply(Matrix operand, double factor) => operand * factor;


        /// <inheritdoc cref="operator /(Matrix, double)"/>
        public static Matrix Divide(Matrix operand, double divisor) => operand / divisor;


        /******************** Other Operations ********************/

        /// <inheritdoc cref="operator *(Matrix, Vect.Vector)"/>
        public static Vect.Vector Multiply(Matrix matrix, Vect.Vector vector) => matrix * vector;

        /// <inheritdoc cref="operator *(Matrix, Vect.DenseVector)"/>
        public static Vect.DenseVector Multiply(Matrix matrix, Vect.DenseVector vector) => matrix * vector;

        /// <inheritdoc cref="operator *(Matrix, Vect.SparseVector)"/>
        public static Vect.Vector Multiply(Matrix matrix, Vect.SparseVector vector) => matrix * vector;


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="Matrix"/> with a <see cref="Vect.Vector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the transposed matrix as a <see cref="Matrix"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector TransposeMultiply(Matrix matrix, Vect.Vector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.TransposeMultiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.TransposeMultiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }

        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="Matrix"/> with a <see cref="Vect.DenseVector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the transposed matrix as a <see cref="Matrix"/> with a <see cref="Vect.DenseVector"/> is not implemented. </exception>
        public static Vect.DenseVector TransposeMultiply(Matrix matrix, Vect.DenseVector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.TransposeMultiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.TransposeMultiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} is not implemented."); }

        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="Matrix"/> with a <see cref="Vect.SparseVector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the transposed matrix as a <see cref="Matrix"/> with a vector as a <see cref="Vect.SparseVector"/> is not implemented. </exception>
        public static Vect.Vector TransposeMultiply(Matrix matrix, Vect.SparseVector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.TransposeMultiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.TransposeMultiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} as a {nameof(Matrix)} and a {vector.GetType()} as a {nameof(Vect.SparseVector)} is not implemented."); }

        }

        #endregion

        #region Operators

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the addition. </param>
        /// <returns> The <see cref="Matrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The addition of left as a <see cref="Matrix"/> with right as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left is DenseMatrix denseLeft) { return denseLeft + right; }
            else if (left is SparseMatrix sparseLeft) { return sparseLeft + right; }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} as a {typeof(Matrix)} with a {right.GetType()} as a {typeof(Matrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix"/> to subtract with. </param>
        /// <returns> The <see cref="Matrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of left as a <see cref="Matrix"/> with right as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left is DenseMatrix denseLeft) { return denseLeft - right; }
            else if (left is SparseMatrix sparseLeft) { return sparseLeft - right; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} as a {typeof(Matrix)} with a {right.GetType()} as a {typeof(Matrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /// <summary>
        /// Computes the opposite of the <see cref="Matrix"/>.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix"/> whose opposite is to be computed. </param>
        /// <returns> The <see cref="Matrix"/>, opposite of the initial one. </returns>
        /// <exception cref="NotImplementedException"> The unary negation of the operand as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix operator -(Matrix operand)
        {
            if (operand is DenseMatrix dense) { return -dense; }
            else if (operand is SparseMatrix sparse) { return -sparse; }
            else { throw new NotImplementedException($"The unary negation of a {operand.GetType()} as a {typeof(Matrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="Matrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The <see cref="Matrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The multiplication of left as a <see cref="Matrix"/> with right as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left is DenseMatrix denseLeft) { return denseLeft * right; }
            else if (left is SparseMatrix sparseLeft) { return sparseLeft * right; }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} as a {typeof(Matrix)} with a {right.GetType()} as a {typeof(Matrix)} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="Matrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the right scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right scalar multiplication of the operand as a <see cref="Matrix"/> with a <see cref="double"/> is not implemented. </exception>
        public static Matrix operator *(Matrix operand, double factor)
        {
            if (operand is DenseMatrix dense) { return dense * factor; }
            else if (operand is SparseMatrix sparse) { return sparse * factor; }
            else { throw new NotImplementedException($"The right scalar multiplication of a {operand.GetType()} as a {typeof(Matrix)} with a {factor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="Matrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix"/> to multiply on the left. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the left scalar multiplication. </returns>
        /// <exception cref="NotImplementedException"> The left scalar multiplication of the operand as a <see cref="Matrix"/> with a <see cref="double"/> is not implemented. </exception>
        public static Matrix operator *(double factor, Matrix operand)
        {
            if (operand is DenseMatrix dense) { return factor * dense; }
            else if (operand is SparseMatrix sparse) { return factor * sparse; }
            else { throw new NotImplementedException($"The left scalar multiplication of a {operand.GetType()} as a {typeof(Matrix)} with a {factor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="Matrix"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="Matrix"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the scalar division. </returns>
        /// <exception cref="NotImplementedException"> The scalar division of the operand as a <see cref="Matrix"/> with a <see cref="double"/> is not implemented. </exception>
        public static Matrix operator /(Matrix operand, double divisor)
        {
            if (operand is DenseMatrix dense) { return dense / divisor; }
            else if (operand is SparseMatrix sparse) { return sparse / divisor; }
            else { throw new NotImplementedException($"The scalar division of a {operand.GetType()} as a {typeof(Matrix)} with a {divisor.GetType()} is not implemented."); }
            /* To Do : Use dynamic to do the operation */
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="Matrix"/> with a <see cref="Vect.Vector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the matrix as a <see cref="Matrix"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector operator *(Matrix matrix, Vect.Vector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.Multiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.Multiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(Matrix)} with a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="Matrix"/> with a <see cref="Vect.DenseVector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the matrix as a <see cref="Matrix"/> with a <see cref="Vect.DenseVector"/> is not implemented. </exception>
        public static Vect.DenseVector operator *(Matrix matrix, Vect.DenseVector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.Multiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.Multiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(Matrix)} with a {vector.GetType()} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="Matrix"/> with a <see cref="Vect.SparseVector"/> : <c>A·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="Matrix"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the matrix as a <see cref="Matrix"/> with a vector as a <see cref="Vect.SparseVector"/> is not implemented. </exception>
        public static Vect.Vector operator *(Matrix matrix, Vect.SparseVector vector)
        {
            if (matrix is DenseMatrix denseMatrix) { return DenseMatrix.Multiply(denseMatrix, vector); }
            else if (matrix is SparseMatrix sparseMatrix) { return SparseMatrix.Multiply(sparseMatrix, vector); }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} as a {nameof(Matrix)} with a {vector.GetType()} as a {nameof(Vect.SparseVector)} is not implemented."); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates this matrix into its bi-dimensional array representation.
        /// </summary>
        /// <returns> The bi-dimensional array representing the matrix. </returns>
        public abstract double[,] ToArray();

        /// <summary>
        /// Get the value of the matrix at the given row and column index.
        /// </summary>
        /// <param name="row"> Row index of the value to get. </param>
        /// <param name="column"> Column index of the value to get.</param>
        /// <returns> The value at the given row and column index. </returns>
        public abstract double At(int row, int column);

        #endregion


        #region Explicit Implementations

        /******************** IGroupAction<Matrix, double> ********************/

        /// <inheritdoc/>
        Matrix Alg_Sets.IGroupAction<Matrix, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        Matrix Alg_Sets.IGroupAction<Matrix, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}
