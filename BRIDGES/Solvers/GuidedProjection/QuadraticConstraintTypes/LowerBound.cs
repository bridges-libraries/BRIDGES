using System;
using System.Collections.Generic;

using BRIDGES.LinearAlgebra.Vectors;
using BRIDGES.LinearAlgebra.Matrices.Sparse;
using BRIDGES.Solvers.GuidedProjection.Abstracts;


namespace BRIDGES.Solvers.GuidedProjection.QuadraticConstraintTypes
{
    /// <summary>
    /// Constraint enforcing a scalar variable to be larger than a fixed lower bound. The list of variables for this constraint consists of:
    /// <list type="bullet">
    ///     <item> 
    ///         <term>S</term>
    ///         <description> Scalar variable to maintain above the fixed lower bound. </description>
    ///     </item>
    ///     <item> 
    ///         <term>X</term>
    ///         <description> Dummy scalar variable used for the constraint formulation. </description>
    ///     </item>
    /// </list>
    /// </summary>
    public class LowerBound : ConstraintType
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LowerBound"/> class.
        /// </summary>
        /// <param name="bound"> Value of the lower bound. </param>
        public LowerBound(double bound)
        {
            /******************** Define LocalHi ********************/

            int[] columnPointers = new int[] { 0, 0, 1 };
            int[] rowIndices = new int[] { 1 };
            double[] values = new double[] { 2d };

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
