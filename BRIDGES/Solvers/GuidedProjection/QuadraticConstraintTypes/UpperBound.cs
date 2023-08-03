using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a scalar variable to be smaller than a fixed upper bound. The list of variables for this constraint consists of:
    /// <list type="bullet">
    ///     <item> 
    ///         <term>S</term>
    ///         <description> Scalar variable to maintain under the fixed upper bound. </description>
    ///     </item>
    ///     <item> 
    ///         <term>X</term>
    ///         <description> Dummy scalar variable used for the constraint formulation. </description>
    ///     </item>
    /// </list>
    /// </summary>
    public class UpperBound : ConstraintType
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="UpperBound"/> class.
        /// </summary>
        /// <param name="bound"> Value of the upper bound. </param>
        public UpperBound(double bound)
        {
            /******************** Define LocalHi ********************/

            int[] columnPointers = new int[3] { 0, 0, 1 };
            int[] rowIndices = new int[1] { 1 };
            double[] values = new double[1] { -2d };

            LocalHi = new CompressedColumn(2, 2, columnPointers, rowIndices, values);


            /******************** Define LocalBi ********************/

            Dictionary<int, double> components = new Dictionary<int, double> { { 0, -1.0 } };

            LocalBi = new SparseVector(2, ref components);


            /******************** Define Ci ********************/

            Ci = bound;
        }

        #endregion
    }
}
