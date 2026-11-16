using System;
using System.Numerics;
using System.Collections.Generic;

using Vect = BRIDGES.Numerics.LinearAlgebra.Vectors;


namespace BRIDGES.Numerics.LinearAlgebra.Matrices.SparseBuilders
{
    /// <summary>
    /// Represents a generic dictionary of keys storage to build a matrix.
    /// </summary>
    /// <typeparam name="T"> Type of the matrix components. </typeparam>
    public class DictionaryOfKeys<T>
        where T : INumberBase<T>
    {
        #region Fields

        /// <summary>
        /// Dictionary containing the values.
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
        private Dictionary<(int RowIndex, int ColumnIndex), T> _storage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dictionary storing the component values.
        /// <list type="table">
        ///     <listheader>
        ///         <term> Key </term>
        ///         <description> (RowIndex, ColumnIndex) pair of a component. </description>
        ///     </listheader>
        ///     <item>
        ///         <term> Value </term>
        ///         <description> Value of the component at the corresponding row and column. </description>
        ///     </item>
        /// </list>
        /// </summary>
        public IReadOnlyDictionary<(int RowIndex, int ColumnIndex), T> Storage => _storage;

        /// <inheritdoc/>
        public int Count => _storage.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryOfKeys{T}"/> class with its size and capacity.
        /// </summary>
        /// <param name="rowCount"> Number of rows for the matrix. </param>
        /// <param name="columnCount"> Number of columns for the matrix. </param>
        /// <param name="capacity"> Number of values that the matrix can hold. </param>
        public DictionaryOfKeys(int capacity)
        {
            _storage = new Dictionary<(int, int), T>(capacity);
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryOfKeys{T}"/> class by deep copying another <see cref="DictionaryOfKeys{T}"/>.
        /// </summary>
        /// <param name="dok"> <see cref="DictionaryOfKeys{T}"/> to deep copy. </param>
        public DictionaryOfKeys(DictionaryOfKeys<T> dok)
        {
            _storage = new Dictionary<(int RowIndex, int ColumnIndex), T>(dok._storage);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Attemps to get the value of a component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to get. </param>
        /// <param name="column"> Column index of the component to get. </param>
        /// <param name="value"> Value of the component at the given row and column if it was found, <see langword="default"/> otherwise. </param>
        /// <returns> <see langword="true"/> if the component was found, <see langword="false"/> otherwise. </returns>
        public bool TryGet(int row, int column, out T? value)
        {
            return _storage.TryGetValue((row, column), out value);
        }


        /// <summary>
        /// Adds a component to this sparse storage.
        /// </summary>
        /// <remarks> For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>. </remarks>
        /// <param name="row"> Row index of the component to insert. </param>
        /// <param name="column"> Column index of the component to insert. </param>
        /// <param name="value"> Value for the component to insert. </param>
        /// <returns> <see langword="true"/> if the component was inserted, <see langword="false"/> otherwise. </returns>
        public void Add(int row, int column, T value)
        {
            if(_storage.ContainsKey((row,column)))
            {
                _storage[(row, column)] += value;
            }
            else { _storage.Add((row, column), value);}
        }

        /// <summary>
        /// Adds a non-zero component to this sparse storage.
        /// </summary>
        /// <param name="row"> Row index of the component to insert. </param>
        /// <param name="column"> Column index of the component to insert. </param>
        /// <param name="value"> Value for the component to insert. </param>
        /// <param name="nonZeroCheck"> Evaluates whether the <paramref name="value"/> should be checked for the non-zero condition. </param>
        /// <returns> <see langword="true"/> if the component was inserted, <see langword="false"/> otherwise. </returns>
        public bool Add(int row, int column, T value, bool nonZeroCheck)
        {
            if (nonZeroCheck)
            {
                if (_storage.TryGetValue((row, column), out T? existing))
                {
                    T addition = existing + value;
                    if (addition != T.Zero) { _storage[(row, column)] = addition; }
                    else { _storage.Remove((row, column)); }

                    return true;
                }
                else if (value != T.Zero) 
                { 
                    _storage.Add((row, column), value);

                    return true;
                }
                else {  return false; }
            }
            else
            { 
                Add(row, column, value);
                return true;
            }
        }


        /// <summary>
        /// Replaces the value of a non-zero component in this sparse storage.
        /// </summary>
        /// <remarks> For efficiency reasons, no checks is performed on the non-zero condition of the <paramref name="value"/>. </remarks>
        /// <param name="row"> Row index of the component to replace. </param>
        /// <param name="column"> Column index of the component to replace. </param>
        /// <param name="value"> Value for the component to replace. </param>
        /// <returns> <see langword="true"/> if the component was retrieved and replaced, <see langword="false"/> if the component was not found in the sparse storage. </returns>
        public bool Replace(int row, int column, T value)
        {
            if (_storage.ContainsKey((row, column)))
            {
                _storage[(row, column)] = value;
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Replaces the value of a non-zero component in this sparse storage.
        /// </summary>
        /// <param name="row"> Row index of the component to replace. </param>
        /// <param name="column"> Column index of the component to replace. </param>
        /// <param name="value"> Value for the component to replace. </param>
        /// <param name="nonZeroCheck"> Evaluates whether the <paramref name="value"/> should be checked for the non-zero condition. </param>
        /// <returns> <see langword="true"/> if the component was retrieved and replaced, <see langword="false"/> if the component was not found in the sparse storage. </returns>
        public bool Replace(int row, int column, T value, bool nonZeroCheck)
        {
            if (nonZeroCheck && value == T.Zero) { return false; }

            return Replace(row, column, value);
        }


        /// <summary>
        /// Removes the value of a non-zero component in this sparse storage. 
        /// </summary>
        /// <param name="row"> Row index of the component to remove. </param>
        /// <param name="column"> Column index of the component to remove. </param>
        /// <returns> <see langword="true"/> if the component was retrieved and removed, <see langword="false"/> if the component was not found in the sparse storage. </returns>
        public bool Remove(int row, int column)
        {
            return _storage.Remove((row, column));
        }


        /// <summary>
        /// Evaluates whether the sparse storage contains an entry for the component at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the component to retrieve. </param>
        /// <param name="column"> Column index of the component to retrieve. </param>
        /// <returns> <see langword="true"/> if the sparse storage has an entry, <see langword="false"/> otherwise.  </returns>
        public bool Contains(int row, int column)
        {
            return _storage.ContainsKey((row, column));
        }

        #endregion
    }
}
