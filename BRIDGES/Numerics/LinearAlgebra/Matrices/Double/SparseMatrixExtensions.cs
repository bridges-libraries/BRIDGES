using System;

using CSp_Stor = CSparse.Storage;

using Mat = BRIDGES.Numerics.LinearAlgebra.Matrices;
using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;

using SStor = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages;
using System.Collections.Generic;

namespace BRIDGES.Numerics.LinearAlgebra.Matrices.Double
{
    /// <summary>
    /// Class containing extension methods for <see cref="SStor.CompressedColumn{T}"/> with <see cref="double"/>-precision values.
    /// </summary>
    public static class SparseMatrixExtensions
    {
        #region Solve Ax=y

        /// <summary>
        /// Solves the system <c>A·x=y</c> using Cholesky decomposition.
        /// </summary>
        /// <param name="matrix"> The sparse matrix A. </param>
        /// <param name="vector"> The vector y. </param>
        /// <returns> The vector x. </returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Vect.DenseVector<double> SolveCholesky(this SparseMatrix<double> matrix, Vect.DenseVector<double> vector)
        {
            if (matrix.StorageFormat is not SStor.SparseStorageFormat.CompressedColumn)
            {
                throw new NotImplementedException("This operation is currently only available for compressed column storage.");
            }

            SStor.CompressedColumn<double>? ccs = matrix.GetSparseStorage() as SStor.CompressedColumn<double>;
            CSp_Stor.CompressedColumnStorage<double> sparseStorage = new CSparse.Double.SparseMatrix(ccs!.RowCount, ccs!.ColumnCount, ccs!._values, ccs!._rowIndices, ccs!._columnPointers);

            var cholesky = CSparse.Double.Factorization.SparseCholesky.Create(sparseStorage, CSparse.ColumnOrdering.MinimumDegreeAtPlusA);

            double[] x = new double[matrix.ColumnCount];
            cholesky.Solve(vector.ToArray(), x);

            return new Vect.DenseVector<double>(x);
        }

        /// <summary>
        /// Solves the system <c>A·x=y</c> using Cholesky decomposition.
        /// </summary>
        /// <param name="matrix"> The sparse matrix A. </param>
        /// <param name="vector"> The vector y. </param>
        /// <returns> The vector x. </returns>
        public static Vect.SparseVector<double> SolveCholesky(this SparseMatrix<double> matrix, Vect.SparseVector<double> vector)
        {
            if (matrix.StorageFormat is not SStor.SparseStorageFormat.CompressedColumn)
            {
                throw new NotImplementedException("This operation is currently only available for compressed column storage.");
            }

            SStor.CompressedColumn<double>? ccs = matrix.GetSparseStorage() as SStor.CompressedColumn<double>;
            CSp_Stor.CompressedColumnStorage<double> sparseStorage = new CSparse.Double.SparseMatrix(ccs!.RowCount, ccs!.ColumnCount, ccs!._values, ccs!._rowIndices, ccs!._columnPointers);

            var cholesky = CSparse.Double.Factorization.SparseCholesky.Create(sparseStorage, CSparse.ColumnOrdering.MinimumDegreeAtPlusA);

            double[] x = new double[matrix.ColumnCount];
            cholesky.Solve(vector.ToArray(), x);

            Dictionary<int, double> components = new Dictionary<int, double>();
            for (int i = 0; i < x.Length; i++)
            {
                double value = x[i];
                if (value != 0.0) { components.Add(i, value); }
            }

            return new Vect.SparseVector<double>(x.Length, components);
        }

        #endregion
    }
}
