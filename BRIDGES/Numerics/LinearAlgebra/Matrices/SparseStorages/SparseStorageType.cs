using System;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.SparseStorages
{
    /// <summary>
    /// Type of sparse storages available in BRIDGES.
    /// </summary>
    public enum SparseStorageType : byte
    {
        /// <summary>
        /// The components are stored in a dictionary where :
        /// <list type="table">
        ///     <listheader>
        ///         <term> Key </term>
        ///         <description> (RowIndex, ColumnIndex) pair of a component. </description>
        ///     </listheader>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Value of the component at the corresponding row and column  </description>
        ///     </item>
        /// </list>
        /// </summary>
        DictionaryOfKeys = 0,

        /// <summary>
        /// The components are stored in a list, column by column, and their position in the matrix is identified by a row index list and a column pointer.
        /// </summary>
        CompressedColumn = 10,

    }
}
