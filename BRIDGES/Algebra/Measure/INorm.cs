using System;


namespace BRIDGES.Algebra.Measure
{
    /// <summary>
    /// Interface defining a method to compute the norm of an element.
    /// </summary>
    /// <typeparam name="TSelf"> Type of the elements in the normed set. </typeparam>
    public interface INorm<TSelf> : IMetric<TSelf>
    {
        #region Methods

        /// <summary>
        /// Computes the norm of this element.
        /// </summary>
        /// <returns> The value of the norm. </returns>
        double Norm();

        #endregion
    }
}
