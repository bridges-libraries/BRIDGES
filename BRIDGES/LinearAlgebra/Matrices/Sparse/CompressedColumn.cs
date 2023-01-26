using System;
using System.Collections.Generic;

using C_Stor = CSparse.Storage;

using Alg_Sets = BRIDGES.Algebra.Sets;

using Vect = BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class defining a sparse matrix with a compressed column storage.
    /// </summary>
    public sealed class CompressedColumn : SparseMatrix,
        Alg_Sets.IGroupAction<CompressedColumn, double>
    {
        #region Fields

        /// <summary>
        /// Sparse matrix from CSparse library, in the form of a compressed column storage.
        /// </summary>
        private readonly C_Stor.CompressedColumnStorage<double> _storedMatrix;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int RowCount => _storedMatrix.RowCount;

        /// <inheritdoc/>
        public override int ColumnCount => _storedMatrix.ColumnCount;


        /// <inheritdoc/>
        public override int NonZerosCount => _storedMatrix.NonZerosCount;

        /// <inheritdoc/>
        public override double this[int index]
        {
            get { return _storedMatrix.Values[index]; }
            set { _storedMatrix.Values[index] = value; }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class by defining its size,
        /// and by giving its values in a <see cref="Storage.DictionaryOfKeys"/>.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedColumn"/>. </param>
        /// <param name="dok"> Values of the <see cref="CompressedColumn"/>. </param>
        public CompressedColumn(int rowCount, int columnCount, Storage.DictionaryOfKeys dok)
            : base(rowCount, columnCount)
        {
            List<(int, double)>[] columns = new List<(int, double)>[columnCount];
            for (int i_C = 0; i_C < columnCount; i_C++) { columns[i_C] = new List<(int, double)>(); }
            
            foreach(((int rowIndex, int columnIndex), double value) in dok)
            {
                columns[columnIndex].Add((rowIndex, value));
            }

            // Sorts each columns with regard to the row index
            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                columns[i_C].Sort((x, y) => x.Item1.CompareTo(y.Item1));
            }

            // Creates the new row index and value lists
            int[] columnPointers = new int[columnCount + 1];
            int[] rowIndices = new int[dok.Count];
            double[] values = new double[dok.Count];

            int counter = 0;

            columnPointers[0] = 0;
            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                List<(int, double)> column = columns[i_C];
                int count = column.Count;

                columnPointers[i_C + 1] = columnPointers[i_C] + columns[i_C].Count;

                for (int i_NZ = 0; i_NZ < count; i_NZ++)
                {
                    rowIndices[counter] = column[i_NZ].Item1;
                    values[counter] = column[i_NZ].Item2;
                    counter++;
                }
            }

            _storedMatrix = new CSparse.Double.SparseMatrix(rowCount, columnCount, values, rowIndices, columnPointers);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class from another <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="ccs"> <see cref="CompressedColumn"/> to deep copy. </param>
        public CompressedColumn(CompressedColumn ccs)
            : base(ccs.RowCount, ccs.ColumnCount)
        {
            int[] columnPointer = (int[])ccs._storedMatrix.ColumnPointers.Clone();
            int[] rowIndices = (int[])ccs._storedMatrix.RowIndices.Clone();
            double[] values = (double[])ccs._storedMatrix.Values.Clone();

            _storedMatrix = new CSparse.Double.SparseMatrix(ccs.RowCount, ccs.ColumnCount, values, rowIndices, columnPointer);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedColumn"/>. </param>
        /// <param name="values"> Non-zero values of the <see cref="CompressedColumn"/>. </param>
        /// <param name="rowIndices"> Row indices of the <see cref="CompressedColumn"/>.</param>
        /// <param name="columnPointers"> Column pointers of the <see cref="CompressedColumn"/>. </param>
        internal CompressedColumn(int rowCount, int columnCount, int[] columnPointers, int[] rowIndices, double[] values)
            : base(rowCount, columnCount)
        {
            // Verifications
            if (columnPointers.Length != columnCount + 1)
            {
                throw new ArgumentException("The number of column pointers is not consistent with the number of columns.", nameof(columnPointers));
            }
            if (values.Length != rowIndices.Length)
            {
                throw new ArgumentException($"The number of elements in {nameof(values)} and {nameof(rowIndices)} do not match.");
            }
            if (columnPointers[columnPointers.Length - 1] != values.Length)
            {
                throw new ArgumentException($"The last value of {nameof(columnPointers)} is not equal to the number of non-zero values.");
            }


            _storedMatrix = new CSparse.Double.SparseMatrix(rowCount, columnCount, values, rowIndices, columnPointers);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedColumn"/> class from a CSparse compressed column storage.
        /// </summary>
        /// <param name="ccs"> CSparse compressed column storage. </param>
        private CompressedColumn(ref C_Stor.CompressedColumnStorage<double> ccs)
            : base(ccs.RowCount, ccs.ColumnCount)
        {
            _storedMatrix = ccs;
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="CompressedColumn"/> for the addition. 
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedColumn"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedColumn"/>. </param>
        /// <returns> The <see cref="CompressedColumn"/> of the given size, with zeros on every coordinates. </returns>
        public static CompressedColumn Zero(int rowCount, int columnCount)
        {
            C_Stor.CompressedColumnStorage<double> ccs = new CSparse.Double.SparseMatrix(rowCount, columnCount, 0);

            return new CompressedColumn(ref ccs);
        }

        /// <summary>
        /// Returns the neutral <see cref="CompressedColumn"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="CompressedColumn"/>. </param>
        /// <returns> The <see cref="CompressedColumn"/> of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static CompressedColumn Identity(int size)
        {
            C_Stor.CompressedColumnStorage<double> ccs = CSparse.Double.SparseMatrix.CreateIdentity(size);

            return new CompressedColumn(ref ccs);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Transposes the <see cref="CompressedRow"/>
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to tranpose. </param>
        /// <returns> The new <see cref="CompressedRow"/>, transposed of the initial one. </returns>
        public static CompressedColumn Transpose(CompressedColumn matrix)
        {
            C_Stor.CompressedColumnStorage<double> result = matrix._storedMatrix.Transpose();

            return new CompressedColumn(ref result);
        }


        /******************** Algebraic Near Ring ********************/

        /// <inheritdoc cref="operator +(CompressedColumn, CompressedColumn)"/>
        public static CompressedColumn Add(CompressedColumn left, CompressedColumn right) => left + right;

        /// <inheritdoc cref="operator -(CompressedColumn, CompressedColumn)"/>
        public static CompressedColumn Subtract(CompressedColumn left, CompressedColumn right) => left - right;


        /// <inheritdoc cref="operator *(CompressedColumn, CompressedColumn)"/>
        public static CompressedColumn Multiply(CompressedColumn left, CompressedColumn right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="CompressedColumn"/> with its transposition : <c>A<sup>t</sup>·A</c>.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedColumn"/> for the operation. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the operation. </returns>
        public static CompressedColumn TransposeMultiplySelf(CompressedColumn operand)
        {
            C_Stor.CompressedColumnStorage<double> transposed = operand._storedMatrix.Transpose();

            C_Stor.CompressedColumnStorage<double> result = transposed.Multiply(operand._storedMatrix);

            return new CompressedColumn(ref result);
        }


        /******************** Embedding : CompressedRow ********************/

        /// <inheritdoc cref="operator +(CompressedColumn, CompressedRow)"/>
        public static CompressedColumn Add(CompressedColumn left, CompressedRow right) => left + right;

        /// <summary>
        /// Computes left the addition of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn Add(CompressedRow left, CompressedColumn right)
        {
            CompressedColumn ccs = (CompressedColumn)left;

            return CompressedColumn.Add(ccs, right);
        }


        /// <inheritdoc cref="operator -(CompressedColumn, CompressedRow)"/>
        public static CompressedColumn Subtract(CompressedColumn left, CompressedRow right) => left - right;

        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedColumn"/> on a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn Subtract(CompressedRow left, CompressedColumn right)
        {
            CompressedColumn ccs = (CompressedColumn)left;

            return CompressedColumn.Subtract(ccs, right);
        }


        /// <inheritdoc cref="operator *(CompressedColumn, CompressedRow)"/>
        public static CompressedColumn Multiply(CompressedColumn left, CompressedRow right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn Multiply(CompressedRow left, CompressedColumn right)
        {
            CompressedColumn ccs = (CompressedColumn)left;

            return CompressedColumn.Multiply(ccs, right);
        }

        /******************** Embed In : Dense Matrix ********************/

        /// <inheritdoc cref="operator +(CompressedColumn, DenseMatrix)"/>
        public static DenseMatrix Add(CompressedColumn left, DenseMatrix right) => left + right;

        /// <inheritdoc cref="operator -(CompressedColumn, DenseMatrix)"/>
        public static DenseMatrix Subtract(CompressedColumn left, DenseMatrix right) => left - right;


        /// <inheritdoc cref="operator *(CompressedColumn, DenseMatrix)"/>
        public static DenseMatrix Multiply(CompressedColumn left, DenseMatrix right) => left * right;


        /******************** Embedding : Sparse Matrix ********************/

        /// <inheritdoc cref="operator +(CompressedColumn, SparseMatrix)"/>
        public static CompressedColumn Add(CompressedColumn left, SparseMatrix right) => left + right;

        /// <summary>
        /// Computes the left addition of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of a matrix as a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/> is not implemented.
        /// </exception>
        public static CompressedColumn Add(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Add(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Add(crsLeft, right); }
            else { throw new NotImplementedException($"The left addition of a {right.GetType()} with a {left.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /// <inheritdoc cref="operator -(CompressedColumn, SparseMatrix)"/>
        public static CompressedColumn Subtract(CompressedColumn left, SparseMatrix right) => left - right;

        /// <summary>
        /// Computes the subtraction of a <see cref="SparseMatrix"/> on a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of a <see cref="CompressedColumn"/> on a matrix as a <see cref="SparseMatrix"/> is not implemented.
        /// </exception>
        public static CompressedColumn Subtract(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Subtract(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Subtract(crsLeft, right); }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} on a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /// <inheritdoc cref="operator *(CompressedColumn, SparseMatrix)"/>
        public static CompressedColumn Multiply(CompressedColumn left, SparseMatrix right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of a matrix as a <see cref="SparseMatrix"/> with a <see cref="CompressedColumn"/> is not implemented.
        /// </exception>
        public static CompressedColumn Multiply(SparseMatrix left, CompressedColumn right)
        {
            if (left is CompressedColumn ccsLeft) { return CompressedColumn.Multiply(ccsLeft, right); }
            else if (left is CompressedRow crsLeft) { return CompressedColumn.Multiply(crsLeft, right); }
            else { throw new NotImplementedException($"The left multiplication of a {right.GetType()} with a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /******************** Embed In : Sparse Matrix ********************/

        /// <inheritdoc cref="operator +(CompressedColumn, Matrix)"/>
        public static Matrix Add(CompressedColumn left, Matrix right) => left + right;

        /// <inheritdoc cref="operator -(CompressedColumn, Matrix)"/>
        public static Matrix Subtract(CompressedColumn left, Matrix right) => left - right;


        /// <inheritdoc cref="operator *(CompressedColumn, Matrix)"/>
        public static Matrix Multiply(CompressedColumn left, Matrix right) => left * right;


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(CompressedColumn, double)"/>
        public static CompressedColumn Multiply(CompressedColumn operand, double factor) => operand * factor;

        /// <inheritdoc cref="operator *(double, CompressedColumn)"/>
        public static CompressedColumn Multiply(double factor, CompressedColumn operand) => factor * operand;


        /// <inheritdoc cref="operator /(CompressedColumn, double)"/>
        public static CompressedColumn Divide(CompressedColumn operand, double divisor) => operand / divisor;


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Vect.Vector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of a <see cref="CompressedRow"/> with a vector as a <see cref="Vect.Vector"/> is not implemented.
        /// </exception>
        public static Vect.Vector Multiply(CompressedColumn matrix, Vect.Vector vector) => matrix * vector;

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Vect.DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector Multiply(CompressedColumn matrix, Vect.DenseVector vector) => matrix * vector;

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Vect.SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector Multiply(CompressedColumn matrix, Vect.SparseVector vector) => matrix * vector;


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedColumn"/> with a <see cref="Vect.Vector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the transposed <see cref="CompressedColumn"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector TransposeMultiply(CompressedColumn matrix, Vect.Vector vector)
        {
            if (vector is Vect.DenseVector denseVector) { return TransposeMultiply(matrix, denseVector); }
            else if (vector is Vect.SparseVector sparseVector) { return TransposeMultiply(matrix, sparseVector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} and a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedColumn"/> with a <see cref="Vect.DenseVector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector TransposeMultiply(CompressedColumn matrix, Vect.DenseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            matrix._storedMatrix.TransposeMultiply(vector.ToArray(), components);

            return new Vect.DenseVector(components);
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedColumn"/> with a <see cref="Vect.SparseVector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector TransposeMultiply(CompressedColumn matrix, Vect.SparseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            matrix._storedMatrix.TransposeMultiply(vector.ToArray(), components);

            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] != 0d) { rowIndices.Add(i); values.Add(components[i]); }
            }

            return new Vect.SparseVector(components.Length, rowIndices, values);
        }

        #endregion

        #region Operators

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn operator +(CompressedColumn left, CompressedColumn right)
        {
            C_Stor.CompressedColumnStorage<double> ccs = left._storedMatrix.Add(right._storedMatrix);

            return new CompressedColumn(ref ccs);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn operator -(CompressedColumn left, CompressedColumn right)
        {
            C_Stor.CompressedColumnStorage<double> ccs = new CSparse.Double.SparseMatrix(left.RowCount, left.ColumnCount, left.NonZerosCount + right.RowCount);

            left._storedMatrix.Add(1.0, -1.0, right._storedMatrix, ccs);
            return new CompressedColumn(ref ccs);
        }


        /// <summary>
        /// Computes the opposite of the <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedColumn"/> whose opposite is to be computed. </param>
        /// <returns> The <see cref="CompressedColumn"/>, opposite of the initial one. </returns>
        public static CompressedColumn operator -(CompressedColumn operand)
        {
            double[] values = new double[operand.NonZerosCount];
            for (int i = 0; i < values.Length; i++) { values[i] = -operand[i]; }

            C_Stor.CompressedColumnStorage<double> ccs = new CSparse.Double.SparseMatrix(operand.RowCount, operand.ColumnCount,
                values, operand._storedMatrix.RowIndices, operand._storedMatrix.ColumnPointers);

            return new CompressedColumn(ref ccs);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of left and the number of rows of right must be equal. </exception>
        public static CompressedColumn operator *(CompressedColumn left, CompressedColumn right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            C_Stor.CompressedColumnStorage<double> ccs = left._storedMatrix.Multiply(right._storedMatrix);
            return new CompressedColumn(ref ccs);
        }


        /******************** Embedding : CompressedRow  ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedColumn operator +(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = (CompressedColumn)right;

            return left + ccs;
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedColumn operator -(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = (CompressedColumn)right;

            return left - ccs;
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedColumn operator *(CompressedColumn left, CompressedRow right)
        {
            CompressedColumn ccs = (CompressedColumn)right;

            return left * ccs;
        }


        /******************** Embed In : Dense Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="CompressedColumn"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix operator +(CompressedColumn left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = new DenseMatrix(right);

            // Iterate on the columns of left
            for (int i_C = 0; i_C < left.ColumnCount; i_C++)
            {
                // Iterate on the non-zero values of the current left column
                for (int i = left.ColumnPointer(i_C); i < left.ColumnPointer(i_C + 1); i++)
                {
                    result[left.RowIndex(i), i_C] = left[i] + result[left.RowIndex(i), i_C];
                }
            }

            return result;
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="CompressedColumn"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix operator -(CompressedColumn left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            DenseMatrix result = DenseMatrix.Opposite(-right);

            // Iterate on the columns of left
            for (int i_C = 0; i_C < left.ColumnCount; i_C++)
            {
                // Iterate on the non-zero values of the current left column
                for (int i = left.ColumnPointer(i_C); i < left.ColumnPointer(i_C + 1); i++)
                {
                    result[left.RowIndex(i), i_C] = left[i] - result[left.RowIndex(i), i_C];
                }
            }

            return result;
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix operator *(CompressedColumn left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            DenseMatrix result = new DenseMatrix(left.RowCount, right.ColumnCount);

            // Iterate on the columns of left
            for (int i_LC = 0; i_LC < left.ColumnCount; i_LC++)
            {
                // Iterate on the non-zero values of the current left column
                for (int i_NZ = left.ColumnPointer(i_LC); i_NZ < left.ColumnPointer(i_LC + 1); i_NZ++)
                {
                    // Iterate on the columns of right
                    for (int i_C = 0; i_C < right.ColumnCount; i_C++)
                    {
                        result[left.RowIndex(i_NZ), i_C] += left[i_NZ] * right[i_LC, i_C];
                    }
                }
            }

            return result;
        }


        /******************** Embedding : Sparse Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The size of the matrices must be the same. </exception>
        public static CompressedColumn operator +(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return left + crsRight; }
            else if (right is CompressedColumn ccsRight) { return left + ccsRight; }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The size of the matrices must be the same. </exception>
        public static CompressedColumn operator -(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return left - crsRight; }
            else if (right is CompressedColumn ccsRight) { return left - ccsRight; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of left and the number of rows of right must be equal. </exception>
        public static CompressedColumn operator *(CompressedColumn left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return left * crsRight; }
            else if (right is CompressedColumn ccsRight) { return left * ccsRight; }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /******************** Embed In : Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="CompressedColumn"/> with a <see cref="Matrix"/> on the right.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the addition. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The addition of a <see cref="CompressedColumn"/> with the right matrix as a <see cref="Matrix"/> is not implemented.
        /// </exception>
        public static Matrix operator +(CompressedColumn left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left + denseRight; }
            else if (right is SparseMatrix sparseRight) { return left + sparseRight; }
            else { throw new NotImplementedException($"The addition of a {left.GetType()} and a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="CompressedColumn"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix"/> to subtract with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The subtraction of a <see cref="CompressedColumn"/> with the right matrix as a <see cref="Matrix"/> is not implemented.
        /// </exception>
        public static Matrix operator -(CompressedColumn left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left - denseRight; }
            else if (right is SparseMatrix sparseRight) { return left - sparseRight; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} and a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The multiplication of a <see cref="CompressedColumn"/> with the right matrix as a <see cref="Matrix"/> is not implemented.
        /// </exception>
        public static Matrix operator *(CompressedColumn left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left * denseRight; }
            else if (right is SparseMatrix sparseRight) { return left * sparseRight; }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} and a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="CompressedColumn"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedColumn"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the right scalar multiplication. </returns>
        public static CompressedColumn operator *(CompressedColumn operand, double factor)
        {

            if (factor == 0d) { return CompressedColumn.Zero(operand.RowCount, operand.ColumnCount); }

            int[] columnPointers = operand._storedMatrix.ColumnPointers.Clone() as int[];
            int[] rowIndices = operand._storedMatrix.RowIndices.Clone() as int[];

            double[] values = new double[operand.NonZerosCount];
            for (int i_NZ = 0; i_NZ < values.Length; i_NZ++)
            {
                values[i_NZ] = operand._storedMatrix.Values[i_NZ] * factor;
            }

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, columnPointers, rowIndices, values);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="CompressedColumn"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedColumn"/> to multiply on the left. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the left scalar multiplication. </returns>
        public static CompressedColumn operator *(double factor, CompressedColumn operand)
        {
            if (factor == 0d) { return CompressedColumn.Zero(operand.RowCount, operand.ColumnCount); }

            int[] columnPointers = operand._storedMatrix.ColumnPointers.Clone() as int[];
            int[] rowIndices = operand._storedMatrix.RowIndices.Clone() as int[];

            double[] values = new double[operand.NonZerosCount];
            for (int i_NZ = 0; i_NZ < values.Length; i_NZ++)
            {
                values[i_NZ] = factor * operand._storedMatrix.Values[i_NZ];
            }

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, columnPointers, rowIndices, values);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="CompressedColumn"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedColumn"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the scalar division. </returns>
        public static CompressedColumn operator /(CompressedColumn operand, double divisor)
        {
            if (divisor == 0d)
            {
                throw new DivideByZeroException("The divisor can not be zero.");
            }

            int[] columnPointers = operand._storedMatrix.ColumnPointers.Clone() as int[];
            int[] rowIndices = operand._storedMatrix.RowIndices.Clone() as int[];

            double[] values = new double[operand.NonZerosCount];
            for (int i_NZ = 0; i_NZ < values.Length; i_NZ++)
            {
                values[i_NZ] = operand._storedMatrix.Values[i_NZ] / divisor;
            }

            return new CompressedColumn(operand.RowCount, operand.ColumnCount, columnPointers, rowIndices, values);
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="Vect.Vector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> 
        /// The right multiplication of a <see cref="CompressedColumn"/> with a vector as a <see cref="Vect.Vector"/> is not implemented.
        /// </exception>
        public static Vect.Vector operator *(CompressedColumn matrix, Vect.Vector vector)
        {
            if (vector is Vect.DenseVector denseVector) { return matrix * denseVector; }
            else if (vector is Vect.SparseVector sparseVector) { return matrix * sparseVector; }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} and a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="Vect.DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector operator *(CompressedColumn matrix, Vect.DenseVector vector)
        {
            double[] components = new double[matrix.RowCount];

            matrix._storedMatrix.Multiply(vector.ToArray(), components);

            return new Vect.DenseVector(ref components);
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedColumn"/> with a <see cref="Vect.SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedColumn"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector operator *(CompressedColumn matrix, Vect.SparseVector vector)
        {
            double[] components = new double[matrix.RowCount];

            matrix._storedMatrix.Multiply(vector.ToArray(), components);

            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] != 0d) { rowIndices.Add(i); values.Add(components[i]); }
            }

            return new Vect.SparseVector(components.Length, rowIndices, values);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a <see cref="CompressedRow"/> number into a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="crs"> <see cref="CompressedRow"/> to convert. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the conversion. </returns>
        public static explicit operator CompressedColumn(CompressedRow crs)
        {
            int rowCount = crs.RowCount;
            int columnCount = crs.ColumnCount;

            // Get the number of elements per column
            int[] columnHelper = new int[columnCount];
            for (int i_NZ = 0; i_NZ < crs.NonZerosCount; i_NZ++)
            {
                columnHelper[crs.ColumnIndex(i_NZ)]++;
            }


            // Creates the column pointer
            int[] columnPointers = new int[columnCount + 1];

            columnPointers[0] = 0;
            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                columnPointers[i_C + 1] = columnPointers[i_C] + columnHelper[i_C];
                columnHelper[i_C] = 0;
            }


            // Creates the row index and value lists
            double[] values = new double[crs.NonZerosCount];
            int[] rowIndices = new int[crs.NonZerosCount];

            int i_RowNZ = crs.RowPointer(0);
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                for (; i_RowNZ < crs.RowPointer(i_R + 1); i_RowNZ++)
                {
                    int i_C = crs.ColumnIndex(i_RowNZ);
                    int i_Pointer = columnPointers[i_C] + columnHelper[i_C];

                    values[i_Pointer] = crs[i_RowNZ];
                    rowIndices[i_Pointer] = i_R;
                    columnHelper[i_C]++;
                }
            }

            return new CompressedColumn(rowCount, columnCount, columnPointers, rowIndices, values);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the row index of a non zero value at a given index.
        /// </summary>
        /// <param name="index"> Index of the non-zero value whose row index to get. </param>
        /// <returns> The row index of this <see cref="CompressedColumn"/> sparse matrix at the given index. </returns>
        public int RowIndex(int index) => _storedMatrix.RowIndices[index];

        /// <summary>
        /// Returns the column pointer at agiven index.
        /// </summary>
        /// <param name="index"> Index of the column pointers to get.</param>
        /// <returns> The column pointers of this <see cref="CompressedColumn"/> sparse matrix. </returns>
        public int ColumnPointer(int index) => _storedMatrix.ColumnPointers[index];

        /// <summary>
        /// Returns the non-zero value at a given index.
        /// </summary>
        /// <param name="index"> Index of the non-zero value to get.</param>
        /// <returns> The non-zero value of this <see cref="CompressedColumn"/> sparse matrix at the given index. </returns>
        public double NonZero(int index) => _storedMatrix.Values[index];


        /// <summary>
        /// Computes the kernel (or null-space) of the current <see cref="SparseMatrix"/> using the QR decomposition.
        /// </summary>
        /// <remarks> The method is adapted for rectangular matrix. </remarks>
        /// <returns> The vectors forming a basis of the null-space. </returns>
        public Vect.DenseVector[] Kernel()
        {
            // Verification
            if (NonZerosCount == 0) { throw new Exception("The kernel of a zero matrix cannot be computed."); }

            CompressedColumn squareCcs = CompleteToSquare();

            Vect.DenseVector[] kernel = ComputeKernel(ref squareCcs);

            if (RowCount != ColumnCount) { kernel = FilterKernelVectors(kernel); }

            return kernel;

            /***** Nested Methods *****/


            // Copies and Completes this matrix to be a square matrix by duplicating the non-zero column with the least component.
            // Returns the new square CompressedColumn.
            CompressedColumn CompleteToSquare()
            {
                // If the matrix is rectangular with more columns
                if (RowCount < ColumnCount)
                {
                    // Complete the current matrix to be a square matrix.
                    // In order to keep the null space unchanged, the non-zero row with the least component is duplicated.
                    CompressedRow crs = (CompressedRow)this;

                    int i_SparseRow = 0;
                    int sparseRowCount = NonZerosCount;

                    int[] rowPointers = new int[ColumnCount + 1];
                    rowPointers[0] = 0;
                    for (int i_R = 0; i_R < RowCount; i_R++)
                    {
                        rowPointers[i_R + 1] = crs.RowPointer(i_R + 1);

                        int count = rowPointers[i_R + 1] - rowPointers[i_R];
                        if (count != 0 & count < sparseRowCount) { sparseRowCount = count; i_SparseRow = i_R; }
                    }
                    for (int i_R = RowCount; i_R < ColumnCount; i_R++)
                    {
                        rowPointers[i_R + 1] = rowPointers[i_R] + sparseRowCount;
                    }

                    List<int> columnIndices = new List<int>(NonZerosCount + (sparseRowCount * (ColumnCount - RowCount)));
                    List<double> values = new List<double>(NonZerosCount + (sparseRowCount * (ColumnCount - RowCount)));
                    for (int i_R = 0; i_R < RowCount; i_R++)
                    {
                        for (int i_NZ = rowPointers[i_R]; i_NZ < rowPointers[i_R + 1]; i_NZ++)
                        {
                            columnIndices.Add(crs.ColumnIndex(i_NZ));
                            values.Add(crs[i_NZ]);
                        }
                    }
                    for (int i_R = 0; i_R < ColumnCount - RowCount; i_R++)
                    {
                        for (int i_NZ = rowPointers[i_SparseRow]; i_NZ < rowPointers[i_SparseRow + 1]; i_NZ++)
                        {
                            columnIndices.Add(crs.ColumnIndex(i_NZ));
                            values.Add(crs[i_NZ]);
                        }
                    }

                    CompressedRow result = new CompressedRow(ColumnCount, ColumnCount, ref rowPointers, ref columnIndices, ref values);
                    return (CompressedColumn)result;
                }
                // If the matrix is rectangular with more rows
                else if (ColumnCount < RowCount)
                {
                    // Complete the current matrix to be a square matrix.
                    // In order to keep the null space unchanged, the non-zero column with the least component is duplicated.
                    int i_SparseColumn = 0;
                    int sparseColumnCount = NonZerosCount;

                    int[] columnPointers = new int[RowCount + 1];
                    columnPointers[0] = 0;
                    for (int i_C = 0; i_C < ColumnCount; i_C++)
                    {
                        columnPointers[i_C + 1] = _storedMatrix.ColumnPointers[i_C + 1];

                        int count = columnPointers[i_C + 1] - columnPointers[i_C];
                        if (count != 0 & count < sparseColumnCount) { sparseColumnCount = count; i_SparseColumn = i_C; }
                    }
                    for (int i_C = ColumnCount; i_C < RowCount; i_C++)
                    {
                        columnPointers[i_C + 1] = columnPointers[i_C] + sparseColumnCount;
                    }

                    List<int> rowIndices = new List<int>(NonZerosCount + (sparseColumnCount * (RowCount - ColumnCount)));
                    List<double> values = new List<double>(NonZerosCount + (sparseColumnCount * (RowCount - ColumnCount)));
                    for (int i_C = 0; i_C < ColumnCount; i_C++)
                    {
                        for (int i_NZ = columnPointers[i_C]; i_NZ < columnPointers[i_C + 1]; i_NZ++)
                        {
                            rowIndices.Add(_storedMatrix.RowIndices[i_NZ]);
                            values.Add(_storedMatrix.Values[i_NZ]);
                        }
                    }
                    for (int i_C = 0; i_C < RowCount - ColumnCount; i_C++)
                    {
                        for (int i_NZ = columnPointers[i_SparseColumn]; i_NZ < columnPointers[i_SparseColumn + 1]; i_NZ++)
                        {
                            rowIndices.Add(_storedMatrix.RowIndices[i_NZ]);
                            values.Add(_storedMatrix.Values[i_NZ]);
                        }
                    }

                    return new CompressedColumn(RowCount, RowCount, columnPointers, rowIndices.ToArray(), values.ToArray());
                }
                // If the matrix is a square matrix
                else
                {
                    int[] columnPointers = new int[_storedMatrix.ColumnPointers.Length];
                    for (int i = 0; i < _storedMatrix.ColumnPointers.Length; i++)
                    {
                        columnPointers[i] = _storedMatrix.ColumnPointers[i];
                    }
                    List<int> rowIndices = new List<int>(NonZerosCount);
                    for (int i = 0; i < NonZerosCount; i++)
                    {
                        rowIndices.Add(_storedMatrix.RowIndices[i]);
                    }
                    List<double> values = new List<double>(NonZerosCount);
                    for (int i = 0; i < NonZerosCount; i++)
                    {
                        values.Add(_storedMatrix.Values[i]);
                    }

                    return new CompressedColumn(RowCount, ColumnCount, columnPointers, rowIndices.ToArray(), values.ToArray());
                }
            }

            // Compute the kernel of a square matrix.
            // Returns the vectors forming a basis of the null-space. If the kernel is reduced to { 0 }, an empty array is returned.
            Vect.DenseVector[] ComputeKernel(ref CompressedColumn ccs)
            {
                double tolerance = 1e-8;

                ccs = CompressedColumn.Transpose(ccs);

                #region QR Factorization

                Factorisation.SparseQR qr = new Factorisation.SparseQR(ccs);

                CSparse.Double.SparseMatrix R = qr.GetR();
                CSparse.Double.SparseMatrix Q = qr.ComputeQ();

                int[] pInv = qr.GetInverseRowPermutation();

                #endregion

                // Dimension of the null-space of the matrix.
                int dimension = 0;
                for (int i_C = 0; i_C < R.ColumnCount; i_C++)
                {
                    if (Math.Abs(R.At(i_C, i_C)) < tolerance) { dimension += 1; }
                }

                // (Inverse) Row Permutation
                int[] rowPermutation = new int[pInv.Length];
                for (int i = 0; i < pInv.Length; i++)
                {
                    rowPermutation[pInv[i]] = i;
                }

                /***** Add non-zero vectors whose diagonal comonent is zero *****/

                Vect.DenseVector[] result = new Vect.DenseVector[dimension];
                if (dimension == 0) { return result; }

                // Check every single null vector in the left-hand
                int counter = 0;
                for (int i_C = 0; i_C < R.ColumnCount; i_C++)
                {
                    // If the diagonal component is not null.
                    if (Math.Abs(R.At(i_C, i_C)) > tolerance) { continue; }

                    double[] column = Q.Column(i_C);
                    double[] temp = new double[column.Length];
                    bool isZero = true;

                    // We perform the row permutation
                    for (int j = 0; j < column.Length; j++)
                    {
                        temp[rowPermutation[j]] = column[j];
                    }

                    // We resize the array to fit the number of columns
                    Array.Resize(ref temp, ccs.ColumnCount);

                    for (int j = 0; j < ccs.ColumnCount; j++)
                    {
                        isZero &= (temp[j] == 0);
                    }
                    if (!isZero)
                    {
                        // We add the new vector if it's not zero
                        // The null vector can be applied to the fictitious rows...
                        result[counter] = new Vect.DenseVector(temp);
                        counter += 1;
                    }
                }

                /***** Add non-zero vectors which ar not on the diagonal *****/

                for (int i_C = R.ColumnCount; i_C < Q.ColumnCount; i_C++)
                {
                    var column = Q.Column(i_C);
                    var temp = new double[column.Length];
                    bool isZero = true;

                    //We perform the permutation
                    for (int j = 0; j < column.Length; j++)
                    {
                        temp[rowPermutation[j]] = column[j];
                    }

                    System.Array.Resize(ref temp, ccs.ColumnCount);

                    for (int j = 0; j < ccs.ColumnCount; j++)
                    {
                        isZero &= (temp[j] == 0);
                    }

                    if (!isZero)
                    {
                        //We add the new vector if it's not zero
                        //The null vector can be applied to the fictitious rows...
                        result[counter] = new Vect.DenseVector(temp);
                        counter += 1;
                    }
                }

                return result;
            }

            // Filters the kernel vectors to fit the initial rectangular matrix (and not the completed square matrix).
            // Returns the kernel vectors of this matrix
            Vect.DenseVector[] FilterKernelVectors(Vect.DenseVector[] vectors)
            {
                // If the matrix is rectangular with more columns
                if (RowCount < ColumnCount)
                {
                    // In this case, the initial rectangular matrix was completed with linearly-dependent row.
                    // Fortunately, the kernel of the initial matrix and the completed matrix are the same.
                    return vectors;
                }
                // If the matrix is rectangular with more rows
                else if (ColumnCount < RowCount)
                {
                    // In this case, the initial rectangular matrix was completed with linearly-dependent columns.
                    // Fortunately, we know that the vectors of the actual null-space should be orthogonal to

                    // This array contains the vectors to which our null space should be orthogonal
                    Vect.DenseVector[] orthogonalVectors = new Vect.DenseVector[RowCount - ColumnCount];
                    for (int i = 0; i < (RowCount - ColumnCount); i++)
                    {
                        orthogonalVectors[i] = Vect.DenseVector.StandardVector(RowCount, ColumnCount + i);
                    }

                    Storage.DictionaryOfKeys dok = new Storage.DictionaryOfKeys((RowCount - ColumnCount) * vectors.Length);
                    for (int i_R = 0; i_R < (RowCount - ColumnCount); i_R++)
                    {
                        for (int i_C = 0; i_C < vectors.Length; i_C++)
                        {
                            double val = Vect.DenseVector.TransposeMultiply(vectors[i_C], orthogonalVectors[i_R]);
                            if (val != 0d) { dok.Add(val, i_R, i_C); }
                        }
                    }

                    CompressedColumn ccs = new CompressedColumn((RowCount - ColumnCount), vectors.Length, dok);

                    Vect.DenseVector[] intermediateresult = ccs.Kernel();

                    // Create the solution
                    Vect.DenseVector[] finalResults = new Vect.DenseVector[intermediateresult.Length];

                    int counter = 0;
                    foreach (Vect.DenseVector combination in intermediateresult)
                    {
                        Vect.DenseVector finalResult = new Vect.DenseVector(RowCount);
                        for (int i = 0; i < vectors.Length; i++)
                        {
                            Vect.DenseVector vec = Vect.DenseVector.Multiply(combination[i], vectors[i]);
                            finalResult = Vect.DenseVector.Add(finalResult, vec);
                        }
                        //This part can be improved
                        var array = finalResult.ToArray();

                        Array.Resize(ref array, ColumnCount);

                        finalResults[counter] = (new Vect.DenseVector(array));
                        counter += 1;
                    }

                    //En fait finalResults contient les vecteurs orthogonaux à notre solution? on peut tenter un gram-schmidt
                    return finalResults;
                }
                else { throw new InvalidOperationException("The filtering of the kernel vectors is not necessary for square matrices."); }
            }

        }

        /// <summary>
        /// Solve the system <c>A·x=y</c> using Cholesky decomposition.
        /// </summary>
        /// <param name="vector"> The vector y in the system. </param>
        /// <returns> The vector x in the system. </returns>
        public Vect.DenseVector SolveCholesky(Vect.Vector vector)
        {
            var cholesky = CSparse.Double.Factorization.SparseCholesky.Create(_storedMatrix, CSparse.ColumnOrdering.MinimumDegreeAtPlusA);

            double[] x = new double[ColumnCount];

            cholesky.Solve(vector.ToArray(), x);
            return new Vect.DenseVector(x);
        }

        #endregion

        #region Other Methods

        internal C_Stor.CompressedColumnStorage<double> StoredMatrix() => _storedMatrix;

        #endregion


        #region Overrides

        /******************** SparseMatrix ********************/

        /// <inheritdoc cref="SparseMatrix.NonZeros()"/>
        public override IEnumerable<(int rowIndex, int columnIndex, double value)> NonZeros()
        {
            for (int i_C = 0; i_C < ColumnCount; i_C++)
            {
                for (int i_NZ = ColumnPointer(i_C); i_NZ < ColumnPointer(i_C + 1); i_NZ++)
                {
                    yield return (RowIndex(i_NZ), i_C, NonZero(i_NZ));
                }
            }
        }

        /******************** Matrix ********************/

        /// <inheritdoc cref="Matrix.At(int, int)"/>
        public override double At(int row, int column) => _storedMatrix.At(row, column);

        #endregion

        #region Explicit Implementations

        /******************** IGroupAction<CompressedColumn, double> ********************/

        /// <inheritdoc/>
        CompressedColumn Alg_Sets.IGroupAction<CompressedColumn, double>.Multiply(double factor) => this * factor;

        /// <inheritdoc/>
        CompressedColumn Alg_Sets.IGroupAction<CompressedColumn, double>.Divide(double divisor) => this / divisor;

        #endregion
    }
}