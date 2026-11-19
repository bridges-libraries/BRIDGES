using System;
using System.Collections.Generic;

using Xunit;

using SBuilders = BRIDGES.Numerics.LinearAlgebra.Matrices.SparseBuilders;


namespace BRIDGES.Tests.Numerics.LinearAlgebra.Matrices.Double.SparseBuilders
{
    public class DictionaryOfKeys
    {
        #region Tests : Properties

        /// <summary>
        /// Tests the property <see cref="Mat.SparseStorages.SparseStorage{T}.Count"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected number of non zero values. </param>
        [Theory(DisplayName = "Prop. Count")]
        [ClassData(typeof(DataClasses.Count))]
        public void Property__Count(SBuilders.DictionaryOfKeys<double> storage, int expected)
        {
            // Arrange

            //Act
            int actual = storage.Count;

            // Assert
            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// Tests the property <see cref="SparseStorages.DictionaryOfKeys{T}.Storage"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to evaluate. </param>
        /// <param name="expected"> Expected dictionary containing (rowIndex, columnIndex) keys and the associated component values. </param>
        [Theory(DisplayName = "Prop. Storage")]
        [ClassData(typeof(DataClasses.Storage))]
        public void Property__Storage(SBuilders.DictionaryOfKeys<double> storage, Dictionary<(int,int), double> expected)
        {
            // Arrange

            //Act
            IReadOnlyDictionary<(int RowIndex, int ColumnIndex), double> result = storage.Storage;

            // Assert
            Assert.Equal(expected.Count, result.Count);
            foreach(KeyValuePair<(int, int), double> kvp in expected)
            {
                Assert.True(result.ContainsKey(kvp.Key));
                Assert.Equal(kvp.Value, result[kvp.Key]);
            }
        }

        #endregion

        #region Tests : Constructors

        /// <summary>
        /// Tests the property <see cref="SparseStorages.DictionaryOfKeys{T}.DictionaryOfKeys(int, int, int)"/>.
        /// </summary>
        /// <param name="rowCount"> Expected number of row. </param>
        /// <param name="columnCount"> Expected number of column. </param>
        /// <param name="capacity"> Expected number of non zero values. </param>
        [Theory(DisplayName = "DictionaryOfKeys<T>(int, int, int)")]
        [ClassData(typeof(DataClasses.Constructor__Int))]
        public void Constructor__Int(int capacity)
        {
            // Arrange

            //Act
            SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(capacity);

            // Assert
            Assert.Equal(0, result.Count);

            Assert.Empty(result.Storage);
        }


        /// <summary>
        /// Tests the property <see cref="SparseStorages.DictionaryOfKeys{T}.DictionaryOfKeys(SparseStorages.DictionaryOfKeys{T})"/>.
        /// </summary>
        /// <param name="expected"> Sparse storage to deep copy. </param>
        [Theory(DisplayName = "DictionaryOfKeys<T>(DictionaryOfKeys<T>)")]
        [ClassData(typeof(DataClasses.Constructor__DictionaryOfKeys))]
        public void Constructor__DictionaryOfKeys(SBuilders.DictionaryOfKeys<double> expected)
        {
            // Arrange

            //Act
            SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(expected);

            // Assert
            Assert.Equal(expected.Count, result.Count);
            foreach(KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(result.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, result.Get(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
            }
        }

        #endregion

        #region Tests : Public Methods

        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.TryGet(int, int, out T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="values"> Two-dimensional array containing the values of zero and non-zero components. </param>
        [Theory(DisplayName = "TryGet(int, int)")]
        [ClassData(typeof(DataClasses.ToArray))]
        public void TryGet__Int_Int(SBuilders.DictionaryOfKeys<double> storage, double[,] values)
        {
            // Arrange

            // Act & Assert
            for (int i_Row = 0; i_Row < values.GetLength(0); i_Row++)
            {
                for (int i_Column = 0; i_Column < values.GetLength(1); i_Column++)
                {
                    if (values[i_Row, i_Column] == 0.0)
                    {
                        Assert.False(storage.TryGet(i_Row, i_Column, out double value));
                    }
                    else
                    {
                        Assert.True(storage.TryGet(i_Row, i_Column, out double value));
                        Assert.Equal(values[i_Row, i_Column], value);
                    }

                }
            }
        }


        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.Insert(int, int, T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected"> Expected sparse storage after insertion of the component in the <paramref name="storage"/>. </param>
        [Theory(DisplayName = "Add(int, int, T)")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T(SBuilders.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, SBuilders.DictionaryOfKeys<double> expected)
        {
            //     -----     Arrange
            bool isZero = value == 0.0;
            (bool isExisting, bool isOpposite)= storage.TryGet(rowIndex, columnIndex, out double existing) ? (true, existing == -value) : (false, false); 

            //     -----     Act
            storage.Add(rowIndex, columnIndex, value);

            //     -----     Assert
            if (isOpposite | (!isExisting & isZero)) { Assert.Equal(expected.Count + 1, storage.Count); }
            else { Assert.Equal(expected.Count, storage.Count); }

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage.Get(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
            }

            // Undesired cases resulting from not checking potential zeros.
            if (isOpposite) 
            {
                // The storage ends with a zero values because no check is performed when the added value is the opposite
                Assert.True(storage.Contains(rowIndex, columnIndex));
                Assert.Equal(0.0, storage.Get(rowIndex, columnIndex));
            }
            else if (!isExisting & isZero) 
            {
                // The storage ends with a zero values because no check is performed when the added value is zero
                Assert.True(storage.Contains(rowIndex, columnIndex));
                Assert.Equal(0.0, storage.Get(rowIndex, columnIndex));
            }
        }

        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.Insert(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected">  Expected sparse storage after insertion of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Add(int, int, T, Bool) - Check")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T_Bool__Check(SBuilders.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, SBuilders.DictionaryOfKeys<double> expected)
        {
            // Arrange

            // Act
            bool isAdded = storage.Add(rowIndex, columnIndex, value, true);

            // Assert
            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage.Get(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
            }
        }

        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.Insert(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to insert. </param>
        /// <param name="columnIndex"> Column index of the component to insert. </param>
        /// <param name="value"> Value of the component to insert. </param>
        /// <param name="expected">  Expected sparse storage after insertion of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Add(int, int, T, Bool) - No Check")]
        [ClassData(typeof(DataClasses.Add))]
        public void Add__Int_Int_T_Bool__NoCheck(SBuilders.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, SBuilders.DictionaryOfKeys<double> expected)
        {
            //     -----     Arrange
            bool isZero = value == 0.0;
            (bool isExisting, bool isOpposite) = storage.TryGet(rowIndex, columnIndex, out double existing) ? (true, existing == -value) : (false, false);

            //     -----     Act
            bool _ = storage.Add(rowIndex, columnIndex, value, false);

            //     -----     Assert
            if (isOpposite | (!isExisting & isZero)) { Assert.Equal(expected.Count + 1, storage.Count); }
            else { Assert.Equal(expected.Count, storage.Count); }

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage.Get(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
            }

            // Undesired cases resulting from not checking potential zeros.
            if (isOpposite)
            {
                // The storage ends with a zero values because no check is performed when the added value is the opposite
                Assert.True(storage.Contains(rowIndex, columnIndex));
                Assert.Equal(0.0, storage.Get(rowIndex, columnIndex));
            }
            else if (!isExisting & isZero)
            {
                // The storage ends with a zero values because no check is performed when the added value is zero
                Assert.True(storage.Contains(rowIndex, columnIndex));
                Assert.Equal(0.0, storage.Get(rowIndex, columnIndex));
            }
        }


        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.Replace(int, int, T)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Replace(int, int, T)")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T(SBuilders.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, SBuilders.DictionaryOfKeys<double> expected)
        {
            //     -----    Arrange
            bool isZero = value == 0.0;
            bool isExisting = storage.Contains(rowIndex, columnIndex);

            //     -----    Act
            bool isReplaced = storage.Replace(rowIndex, columnIndex, value);

            //     -----    Assert
            Assert.Equal(isExisting, isReplaced);

            if (isExisting & isZero) { Assert.Equal(expected.Count + 1, storage.Count); }
            else { Assert.Equal(expected.Count, storage.Count); }

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage.Get(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
            }

            // Undesired cases resulting from not checking potential zeros.
            if (isExisting & isZero)
            {
                // The storage ends with a zero values because no check is performed when the added value is zero
                Assert.True(storage.Contains(rowIndex, columnIndex));
                Assert.Equal(0.0, storage.Get(rowIndex, columnIndex));
            }
        }

        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.Replace(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/>. </param>
        [Theory(DisplayName = "Replace(int, int, T, Bool) - Check")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T_Bool__Check(SBuilders.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, SBuilders.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldReplace = storage.Contains(rowIndex, columnIndex) && value != 0.0;

            if (!shouldReplace) { expected = new SBuilders.DictionaryOfKeys<double>(storage); }

            // Act
            bool isReplaced = storage.Replace(rowIndex, columnIndex, value, true);

            // Assert
            Assert.Equal(shouldReplace, isReplaced);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage.Get(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
            }
        }

        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.Replace(int, int, T, bool)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to replace. </param>
        /// <param name="columnIndex"> Column index of the component to replace. </param>
        /// <param name="value"> Value of the component to replace. </param>
        /// <param name="expected">  Expected sparse storage after replacement of the component in the <paramref name="storage"/>. </param>
        [Theory(DisplayName = "Replace(int, int, T, Bool) - No Check")]
        [ClassData(typeof(DataClasses.Replace))]
        public void Replace__Int_Int_T_Bool__NoCheck(SBuilders.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, double value, SBuilders.DictionaryOfKeys<double> expected)
        {
            //     -----     Arrange
            bool isZero = value == 0.0;
            bool isExisting = storage.Contains(rowIndex, columnIndex);

            //     -----     Act
            bool isReplaced = storage.Replace(rowIndex, columnIndex, value, false);

            //     -----    Assert
            Assert.Equal(isExisting, isReplaced);

            if (isExisting & isZero) { Assert.Equal(expected.Count + 1, storage.Count); }
            else { Assert.Equal(expected.Count, storage.Count); }

            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage.Get(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
            }

            // Undesired cases resulting from not checking potential zeros.
            if (isExisting & isZero)
            {
                // The storage ends with a zero values because no check is performed when the added value is zero
                Assert.True(storage.Contains(rowIndex, columnIndex));
                Assert.Equal(0.0, storage.Get(rowIndex, columnIndex));
            }
        }


        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.Remove(int, int)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="rowIndex"> Row index of the component to remove. </param>
        /// <param name="columnIndex"> Column index of the component to remove. </param>
        /// <param name="expected">  Expected sparse storage after removal of the component in the <paramref name="storage"/> </param>
        [Theory(DisplayName = "Remove(int, int)")]
        [ClassData(typeof(DataClasses.Remove))]
        public void Remove__Int_Int(SBuilders.DictionaryOfKeys<double> storage, int rowIndex, int columnIndex, SBuilders.DictionaryOfKeys<double> expected)
        {
            // Arrange
            bool shouldRemove = storage.Contains(rowIndex, columnIndex);

            // Act
            bool isRemoved = storage.Remove(rowIndex, columnIndex);

            // Assert
            Assert.Equal(shouldRemove, isRemoved);

            Assert.Equal(expected.Count, storage.Count);
            foreach (KeyValuePair<(int RowIndex, int ColumnIndex), double> kvp in expected.Storage)
            {
                Assert.True(storage.Contains(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
                Assert.Equal(kvp.Value, storage.Get(kvp.Key.RowIndex, kvp.Key.ColumnIndex));
            }
        }


        /// <summary>
        /// Tests the method <see cref="SparseStorages.DictionaryOfKeys{T}.Contains(int, int)"/>.
        /// </summary>
        /// <param name="storage"> Sparse storage to operate on. </param>
        /// <param name="isContained"> Two-dimensional array evaluating whether a component at the given row and column has an entry in the sparse storage.</param>
        [Theory(DisplayName = "Contains(int, int)")]
        [ClassData(typeof(DataClasses.Contains))]
        public void Contains__Int_Int(SBuilders.DictionaryOfKeys<double> storage, bool[,] isContained)
        {
            // Arrange

            // Act & Assert
            for (int i_Row = 0; i_Row < isContained.GetLength(0); i_Row++)
            {
                for (int i_Column = 0; i_Column < isContained.GetLength(1); i_Column++)
                {
                    Assert.Equal(isContained[i_Row, i_Column], storage.Contains(i_Row, i_Column));
                }
            }
        }

        #endregion


        #region Storage Classes for Data Classes

        internal static class DataStorages
        {
            /// <summary>
            /// Computes and stores general data related to general <see cref="SBuilders.DictionaryOfKeys{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse builder <see cref="B1"/> represents a [2x3] matrix. </remarks>
            internal static class B1
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse builder <see cref="B1"/>.
                /// </summary>
                private static readonly SBuilders.DictionaryOfKeys<double> _s1 = CreateS1();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B1"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s1];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B1"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS1()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the sparse builder <see cref="B1"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [2];

                /// <summary>
                /// Provides the expected number of columns of the sparse builder <see cref="B1"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [3];

                /// <summary>
                /// Provides the expected number of entries of the sparse builder <see cref="B1"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [6];


                //     -----     Methods

                /// <summary>
                /// Provides an array representation of the sparse builder.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A two dimensional array of <see cref="double"/> representing the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    double[,] values = new double[,]
                    {
                        {    1.0,    4.0,    -2.0    },
                        {    2.0,    7.0,     3.0    },
                    };

                    return [values];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse builder <see cref="B1"/>.
                /// </summary>
                /// <returns> The <see cref="SBuilders.DictionaryOfKeys{T}"/>. </returns>
                internal static SBuilders.DictionaryOfKeys<double> CreateS1()
                {
                    SBuilders.DictionaryOfKeys<double> dok = new SBuilders.DictionaryOfKeys<double>(6);

                    dok.Add(0, 0, 1.0);  dok.Add(0, 1, 4.0); dok.Add(0, 2, -2.0);
                    dok.Add(1, 0, 2.0);  dok.Add(1, 1, 7.0); dok.Add(1, 2,  3.0);

                    return dok;
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="SBuilders.DictionaryOfKeys{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse builder <see cref="B2"/> represents a [2x3] matrix. </remarks>
            internal static class B2
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse builder <see cref="B2"/>.
                /// </summary>
                private static readonly SBuilders.DictionaryOfKeys<double> _s2 = CreateS2();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B2"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s2];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B2"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS2()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the sparse builder <see cref="B2"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [2];

                /// <summary>
                /// Provides the expected number of columns of the sparse builder <see cref="B2"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [3];

                /// <summary>
                /// Provides the expected number of entries of the sparse builder <see cref="B2"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [6];


                //     -----     Methods

                /// <summary>
                /// Provides an array representation of the sparse builder.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A two dimensional array of <see cref="double"/> representing the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    double[,] values = new double[,]
                    {
                        {     5,    -1,     6    },
                        {    -2,     1,    -3    },
                    };

                    return [values];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse builder <see cref="B2"/>.
                /// </summary>
                /// <returns> The <see cref="SBuilders.DictionaryOfKeys{T}"/>. </returns>
                internal static SBuilders.DictionaryOfKeys<double> CreateS2()
                {
                    SBuilders.DictionaryOfKeys<double> dok = new SBuilders.DictionaryOfKeys<double>(6);

                    dok.Add(0, 0,  5.0); dok.Add(0, 1, -1.0); dok.Add(0, 2,  6.0);
                    dok.Add(1, 0, -2.0); dok.Add(1, 1,  1.0); dok.Add(1, 2, -3.0);

                    return dok;
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="SBuilders.DictionaryOfKeys{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse builder <see cref="B3"/> represents a [6x6] matrix. </remarks>
            internal static class B3
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse builder <see cref="B3"/>.
                /// </summary>
                private static readonly SBuilders.DictionaryOfKeys<double> _s3 = CreateS3();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B3"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s3];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B3"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS3()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the sparse builder <see cref="B3"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [6];

                /// <summary>
                /// Provides the expected number of columns of the sparse builder <see cref="B3"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [6];

                /// <summary>
                /// Provides the expected number of entries of the sparse builder <see cref="B3"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [35];


                //     -----     Methods

                /// <summary>
                /// Provides an array representation of the sparse builder.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A two dimensional array of <see cref="double"/> representing the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    double[,] values = new double[,]
                    {
                        {    10,    15,    20,    25,    30,    35    },
                        {     7,    14,    21,    28,    35,    42    },
                        {     0,    -1,     2,    -3,     4,    -5    },
                        {     6,     4,     2,    -2,    -4,    -6    },
                        {     1,     1,     2,     3,     5,     8    },
                        {     2,     4,     8,    16,    32,    64    },
                    };

                    return [values];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse builder <see cref="B3"/>.
                /// </summary>
                /// <returns> The <see cref="SBuilders.DictionaryOfKeys{T}"/>. </returns>
                internal static SBuilders.DictionaryOfKeys<double> CreateS3()
                {
                    SBuilders.DictionaryOfKeys<double> dok = new SBuilders.DictionaryOfKeys<double>(35);

                      dok.Add(0, 0, 10.0);   dok.Add(0, 1, 15.0); dok.Add(0, 2, 20.0); dok.Add(0, 3, 25.0); dok.Add(0, 4, 30.0); dok.Add(0, 5, 35.0);
                      dok.Add(1, 0,  7.0);   dok.Add(1, 1, 14.0); dok.Add(1, 2, 21.0); dok.Add(1, 3, 28.0); dok.Add(1, 4, 35.0); dok.Add(1, 5, 42.0);
                    /*dok.Add(2, 0,  0.0);*/ dok.Add(2, 1, -1.0); dok.Add(2, 2,  2.0); dok.Add(2, 3, -3.0); dok.Add(2, 4,  4.0); dok.Add(2, 5, -5.0);
                      dok.Add(3, 0,  6.0);   dok.Add(3, 1,  4.0); dok.Add(3, 2,  2.0); dok.Add(3, 3, -2.0); dok.Add(3, 4, -4.0); dok.Add(3, 5, -6.0);
                      dok.Add(4, 0,  1.0);   dok.Add(4, 1,  1.0); dok.Add(4, 2,  2.0); dok.Add(4, 3,  3.0); dok.Add(4, 4,  5.0); dok.Add(4, 5,  8.0);
                      dok.Add(5, 0,  2.0);   dok.Add(5, 1,  4.0); dok.Add(5, 2,  8.0); dok.Add(5, 3, 16.0); dok.Add(5, 4, 32.0); dok.Add(5, 5, 64.0);

                    return dok;
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="SBuilders.DictionaryOfKeys{T}"/>, for <see cref="BaseDataClass"/>.
            /// </summary>
            /// <remarks> The sparse builder <see cref="B4"/> represents a [6x6] matrix. </remarks>
            internal static class B4
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse builder <see cref="B4"/>.
                /// </summary>
                private static readonly SBuilders.DictionaryOfKeys<double> _s4 = CreateS4();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B4"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s4];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B4"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS4()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the sparse builder <see cref="B4"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [6];

                /// <summary>
                /// Provides the expected number of columns of the sparse builder <see cref="B4"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [6];

                /// <summary>
                /// Provides the expected number of entries of the sparse builder <see cref="B4"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [36];


                //     -----     Methods

                /// <summary>
                /// Provides an array representation of the sparse builder.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A two dimensional array of <see cref="double"/> representing the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    double[,] values = new double[,]
                    {
                        {    11,     12,     13,     14,     15,     16    },
                        {    21,     22,     23,     24,     25,     26    },
                        {    31,     32,     33,     34,     35,     36    },
                        {    41,     42,     43,     44,     45,     46    },
                        {    51,     52,     53,     54,     55,     56    },
                        {    61,     62,     63,     64,     65,     66    },
                    };

                    return [values];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse builder <see cref="B4"/>.
                /// </summary>
                /// <returns> The <see cref="SBuilders.DictionaryOfKeys{T}"/>. </returns>
                internal static SBuilders.DictionaryOfKeys<double> CreateS4()
                {
                    SBuilders.DictionaryOfKeys<double> dok = new SBuilders.DictionaryOfKeys<double>(36);

                    dok.Add(0, 0, 11); dok.Add(0, 1, 12); dok.Add(0, 2, 13); dok.Add(0, 3, 14); dok.Add(0, 4, 15); dok.Add(0, 5, 16);
                    dok.Add(1, 0, 21); dok.Add(1, 1, 22); dok.Add(1, 2, 23); dok.Add(1, 3, 24); dok.Add(1, 4, 25); dok.Add(1, 5, 26);
                    dok.Add(2, 0, 31); dok.Add(2, 1, 32); dok.Add(2, 2, 33); dok.Add(2, 3, 34); dok.Add(2, 4, 35); dok.Add(2, 5, 36);
                    dok.Add(3, 0, 41); dok.Add(3, 1, 42); dok.Add(3, 2, 43); dok.Add(3, 3, 44); dok.Add(3, 4, 45); dok.Add(3, 5, 46);
                    dok.Add(4, 0, 51); dok.Add(4, 1, 52); dok.Add(4, 2, 53); dok.Add(4, 3, 54); dok.Add(4, 4, 55); dok.Add(4, 5, 56);
                    dok.Add(5, 0, 61); dok.Add(5, 1, 62); dok.Add(5, 2, 63); dok.Add(5, 3, 64); dok.Add(5, 4, 65); dok.Add(5, 5, 66);

                    return dok;
                }

                #endregion
            }


            /// <summary>
            /// Computes and stores general data related to general <see cref="SBuilders.DictionaryOfKeys{T}"/>, for <see cref="DataClasses"/>.
            /// </summary>
            /// <remarks> The sparse builder <see cref="B5"/> represents a [3x5] matrix. </remarks>
            internal static class B5
            {
                #region Static Fields

                /// <summary>
                /// Staticly stored sparse builder <see cref="B5"/>.
                /// </summary>
                private static readonly SBuilders.DictionaryOfKeys<double> _s5 = CreateS5();

                #endregion

                #region Public Static Methods

                /// <summary>
                /// Provides a readable version of the data that must not be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> The staticly stored <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Readable()
                {
                    return [_s5];
                }

                /// <summary>
                /// Provides a writable version of the data that can be altered during the parametrised tests.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> A newly computed <see cref="SBuilders.DictionaryOfKeys{T}"/> of type <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Writable()
                {
                    return [CreateS5()];
                }


                //     -----     Properties

                /// <summary>
                /// Provides the expected number of row of the sparse builder <see cref="B5"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the row count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] RowCount() => [3];

                /// <summary>
                /// Provides the expected number of columns of the sparse builder <see cref="B5"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the column count. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ColumnCount() => [5];


                /// <summary>
                /// Provides the expected number of entries of the sparse builder <see cref="B5"/>.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> An <see cref="int"/> representing the number of entries in the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Count() => [4];


                /// <summary>
                /// Provides the expected list of values of the storage.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> List of <see cref="double"/> values containing the non-zero values, arranged column by column. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Storage()
                {
                    Dictionary<(int, int), double> dictionary = new Dictionary<(int, int), double>
                    {
                        { (0, 0), 1.0 },
                        { (0, 3), -2.0 },
                        { (2, 0), 2.0 },
                        { (2, 4), 5.5 }
                    };

                    return [dictionary];
                }


                //     -----     Public Methods

                /// <summary>
                /// Provides the information to insert a component in the sparse builder and the expected result. 
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted between two existing components.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after insertion of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_01()
                {
                    int rowIndex = 1;
                    int columnIndex = 0;
                    double value = -3.2;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(5);

                    result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    result.Add(1, 0, -3.2);  /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                    result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse builder and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted after an existing component and at the end of the column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after insertion of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_02()
                {
                    int rowIndex = 2;
                    int columnIndex = 3;
                    double value = 0.7;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/   result.Add(2, 3,  0.7);    result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse builder and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted after an existing component and in the middle of the column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after insertion of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_03()
                {
                    int rowIndex = 1;
                    int columnIndex = 3;
                    double value = 0.3;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/   result.Add(1, 3,  0.3);   /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse builder and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted before an existing component and in the start of the column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after insertion of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_04()
                {
                    int rowIndex = 0;
                    int columnIndex = 4;
                    double value = 2.2;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);     result.Add(0, 4, 2.2);
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse builder and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The component is inserted in an empty column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after insertion of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_05()
                {
                    int rowIndex = 1;
                    int columnIndex = 2;
                    double value = 1.5;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/   result.Add(1, 2, 1.5);   /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse builder and the expected result.
                /// </summary>
                /// <remarks>
                /// The component has a zero is zero.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after insertion of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_06()
                {
                    int rowIndex = 0;
                    int columnIndex = 1;
                    double value = 0.0;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(5);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to insert a component in the sparse builder and the expected result.
                /// </summary>
                /// <remarks>
                /// Case : The inserted component already exists.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to insert. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after insertion of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Add__Case_07()
                {
                    int rowIndex = 0;
                    int columnIndex = 3;
                    double value = 8.0;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, 6.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3, 0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3, 0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }


                /// <summary>
                /// Provides the information to replace a component in the sparse builder.
                /// </summary>
                /// <remarks>
                /// Case : The value is non-zero and the component already exists.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after replacement of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Replace__Case_01()
                {
                    int rowIndex = 2;
                    int columnIndex = 0;
                    double value = -7.3;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(4);

                      result.Add(0, 0, 1.0);    /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/  /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, -7.3);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to replace a component in the sparse builder.
                /// </summary>
                /// <remarks>
                /// Case : The value is non-zero and the component does not exist.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after replacement of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Replace__Case_02()
                {
                    int rowIndex = 1;
                    int columnIndex = 1;
                    double value = 5.1;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to replace a component in the sparse builder.
                /// </summary>
                /// <remarks>
                /// Case : The value is zero and the component already exists.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after replacement of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Replace__Case_03()
                {
                    int rowIndex = 0;
                    int columnIndex = 3;
                    double value = 0.0;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/  /*result.Add(0, 2, 0.0);*/  /*result.Add(0, 3, 0.0);*/  /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/  /*result.Add(1, 2, 0.0);*/  /*result.Add(1, 3, 0.0);*/  /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/  /*result.Add(2, 2, 0.0);*/  /*result.Add(2, 3, 0.0);*/    result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }

                /// <summary>
                /// Provides the information to replace a component in the sparse builder.
                /// </summary>
                /// <remarks>
                /// Case : The value is zero and the component does not exist.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Value of the component to replace. </description>
                ///     </item>
                ///     <item>
                ///         <term> 3 </term>
                ///         <description> Expected sparse builder after replacement of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Replace__Case_04()
                {
                    int rowIndex = 1;
                    int columnIndex = 2;
                    double value = 0.0;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, value, result];
                }


                /// <summary>
                /// Provides the information to remove a component in the sparse builder.
                /// </summary>
                /// <remarks>
                /// Case : The component already exists and is before another component.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Expected sparse builder after removal of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Remove__Case_01()
                {
                    int rowIndex = 0;
                    int columnIndex = 0;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(3);

                    /*result.Add(0, 0, 0.0);*/ /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, result];
                }

                /// <summary>
                /// Provides the information to remove a component in the sparse builder.
                /// </summary>
                /// <remarks>
                /// Case : The component already exists and is after another component.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Expected sparse builder after removal of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Remove__Case_02()
                {
                    int rowIndex = 2;
                    int columnIndex = 0;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/   result.Add(0, 3, -2.0);   /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3,  0.0);*/ /*result.Add(1, 4, 0.0);*/
                    /*result.Add(2, 0, 0.0);*/ /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3,  0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, result];
                }

                /// <summary>
                /// Provides the information to remove a component in the sparse builder.
                /// </summary>
                /// <remarks>
                /// Case : The component already exists and is alone in the column.
                /// </remarks>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Row index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 1 </term>
                ///         <description> Column index of the component to remove. </description>
                ///     </item>
                ///     <item>
                ///         <term> 2 </term>
                ///         <description> Expected sparse builder after removal of the component in the sparse builder <see cref="B5"/>. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Remove__Case_03()
                {
                    int rowIndex = 0;
                    int columnIndex = 3;

                    SBuilders.DictionaryOfKeys<double> result = new SBuilders.DictionaryOfKeys<double>(4);

                      result.Add(0, 0, 1.0);   /*result.Add(0, 1, 0.0);*/ /*result.Add(0, 2, 0.0);*/ /*result.Add(0, 3, 0.0);*/ /*result.Add(0, 4, 0.0);*/
                    /*result.Add(1, 0, 0.0);*/ /*result.Add(1, 1, 0.0);*/ /*result.Add(1, 2, 0.0);*/ /*result.Add(1, 3, 0.0);*/ /*result.Add(1, 4, 0.0);*/
                      result.Add(2, 0, 2.0);   /*result.Add(2, 1, 0.0);*/ /*result.Add(2, 2, 0.0);*/ /*result.Add(2, 3, 0.0);*/   result.Add(2, 4, 5.5);

                    return [rowIndex, columnIndex, result];
                }


                /// <summary>
                /// Provides the information to evaluate whether a component is contained in the sparse builder.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Two-dimensional array evaluating whether a component at the given row and column has an entry in the sparse builder. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] Contains()
                {
                    return [new bool[3, 5] { 
                        {    true,    false,    false,     true,    false}, 
                        {   false,    false,    false,    false,    false},
                        {    true,    false,    false,    false,     true}
                    }];
                }


                /// <summary>
                /// Provides the value of each components of the represented matrix.
                /// </summary>
                /// <returns> An <see cref="T:object[]"/> whose array item are :
                /// <list type="table">
                ///     <item>
                ///         <term> 0 </term>
                ///         <description> Two-dimensional array containing the values of zero and non-zero components. </description>
                ///     </item>
                /// </list>
                /// </returns>
                public static object[] ToArray()
                {
                    return [new double[3, 5] {
                        {    1.0,    0.0,    0.0,    -2.0,    0.0},
                        {    0.0,    0.0,    0.0,    0.0,     0.0},
                        {    2.0,    0.0,    0.0,    0.0,     5.5}
                    }];
                }

                #endregion

                #region Other Static Methods

                /// <summary>
                /// Creates a sparse builder <see cref="B5"/>.
                /// </summary>
                /// <returns> The <see cref="SBuilders.DictionaryOfKeys{T}"/>. </returns>
                internal static SBuilders.DictionaryOfKeys<double> CreateS5()
                {
                    SBuilders.DictionaryOfKeys<double> dok = new SBuilders.DictionaryOfKeys<double>(4);

                      dok.Add(0, 0, 1.0);   /*dok.Add(0, 1, 0.0);*/ /*dok.Add(0, 2, 0.0);*/   dok.Add(0, 3, -2.0);   /*dok.Add(0, 4, 0.0);*/
                    /*dok.Add(1, 0, 0.0);*/ /*dok.Add(1, 1, 0.0);*/ /*dok.Add(1, 2, 0.0);*/ /*dok.Add(1, 3,  0.0);*/ /*dok.Add(1, 4, 0.0);*/
                      dok.Add(2, 0, 2.0);   /*dok.Add(2, 1, 0.0);*/ /*dok.Add(2, 2, 0.0);*/ /*dok.Add(2, 3,  0.0);*/   dok.Add(2, 4, 5.5);

                    return dok;
                }

                #endregion
            }
        }

        #endregion

        #region Data Classes for Parametrised Tests

        internal static class DataClasses
        {
            #region For Properties

            /// <summary>
            /// Class data for <see cref="Property__Count(Mat.SparseStorages.CompressedColumn{double}, int)"/>.
            /// </summary>
            internal class Count : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B1.Readable, DataStorages.B1.Count },
                        { DataStorages.B1.Readable, DataStorages.B2.Count },
                        { DataStorages.B3.Readable, DataStorages.B3.Count },
                        { DataStorages.B4.Readable, DataStorages.B4.Count },
                        { DataStorages.B5.Readable, DataStorages.B5.Count },
                    };
            }


            /// <summary>
            /// Class data for <see cref="Property__Values(Mat.SparseStorages.CompressedColumn{double}, List{double})"/>.
            /// </summary>
            internal class Storage : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B5.Readable, DataStorages.B5.Storage },
                    };
            }

            #endregion

            #region For Constructors

            /// <summary>
            /// Class data for <see cref="DictionaryOfKeys.Constructor__Int(int)"/>.
            /// </summary>
            internal class Constructor__Int : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B5.Count },
                    };
            }

            /// <summary>
            /// Class data for <see cref="DictionaryOfKeys.Constructor__DictionaryOfKeys(SBuilders.DictionaryOfKeys{double})"/>.
            /// </summary>
            internal class Constructor__DictionaryOfKeys : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B5.Readable },
                    };
            }

            #endregion

            #region For Public Methods

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Add__Int_Int_T(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/> and 
            /// <see cref="CompressedColumn.Add__Int_Int_T_Bool__Check(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/> and
            /// <see cref="CompressedColumn.Add__Int_Int_T_Bool__NoCheck(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Add : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B5.Writable,  DataStorages.B5.Add__Case_01 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Add__Case_02 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Add__Case_03 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Add__Case_04 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Add__Case_05 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Add__Case_06 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Add__Case_07 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Replace__Int_Int_T(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/> and 
            /// <see cref="CompressedColumn.Replace__Int_Int_T_Bool__Check(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/> and
            /// <see cref="CompressedColumn.Replace__Int_Int_T_Bool__NoCheck(Mat.SparseStorages.CompressedColumn{double}, int, int, double, Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Replace : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B5.Writable,  DataStorages.B5.Replace__Case_01 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Replace__Case_02 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Replace__Case_03 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Replace__Case_04 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Remove__Int_Int(Mat.SparseStorages.CompressedColumn{double}, int, int, Mat.SparseStorages.CompressedColumn{double})"/>.
            /// </summary>
            internal class Remove : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B5.Writable,  DataStorages.B5.Remove__Case_01 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Remove__Case_02 },
                        { DataStorages.B5.Writable,  DataStorages.B5.Remove__Case_03 },
                    };
            }

            /// <summary>
            /// Class data for <see cref="CompressedColumn.Contains__Int_Int(Mat.SparseStorages.CompressedColumn{double}, bool[,])"/>.
            /// </summary>
            internal class Contains : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B5.Readable,  DataStorages.B5.Contains },
                    };
            }


            /// <summary>
            /// Class data for <see cref="CompressedColumn.Property__Indexer__Int_Int(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.TryGet__Int_Int(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.ToArray(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.ToRowMajorArray(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.ToColumnMajorArray(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.RowVectors(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// <see cref="CompressedColumn.ColumnVectors(Mat.SparseStorages.CompressedColumn{double}, double[,])"/>,
            /// </summary>
            internal class ToArray : BaseDataClass
            {
                /// <inheritdoc/>
                internal override Func<object[]>[,] DataProviders => new Func<object[]>[,]
                    {
                        { DataStorages.B1.Readable,  DataStorages.B1.ToArray },
                        { DataStorages.B2.Readable,  DataStorages.B2.ToArray },
                        { DataStorages.B3.Readable,  DataStorages.B3.ToArray },
                        { DataStorages.B4.Readable,  DataStorages.B4.ToArray },
                        { DataStorages.B5.Readable,  DataStorages.B5.ToArray },
                    };
            }

            #endregion
        }

        #endregion
    }
}
