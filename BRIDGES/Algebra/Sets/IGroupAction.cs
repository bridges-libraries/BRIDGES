using System;


namespace BRIDGES.Algebra.Sets
{
    /// <summary>
    /// Interface defining methods to multiply or divide an element with a scalar.
    /// </summary>
    /// <typeparam name="TSelf"> Type of the elements in the vector space. </typeparam>
    /// <typeparam name="TValue"> Type of the scalar in the field. </typeparam>
    public interface IGroupAction<TSelf, TValue>
    {
        #region Methods

        /// <summary>
        /// Computes the right scalar multiplication of the current element.
        /// </summary>
        /// <param name="factor"> The scalar to multiply with on the right. </param>
        /// <returns> The new element resulting from the scalar multiplication. </returns>
        TSelf Multiply(TValue factor);

        /// <summary>
        /// Computes the right scalar division of the current element.
        /// </summary>
        /// <param name="divisor"> The scalar to divide with on the right. </param>
        /// <returns> The new element resulting from the scalar dividion. </returns>
        TSelf Divide(TValue divisor);

        #endregion
    }
}
