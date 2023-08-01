using System;
using System.Numerics;
using System.Collections.Generic;

using Vect = BRIDGES.LinearAlgebra.Vectors;


namespace BRIDGES.LinearAlgebra.Matrices.Sparse
{
    /// <summary>
    /// Class defining a sparse matrix with a compressed row storage.
    /// </summary>
    public class CompressedRow : SparseMatrix,
        IMultiplyOperators<CompressedRow, double, CompressedRow>, IDivisionOperators<CompressedRow, double, CompressedRow>
    {
        #region Fields

        /// <summary>
        /// Number of row of this <see cref="CompressedColumn"/>
        /// </summary>
        private readonly int _rowCount;

        /// <summary>
        /// Number of column of this <see cref="CompressedColumn"/>
        /// </summary>
        private readonly int _columnCount;


        /// <summary>
        /// Pointers giving the number of non-zero values before the row at a given index.
        /// </summary>
        /// <remarks> Array of length (ColumnCount + 1), starting at 0 and ending at <see cref="SparseMatrix.NonZerosCount"/>. </remarks>
        private readonly int[] _rowPointers;

        /// <summary>
        /// Row indices associated with the non-zero values.
        /// </summary>
        private readonly List<int> _columnIndices;

        /// <summary>
        /// Non-zero values of the matrix
        /// </summary>
        private readonly List<double> _values;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override int RowCount => _rowCount;

        /// <inheritdoc/>
        public override int ColumnCount => _columnCount;


        /// <inheritdoc/>
        public override int NonZerosCount => _values.Count;

        /// <inheritdoc/>
        public override double this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedRow"/> class by defining its size,
        /// and by giving its values in a <see cref="Storage.DictionaryOfKeys"/>.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedRow"/>. </param>
        /// <param name="dok"> Values of the <see cref="CompressedRow"/>. </param>
        public CompressedRow(int rowCount, int columnCount, Storage.DictionaryOfKeys dok)
            : base (rowCount, columnCount)
        {
            _rowCount = rowCount;
            _columnCount = columnCount;


            List<(int, double)>[] rows = new List<(int, double)>[rowCount];
            for (int i_R = 0; i_R < rowCount; i_R++) { rows[i_R] = new List<(int, double)>(); }

            // Distribute among the columns
            foreach( ((int rowIndex, int columnIndex), double value) in dok)
            {
                rows[rowIndex].Add((columnIndex, value));
            }

            // Sorts each columns with regard to the row index
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                rows[i_R].Sort((x, y) => x.Item1.CompareTo(y.Item1));
            }

            // Creates the new column pointer, the row index list and value lists
            _rowPointers = new int[rowCount + 1];
            _columnIndices = new List<int>(dok.Count);
            _values = new List<double>(dok.Count);

            _rowPointers[0] = 0;
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                List<(int, double)> row = rows[i_R];
                int count = row.Count;

                _rowPointers[i_R + 1] = _rowPointers[i_R] + count;

                for (int i_NZ = 0; i_NZ < count; i_NZ++)
                {
                    _columnIndices.Add(row[i_NZ].Item1);
                    _values.Add(row[i_NZ].Item2);
                }
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedRow"/> class from another <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="crs"> <see cref="CompressedRow"/> to deep copy. </param>
        public CompressedRow(CompressedRow crs)
            : base(crs.RowCount, crs.ColumnCount)
        {
            _rowCount = crs.RowCount;
            _columnCount = crs.ColumnCount;

            _rowPointers = (int[]) crs._rowPointers.Clone();
            _columnIndices = new List<int>(crs._columnIndices);
            _values = new List<double>(crs._values);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="CompressedRow"/> class by defining its number of row and column.
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedRow"/>. </param>
        /// <param name="values"> Non-zero values of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnIndices"> Column indices of the <see cref="CompressedRow"/>.</param>
        /// <param name="rowPointers"> Row pointers of the <see cref="CompressedRow"/>. </param>
        internal CompressedRow(int rowCount, int columnCount, ref int[] rowPointers, ref List<int> columnIndices, ref List<double> values)
            : base(rowCount, columnCount)
        {
            _rowCount = rowCount;
            _columnCount = columnCount;


            // Verifications
            if (rowPointers.Length != rowCount + 1)
            {
                throw new ArgumentException("The number of row pointers is not consistent with the number of columns.", nameof(rowPointers));
            }
            if (values.Count != columnIndices.Count)
            {
                throw new ArgumentException($"The number of elements in {nameof(values)} and {nameof(columnIndices)} do not match.");
            }
            if (rowPointers[rowPointers.Length - 1] != values.Count)
            {
                throw new ArgumentException($"The last value of {nameof(rowPointers)} is not equal to the number of non-zero values.");
            }

            _rowPointers = rowPointers;
            _columnIndices = columnIndices;
            _values = values;

        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the neutral <see cref="CompressedRow"/> for the addition. 
        /// </summary>
        /// <param name="rowCount"> Number of rows of the <see cref="CompressedRow"/>. </param>
        /// <param name="columnCount"> Number of columns of the <see cref="CompressedRow"/>. </param>
        /// <returns> The <see cref="CompressedRow"/> of the given size, with zeros on every coordinates. </returns>
        public static CompressedRow Zero(int rowCount, int columnCount)
        {
            int[] rowPointers = new int[rowCount + 1];
            List<int> columnIndices = new List<int>();
            List<double> values = new List<double>();

            return new CompressedRow(rowCount, columnCount, ref rowPointers, ref columnIndices, ref values);
        }

        /// <summary>
        /// Returns the neutral <see cref="CompressedRow"/> for the multiplication. 
        /// </summary>
        /// <param name="size"> Number of rows and columns of the <see cref="CompressedRow"/>. </param>
        /// <returns> The <see cref="CompressedRow"/> of the given size, with ones on the diagonal and zeros elsewhere. </returns>
        public static CompressedRow Identity(int size)
        {
            int[] rowPointers = new int[size + 1];
            List<int> columnIndices = new List<int>();
            List<double> values = new List<double>();

            for (int i = 0; i < size; i++)
            {
                rowPointers[i] = i;
                columnIndices.Add(i);
                values.Add(1d);
            }
            rowPointers[size] = size;

            return new CompressedRow(size, size, ref rowPointers, ref columnIndices, ref values);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Transposes the <see cref="CompressedRow"/>
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to tranpose. </param>
        /// <returns> The new <see cref="CompressedRow"/>, transposed of the initial one. </returns>
        public static CompressedRow Transpose(CompressedRow matrix)
        {
            // Get the number of elements per column of the current matrix
            int[] columnHelper = new int[matrix.ColumnCount];
            for (int i_NZ = 0; i_NZ < matrix.NonZerosCount; i_NZ++)
            {
                columnHelper[matrix.ColumnIndex(i_NZ)]++;
            }


            // Creates the new row pointer
            int[] rowPointers = new int[matrix.ColumnCount + 1];

            rowPointers[0] = 0;
            for (int i_R = 0; i_R < rowPointers.Length; i_R++)
            {
                rowPointers[i_R + 1] = rowPointers[i_R] + columnHelper[i_R];
                columnHelper[i_R] = 0;
            }


            // Creates the new column index and value lists
            int[] columnIndices = new int[matrix.NonZerosCount];
            double[] values = new double[matrix.NonZerosCount];

            int i_RowNZ = matrix.RowPointer(0);
            for (int i_R = 0; i_R < matrix.RowCount; i_R++)
            {
                for (; i_RowNZ < matrix.RowPointer(i_R + 1); i_RowNZ++)
                {

                    int i_C = matrix.ColumnIndex(i_RowNZ);
                    int i_Pointer = rowPointers[i_C] + columnHelper[i_C];

                    values[i_Pointer] = matrix.NonZero(i_RowNZ);
                    columnIndices[i_Pointer] = i_R;
                    columnHelper[i_C]++;
                }
            }
            List<int> list_ColumnIndices = new List<int>(columnIndices);
            List<double> list_Values = new List<double>(values);

            return new CompressedRow(matrix.ColumnCount, matrix.RowCount, ref rowPointers, ref list_ColumnIndices, ref list_Values);
        }


        /******************** Algebraic Near Ring ********************/

        /// <inheritdoc cref="operator +(CompressedRow, CompressedRow)"/>
        public static CompressedRow Add(CompressedRow left, CompressedRow right) => left + right;

        /// <inheritdoc cref="operator -(CompressedRow, CompressedRow)"/>
        public static CompressedRow Subtract(CompressedRow left, CompressedRow right) => left - right;


        /// <inheritdoc cref="operator *(CompressedRow, CompressedRow)"/>
        public static CompressedRow Multiply(CompressedRow left, CompressedRow right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="CompressedRow"/> with its transposition : <c>A<sup>t</sup>·A</c>.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedRow"/> for the operation. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the operation. </returns>
        public static CompressedRow TransposeMultiplySelf(CompressedRow operand)
        {
            CompressedRow transposed = Transpose(operand);

            return Multiply(transposed, operand);
        }


        /******************** Embedding : CompressedColumn ********************/

        /// <inheritdoc cref="operator +(CompressedRow, CompressedColumn)"/>
        public static CompressedRow Add(CompressedRow left, CompressedColumn right) => left + right;

        /// <summary>
        /// Computes the left addition of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the addition. </returns>
        public static CompressedRow Add(CompressedColumn left, CompressedRow right)
        {
            CompressedRow ccs = (CompressedRow)left;

            return ccs + right;
        }


        /// <inheritdoc cref="operator -(CompressedRow, CompressedColumn)"/>
        public static CompressedRow Subtract(CompressedRow left, CompressedColumn right) => left - right;

        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedRow"/> on a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> to subtract with. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> to subtract. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        public static CompressedRow Subtract(CompressedColumn left, CompressedRow right)
        {
            CompressedRow ccs = (CompressedRow)left;

            return ccs - right;
        }


        /// <inheritdoc cref="operator *(CompressedRow, CompressedColumn)"/>
        public static CompressedRow Multiply(CompressedRow left, CompressedColumn right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        public static CompressedRow Multiply(CompressedColumn left, CompressedRow right)
        {
            CompressedRow ccs = (CompressedRow)left;

            return ccs * right;
        }


        /******************** Embed In : Dense Matrix ********************/

        /// <inheritdoc cref="operator +(CompressedRow, DenseMatrix)"/>
        public static DenseMatrix Add(CompressedRow left, DenseMatrix right) => left + right;

        /// <inheritdoc cref="operator -(CompressedRow, DenseMatrix)"/>
        public static DenseMatrix Subtract(CompressedRow left, DenseMatrix right) => left - right;


        /// <inheritdoc cref="operator *(CompressedRow, DenseMatrix)"/>
        public static DenseMatrix Multiply(CompressedRow left, DenseMatrix right) => left * right;


        /******************** Embedding : Sparse Matrix ********************/

        /// <inheritdoc cref="operator +(CompressedRow, SparseMatrix)"/>
        public static CompressedRow Add(CompressedRow left, SparseMatrix right) => left + right;

        /// <summary>
        /// Computes the left addition of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The left addition of a <see cref="CompressedRow"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static CompressedRow Add(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Add(crsLeft, right); }
            else if (left is CompressedColumn ccsLeft) { return CompressedRow.Add(ccsLeft, right); }
            else { throw new NotImplementedException($"The left addition of a {right.GetType()} with a {left.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /// <inheritdoc cref="operator -(CompressedRow, SparseMatrix)"/>
        public static CompressedRow Subtract(CompressedRow left, SparseMatrix right) => left - right;

        /// <summary>
        /// Computes the subtraction of a <see cref="CompressedRow"/> on a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> to subtract with. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> to subtract. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of a <see cref="CompressedRow"/> on a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static CompressedRow Subtract(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Subtract(crsLeft, right); }
            else if (left is CompressedColumn ccsLeft) { return CompressedRow.Subtract(ccsLeft, right); }
            else { throw new NotImplementedException($"The subtraction of a {right.GetType()} on a {left.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /// <inheritdoc cref="operator *(CompressedRow, SparseMatrix)"/>
        public static CompressedRow Multiply(CompressedRow left, SparseMatrix right) => left * right;

        /// <summary>
        /// Computes the left multiplication of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The left multiplication of a <see cref="CompressedRow"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static CompressedRow Multiply(SparseMatrix left, CompressedRow right)
        {
            if (left is CompressedRow crsLeft) { return CompressedRow.Multiply(crsLeft, right); }
            else if (left is CompressedColumn ccsLeft) { return CompressedRow.Multiply(ccsLeft, right); }
            else { throw new NotImplementedException($"The left multiplication of a {right.GetType()} with a {left.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /******************** Embed In : Sparse Matrix ********************/

        /// <inheritdoc cref="operator +(CompressedRow, Matrix)"/>
        public static Matrix Add(CompressedRow left, Matrix right) => left + right;

        /// <inheritdoc cref="operator -(CompressedRow, Matrix)"/>
        public static Matrix Subtract(CompressedRow left, Matrix right) => left - right;


        /// <inheritdoc cref="operator *(CompressedRow, Matrix)"/>
        public static Matrix Multiply(CompressedRow left, Matrix right) => left * right;


        /******************** Group Action ********************/

        /// <inheritdoc cref="operator *(CompressedRow, double)"/>
        public static CompressedRow Multiply(CompressedRow operand, double factor) => operand * factor;

        /// <inheritdoc cref="operator *(double, CompressedRow)"/>
        public static CompressedRow Multiply(double factor, CompressedRow operand) => factor * operand;


        /// <inheritdoc cref="operator /(CompressedRow, double)"/>
        public static CompressedRow Divide(CompressedRow operand, double divisor) => operand / divisor;


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
        public static Vect.Vector Multiply(CompressedRow matrix, Vect.Vector vector) => matrix * vector;

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Vect.DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector Multiply(CompressedRow matrix, Vect.DenseVector vector) => matrix * vector;

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Vect.SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector Multiply(CompressedRow matrix, Vect.SparseVector vector) => matrix * vector;


        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedRow"/> with a <see cref="Vect.Vector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of the transposed <see cref="CompressedRow"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector TransposeMultiply(CompressedRow matrix, Vect.Vector vector)
        {
            if (vector is Vect.DenseVector denseVector) { return TransposeMultiply(matrix, denseVector); }
            else if (vector is Vect.SparseVector sparseVector) { return TransposeMultiply(matrix, sparseVector); }
            else { throw new NotImplementedException($"The right multiplication of a transposed {matrix.GetType()} with a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedRow"/> with a <see cref="Vect.DenseVector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector TransposeMultiply(CompressedRow matrix, Vect.DenseVector vector)
        {
            double[] components = new double[matrix.ColumnCount];

            int i_NZ = matrix._rowPointers[0];
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (; i_NZ < matrix._rowPointers[i + 1]; i_NZ++)
                {
                    int i_R = matrix._columnIndices[i_NZ];
                    components[i_R] += matrix._values[i_NZ] * vector[i];
                }
            }

            return new Vect.DenseVector(ref components);
        }

        /// <summary>
        /// Computes the right multiplication of a transposed <see cref="CompressedRow"/> with a <see cref="Vect.SparseVector"/> : <c>A<sup>t</sup>·V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to transpose then multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector TransposeMultiply(CompressedRow matrix, Vect.SparseVector vector)
        {
            Vect.SparseVector result = new Vect.SparseVector(matrix.ColumnCount);

            for (int i = 0; i < matrix.RowCount; i++)
            {
                bool isZero = !vector.TryGetComponent(i, out double val);
                if (isZero) { continue; }

                for (int i_NZ = matrix._rowPointers[i]; i_NZ < matrix._rowPointers[i + 1]; i_NZ++)
                {
                    int i_R = matrix._columnIndices[i_NZ];

                    if (result.TryGetComponent(i_R, out double existing)) { result[i_R] = existing + matrix._values[i_NZ] * val; }
                    else { result[i_R] = matrix._values[i_NZ] * val; }
                }
            }

            return result;
        }

        #endregion

        #region Operators

        /******************** Algebraic Near Ring ********************/

        /// <summary>
        /// Computes the addition of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the addition. </param>
        /// <returns> The <see cref="CompressedRow"/> resulting from the addition. </returns>
        public static CompressedRow operator +(CompressedRow left, CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            List<double> values = new List<double>(left.NonZerosCount + right.NonZerosCount);
            List<int> columnIndices = new List<int>(left.NonZerosCount + right.NonZerosCount);

            int[] rowPointers = new int[left.RowCount + 1];
            rowPointers[0] = 0;

            int i_RightNZ = 0;

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                int i_RighRowPointer = right._rowPointers[i_R + 1];

                // Iterate on the non-zero values of the current left row
                for (int i_LeftNZ = left._rowPointers[i_R]; i_LeftNZ < left._rowPointers[i_R + 1]; i_LeftNZ++)
                {
                    int i_LeftColumn = left._columnIndices[i_LeftNZ];

                    // Add the non-zero values of the current right row which are before the current left non-zero value.
                    while (right._columnIndices[i_RightNZ] < i_LeftColumn && i_RightNZ < i_RighRowPointer)
                    {
                        values.Add(right._values[i_RightNZ]);
                        columnIndices.Add(right._columnIndices[i_RightNZ]);

                        i_RightNZ++;
                    }

                    // If the the non-zero values of the current left and right row are a the same column.
                    if (right._columnIndices[i_RightNZ] == i_LeftColumn)
                    {
                        values.Add(left._values[i_LeftNZ] + right._values[i_RightNZ]);
                        columnIndices.Add(i_LeftColumn);

                        i_RightNZ++;
                    }
                    else
                    {
                        values.Add(left._values[i_LeftNZ]);
                        columnIndices.Add(i_LeftColumn);
                    }
                }

                // Add the remaining non-zero values of the current right row which are after these of the current left row.
                for (; i_RightNZ < i_RighRowPointer; i_RightNZ++)
                {
                    values.Add(right._values[i_RightNZ]);
                    columnIndices.Add(right._columnIndices[i_RightNZ]);
                }

                rowPointers[i_R + 1] = values.Count;
            }

            return new CompressedRow(left.RowCount, left.ColumnCount, ref rowPointers, ref columnIndices, ref values);
        }

        /// <summary>
        /// Computes the subtraction of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> to subtract with. </param>
        /// <returns> The <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        public static CompressedRow operator -(CompressedRow left, CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            List<double> values = new List<double>(left.NonZerosCount + right.NonZerosCount);
            List<int> columnIndices = new List<int>(left.NonZerosCount + right.NonZerosCount);

            int[] rowPointers = new int[left.RowCount + 1];
            rowPointers[0] = 0;

            int i_RightNZ = 0;

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                int i_RighRowPointer = right._rowPointers[i_R + 1];

                // Iterate on the non-zero values of the current left row
                for (int i_LeftNZ = left._rowPointers[i_R]; i_LeftNZ < left._rowPointers[i_R + 1]; i_LeftNZ++)
                {
                    int i_LeftColumn = left._columnIndices[i_LeftNZ];

                    // Add the non-zero values of the current right row which are before the current left non-zero value.
                    while (right._columnIndices[i_RightNZ] < i_LeftColumn && i_RightNZ < i_RighRowPointer)
                    {
                        values.Add(-right._values[i_RightNZ]);
                        columnIndices.Add(right._columnIndices[i_RightNZ]);

                        i_RightNZ++;
                    }

                    // If the the non-zero values of the current left and right row are a the same column.
                    if (right._columnIndices[i_RightNZ] == i_LeftColumn)
                    {
                        values.Add(left._values[i_LeftNZ] - right._values[i_RightNZ]);
                        columnIndices.Add(i_LeftColumn);

                        i_RightNZ++;
                    }
                    else
                    {
                        values.Add(left._values[i_LeftNZ]);
                        columnIndices.Add(i_LeftColumn);
                    }
                }

                // Add the remaining non-zero values of the current right row which are after these of the current left row.
                for (; i_RightNZ < i_RighRowPointer; i_RightNZ++)
                {
                    values.Add(-right._values[i_RightNZ]);
                    columnIndices.Add(right._columnIndices[i_RightNZ]);
                }

                rowPointers[i_R + 1] = values.Count;
            }

            return new CompressedRow(left.RowCount, left.ColumnCount, ref rowPointers, ref columnIndices, ref values);
        }


        /// <summary>
        /// Computes the opposite of the <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedRow"/> whose opposite is to be computed. </param>
        /// <returns> The <see cref="CompressedRow"/>, opposite of the initial one. </returns>
        public static CompressedRow operator -(CompressedRow operand)
        {
            int[] rowPointers = (int[])operand._rowPointers.Clone();

            List<int> columnIndices = new List<int>(operand.NonZerosCount);
            List<double> values = new List<double>(operand.NonZerosCount);
            for (int i_NZ = 0; i_NZ < values.Count; i_NZ++)
            {
                columnIndices.Add(operand._columnIndices[i_NZ]);
                values.Add(-operand._values[i_NZ]);
            }

            return new CompressedRow(operand.RowCount, operand.ColumnCount, ref rowPointers, ref columnIndices, ref values);
        }


        /// <summary>
        /// Computes the multiplication of two <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedRow"/> for the multiplication. </param>
        /// <returns> The <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The number of columns of left and the number of rows of right must be equal. </exception>
        public static CompressedRow operator *(CompressedRow left, CompressedRow right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            int[] rowPointers = new int[left.RowCount + 1];
            List<double> values = new List<double>(left.NonZerosCount * right.NonZerosCount);
            List<int> columnIndices = new List<int>(left.NonZerosCount * right.NonZerosCount);

            rowPointers[0] = 0;

            int i_LeftNZ = left._rowPointers[0];
            // Iterate on the row of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                int i_LeftNZ_RowBound = left._rowPointers[i_R + 1];

                // For the initialisation of rowIndices[i_C] and values[i_C]
                while (columnIndices.Count == rowPointers[i_R] & i_LeftNZ < i_LeftNZ_RowBound)
                {
                    int i_LC = left._columnIndices[i_LeftNZ];
                    double leftVal = left._values[i_LeftNZ];

                    for (int i_RightNZ = right._rowPointers[i_LC]; i_RightNZ < right._rowPointers[i_LC + 1]; i_RightNZ++)
                    {
                        int i_C = right._columnIndices[i_RightNZ];
                        double rightVal = right._values[i_RightNZ];

                        columnIndices.Add(i_C);
                        values.Add(leftVal * rightVal);
                    }

                    i_LeftNZ++;
                }

                for (; i_LeftNZ < i_LeftNZ_RowBound; i_LeftNZ++)
                {
                    int i_LC = left._columnIndices[i_LeftNZ];
                    double leftVal = left._values[i_LeftNZ];

                    int i_Pointer = rowPointers[i_R];
                    int i_RightNZ_RowBound = right._rowPointers[i_LC + 1];

                    // Insert or add the values whose column is below or equal to the last row column
                    int i_RightNZ = right._rowPointers[i_LC];
                    for (; i_RightNZ < i_RightNZ_RowBound; i_RightNZ++)
                    {
                        int i_C = right._columnIndices[i_RightNZ];
                        double rightVal = right._values[i_RightNZ];

                        if (i_C < columnIndices[i_Pointer])
                        {
                            columnIndices.Insert(i_Pointer, i_C);
                            values.Insert(i_Pointer, leftVal * rightVal);

                            i_Pointer++;
                        }
                        else if (i_C == columnIndices[i_Pointer])
                        {
                            values[i_Pointer] += leftVal * rightVal;

                            i_Pointer++;
                            if (i_Pointer == values.Count) { i_RightNZ++; break; }
                        }
                        else
                        {
                            i_Pointer++;
                            if (i_Pointer == values.Count) { break; }

                            i_RightNZ--;
                        }
                    }

                    for (; i_RightNZ < i_RightNZ_RowBound; i_RightNZ++)
                    {
                        int i_C = right._columnIndices[i_RightNZ];
                        double rightVal = right._values[i_RightNZ];

                        columnIndices.Add(i_C);
                        values.Add(leftVal * rightVal);
                    }

                }

                rowPointers[i_R + 1] = values.Count;
            }

            return new CompressedRow(left.RowCount, right.ColumnCount, ref rowPointers, ref columnIndices, ref values);
        }


        /******************** Embedding : CompressedColumn  ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the addition. </returns>
        public static CompressedRow operator +(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = (CompressedRow)right;

            return left + ccs;
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the subtraction. </returns>
        public static CompressedRow operator -(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = (CompressedRow)right;

            return left - ccs;
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="CompressedColumn"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="CompressedColumn"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedColumn"/> resulting from the multiplication. </returns>
        public static CompressedRow operator *(CompressedRow left, CompressedColumn right)
        {
            CompressedRow ccs = (CompressedRow)right;

            return left * ccs;
        }


        /******************** Embed In : Dense Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="CompressedRow"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the addition. </returns>
        public static DenseMatrix operator +(CompressedRow left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            double[,] result = new double[left.RowCount, left.ColumnCount];
            for (int i_R = 0; i_R < right.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < right.ColumnCount; i_C++)
                {
                    result[i_R, i_C] = right[i_R, i_C];
                }
            }

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                // Iterate on the non-zero values of the current left row
                for (int i_NZ = left.RowPointer(i_R); i_NZ < left.RowPointer(i_R + 1); i_NZ++)
                {
                    result[i_R, left.ColumnIndex(i_NZ)] += left.NonZero(i_NZ);
                }
            }

            return new DenseMatrix(result); ;
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="CompressedRow"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the subtraction. </returns>
        public static DenseMatrix operator -(CompressedRow left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.ColumnCount || left.RowCount != right.RowCount)
            {
                throw new ArgumentException("The matrices do not have the same size.");
            }

            double[,] result = new double[left.RowCount, left.ColumnCount];
            for (int i_R = 0; i_R < right.RowCount; i_R++)
            {
                for (int i_C = 0; i_C < right.ColumnCount; i_C++)
                {
                    result[i_R, i_C] = -right[i_R, i_C];
                }
            }

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                // Iterate on the non-zero values of the current left row
                for (int i_NZ = left.RowPointer(i_R); i_NZ < left.RowPointer(i_R + 1); i_NZ++)
                {
                    result[i_R, left.ColumnIndex(i_NZ)] += left.NonZero(i_NZ);
                }
            }

            return new DenseMatrix(result);
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="DenseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="DenseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="DenseMatrix"/> resulting from the multiplication. </returns>
        public static DenseMatrix operator *(CompressedRow left, DenseMatrix right)
        {
            // Verifications
            if (left.ColumnCount != right.RowCount)
            {
                throw new ArgumentException("The matrices size does not allow their multiplication.");
            }

            double[,] result = new double[left.RowCount, right.ColumnCount];

            // Iterate on the rows of left
            for (int i_R = 0; i_R < left.RowCount; i_R++)
            {
                // ITerate on the columns of righ
                for (int i_C = 0; i_C < right.ColumnCount; i_C++)
                {
                    double sum = 0d;

                    // Iterate on the non-zero values of the current left row
                    for (int i_NZ = left.RowPointer(i_R); i_NZ < left.RowPointer(i_R + 1); i_NZ++)
                    {
                        sum += left.NonZero(i_NZ) * right[left.ColumnIndex(i_NZ), i_C];
                    }

                    result[i_R, i_C] = sum;
                }
            }

            return new DenseMatrix(result);
        }


        /******************** Embedding : Sparse Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the addition. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the addition. </returns>
        /// <exception cref="ArgumentException"> The right addition of a <see cref="CompressedRow"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static CompressedRow operator +(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return left + crsRight; }
            else if (right is CompressedColumn ccsRight) { return left + ccsRight; }
            else { throw new NotImplementedException($"The right addition of a {left.GetType()} with a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> to subtract with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the subtraction. </returns>
        /// <exception cref="ArgumentException"> The subtraction of a <see cref="CompressedRow"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static CompressedRow operator -(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return left - crsRight; }
            else if (right is CompressedColumn ccsRight) { return left - ccsRight; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} with a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="SparseMatrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the multiplication. </returns>
        /// <exception cref="ArgumentException"> The right addition of a <see cref="CompressedRow"/> with a matrix as a <see cref="SparseMatrix"/> is not implemented. </exception>
        public static CompressedRow operator *(CompressedRow left, SparseMatrix right)
        {
            if (right is CompressedRow crsRight) { return left * crsRight; }
            else if (right is CompressedColumn ccsRight) { return left * ccsRight; }
            else { throw new NotImplementedException($"The right multiplication of a {left.GetType()} with a {right.GetType()} as a {nameof(SparseMatrix)} is not implemented."); }
        }


        /******************** Embed In : Matrix ********************/

        /// <summary>
        /// Computes the right addition of a <see cref="CompressedRow"/> with a <see cref="Matrix"/> on the right.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the addition. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the addition. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the addition. </returns>
        /// <exception cref="NotImplementedException"> The right addition of a <see cref="CompressedRow"/> with a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix operator +(CompressedRow left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left + denseRight; }
            else if (right is SparseMatrix sparseRight) { return left + sparseRight; }
            else { throw new NotImplementedException($"The right addition of a {left.GetType()} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right subtraction of a <see cref="CompressedRow"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> to subtract. </param>
        /// <param name="right"> Right <see cref="Matrix"/> to subtract with. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the subtraction. </returns>
        /// <exception cref="NotImplementedException"> The subtraction of a <see cref="CompressedRow"/> with the a matrix as a <see cref="Matrix"/> is not implemented. </exception>
        public static Matrix operator -(CompressedRow left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left - denseRight; }
            else if (right is SparseMatrix sparseRight) { return left - sparseRight; }
            else { throw new NotImplementedException($"The subtraction of a {left.GetType()} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="left"> Left <see cref="CompressedRow"/> for the multiplication. </param>
        /// <param name="right"> Right <see cref="Matrix"/> for the multiplication. </param>
        /// <returns> The new <see cref="Matrix"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a <see cref="CompressedRow"/> with the a matrix as a <see cref="Matrix"/> is not implemented.
        /// </exception>
        public static Matrix operator *(CompressedRow left, Matrix right)
        {
            if (right is DenseMatrix denseRight) { return left * denseRight; }
            else if (right is SparseMatrix sparseRight) { return left * sparseRight; }
            else { throw new NotImplementedException($"The multiplication of a {left.GetType()} with a {right.GetType()} as a {nameof(Matrix)} is not implemented."); }
        }


        /******************** Group Action ********************/

        /// <summary>
        /// Computes the right scalar multiplication of a <see cref="CompressedRow"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the right scalar multiplication. </returns>
        public static CompressedRow operator *(CompressedRow operand, double factor)
        {
            List<double> values = new List<double>(operand.NonZerosCount);
            for (int i_NZ = 0; i_NZ < operand.NonZerosCount; i_NZ++)
            {
                values.Add(operand._values[i_NZ] * factor);
            }

            List<int> columnIndices = new List<int>(operand._columnIndices);

            int[] rowPointers = new int[operand._rowPointers.Length];
            for (int i = 0; i < rowPointers.Length; i++)
            {
                rowPointers[i] = operand._rowPointers[i];
            }

            return new CompressedRow(operand.RowCount, operand.ColumnCount, ref rowPointers, ref columnIndices, ref values);
        }

        /// <summary>
        /// Computes the left scalar multiplication of a <see cref="CompressedRow"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedRow"/> to multiply on the left. </param>
        /// <param name="factor"> <see cref="double"/> number to multiply with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the left scalar multiplication. </returns>
        public static CompressedRow operator *(double factor, CompressedRow operand)
        {
            List<double> values = new List<double>(operand.NonZerosCount);
            for (int i_NZ = 0; i_NZ < operand.NonZerosCount; i_NZ++)
            {
                values.Add(factor * operand._values[i_NZ]);
            }

            List<int> columnIndices = new List<int>(operand._columnIndices);

            int[] rowPointers = new int[operand._rowPointers.Length];
            for (int i = 0; i < rowPointers.Length; i++)
            {
                rowPointers[i] = operand._rowPointers[i];
            }

            return new CompressedRow(operand.RowCount, operand.ColumnCount, ref rowPointers, ref columnIndices, ref values);
        }


        /// <summary>
        /// Computes the scalar division of a <see cref="CompressedRow"/> with a <see cref="double"/> number.
        /// </summary>
        /// <param name="operand"> <see cref="CompressedRow"/> to divide. </param>
        /// <param name="divisor"> <see cref="double"/> number to divide with. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the scalar division. </returns>
        public static CompressedRow operator /(CompressedRow operand, double divisor)
        {
            List<double> values = new List<double>(operand.NonZerosCount);
            for (int i_NZ = 0; i_NZ < operand.NonZerosCount; i_NZ++)
            {
                values.Add(operand._values[i_NZ] / divisor);
            }

            List<int> columnIndices = new List<int>(operand._columnIndices);

            int[] rowPointers = new int[operand._rowPointers.Length];
            for (int i = 0; i < rowPointers.Length; i++)
            {
                rowPointers[i] = operand._rowPointers[i];
            }

            return new CompressedRow(operand.RowCount, operand.ColumnCount, ref rowPointers, ref columnIndices, ref values);
        }


        /******************** Other Operations ********************/

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Vect.Vector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.Vector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.Vector"/> resulting from the multiplication. </returns>
        /// <exception cref="NotImplementedException"> The right multiplication of a <see cref="CompressedRow"/> with a vector as a <see cref="Vect.Vector"/> is not implemented. </exception>
        public static Vect.Vector operator *(CompressedRow matrix, Vect.Vector vector)
        {
            if (vector is Vect.DenseVector denseVector) { return matrix * denseVector; }
            else if (vector is Vect.SparseVector sparseVector) { return matrix * sparseVector; }
            else { throw new NotImplementedException($"The right multiplication of a {matrix.GetType()} with a {vector.GetType()} as a {nameof(Vect.Vector)} is not implemented."); }
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Vect.DenseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.DenseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.DenseVector"/> resulting from the multiplication. </returns>
        public static Vect.DenseVector operator *(CompressedRow matrix, Vect.DenseVector vector)
        {
            double[] components = new double[matrix.RowCount];

            int i_NZ = matrix._rowPointers[0];
            for (int i_R = 0; i_R < components.Length; i_R++)
            {
                double component = 0d;

                for (; i_NZ < matrix._rowPointers[i_R + 1]; i_NZ++)
                {
                    component += matrix._values[i_NZ] * vector[matrix._columnIndices[i_NZ]];
                }

                components[i_R] = component;
            }

            return new Vect.DenseVector(ref components);
        }

        /// <summary>
        /// Computes the right multiplication of a <see cref="CompressedRow"/> with a <see cref="Vect.SparseVector"/> : <c>A*V</c>.
        /// </summary>
        /// <param name="matrix"> <see cref="CompressedRow"/> to multiply on the right. </param>
        /// <param name="vector"> <see cref="Vect.SparseVector"/> to multiply with. </param>
        /// <returns> The new <see cref="Vect.SparseVector"/> resulting from the multiplication. </returns>
        public static Vect.SparseVector operator *(CompressedRow matrix, Vect.SparseVector vector)
        {
            int size = matrix.RowCount;
            List<int> rowIndices = new List<int>();
            List<double> values = new List<double>();

            int i_NZ = matrix._rowPointers[0];
            for (int i_R = 0; i_R < size; i_R++)
            {
                double component = 0d;

                for (; i_NZ < matrix._rowPointers[i_R + 1]; i_NZ++)
                {
                    if (vector.TryGetComponent(matrix._columnIndices[i_NZ], out double val)) { component += matrix._values[i_NZ] * val; }
                }
                if (component != 0d) { rowIndices.Add(i_R); values.Add(component); }
            }

            return new Vect.SparseVector(size, rowIndices, values);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a <see cref="CompressedColumn"/> number into a <see cref="CompressedRow"/>.
        /// </summary>
        /// <param name="ccs"> <see cref="CompressedRow"/> to convert. </param>
        /// <returns> The new <see cref="CompressedRow"/> resulting from the conversion. </returns>
        public static explicit operator CompressedRow(CompressedColumn ccs)
        {
            int rowCount = ccs.RowCount;
            int columnCount = ccs.ColumnCount;

            // Get the number of elements per row
            int[] rowHelper = new int[rowCount];
            for (int i_NZ = 0; i_NZ < ccs.NonZerosCount; i_NZ++)
            {
                rowHelper[ccs.RowIndex(i_NZ)]++;
            }


            // Creates the row pointer
            int[] rowPointers = new int[rowCount + 1];

            rowPointers[0] = 0;
            for (int i_R = 0; i_R < rowCount; i_R++)
            {
                rowPointers[i_R + 1] = rowPointers[i_R] + rowHelper[i_R];
                rowHelper[i_R] = 0;
            }


            // Creates the column index and value lists
            int[] columnIndices = new int[ccs.NonZerosCount];
            double[] values = new double[ccs.NonZerosCount];

            int i_ColumnNZ = ccs.ColumnPointer(0);
            for (int i_C = 0; i_C < columnCount; i_C++)
            {
                for (; i_ColumnNZ < ccs.ColumnPointer(i_C + 1); i_ColumnNZ++)
                {
                    int i_R = ccs.RowIndex(i_ColumnNZ);
                    int i_Pointer = rowPointers[i_R] + rowHelper[i_R];

                    columnIndices[i_Pointer] = i_C;
                    values[i_Pointer] = ccs[i_ColumnNZ];
                    rowHelper[i_R]++;
                }
            }


            List<int> list_ColumnsIndices = new List<int>(columnIndices);
            List<double> list_Values = new List<double>(values);

            return new CompressedRow(rowCount, columnCount, ref rowPointers, ref list_ColumnsIndices, ref list_Values);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the row pointers at a given index.
        /// </summary>
        /// <param name="index"> Index row pointer to get.</param>
        /// <returns> The row pointers of this <see cref="CompressedRow"/> sparse matrix. </returns>
        public int RowPointer(int index) => _rowPointers[index];

        /// <summary>
        /// Returns the column index of a non-zero value at a given index.
        /// </summary>
        /// <param name="index"> Index of the non-zero value whose column index to get.</param>
        /// <returns> The column index of this <see cref="CompressedRow"/> sparse matrix at the given index. </returns>
        public int ColumnIndex(int index) => _columnIndices[index];

        /// <summary>
        /// Returns the non-zero value at a given index.
        /// </summary>
        /// <param name="index"> Index of the non-zero value to get.</param>
        /// <returns> The non-zero value of this <see cref="CompressedRow"/> sparse matrix at the given index. </returns>
        public double NonZero(int index) => _values[index];


        /// <summary>
        /// Inserts a value at the given row and column index. If a non-zero value already exists, the value replaces the existing one.
        /// </summary>
        /// <remarks> This operation can be time consuming. </remarks>
        /// <param name="rowIndex"> Row index of the value to insert. </param>
        /// <param name="columnIndex"> Column index of the value to insert. </param>
        /// <param name="value"> Value to insert.</param>
        /// <returns> The index of the inserted non-zero value. </returns>
        public int Insert(int rowIndex, int columnIndex, double value)
        {
            if (value == 0d) { throw new ArgumentException($"A zero value cannot be inserted in the {nameof(CompressedRow)}", nameof(value)); }


            int i_NZ = RowPointer(rowIndex);
            for ( ; i_NZ < RowPointer(rowIndex + 1); i_NZ++)
            {
                if (ColumnIndex(i_NZ) < columnIndex) { continue; }
                else { break; }
            }

            if (ColumnIndex(i_NZ) == columnIndex) { this[i_NZ] = value; }
            else 
            {
                _columnIndices.Insert(i_NZ, columnIndex);
                _values.Insert(i_NZ, value);

                for (int i_R = rowIndex; i_R < RowCount; i_R++)
                {
                    _rowPointers[i_R + 1]++;
                }
            }

            return i_NZ;
        }


        /// <summary>
        /// Solve the system <code>Ax=y</code> using Cholesky decomposition.
        /// </summary>
        /// <param name="vector"> The vector y in the system. </param>
        /// <returns> The vector x in the system. </returns>
        public Vect.DenseVector SolveCholesky(Vect.Vector vector)
        {
            CompressedColumn ccs = (CompressedColumn)this;

            return ccs.SolveCholesky(vector);
        }

        #endregion


        #region Override : Matrix

        /// <inheritdoc cref="Matrix.At(int, int)"/>
        public override double At(int row, int column)
        {
            for (int i_NZ = _rowPointers[row]; i_NZ < _rowPointers[row + 1]; i_NZ++)
            {
                if (_columnIndices[i_NZ] == column) { return _values[i_NZ]; }
                else if (column < _columnIndices[i_NZ]) { break; }
            }
            return 0d;
        }

        #endregion

        #region Override : SparseMatrix

        /// <inheritdoc cref="SparseMatrix.NonZeros()"/>
        public override IEnumerable<(int rowIndex, int columnIndex, double value)> NonZeros()
        {
            for (int i_R = 0; i_R < RowCount; i_R++)
            {
                for (int i_NZ = _rowPointers[i_R]; i_NZ < _rowPointers[i_R + 1]; i_NZ++)
                {
                    yield return (i_R, _columnIndices[i_NZ], _values[i_NZ]);
                }
            }
        }

        #endregion

    }
}
