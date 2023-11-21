using System;


namespace BRIDGES.Solvers.GuidedProjection
{
    /// <summary>
    /// Interfaces representing a set of component for the global vector X, used to define <see cref="Energy"/> and  <see cref="Constraint"/>. 
    /// </summary>
    /// 
    public interface IVariable
    {
        #region Properties 

        /// <summary>
        /// Evaluates whether this variable references an external array for its components.
        /// </summary>
        bool IsReference { get; }

        /// <summary>
        /// Gets the component at the specified index in this variable.
        /// </summary>
        /// <param name="index"> The zero-based index of the variable's component to get. </param>
        /// <returns> The component at the specified index. </returns>
        double this[int index] { get; }

        /// <summary>
        /// Get the number of components of this variable.
        /// </summary>
        int Dimension { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Translate the component's index into the index of component in the <see cref="Variable"/>'s referenced array.
        /// </summary>
        /// <param name="index"> The zero-based index of the variable's component. </param>
        /// <returns> The zero-based index of the component in the <see cref="Variable"/>'s referenced array. </returns>
        int ReferenceIndex(int index);


        /// <summary>
        /// Creates an array representation of the variable.
        /// </summary>
        /// <returns> The double-precision array representing the variable. </returns>
        double[] ToArray();

        #endregion
    }
}
