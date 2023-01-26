using System;
using System.Collections;
using System.Collections.Generic;


namespace BRIDGES.LinearAlgebra.Matrices.Storage
{
    /// <summary>
    /// Class defining a dictionary of keys storage for sparse matrix.
    /// </summary>
    public sealed class DictionaryOfKeys :
        IEnumerable<((int, int), double)>
    {
        #region Fields

        /// <summary>
        /// Values for the sparse matrix associated with there row-column pair.
        /// </summary>
        internal Dictionary<(int, int), double> _values;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements in the <see cref="DictionaryOfKeys"/>.
        /// </summary>
        public int Count => _values.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryOfKeys"/> class.
        /// </summary>
        /// <param name="capacity"> Capacity of the dictionnary of keys.</param>
        public DictionaryOfKeys(int capacity)
        {
            _values = new Dictionary<(int, int), double> (capacity);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryOfKeys"/> class.
        /// </summary>
        /// <param name="values"> Values of the new <see cref="DictionaryOfKeys"/>. </param>
        /// <param name="rows"> Row indices of the new <see cref="DictionaryOfKeys"/>. </param>
        /// <param name="columns"> Column indices of the new <see cref="DictionaryOfKeys"/>. </param>
        /// <exception cref="ArgumentException"> The input arrays should have the same length. </exception>
        public DictionaryOfKeys(double[] values, int[] rows, int[] columns)
        {
            if ((values.Length != rows.Length) | (values.Length != columns.Length))
            {
                throw new ArgumentException("The arrays should have the same length.");
            }

            _values = new Dictionary<(int, int), double>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i], rows[i], columns[i]);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates whether the <see cref="DictionaryOfKeys"/> contains an element at the given row and column index.
        /// </summary>
        /// <param name="row"> Row index.</param>
        /// <param name="column"> Column index. </param>
        /// <returns> <see langword="true"/> if the storage doesn't have element at the specified row and column index, <see langword="false"/> otherwise. </returns>
        public bool IsEmpty(int row, int column)
        {
            return (!_values.ContainsKey((row, column)));
        }


        /// <summary>
        /// Adds an element to the storage. If the storage contains an element at the given row and column, the value is added to the existing one.
        /// </summary>
        /// <param name="value"> Value to add. </param>
        /// <param name="row"> Row index of the value. </param>
        /// <param name="column"> Column index of the value. </param>
        public void Add(double value, int row, int column)
        {
            if (_values.ContainsKey((row, column))) // Complexity : O(1)
            {
                _values[(row, column)] += value;
            }
            else
            {
                _values.Add((row, column), value); // Complexity : O(1)
            }
        }

        /// <summary>
        /// Adds an element to the storage. If the storage contains an element at the given row and column, the value replaces the existing one.
        /// </summary>
        /// <param name="value"> Value to add. </param>
        /// <param name="row"> Row index of the value. </param>
        /// <param name="column"> Column index of the value. </param>
        public void AddOrReplace(double value, int row, int column)
        {
            if (_values.ContainsKey((row, column))) // Complexity : O(1)
            {
                _values[(row, column)] = value;
            }
            else
            {
                _values.Add((row, column), value); // Complexity : O(1)
            }
        }

        /// <summary>
        /// Replaces an element of the storage at the given row and column.
        /// </summary>
        /// <param name="value"> Value to replace with. </param>
        /// <param name="row"> Row index of the value. </param>
        /// <param name="column"> Column index of the value. </param>
        /// <exception cref="MethodAccessException"> No element exist at the given row and column. </exception>
        public void Replace(double value, int row, int column)
        {
            if (_values.ContainsKey((row, column))) // Complexity : O(1)
            {
                _values[(row, column)] = value;
            }
            else
            {
                throw new MethodAccessException("No element exist at the given row and column.");
            }
        }

        /// <summary>
        /// Removes an element of the storage at the given row and column.
        /// </summary>
        /// <param name="row"> Row index of the value. </param>
        /// <param name="column"> Column index of the value. </param>
        /// <exception cref="MethodAccessException"> No element exist at the given row and column. </exception>
        public void Remove(int row, int column)
        {
            if (!_values.Remove((row, column)))
            {
                throw new MethodAccessException("No element exist at the given row and column.");
            }
        }


        /// <summary>
        /// Removes all zeros in the storage.
        /// </summary>
        /// <param name="tolerance"> Tolerance around the zero. </param>
        public void Clean(double tolerance)
        {
            List<(int, int)> keys = new List<(int, int)>();

            foreach (KeyValuePair<(int,int), double> kvp in _values)
            {
                if (Math.Abs(kvp.Value) < tolerance) { keys.Add(kvp.Key); }
            }

            for (int i_K = 0; i_K < keys.Count; i_K++)
            {
                _values.Remove(keys[i_K]);
            }
        }


        /// <summary>
        /// Returns an enumerator which reads through the non-zero components of the current <see cref="DictionaryOfKeys"/>. <br/>
        /// The <see cref="Tuple{T1, T2}"/> represents is composed of the row-column pair and the component value.
        /// </summary>
        /// <returns> The enumerator of the <see cref="DictionaryOfKeys"/>. </returns>
        public IEnumerator<((int, int), double)> GetEnumerator()
        {
            foreach (KeyValuePair<(int, int), double> component in _values) 
            { 
                yield return ((component.Key.Item1, component.Key.Item2), component.Value); 
            }
        }

        #endregion


        #region Explicit Implementations

        /******************** IEnumerable ********************/

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
