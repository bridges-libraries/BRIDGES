using System;


namespace BRIDGES.Algebra.Measure
{
    /// <summary>
    /// Interface defining a method computing the distance between two elements.
    /// </summary>
    /// <typeparam name="TSelf"> Type of the elements in the metric set. </typeparam>
    public interface IMetric<TSelf>
    {
        #region Methods

        /// <summary>
        /// Computes the distance of this element to another element.
        /// </summary>
        /// <param name="other"> Element to evaluate the distance to. </param>
        /// <returns> The value of the distance between the two elements. </returns>
        double DistanceTo(TSelf other);

        #endregion
    }
}