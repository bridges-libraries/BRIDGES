using System;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages
{
    /// <summary>
    /// Format of sparse storages available in BRIDGES.
    /// </summary>
    public enum SparseStorageFormat : byte
    {
        /// <summary>
        /// The components are stored in a list, column by column, and their position in the matrix is identified by a row index list and a column pointer.
        /// </summary>
        CompressedColumn = 10,
    }
}
