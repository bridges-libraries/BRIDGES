using System;
using System.Numerics;

namespace BRIDGES.Numerics.LinearAlgebra.Vectors.Operations
{
    internal static partial class VectorOperations
    {
        #region Methods

        /// <summary>
        /// Ensures that the two vectors have the same dimensions.
        /// </summary>
        /// <typeparam name="T"> Type of the vector component values. </typeparam>
        /// <param name="left"> Left operand of the operation. </param>
        /// <param name="right"> Right operand of the operation. </param>
        /// <exception cref="InvalidOperationException"> The two vectors must have the same number of rows. </exception>
        internal static void EnsureSameDimensions<T>(Vector<T> left, Vector<T> right)
            where T : INumberBase<T>
        {
            if (left.RowCount != right.RowCount)
            {
                throw new InvalidOperationException("The two vectors must have the same number of rows.");
            }
        }

        #endregion
    }
}
