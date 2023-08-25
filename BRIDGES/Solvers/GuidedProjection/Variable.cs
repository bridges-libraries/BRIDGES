using System;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Variable of the <see cref="GuidedProjectionAlgorithm"/>.
    /// </summary>
    public struct Variable
    {
        #region Fields

        /// <summary>
        /// Segment of the global vector X representing this variable. 
        /// </summary>
        private ArraySegment<double> _components;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the component of the variable at the specified index.
        /// </summary>
        /// <param name="index"> The zero-based index of the component to get. </param>
        /// <returns> The component at the specified index. </returns>
        public double this[int index] => _components.Array[_components.Offset + index];

        /// <summary>
        /// Returns the number of components of the variable.
        /// </summary>
        public int Dimension => _components.Count;


        /// <summary>
        /// Gets the index of the variable's first component, relative to the start of the global vector X.
        /// </summary>
        internal int Offset => _components.Offset;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="Variable"/> structure from its components.
        /// </summary>
        /// <param name="components"> Components of the variable. </param>
        public Variable(params double[] components)
        {
            double[] array = (double[])components.Clone();

            _components = new ArraySegment<double>(array);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Changes the reference of this variable.
        /// </summary>
        /// <param name="array"> New array to refer to.</param>
        /// <param name="offset"> Index of the variable's first component in the array. </param>
        internal void ChangeReference(double[] array, int offset)
        {
            _components = new ArraySegment<double>(array, offset, Dimension);
        }

        #endregion
    }
}
