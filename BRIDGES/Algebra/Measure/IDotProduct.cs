using System;


namespace BRIDGES.Algebra.Measure
{
    /// <summary>
    /// Interface defining a method computing the dot product of two elements.
    /// </summary>
    /// <typeparam name="TSelf"> Type of the elements in the pre-hilbertian set. </typeparam>
    /// <typeparam name="TValue"> Type of the elements in the field. </typeparam>
    public interface IDotProduct<TSelf, TValue> : INorm<TSelf>
    {
        #region Methods

        /// <summary>
        /// Computes the dot product of this element with another element.
        /// </summary>
        /// <param name="other"> Right element of the dot product. </param>
        /// <returns> The value of the dot product of the two elements. </returns>
        TValue DotProduct(TSelf other);

        #endregion

    }
}
